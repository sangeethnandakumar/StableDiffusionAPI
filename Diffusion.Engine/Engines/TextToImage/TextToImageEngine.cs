using Diffusion.Models.SDModels;
using Diffusion.Models.UserRequests;
using Newtonsoft.Json;
using System.Collections;

namespace Diffusion.Engine.Engines.TextToImage
{

    public class TextToImageEngine : ITextToImageEngine
    {
        public void GenerateImage(BaseRequest baseRequest, TextToImageUserRequest request)
        {
            var session = Guid.NewGuid().ToString();

            var apiRequest = new TextToImageAPIRequest
            {
                session_hash = session,
                fn_index = 85,
                data = new ArrayList
                {
                     "task(gnn6h7mmet875q4)",
                     request.Prompt + ", " + Guid.NewGuid(),
                     request.NegativePrompt,
                     new string[0],
                     request.SamplingSteps,
                     request.Sampler,
                     false,
                     false,
                     1,
                     1,
                     request.CFGScale,
                     request.Seed,
                     -1,
                     0,
                     0,
                     0,
                     false,
                     request.Height,
                     request.Width,
                     false,
                     0.7,
                     2,
                     "Latent",
                     0,
                     0,
                     0,
                     new string[0],
                     "None",
                     false,
                     false,
                     "positive",
                     "comma",
                     0,
                     false,
                     false,
                     "",
                     "Seed",
                     "",
                     "Nothing",
                     "",
                     "Nothing",
                     "",
                     true,
                     false,
                     false,
                     false,
                     0
                }
            };

            var json = JsonConvert.SerializeObject(apiRequest);

            Commons.SendPOST(baseRequest, "run/predict", json);
        }


    }
}
