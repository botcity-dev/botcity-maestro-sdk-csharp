
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

        var content = ToContentParamAndObj(Token, Organization, sendLogDTO);

        await ToPostResponse(content, URIs_Log.LOG_POST_CREATE);

        return ToObject<ResultLogDTO>();

    }

    public async Task<ResultLogDTO> LogById(string Token, string Organization, string idLog)
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

        await ToGetResponseURL( ToStrUri(URIs_Log.LOG_GET_ID,idLog));

        return ToObject<ResultLogDTO>();

    }

    public async Task<ResultLogEntryDTO> LogGetLog(string Token, string Organization,
                                              string idLog, List<Param> Queries, SendLogEntryDTO sendLogEntryDTO)
    {

        string Query = "?";

        foreach(Param param in Queries)
        {
            Query += param.Name + "=" + param.Value + "&";
        }
        
        InitializeClient();

        var content = ToContentParamAndObj(Token, Organization, sendLogEntryDTO);

        await ToGetResponseURL(ToStrUri(URIs_Log.LOG_GET_ID_ENTRY, idLog) + Query);

        return ToObject<ResultLogEntryDTO>();

    }

    public async Task<ResultLogEntryDTO> LogGetLog(string Token, string Organization,
                                              string idLog, List<Param> Queries)
    {

        string Query = "?";

        foreach (Param param in Queries)
        {
            Query += param.Name + "=" + param.Value + "&";
        }

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

        await ToGetResponseURL(ToStrUri(URIs_Log.LOG_GET_ID_ENTRY, idLog) + Query);

        return ToObject<ResultLogEntryDTO>();

    }


}
