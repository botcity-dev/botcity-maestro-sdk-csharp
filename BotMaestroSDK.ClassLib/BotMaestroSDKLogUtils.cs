
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using BotCityMaestroSDK.Dtos.Login;
using BotCityMaestroSDK.Dtos.Task;
using BotCityMaestroSDK.Dtos;

namespace BotCityMaestroSDK.Lib;

public partial class BotMaestroSDK
{
    

    /*
   public StringContent ToContentLog(string Token, string Organization, SendLogDTO sendLogDTO)
   {

       StringContent content = new StringContent(JsonConvert.SerializeObject(sendLogDTO), Encoding.UTF8, "application/json");

       List<Param> listHeaderParams = new List<Param>();

       var paramToken = new Param
       {
           Name = "token",
           Value = Token
       };

       var paramOrg = new Param
       {
           Name = "organization",
           Value = Organization
       };

       listHeaderParams.Add(paramToken);
       listHeaderParams.Add(paramOrg);

       foreach (Param param in listHeaderParams)
       {
           content.Headers.Add(
               param.Name,
               param.Value
           );
       }

       return content;
   }

   public StringContent ToContentTask(string Token, string Organization, Activity activity)
   {

       SendTaskDTO sendTask = new SendTaskDTO
       {
           activityLabel = activity.ActivityLabel,
           test = activity.Test,
           Parameters = activity.Parameters
       };


       StringContent content = new StringContent(JsonConvert.SerializeObject(sendTask), Encoding.UTF8, "application/json");

       List<Param> listHeaderParams = new List<Param>();

       var paramToken = new Param
       {
           Name = "token",
           Value = Token
       };

       var paramOrg = new Param
       {
           Name = "organization",
           Value = Organization
       };

       listHeaderParams.Add(paramToken);
       listHeaderParams.Add(paramOrg);

       foreach (Param param in listHeaderParams)
       {
           content.Headers.Add(
               param.Name,
               param.Value
           );
       }

       return content;
   }


   public StringContent ToContentTask(string Token, string Organization)
   {

       StringContent content = new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "application/json");

       List<Param> listHeaderParams = new List<Param>();

       var paramToken = new Param
       {
           Name = "token",
           Value = Token
       };

       var paramOrg = new Param
       {
           Name = "organization",
           Value = Organization
       };

       listHeaderParams.Add(paramToken);
       listHeaderParams.Add(paramOrg);

       foreach (Param param in listHeaderParams)
       {
           content.Headers.Add(
               param.Name,
               param.Value
           );
       }

       return content;
   }


   public async Task<HttpResponseMessage> ToGetTaskResponseURL( string URI)
   {
       var b = ToStrUri(URI);

       var response = BotMaestroSDK.ApiClient.GetAsync(
               ToStrUri(URI)).Result;

       ResponseMessage = response;
       //Console.WriteLine("response:" + response);
       var statusCode = response.StatusCode;
       ResultRaw = await response.Content.ReadAsStringAsync();
       if ((int)statusCode != 200) return null;


       return response;

   }
   */


}
