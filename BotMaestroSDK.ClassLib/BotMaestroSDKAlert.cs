
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System;

using BotCityMaestroSDK.Dtos.Maestro;
using BotCityMaestroSDK.Dtos.Task;
using BotCityMaestroSDK.Dtos;
using System.Diagnostics.SymbolStore;
using Microsoft.AspNetCore.Http.Features;
using BotCityMaestroSDK.Dtos.Alert;

namespace BotCityMaestroSDK.Lib;

public partial class BotMaestroSDK
{

    public async Task<ResultAlert> AlertCreate(string Token, string Organization, SendAlert sendAlert)
    {
        InitializeClient();

        var content = ToContentParamAndObj(Token, Organization, sendAlert);

        var response = await ToPostResponse(content, URIs_Alert.ALERT_CREATE);

        return ToObject<ResultAlert>();

    }

}
