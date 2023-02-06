using System.Collections;

namespace Diffusion.Models.SDModels
{
    public class TextToImageAPIRequest
    {
        public string session_hash { get; set; }
        public ArrayList data { get; set; }
        public int fn_index { get; set; }
    }
}
