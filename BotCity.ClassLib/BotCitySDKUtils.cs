
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using BotCity.ClassLib.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace BotCity.ClassLib;

public partial class BotCitySDK
{

    private ByteArrayContent ToContent(string JsonString){

        var buffer = System.Text.Encoding.UTF8.GetBytes(JsonString);
        var byteContent = new ByteArrayContent(buffer);
        return byteContent;
    }

    private string ToStrUri(string JsonString){

        return URL_BOT_SERVER_API_HOTS + JsonString;
        
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
        var response = BotCitySDK.ApiClient.PostAsync(
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

        TokenLoginDTO result1 = JsonConvert.DeserializeObject<TokenLoginDTO>(ResultRaw);
        this.TokenLoginDTO = result1;

        var result = result1;
        return (T)Convert.ChangeType(result, typeof(T));
    }

    public T ToObject<T>()
    {

        //var resultRaw = ResponseMessage.Content.ReadAsStringAsync();
        if (typeof(TokenLoginDTO) == typeof(T))
        {
            TokenLoginDTO result1 = JsonConvert.DeserializeObject<TokenLoginDTO>(ResultRaw);
            this.TokenLoginDTO = result1;

            return (T)Convert.ChangeType(result1, typeof(T));

        }

        if (typeof(TokenLoginStudioDTO) == typeof(T))
        {
            TokenLoginStudioDTO result1 = JsonConvert.DeserializeObject<TokenLoginStudioDTO>(ResultRaw);
            this.TokenLoginStudioDTO = result1;

            return (T)Convert.ChangeType(result1, typeof(T));

        }

        if (typeof(TokenLoginCliDTO) == typeof(T))
        {
            TokenLoginCliDTO result1 = JsonConvert.DeserializeObject<TokenLoginCliDTO>(ResultRaw);
            this.TokenLoginCliDTO = result1;

            return (T)Convert.ChangeType(result1, typeof(T));

        }

        

        string resultNull = null;

        return (T)Convert.ChangeType(resultNull, typeof(T));
    }

}
