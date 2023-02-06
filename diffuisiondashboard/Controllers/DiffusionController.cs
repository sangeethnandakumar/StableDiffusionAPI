using diffuisiondashboard.Repository;
using Diffusion.Engine.Engines.Meta;
using Diffusion.Engine.Engines.TextToImage;
using Diffusion.Models;
using Diffusion.Models.SDModels;
using Diffusion.Models.UserRequests;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Diagnostics;

namespace diffuisiondashboard.Controllers
{



    [Route("api/[controller]")]
    [ApiController]
    public class DiffusionController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMetaEngine meta;
        private readonly ITextToImageEngine textToImg;
        private readonly BaseRequest baseRequest;

        public DiffusionController(IHttpContextAccessor context, IMetaEngine meta, ITextToImageEngine textToImg)
        {
            this.httpContextAccessor = context;
            this.meta = meta;
            this.textToImg = textToImg;
            baseRequest = new BaseRequest
            {
                BaseURL = new Uri("http://127.0.0.1:7860/"),
                TextToImageSaveLoc = @"D:\SD v2\UI\stable-diffusion-webui\outputs\txt2img-images"
            };
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            try
            {
                var healthResponse = new HealthResponse();
                var activeHealthDaa = new HealthRepo("Health").GetAll();
                if (activeHealthDaa.Any())
                {
                    healthResponse.GPU.Used = int.Parse(activeHealthDaa?.LastOrDefault()?.GPU.Used);
                    healthResponse.GPU.Total = int.Parse(activeHealthDaa?.LastOrDefault()?.GPU.Total);
                    healthResponse.GPU.TimeData = activeHealthDaa.Select(x => x.GPU).Select(x => int.Parse(x.Used)).ToArray();
                    return Ok(healthResponse);
                }
            }
            catch (Exception)
            {
            }
            return Ok(new HealthResponse());
        }

        [HttpGet("ChangeAPIKey")]
        public async Task<IActionResult> ChangeAPIKey([FromHeader] string OLD_API_KEY, [FromHeader] string NEW_API_KEY)
        {
            var APIConfig = new KeyRepo("APICONFIG").GetAll().FirstOrDefault();
            if (APIConfig is null)
            {
                new KeyRepo("APICONFIG").Add(new APIKey
                {
                    API_KEY = "46A8D9F9-F000-4AB6-A989-89B10EC1CFB1"
                });
                APIConfig = new KeyRepo("APICONFIG").GetAll().FirstOrDefault();
            }
            if (OLD_API_KEY == APIConfig.API_KEY)
            {
                new KeyRepo("APICONFIG").Truncate();
                new KeyRepo("APICONFIG").Add(new APIKey
                {
                    API_KEY = NEW_API_KEY
                });
                return Ok(new APIKey { API_KEY = NEW_API_KEY });
            }
            return BadRequest("The old API_KEY is invalid");
        }

        [HttpGet]
        [Route("Models")]
        public async Task<IActionResult> Models()
        {
            try
            {
                if (IsAuthenticated())
                {
                    //Step 1: Generate image
                    var response = meta.GetAllModels(baseRequest);
                    return Ok(response);
                }
                else
                {
                    return BadRequest("Invalid API_KEY. Security validation failed. You cannot consume this service");
                }
            }
            catch (Exception)
            {
                return BadRequest("The system went into an error or cannot handle load. The service is temporarly haulted. Please try again or contact developer for support");
            }
        }

        [HttpGet]
        [Route("SwitchModel")]
        public async Task<IActionResult> SwitchModel([FromQuery] string modelName)
        {
            try
            {
                if (IsAuthenticated())
                {
                    //Step 1: Generate image
                    var allModels = meta.GetAllModels(baseRequest);
                    if(allModels.InstalledModels.FirstOrDefault(x=>x.ModelName == modelName) is not null)
                    {
                        var sw = Stopwatch.StartNew();
                        meta.SwitchModel(baseRequest, modelName);
                        sw.Stop();
                        return Ok($"Successfully loaded model '{modelName}' into memory under {sw.Elapsed}secs");
                    }
                    return BadRequest("Cannot switch model. Make sure you spelled correctly. Hit the models endpoint to fetch a list of installed models");                 
                }
                else
                {
                    return BadRequest("Invalid API_KEY. Security validation failed. You cannot consume this service");
                }
            }
            catch (Exception)
            {
                return BadRequest("The system went into an error or cannot handle load. The service is temporarly haulted. Please try again or contact developer for support");
            }
        }


        [HttpGet]
        [Route("TextToImage")]
        public async Task<IActionResult> TextToImg([FromQuery] TextToImageUserRequest request)
        {
            try
            {
                if (IsAuthenticated())
                {
                    //Step 1: Generate image
                    textToImg.GenerateImage(baseRequest, request);

                    //Step 2: Grab image
                    var ext = new List<string> { "png" };
                    var targetFile = Directory
                        .EnumerateFiles(baseRequest.TextToImageSaveLoc, "*.*", SearchOption.AllDirectories)
                        .Where(s => ext.Contains(Path.GetExtension(s).TrimStart('.').ToLowerInvariant()))
                        .OrderByDescending(d => new FileInfo(d).LastWriteTime)
                        .FirstOrDefault();
                    if (targetFile is not null)
                    {
                        MemoryStream ms = new MemoryStream(System.IO.File.ReadAllBytes(targetFile));
                        return new FileStreamResult(ms, "image/png");
                    }
                }
                else
                {
                    return BadRequest("Invalid API_KEY. Security validation failed. You cannot consume this service");
                }
                return BadRequest("The system went into an error or cannot handle load. The service is temporarly haulted. Please try again or contact developer for support");
            }
            catch (Exception)
            {
                return BadRequest("The system went into an error or cannot handle load. The service is temporarly haulted. Please try again or contact developer for support");
            }
        }

        private bool IsAuthenticated()
        {
            var APIConfig = new KeyRepo("APICONFIG").GetAll().FirstOrDefault();
            if (APIConfig is null)
            {
                new KeyRepo("APICONFIG").Add(new APIKey
                {
                    API_KEY = "46A8D9F9-F000-4AB6-A989-89B10EC1CFB1"
                });
                APIConfig = new KeyRepo("APICONFIG").GetAll().FirstOrDefault();
            }
            var containsHeader = httpContextAccessor.HttpContext.Request.Headers.ContainsKey("API_KEY");
            if (containsHeader)
            {
                var headerValue = httpContextAccessor.HttpContext.Request.Headers["API_KEY"].ToString();
                if (headerValue == APIConfig.API_KEY)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
