namespace Diffusion.Models.SDModels
{
    public class Data
    {
        public List<string> choices { get; set; }
        public string __type__ { get; set; }
    }

    public class BaseResponse
    {
        public List<Data> data { get; set; }
        public bool is_generating { get; set; }
        public double duration { get; set; }
        public double average_duration { get; set; }
    }

}
