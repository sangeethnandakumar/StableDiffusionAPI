using diffuisiondashboard.Repository;
using Diffusion.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace diffuisiondashboard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiffusionController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly IHttpContextAccessor httpContextAccessor;

        public DiffusionController(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
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
        public async Task<IActionResult> GetImg([FromQuery] string prompt, int width = 512, int height = 512)
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
            var headerValue = httpContextAccessor.HttpContext.Request.Headers["API_KEY"].ToString();
            if (headerValue is not null && headerValue == APIConfig.API_KEY)
            {
                try
                {
                    var url = "http://127.0.0.1:7860/api/predict/";
                    var payloadBody = "{\"fn_index\":50,\"data\":[\"" + prompt + "\",\"\",\"None\",\"None\",20,\"Euler a\",false,false,1,1,7,-1,-1,0,0,0,false," + width + "," + height + ",false,0.7,0,0,\"None\",false,false,null,\"\",\"Seed\",\"\",\"Nothing\",\"\",true,false,false,null,\"\",\"\"],\"session_hash\":\"lnaw6m1l4wo\"}";
                    var client = new RestClient(url);
                    var request = new RestRequest();
                    request.AddStringBody(payloadBody, DataFormat.Json);
                    var response = client.Post<string>(request);
                }
                catch (Exception)
                {
                }
                //Check if any image generated with that prompt
                //C:\Users\instaread_summaries\Documents\stable-diffusion-webui-master\outputs\txt2img-images
                var ext = new List<string> { "png" };
                var myFiles = Directory
                    .EnumerateFiles(@"C:\Users\instaread_summaries\Documents\stable-diffusion-webui-master\outputs\txt2img-images", "*.*", SearchOption.AllDirectories)
                    .Where(s => ext.Contains(Path.GetExtension(s).TrimStart('.').ToLowerInvariant()));
                var targetFile = myFiles.Where(x => x.ToLower().Contains(prompt.ToLower()));
                if (targetFile.Any())
                {
                    MemoryStream ms = new MemoryStream(System.IO.File.ReadAllBytes(targetFile.FirstOrDefault()));
                    return new FileStreamResult(ms, "image/png");
                }
                return BadRequest("Unable to generate image. The GPU has crashed (Out of memory) during generation of image. This can be caused when generating higher resolution images or too freequent generations. Please contact support to reset GPU.");
            }
            return BadRequest("You're not authorized to access this API.");
        }
    }
}
