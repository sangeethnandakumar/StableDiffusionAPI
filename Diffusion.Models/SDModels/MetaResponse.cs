namespace Diffusion.Models.SDModels
{
    public class SDModel
    {
        public int ModelFunctionIndex { get; set; }
        public string ModelName { get; set; }
    }

    public class SDModels
    {
        public List<SDModel> InstalledModels { get; set; }
    }
}
