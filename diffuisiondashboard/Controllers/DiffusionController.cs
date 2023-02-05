using diffuisiondashboard.Repository;
using Diffusion.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.IO;

namespace diffuisiondashboard.Controllers
{

    public class ModelParameters
    {
        public string Prompt { get; set; }
        public string NegativePrompt { get; set; } = "";
        public int Width { get; set; } = 512;
        public int Height { get; set; } = 512;
        public int SamplingSteps { get; set; } = 20;
        public int CFGScale { get; set; } = 5;
        public int Seed { get; set; } = -1;
        public string Sampler { get; set; } = "Euler a";
        public string SessionHash { get; set; } = "niukm1hmeup";
        public string RootFolder { get; set; } = AppContext.BaseDirectory;
        public Guid FileGuid { get; set; } = Guid.NewGuid();
    }

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
        public async Task<IActionResult> GetImg([FromQuery] ModelParameters parameters)
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


                    var payloadBody = $@"{{
                      ""fn_index"": 85,
                      ""data"": [
                        ""task(gnn6h7mmet875q4)"",
                        ""{parameters.Prompt}{", " + parameters.FileGuid}"",
                        ""{parameters.NegativePrompt}{", " + parameters.FileGuid}"",
                        [],
                        {parameters.SamplingSteps},
                        ""{parameters.Sampler}"",
                        false,
                        false,
                        1,
                        1,
                        {parameters.CFGScale},
                        {parameters.Seed},
                        -1,
                        0,
                        0,
                        0,
                        false,
                        {parameters.Height},
                        {parameters.Width},
                        false,
                        0.7,
                        2,
                        ""Latent"",
                        0,
                        0,
                        0,
                        [],
                        ""None"",
                        false,
                        false,
                        ""positive"",
                        ""comma"",
                        0,
                        false,
                        false,
                        """",
                        ""Seed"",
                        """",
                        ""Nothing"",
                        """",
                        ""Nothing"",
                        """",
                        true,
                        false,
                        false,
                        false,
                        0,
                        [
                          {{
                            ""name"": ""{parameters.RootFolder.Replace("\\", "\\\\")}{parameters.FileGuid}.png"",
                            ""data"": ""file={parameters.RootFolder.Replace("\\", "\\\\")}{parameters.FileGuid}.png"",
                            ""is_file"": true
                          }}
                        ],
                        ""{{\""prompt\"": \""a young woman, street smiling, backpack, ponytails, epic realistic, photo, faded, complex stuff around, intricate background, soaking wet, neutral colors, ((((hdr)))), ((((muted colors)))), intricate scene, artstation, intricate details, vignette\"", \""all_prompts\"": [\""a young woman, street smiling, backpack, ponytails, epic realistic, photo, faded, complex stuff around, intricate background, soaking wet, neutral colors, ((((hdr)))), ((((muted colors)))), intricate scene, artstation, intricate details, vignette\""], \""negative_prompt\"": \""deformed, bad anatomy, disfigured, poorly drawn face, mutation, mutated, extra limb, ugly, disgusting, poorly drawn hands, missing limb, floating limbs, disconnected limbs, malformed hands, blurry, ((((mutated hands and fingers)))), watermark, watermarked, oversaturated, censored, distorted hands, amputation, missing hands, obese, doubled face, double hands\"", \""all_negative_prompts\"": [\""deformed, bad anatomy, disfigured, poorly drawn face, mutation, mutated, extra limb, ugly, disgusting, poorly drawn hands, missing limb, floating limbs, disconnected limbs, malformed hands, blurry, ((((mutated hands and fingers)))), watermark, watermarked, oversaturated, censored, distorted hands, amputation, missing hands, obese, doubled face, double hands\""], \""seed\"": 737670053, \""all_seeds\"": [737670053], \""subseed\"": 473922482, \""all_subseeds\"": [473922482], \""subseed_strength\"": 0, \""width\"": 1080, \""height\"": 720, \""sampler_name\"": \""Euler a\"", \""cfg_scale\"": 5, \""steps\"": 28, \""batch_size\"": 1, \""restore_faces\"": false, \""face_restoration_model\"": null, \""sd_model_hash\"": \""1254103966\"", \""seed_resize_from_w\"": 0, \""seed_resize_from_h\"": 0, \""denoising_strength\"": null, \""extra_generation_params\"": {{}}, \""index_of_first_image\"": 0, \""infotexts\"": [\""a young woman, street smiling, backpack, ponytails, epic realistic, photo, faded, complex stuff around, intricate background, soaking wet, neutral colors, ((((hdr)))), ((((muted colors)))), intricate scene, artstation, intricate details, vignette\\nNegative prompt: deformed, bad anatomy, disfigured, poorly drawn face, mutation, mutated, extra limb, ugly, disgusting, poorly drawn hands, missing limb, floating limbs, disconnected limbs, malformed hands, blurry, ((((mutated hands and fingers)))), watermark, watermarked, oversaturated, censored, distorted hands, amputation, missing hands, obese, doubled face, double hands\\nSteps: 28, Sampler: Euler a, CFG scale: 5, Seed: 737670053, Size: 1080x720, Model hash: 1254103966, Model: protogenV22Anime_22\""], \""styles\"": [], \""job_timestamp\"": \""20230205235127\"", \""clip_skip\"": 1, \""is_using_inpainting_conditioning\"": false}}"",
                        ""<p>a young woman, street smiling, backpack, ponytails, epic realistic, photo, faded, complex stuff around, intricate background, soaking wet, neutral colors, ((((hdr)))), ((((muted colors)))), intricate scene, artstation, intricate details, vignette<br>\nNegative prompt: deformed, bad anatomy, disfigured, poorly drawn face, mutation, mutated, extra limb, ugly, disgusting, poorly drawn hands, missing limb, floating limbs, disconnected limbs, malformed hands, blurry, ((((mutated hands and fingers)))), watermark, watermarked, oversaturated, censored, distorted hands, amputation, missing hands, obese, doubled face, double hands<br>\nSteps: 28, Sampler: Euler a, CFG scale: 5, Seed: 737670053, Size: 1080x720, Model hash: 1254103966, Model: protogenV22Anime_22</p>"",
                        ""<p></p><div class='performance'><p class='time'>Time taken: <wbr>32.10s</p><p class='vram'>Torch active/reserved: 1857/2280 MiB, <wbr>Sys VRAM: 4070/4096 MiB (99.37%)</p></div>""
                      ],
                      ""session_hash"": ""{parameters.SessionHash}""
                    }}";

                    var client = new RestClient(url);
                    var request = new RestRequest();
                    request.AddStringBody(payloadBody, DataFormat.Json);
                    var response = client.Post(request);
                }
                catch (Exception)
                {
                }
                //Check if any image generated with that prompt
                //C:\Users\instaread_summaries\Documents\stable-diffusion-webui-master\outputs\txt2img-images
                var ext = new List<string> { "png" };
                var targetFile = Directory
                    .EnumerateFiles(@"D:\SD v2\UI\stable-diffusion-webui\outputs\txt2img-images", "*.*", SearchOption.AllDirectories)
                    .Where(s => ext.Contains(Path.GetExtension(s).TrimStart('.').ToLowerInvariant()))
                    .OrderByDescending(d => new FileInfo(d).LastWriteTime)
                    .FirstOrDefault();
                if (targetFile is not null)
                {
                    MemoryStream ms = new MemoryStream(System.IO.File.ReadAllBytes(targetFile));
                    return new FileStreamResult(ms, "image/png");
                }
                return BadRequest("Unable to generate image. The GPU has crashed (Out of memory) during generation of image. This can be caused when generating higher resolution images or too freequent generations. Please contact support to reset GPU.");
            }
            return BadRequest("You're not authorized to access this API.");
        }
    }
}
