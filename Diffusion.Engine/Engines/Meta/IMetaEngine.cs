using Diffusion.Models.SDModels;

namespace Diffusion.Engine.Engines.Meta
{
    public interface IMetaEngine
    {
        SDModels GetAllModels(BaseRequest baseRequest);
        void SwitchModel(BaseRequest baseRequest,string modelName);
    }

}
