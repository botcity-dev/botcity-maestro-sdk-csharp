
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System;

using BotCityMaestroSDK.Dtos.Task;
using BotCityMaestroSDK.Dtos;
using Newtonsoft.Json.Linq;


namespace BotCityMaestroSDK.Lib;

public partial class BotMaestroSDK
{


    public async Task<ResultTaskDTO> TaskCreate(string Token, string Organization, Activity Activity){
       
        InitializeClient();

        var content = ToContentTask(Token,Organization, Activity);

        await ToPostResponse(content, URIs_Task.TASK_POST_CREATE);

        return ToObject<ResultTaskDTO>();
        
    }

    public async Task<ResultTaskDTO> TaskSetState(string Token, string Organization, SendTaskStateDTO SendTaskStateDTO,  int? TaskId)
    {
        if (SendTaskStateDTO.FinishStatus == "")
            SendTaskStateDTO.FinishStatus = Enum.GetName(typeof(FinishedStatus), SendTaskStateDTO.SendStatus);

        var content = ToContentTask(Token, Organization, SendTaskStateDTO);

        await ToPostResponseURL(content, URL_ID( URIs_Task.TASK_POST_SET_STATE, TaskId.ToString()));

        return ToObject<ResultTaskDTO>();

    }

    public async Task<ResultTaskDTO> TaskGetState(string Token, string Organization, int? TaskId)
    {
     
        List<Param> list = new List<Param>();

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

        list.Add(paramToken);
        list.Add(paramOrg);
        InitializeClient(list);

        var result = await ToGetTaskResponseURL( URL_ID(URIs_Task.TASK_GETID, TaskId.ToString()));

        
        if (result == null)
        {
            ResultTaskDTO emptyTaskDTO = new ResultTaskDTO();
            return emptyTaskDTO;
        }
    
 
        return ToObject<ResultTaskDTO>();

    }





}
