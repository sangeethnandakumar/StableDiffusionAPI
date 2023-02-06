using Diffusion.Models.SDModels;
using RestSharp;

namespace Diffusion.Engine
{
    public static class Commons
    {
        public static string SendPOST(BaseRequest baseRequest, string endpoint, string jsonPayload)
        {
            var client = new RestClient(baseRequest.BaseURL + endpoint);
            var restRequest = new RestRequest();
            restRequest.AddStringBody(jsonPayload, DataFormat.Json);
            var response = client.Post(restRequest);
            return response.Content;
        }
    }
}
