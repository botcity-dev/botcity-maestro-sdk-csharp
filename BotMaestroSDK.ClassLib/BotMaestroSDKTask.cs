
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System;
using NsBotCityMaestroSDK.ClassLib.Dtos.Task;
using NsBotMaestroSDK.ClassLib.Dtos.Task;



namespace NsBotMaestroSDK.ClassLib;

public partial class BotMaestroSDK
{


    public async Task<ResultTaskDTO> Task(string Token, string Organization, Activity Activity){

        List<Param> list = new List<Param>();
        
        var paramToken = new Param{
            Name = "token",
            Value = Token
        };

        var paramOrg = new Param{
            Name = "organization",
            Value = Organization
        };

        list.Add(paramToken);
        list.Add(paramOrg);

        InitializeClient();

        var content = ToContent(Token,Organization, Activity);

        await ToPostResponse(content, URIs_Task.TASK_POST);

        return ToObject<ResultTaskDTO>();
        
    }

  

}
