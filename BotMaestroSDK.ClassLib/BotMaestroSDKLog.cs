
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System;

using BotCityMaestroSDK.Dtos.Maestro;
using BotCityMaestroSDK.Dtos.Task;
using BotCityMaestroSDK.Dtos;

namespace BotCityMaestroSDK.Lib;

public partial class BotMaestroSDK
{


    public async Task<ResultLogDTO> LogCreate(string Token, string Organization, SendLogDTO sendLogDTO)
    {

        InitializeClient();

        var content = ToContentLog(Token, Organization, sendLogDTO);

        await ToPostResponse(content, URIs_Log.LOG_POST_CREATE);

        return ToObject<ResultLogDTO>();

    }


}
