
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

public partial class Maestro
{

    public async Task<ResultAlert> Alert(SendAlert sendAlert)
    {
        InitializeClient();

        var content = ToContentParamAndObj(sendAlert);

        var response = await ToPostResponse(content, ToStrUri(URIs_Alert.ALERT_CREATE));

        return ToObject<ResultAlert>();

    }

}
