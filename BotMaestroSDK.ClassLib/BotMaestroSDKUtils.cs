
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using BotCityMaestroSDK.Dtos.Login;
using BotCityMaestroSDK.Dtos.Task;
using BotCityMaestroSDK.Dtos;
using System.Reflection;
using System;
using System.Net.Mime;
using Microsoft.AspNetCore.Hosting.Internal;
using BotCityMaestroSDK.Dtos.Alert;

namespace BotCityMaestroSDK.Lib;

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

   


    public StringContent ToContentOnlyParams(string Token, string Organization)
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

    public StringContent ToContentParamAndObj<T>(string Token, string Organization, T item)
    {
     
        StringContent content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");

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

    public StringContent ToContentParamAndStr(string Token, string Organization, string item)
    {

        StringContent content = new StringContent(item, Encoding.UTF8, "application/json");

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

    public StringContent ToContentLoginObj<T>(string userName, string pwd)
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
                URI,
                content).Result;

        ResponseMessage = response;
        
        var statusCode = response.StatusCode;
        ResultRaw = await response.Content.ReadAsStringAsync();

        

        

        if ((int)statusCode != 200)
        {
            GetError.ErrorNumber = (int)statusCode;
            GetError.Message = ResultRaw;
            GetError.ErroDetail = response.ToString();
            Console.WriteLine("ERROR:" + GetError.Message);
            BotMaestroSDK.ApiClient = null;
            //Console.WriteLine("ErroDetail:" + GetError.ErroDetail);
            return null;
        }
        return response;

    }

    public async Task<HttpResponseMessage> ToPostResponseURL(StringContent content, string URI )
    {
        var response = BotMaestroSDK.ApiClient.PostAsync(
                URI,
                content).Result;

        ResponseMessage = response;
        //Console.WriteLine("response:" + response);
        var statusCode = response.StatusCode;
        ResultRaw = await response.Content.ReadAsStringAsync();
        if ((int)statusCode != 200) return null;

        return response;

    }

    public async Task<HttpResponseMessage> ToGetResponseURL(string URI)
    {

        var response = BotMaestroSDK.ApiClient.GetAsync(
                URI).Result;

        ResponseMessage = response;
       
        var statusCode = response.StatusCode;
        ResultRaw = await response.Content.ReadAsStringAsync();
        Console.WriteLine("ResultRaw:" + ResultRaw);
        if ((int)statusCode != 200) return null;


        return response;

    }


    public async Task<HttpResponseMessage> ToGetResponseFile(string URI, string filename)
    {


        var response = await BotMaestroSDK.ApiClient.GetAsync(URI);

        if (response.IsSuccessStatusCode)
        {
            using (var fs = new FileStream(filename, FileMode.Create))
            {
                await response.Content.CopyToAsync(fs);
            }
        }
        else
        {
            throw new FileNotFoundException();
        }

        ResponseMessage = response;

        var statusCode = response.StatusCode;
        ResultRaw = await response.Content.ReadAsStringAsync();
        Console.WriteLine("ResultRaw:" + ResultRaw);
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
               ResultTaskDTO result1 = JsonConvert.DeserializeObject<ResultTaskDTO>(ResultRaw);
               this.ResultTaskDTO = result1;
               return (T)Convert.ChangeType(result1, typeof(T));
        }

        if (typeof(ResultLogDTO) == typeof(T))
        {
            ResultLogDTO result1 = JsonConvert.DeserializeObject<ResultLogDTO>(ResultRaw);
            this.ResultLogDTO = result1;
            return (T)Convert.ChangeType(result1, typeof(T));
        }

        if (typeof(ResultLogEntryDTO) == typeof(T))
        {
            ResultLogEntryDTO result1 = JsonConvert.DeserializeObject<ResultLogEntryDTO>(ResultRaw);
            this.ResultLogEntryDTO = result1;
            return (T)Convert.ChangeType(result1, typeof(T));
        }

        if (typeof(ResultAlert) == typeof(T))
        {
            ResultAlert result1 = JsonConvert.DeserializeObject<ResultAlert>(ResultRaw);
            return (T)Convert.ChangeType(result1, typeof(T)); 
        }

        string resultNull = null;

        return (T)Convert.ChangeType(resultNull, typeof(T));
    }

    private string ToStrUri(string JsonString, string id = "")
    {

        var result = JsonString.Replace("{id}", id);

        return URL_BOT_SERVER_API_HOTS + result;

    }


}
