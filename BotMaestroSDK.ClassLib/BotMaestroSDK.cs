
using System.Net.Http;
using System.Net.Http.Headers;
using BotCityMaestroSDK.Dtos;
using BotCityMaestroSDK.Dtos.Login;
using BotCityMaestroSDK.Dtos.Maestro;
using BotCityMaestroSDK.Dtos.Task;

namespace BotCityMaestroSDK.Lib;

public partial class BotMaestroSDK
{
 
    public static HttpClient ApiClient { get; set; }

    private string ResultRaw { get; set; }

    public string Token { get; set; }
    public string Organization { get; set; }
    public HttpResponseMessage ResponseMessage { get; set; }
    public static GetError GetError { get; set; }
    public ResultLoginDTO TokenLoginDTO { get; set; }
    public ResultLoginStudioDTO TokenLoginStudioDTO {get; set;}
    public ResultLoginCliDTO TokenLoginCliDTO { get; set; }
    public TokenMaestroVersion TokenMaestroVersion { get; set; }
    public ResultTaskDTO ResultTaskDTO { get; set; }
    public ResultLogDTO ResultLogDTO { get; set; }
    public List<ResultLogDTO> ListLog { get; set; }
    public ResultLogEntryDTO ResultLogEntryDTO { get; set; }
    public List<Activity> Activities {get; set;}
    public static string URL_BOT_SERVER_API_HOTS { get; set; }
    public static void InitializeClient(){

        ApiClient = new HttpClient();
        ApiClient.BaseAddress = new Uri(URL_BOT_SERVER_API_HOTS);
        ApiClient.DefaultRequestHeaders.Accept.Clear();
        ApiClient.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
        );

        GetError = new GetError();
  

    }

    public static void InitializeClient(List<Param> ListParams){

        ApiClient = new HttpClient();
        ApiClient.BaseAddress = new Uri(URL_BOT_SERVER_API_HOTS);
        ApiClient.DefaultRequestHeaders.Accept.Clear();
        ApiClient.DefaultRequestHeaders.Accept.Add(
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
        );
        
        foreach (Param param in ListParams){
            ApiClient.DefaultRequestHeaders.Add(
                param.Name,
                param.Value
            );
        }

        
  

    }

    public BotMaestroSDK(string url){
        URL_BOT_SERVER_API_HOTS = url;
        Activities = new List<Activity>();
        InitializeClient();
    }

    public void ActivitiesClear(){
        Activities = new List<Activity>();
    }


}
