namespace Diffusion.Models
{
    public class LoadRequest
    {
        public string Prompt { get; set; } = "Funny cat";
        public string Filename { get; set; } = "Funny cat";
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public Status Status { get; set; } = Status.AWAITING;
    }

    public enum Status
    {
        AWAITING,
        PROCESSING,
        DONE
    }
}
