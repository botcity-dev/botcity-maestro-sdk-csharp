
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
using System.Globalization;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;
using static System.Net.Mime.MediaTypeNames;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using System.Reflection.Metadata;
using Newtonsoft.Json.Converters;

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

    public async Task<string> ToPostSendFile(byte[] file, int idArtifact, string URI)
    {
       

        using (var content = new ByteArrayContent(file))
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

            foreach (Param param in ListParams)
            {
                content.Headers.Add(
                param.Name,
                    param.Value
                );
            }

            content.Headers.ContentType = new MediaTypeHeaderValue("*/*");
 
            //Send it
            var response = await BotMaestroSDK.ApiClient.PostAsync(URI, content);
            response.EnsureSuccessStatusCode();
            Stream responseStream = await response.Content.ReadAsStreamAsync();
            StreamReader reader = new StreamReader(responseStream);
            return reader.ReadToEnd();
        }


        
    }

    public async Task<bool> ToPostSendFile(string filePath, int idArtifact, string URI)
    {
        //StringContent content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
 



        using (ApiClient)
        {

            using (var multipartFormDataContent = new MultipartFormDataContent())
            {
                var values = new[]
                {
                    new KeyValuePair<string, string>("Id", Guid.NewGuid().ToString()),
                    new KeyValuePair<string, string>("Key", "awesome"),
                    new KeyValuePair<string, string>("From", "khalid@home.com")
                     //other values
                };

                /*
                foreach (var keyValuePair in values)
                {
                    multipartFormDataContent.Add(new StringContent(keyValuePair.Value),
                        String.Format("\"{0}\"", keyValuePair.Key));
                }
                */

                multipartFormDataContent.Add(new ByteArrayContent(System.IO.File.ReadAllBytes(filePath)),
                    '"' + "file" + '"',
                    '"' + "arqTeste" + '"');

                
                var result = ApiClient.PostAsync(URI, multipartFormDataContent).Result;

                var statusCode = result.StatusCode;

                if ((int)statusCode != 200)
                {
                    GetError.ErrorNumber = (int)statusCode;
                    GetError.Message = ResultRaw;
                    GetError.ErroDetail = result.ToString();
                    Console.WriteLine("ERROR:" + GetError.Message);
                    BotMaestroSDK.ApiClient = null;
                    //Console.WriteLine("ErroDetail:" + GetError.ErroDetail);
                    return false;
                }

                return true;

            }
        }


    }

    public async Task<bool> ToPostSendFile1(string filePath, int idArtifact, string URI)
    {
        
        try
        {
            using (var content = new MultipartFormDataContent())
            {
                //Content-Disposition: form-data; name="json"
                var item = "";
                StringContent stringContent = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                stringContent.Headers.Add("Content-Disposition", "form-data; name=\"json\"");
                content.Add(stringContent, "json");

                stringContent = AddParamsToHeader(stringContent);

                FileStream fs = System.IO.File.OpenRead(filePath);

                var streamContent = new StreamContent(fs);
                streamContent.Headers.Add("Content-Type", "application/octet-stream");
                streamContent.Headers.Add("Content-Disposition", "form-data; name=\"file\"; filename=\"" + Path.GetFileName(filePath) + "\"");
                content.Add(streamContent, "file", Path.GetFileName(filePath));

                //content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");

                Task<HttpResponseMessage> message = ApiClient.PostAsync(URI, content);

                //var result = ApiClient.PostAsync(URI, multipartFormDataContent).Result;

                var statusCode = message.Result.StatusCode;

                if ((int)statusCode != 200)
                {
                    GetError.ErrorNumber = (int)statusCode;
                    GetError.Message = ResultRaw;
                    GetError.ErroDetail = message.Result.ToString();
                    Console.WriteLine("ERROR:" + GetError.Message);
                    BotMaestroSDK.ApiClient = null;
                    //Console.WriteLine("ErroDetail:" + GetError.ErroDetail);
                    return false;
                }

               

                var input = message.Result.Content.ReadAsStringAsync();
                Console.WriteLine(input.Result);
                Console.Read();
                
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
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



    public T ToObject<T>()
    {

        //var resultRaw = ResponseMessage.Content.ReadAsStringAsync();
        if (typeof(ResultLoginDTO) == typeof(T))
        {
            ResultLoginDTO result1 = JsonConvert.DeserializeObject<ResultLoginDTO>(ResultRaw);
            //this.TokenLoginDTO = result1;
            this.Token = result1.Token;
            this.Organization = result1.Organizations.FirstOrDefault(x => x.Label != "").Label;
            return (T)Convert.ChangeType(result1, typeof(T));

        }

        if (typeof(ResultLoginStudioDTO) == typeof(T))
        {
            ResultLoginStudioDTO result1 = JsonConvert.DeserializeObject<ResultLoginStudioDTO>(ResultRaw);
            this.TokenLoginStudioDTO = result1;
            this.Token = result1.Access_Token;
            //this.Organization = result1.Organizations.FirstOrDefault(x => x.Label != "").Label;
            return (T)Convert.ChangeType(result1, typeof(T));

        }

        if (typeof(ResultLoginCliDTO) == typeof(T))
        {
            ResultLoginCliDTO result1 = JsonConvert.DeserializeObject<ResultLoginCliDTO>(ResultRaw);
            this.Token = result1.Access_Token;
            //this.Organization = result1.Organizations.FirstOrDefault(x => x.Label != "").Label;
            this.TokenLoginCliDTO = result1;
            return (T)Convert.ChangeType(result1, typeof(T));

        }
        
        if (typeof(ResultTaskDTO) == typeof(T))
        {
               ResultTaskDTO result1 = JsonConvert.DeserializeObject<ResultTaskDTO>(ResultRaw);
               //this.ResultTaskDTO = result1;
               return (T)Convert.ChangeType(result1, typeof(T));
        }

        if (typeof(ResultLogDTO) == typeof(T))
        {
            try
            {
                ResultLogDTO result1 = JsonConvert.DeserializeObject<ResultLogDTO>(ResultRaw);
                //this.ResultLogDTO = result1;
                return (T)Convert.ChangeType(result1, typeof(T));
            }
            catch (Exception e)
            {
                try
                {
                    List<ResultLogDTO> resultList = JsonConvert.DeserializeObject<List<ResultLogDTO>>(ResultRaw);
                    //this.ListLog = resultList;
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
            //this.ResultLogEntryDTO = result1;
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

    private StringContent AddParamsToHeader(StringContent content)
    {
        StringContent content1 = content;

        if (ListParams.Count<=0)
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
