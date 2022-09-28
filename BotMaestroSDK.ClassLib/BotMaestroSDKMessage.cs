
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
using BotCityMaestroSDK.Dtos.Message;

namespace BotCityMaestroSDK.Lib;

public partial class BotMaestroSDK
{

    public async Task<bool> MessageCreate(SendMessage sendMessage)
    {
        InitializeClient();

        var content = ToContentParamAndObj(sendMessage);

        var response = await ToPostResponse(content, ToStrUri(URIs_Message.MESSAGE_CREATE));

        if (response == null) return false;

        var statusCode = response.StatusCode;

        if ((int)statusCode != 200) return false;

        return true;

    }

}
