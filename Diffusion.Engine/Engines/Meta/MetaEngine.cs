using Diffusion.Models.SDModels;
using Newtonsoft.Json;
using System.Collections;

namespace Diffusion.Engine.Engines.Meta
{
    public class MetaEngine : IMetaEngine
    {
        public SDModels GetAllModels(BaseRequest baseRequest)
        {
            var apiRequest = new TextToImageAPIRequest
            {
                fn_index = 0,
                data = new ArrayList()
            };

            var json = JsonConvert.SerializeObject(apiRequest);
            var response = Commons.SendPOST(baseRequest, "run/predict", json);
            var baseResponse = JsonConvert.DeserializeObject<BaseResponse>(response);

            var sdModels = new SDModels();
            var models = new List<SDModel>();

            foreach(var modelName in baseResponse.data.FirstOrDefault().choices)
            {
                models.Add(new SDModel
                {
                    ModelFunctionIndex = 232,
                    ModelName = modelName
                });
            }
            
            return new SDModels { InstalledModels = models };
        }

        public void SwitchModel(BaseRequest baseRequest, string modelName)
        {
            var apiRequest = new TextToImageAPIRequest
            {
                fn_index = 232,
                data = new ArrayList()
                {
                    modelName
                }
            };

            var json = JsonConvert.SerializeObject(apiRequest);
            var response = Commons.SendPOST(baseRequest, "run/predict", json);
        }
    }
}
