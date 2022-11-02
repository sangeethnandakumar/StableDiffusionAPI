namespace Diffusion.Models
{
    public class TxtToImgResponse
    {
        public Guid ResourceId { get; set; } = Guid.NewGuid();
        public string Prompt { get; set; }
        public DateTime RequestedOnUTC { get; set; }
        public TimeSpan ProcessingTime { get; set; }
        public string StreamImageURL { get; set; }
        public string Base64Image { get; set; }
    }
}