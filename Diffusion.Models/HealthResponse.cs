namespace Diffusion.Models
{
    public class HealthResponse
    {
        public GPU GPU { get; set; } = new GPU();
    }

    public class GPU
    {
        public int Used { get; set; }
        public int Total { get; set; }
        public int[] TimeData { get; set; }
    }
}
