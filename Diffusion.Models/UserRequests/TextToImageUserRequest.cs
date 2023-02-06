namespace Diffusion.Models.UserRequests
{
    public class TextToImageUserRequest
    {
        public string Prompt { get; set; }
        public string NegativePrompt { get; set; } = "";
        public int Width { get; set; } = 512;
        public int Height { get; set; } = 512;
        public int SamplingSteps { get; set; } = 20;
        public int CFGScale { get; set; } = 5;
        public int Seed { get; set; } = -1;
        public string Sampler { get; set; } = "Euler a";
    }
}
