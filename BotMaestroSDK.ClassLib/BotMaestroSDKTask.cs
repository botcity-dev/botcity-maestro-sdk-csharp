
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System;

using BotCityMaestroSDK.Dtos.Task;
using BotCityMaestroSDK.Dtos;
using Newtonsoft.Json.Linq;
using System.Diagnostics;


namespace BotCityMaestroSDK.Lib;

public partial class BotMaestroSDK
{
    public async Task<ResultTaskDTO> TaskCreate(BotCityMaestroSDK.Dtos.Task.Activity Activity){
       
        InitializeClient();

        SendTaskDTO sendTask = new SendTaskDTO
        {
            activityLabel = Activity.ActivityLabel,
            test = Activity.Test,
            Parameters = Activity.Parameters
        };

        var content = ToContentParamAndObj(sendTask);

        await ToPostResponse(content, ToStrUri(URIs_Task.TASK_POST_CREATE));

        return ToObject<ResultTaskDTO>();
        
    }

    public async Task<ResultTaskDTO> TaskSetState(SendTaskStateDTO SendTaskStateDTO,  int? TaskId)
    {
        if (SendTaskStateDTO.FinishStatus == "")
            SendTaskStateDTO.FinishStatus = Enum.GetName(typeof(FinishedStatus), SendTaskStateDTO.SendStatus);

        var content = ToContentParamAndObj( SendTaskStateDTO);

        await ToPostResponseURL(content, ToStrUri( URIs_Task.TASK_POST_SET_STATE, TaskId.ToString()));

        return ToObject<ResultTaskDTO>();

    }

    public async Task<ResultTaskDTO> TaskGetState(int? TaskId)
    {
        AddParamsToList();
        InitializeClient(ListParams);

        var result = await ToGetResponseURL(ToStrUri(URIs_Task.TASK_GETID, TaskId.ToString()));

        if (result == null)
        {
            ResultTaskDTO emptyTaskDTO = new ResultTaskDTO();
            return emptyTaskDTO;
        }
    
 
        return ToObject<ResultTaskDTO>();

    }





}
