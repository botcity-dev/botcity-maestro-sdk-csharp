
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
using BotCityMaestroSDK.Dtos.Artefact;

using RestSharp;


namespace BotCityMaestroSDK.Lib;

public partial class BotMaestroSDK
{

    private ByteArrayContent ToContent(string JsonString)
    {
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

   


    public StringContent ToContentOnlyParams()
    {
        StringContent content = new StringContent(JsonConvert.SerializeObject(""), Encoding.UTF8, "application/json");

        content = AddParamsToHeader(content);

        return content;
    }

    public StringContent ToContentParamAndObj<T>(T item)
    {
        StringContent content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");

        content = AddParamsToHeader(content);

        return content;
    }

    public StringContent ToContentParamAndStr( string item)
    {
        StringContent content = new StringContent(item, Encoding.UTF8, "application/json");

        content = AddParamsToHeader(content);

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

    

    public async Task<bool> ToPostSendFile(string filePath, int idArtifact, string URI)
    {
        var client = new RestClient(URI);
   
        RestRequest request = new RestRequest();
        request.Method = Method.Post;

        foreach (Param param in ListParams)
        {
            request.AddHeader(
                param.Name,
                param.Value
            );
        }

        request.AddFile("file", filePath);
        var response = client.Execute(request);

        var statusCode = response.StatusCode;
        if ((int)statusCode != 200)
        {
            GetError.ErrorNumber = (int)statusCode;
            GetError.Message = ResultRaw;
            GetError.ErroDetail = response.ToString();
            BotMaestroSDK.ApiClient = null;
            return false;
        }

        return true;
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
            BotMaestroSDK.ApiClient = null;
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
        if ((int)statusCode != 200) return null;


        return response;
    }

    public T ToObject<T>()
    {

        if (typeof(ResultLoginDTO) == typeof(T))
        {
            ResultLoginDTO result1 = JsonConvert.DeserializeObject<ResultLoginDTO>(ResultRaw);
            this.Token = result1.Token;
            this.Organization = result1.Organizations.FirstOrDefault(x => x.Label != "").Label;
            return (T)Convert.ChangeType(result1, typeof(T));

        }

        if (typeof(ResultLoginStudioDTO) == typeof(T))
        {
            ResultLoginStudioDTO result1 = JsonConvert.DeserializeObject<ResultLoginStudioDTO>(ResultRaw);
            this.TokenLoginStudioDTO = result1;
            this.Token = result1.Access_Token;
            return (T)Convert.ChangeType(result1, typeof(T));
        }

        if (typeof(ResultLoginCliDTO) == typeof(T))
        {
            ResultLoginCliDTO result1 = JsonConvert.DeserializeObject<ResultLoginCliDTO>(ResultRaw);
            this.Token = result1.Access_Token;
            this.TokenLoginCliDTO = result1;
            return (T)Convert.ChangeType(result1, typeof(T));
        }
        
        if (typeof(ResultTaskDTO) == typeof(T))
        {
               ResultTaskDTO result1 = JsonConvert.DeserializeObject<ResultTaskDTO>(ResultRaw);
               return (T)Convert.ChangeType(result1, typeof(T));
        }

        if (typeof(ResultLogDTO) == typeof(T))
        {
            try
            {
                ResultLogDTO result1 = JsonConvert.DeserializeObject<ResultLogDTO>(ResultRaw);
                return (T)Convert.ChangeType(result1, typeof(T));
            }
            catch (Exception e)
            {
                try
                {
                    List<ResultLogDTO> resultList = JsonConvert.DeserializeObject<List<ResultLogDTO>>(ResultRaw);
                    return (T)Convert.ChangeType(resultList.FirstOrDefault(), typeof(T));
                }
                catch(Exception f)
                {

                }
            }
        }

        if (typeof(ResultLogEntryDTO) == typeof(T))
        {
            ResultLogEntryDTO result1 = JsonConvert.DeserializeObject<ResultLogEntryDTO>(ResultRaw);
            return (T)Convert.ChangeType(result1, typeof(T));
        }

        if (typeof(ResultAlert) == typeof(T))
        {
            ResultAlert result1 = JsonConvert.DeserializeObject<ResultAlert>(ResultRaw);
            return (T)Convert.ChangeType(result1, typeof(T)); 
        }

        if (typeof(Artefact) == typeof(T))
        {
            Artefact result1 = JsonConvert.DeserializeObject<Artefact>(ResultRaw);
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

    private void AddParamsToList()
    {
        if (ListParams.Count <=0)
        {
            if (ListParams.Count <= 0)
            {

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

                ListParams.Add(paramToken);
                ListParams.Add(paramOrg);
            }
        }
    }

    private StringContent AddParamsToHeader(StringContent content)
    {
        StringContent content1 = content;

        AddParamsToList();

        foreach (Param param in ListParams)
        {
            content.Headers.Add(
                param.Name,
                param.Value
            );
        }

        return content1;
    }


}
