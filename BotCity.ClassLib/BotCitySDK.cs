
using System.Net.Http;
using System.Net.Http.Headers;

using BotCity.ClassLib.Dtos;

namespace BotCity.ClassLib;

public partial class BotCitySDK
{
 
    public static HttpClient ApiClient { get; set; }

    private string ResultRaw { get; set; }
    public HttpResponseMessage ResponseMessage { get; set; }
    public TokenLoginDTO TokenLoginDTO { get; set; }
    public TokenLoginStudioDTO TokenLoginStudioDTO {get; set;}
    public TokenLoginCliDTO TokenLoginCliDTO { get; set; }
    public static string URL_BOT_SERVER_API_HOTS { get; set; }
    public static void InitializeClient(){

        ApiClient = new HttpClient();
        ApiClient.BaseAddress = new Uri(URL_BOT_SERVER_API_HOTS);
        ApiClient.DefaultRequestHeaders.Accept.Clear();
        ApiClient.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
        );
  

    }

    public BotCitySDK(string url){
        URL_BOT_SERVER_API_HOTS = url;
        InitializeClient();
    }


}
