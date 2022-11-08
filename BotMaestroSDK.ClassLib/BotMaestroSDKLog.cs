
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

namespace BotCityMaestroSDK.Lib;

public partial class BotMaestroSDK
{


    public async Task<ResultLogDTO> LogCreate(SendLogDTO sendLogDTO)
    {

        InitializeClient();

        var content = ToContentParamAndObj(sendLogDTO);

        await ToPostResponse(content, URIs_Log.LOG_POST_CREATE);

        return ToObject<ResultLogDTO>();

    }

    public async Task<bool> LogInsertEntry( string idLog, List<string> Columns )
    {

        if (Columns.Count != 3) {
            throw new InvalidOperationException("Expected three columns");
        }

        InitializeClient();
        SendLogEntryIDColumn columns = new SendLogEntryIDColumn();
        columns.col1 = Columns[0];
        columns.col2 = Columns[1];
        columns.col3 = Columns[2];

        var content = ToContentParamAndObj(columns);

        var response = await ToPostResponse(content, ToStrUri(URIs_Log.LOG_POST_ID_ENTRY,idLog));

        if (response == null) return false;
            
        var statusCode = response.StatusCode;

        if ((int)statusCode != 200) return false;

        return true;

    }

    public async Task<ResultLogDTO> LogById(string idLog)
    {

        AddParamsToList();
        InitializeClient(ListParams);

        await ToGetResponseURL(ToStrUri(URIs_Log.LOG_GET_ID,idLog));

        return ToObject<ResultLogDTO>();

    }

    public async Task<ResultLogEntryDTO> LogGetLog(string idLog, List<Param> Queries, 
                                                    SendLogEntryDTO sendLogEntryDTO)
    {

        string Query = "?";

        foreach(Param param in Queries)
        {
            Query += param.Name + "=" + param.Value + "&";
        }
        
        InitializeClient();

        var content = ToContentParamAndObj(sendLogEntryDTO);

        await ToGetResponseURL(ToStrUri(URIs_Log.LOG_GET_ID_ENTRY, idLog) + Query);

        return ToObject<ResultLogEntryDTO>();

    }

    public async Task<ResultLogEntryDTO> LogFetchData(string idLog, List<Param> Queries)
    {

        string Query = "?";

        foreach (Param param in Queries)
        {
            Query += param.Name + "=" + param.Value + "&";
        }

        AddParamsToList();
        InitializeClient(ListParams);

        await ToGetResponseURL(ToStrUri(URIs_Log.LOG_GET_ID_ENTRY, idLog) + Query);

        return ToObject<ResultLogEntryDTO>();

    }

    public async Task<bool> LogCSV(string idLog, int days, string filename)
    {
        string Query = "?days=" + days.ToString();

        InitializeClient(ListParams);

        var response = await ToGetResponseFile(ToStrUri(URIs_Log.LOG_GET_ID_CSV, idLog) + Query, filename);

        var statusCode = response.StatusCode;

        if ((int)statusCode != 200) return false;

        return true;

    }


}
