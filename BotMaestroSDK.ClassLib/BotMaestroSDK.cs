
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using BotCityMaestroSDK.Dtos;
using BotCityMaestroSDK.Dtos.Login;
using BotCityMaestroSDK.Dtos.Maestro;
using BotCityMaestroSDK.Dtos.Task;
using static System.Net.Mime.MediaTypeNames;

namespace BotCityMaestroSDK.Lib;

public partial class Maestro
{
    public static HttpClient ApiClient { get; set; }
    private string ResultRaw { get; set; }
    public string Token { get; set; }
    public string Organization { get; set; }
    public List<Param> ListParams { get; set; }
    public HttpResponseMessage ResponseMessage { get; set; }
    public static GetError GetError { get; set; }
    public ResultLoginStudioDTO TokenLoginStudioDTO {get; set;}
    public ResultLoginCliDTO TokenLoginCliDTO { get; set; }
    public static List<ResultLogDTO> ListLog { get; set; }

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

    public Maestro(string url){
        URL_BOT_SERVER_API_HOTS = url;
        //Activities = new List<Activity>();
        ListLog = new List<ResultLogDTO>();
        ListParams = new List<Param>();
        InitializeClient();
    }

}
