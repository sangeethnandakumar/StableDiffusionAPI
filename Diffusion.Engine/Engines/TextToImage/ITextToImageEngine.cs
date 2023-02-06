using Diffusion.Models.SDModels;
using Diffusion.Models.UserRequests;

namespace Diffusion.Engine.Engines.TextToImage
{
    public interface ITextToImageEngine
    {
        void GenerateImage(BaseRequest baseRequest, TextToImageUserRequest request);
    }

}
