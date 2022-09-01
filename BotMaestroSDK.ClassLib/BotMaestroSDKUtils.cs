
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NsBotCityMaestroSDK.ClassLib.Dtos.Login;
using NsBotCityMaestroSDK.ClassLib.Dtos.Task;
using NsBotMaestroSDK.ClassLib.Dtos.Task;

namespace NsBotMaestroSDK.ClassLib;

public partial class BotMaestroSDK
{

    private ByteArrayContent ToContent(string JsonString){

        var buffer = System.Text.Encoding.UTF8.GetBytes(JsonString);
        var byteContent = new ByteArrayContent(buffer);
        return byteContent;
    }

    private ByteArrayContent ToContent()
    {

        var buffer = System.Text.Encoding.UTF8.GetBytes("");
        var byteContent = new ByteArrayContent(buffer);
        return byteContent;
    }

    private string ToStrUri(string JsonString){

        return URL_BOT_SERVER_API_HOTS + JsonString;
        
    }

   
    public StringContent ToContent(string Token, string Organization, Activity activity ){

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

    public StringContent ToContent<T>(string userName, string pwd)
    {

        StringContent content = new StringContent("");

        if (typeof(SendLoginDTO) == typeof(T))
        {
            SendLoginDTO login = new SendLoginDTO();
            login.username = userName;
            login.password = pwd;

            var content1 = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
            content = content1;

        }

        if (typeof(SendLoginStudioDTO) == typeof(T))
        {
            SendLoginStudioDTO login = new SendLoginStudioDTO();
            login.email = userName;
            login.password = pwd;

            var content1 = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
            content = content1;

        }

        if (typeof(SendLoginCliDTO) == typeof(T))
        {
            SendLoginCliDTO login = new SendLoginCliDTO();
            login.email = userName;
            login.password = pwd;

            var content1 = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
            content = content1;

        }


        return content;

    }

    public async Task<HttpResponseMessage> ToPostResponse(StringContent content, string URI)
    {
        var response = BotMaestroSDK.ApiClient.PostAsync(
                ToStrUri(URI),
                content).Result;

        ResponseMessage = response;
        //Console.WriteLine("response:" + response);
        var statusCode = response.StatusCode;
        ResultRaw = await response.Content.ReadAsStringAsync();
        if ((int)statusCode != 200) return null;


        return response;

    }

    public T ToObject1<T>()
    {

        //var resultRaw = ResponseMessage.Content.ReadAsStringAsync();

        ResultLoginDTO result1 = JsonConvert.DeserializeObject<ResultLoginDTO>(ResultRaw);
        this.TokenLoginDTO = result1;

        var result = result1;
        return (T)Convert.ChangeType(result, typeof(T));
    }

    public T ToObject<T>()
    {

        //var resultRaw = ResponseMessage.Content.ReadAsStringAsync();
        if (typeof(ResultLoginDTO) == typeof(T))
        {
            ResultLoginDTO result1 = JsonConvert.DeserializeObject<ResultLoginDTO>(ResultRaw);
            this.TokenLoginDTO = result1;

            return (T)Convert.ChangeType(result1, typeof(T));

        }

        if (typeof(ResultLoginStudioDTO) == typeof(T))
        {
            ResultLoginStudioDTO result1 = JsonConvert.DeserializeObject<ResultLoginStudioDTO>(ResultRaw);
            this.TokenLoginStudioDTO = result1;

            return (T)Convert.ChangeType(result1, typeof(T));

        }

        if (typeof(ResultLoginCliDTO) == typeof(T))
        {
            ResultLoginCliDTO result1 = JsonConvert.DeserializeObject<ResultLoginCliDTO>(ResultRaw);
            this.TokenLoginCliDTO = result1;

            return (T)Convert.ChangeType(result1, typeof(T));

        }
        
        if (typeof(ResultTaskDTO) == typeof(T))
        {

            Console.WriteLine(ResultRaw);
            ResultTaskDTO result1 = JsonConvert.DeserializeObject<ResultTaskDTO>(ResultRaw);
            this.ResultTaskDTO = result1;

            return (T)Convert.ChangeType(result1, typeof(T));

        }

        string resultNull = null;

        return (T)Convert.ChangeType(resultNull, typeof(T));
    }



}
