namespace Diffusion.Models
{
    public class TextToImageRequest
    {
        public int fn_index { get; set; }
        public string task { get; set; }
        public string prompt { get; set; }
        public string negative_prompt { get; set; }
        public int[] emptyArray { get; set; }
        public int steps { get; set; }
        public string sampler_name { get; set; }
        public bool restore_faces { get; set; }
        public bool is_using_inpainting_conditioning { get; set; }
        public int batch_size { get; set; }
        public int cfg_scale { get; set; }
        public int seed_resize_from_w { get; set; }
        public int seed_resize_from_h { get; set; }
        public int denoising_strength { get; set; }
        public bool extra_generation_params { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public bool is_file { get; set; }
        public float clip_skip { get; set; }
        public int subseed_strength { get; set; }
        public string subseed { get; set; }
        public int latentspace_type { get; set; }
        public int index_of_first_image { get; set; }
        public string infotexts { get; set; }
        public string styles { get; set; }
        public string job_timestamp { get; set; }
        public bool seed { get; set; }
        public string sd_model_hash { get; set; }
        public string face_restoration_model { get; set; }
        public string nothing1 { get; set; }
        public string nothing2 { get; set; }
        public string nothing3 { get; set; }
        public bool nothing4 { get; set; }
        public bool nothing5 { get; set; }
        public bool nothing6 { get; set; }
        public bool nothing7 { get; set; }
        public List<Dictionary<string, object>> outputs { get; set; }
    }
}
