namespace Diffusion.Models
{
    public class HealthInfo
    {
        public GPUMetrix GPU { get; set; }
    }

    public class GPUMetrix
    {
        public string Used { get; set; }
        public string Total { get; set; }
    }
}
