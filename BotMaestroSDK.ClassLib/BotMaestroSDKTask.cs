
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System;

using BotCityMaestroSDK.Dtos.Task;
using BotCityMaestroSDK.Dtos;


namespace BotCityMaestroSDK.Lib;

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

        var content = ToContentTask(Token,Organization, Activity);

        await ToPostResponse(content, URIs_Task.TASK_POST);

        return ToObject<ResultTaskDTO>();
        
    }

    public async Task<ResultTaskDTO> TaskSetState(string Token, string Organization, SendTaskStateDTO SendTaskStateDTO,  int? TaskId)
    {
        if (SendTaskStateDTO.FinishStatus == "")
            SendTaskStateDTO.FinishStatus = Enum.GetName(typeof(FinishedStatus), SendTaskStateDTO.SendStatus);

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

        InitializeClient();

        var content = ToContentTask(Token, Organization, SendTaskStateDTO);

        await ToPostResponseURL(content, URIs_Task.TASK_POSTID +  TaskId.ToString());

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

        var result = await ToGetTaskResponseURL( URIs_Task.TASK_GETID  + TaskId.ToString());

        
        if (result == null)
        {
            ResultTaskDTO emptyTaskDTO = new ResultTaskDTO();
            return emptyTaskDTO;
        }
    
 
        return ToObject<ResultTaskDTO>();

    }





}
