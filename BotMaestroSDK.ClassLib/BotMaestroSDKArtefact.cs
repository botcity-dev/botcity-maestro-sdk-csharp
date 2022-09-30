
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
using BotCityMaestroSDK.Dtos.Artefact;

namespace BotCityMaestroSDK.Lib;

public partial class BotMaestroSDK
{


    public async Task<Artefact> ArtifactCreate(SendArtefact sendArtefact)
    {

        InitializeClient();

        var content = ToContentParamAndObj(sendArtefact);

        await ToPostResponse(content, URIs_ResultFiles.ARTIFACT_POST_CREATE);

       return ToObject<Artefact>();


    }

    public async Task<bool> ArtifactSend(int ArtifactId, string filePath)
    {

        InitializeClient();

        var response = await ToPostSendFile1(filePath, ArtifactId, ToStrUri(URIs_ResultFiles.ARTIFACT_POST_UPLOAD_ARTIFACT,ArtifactId.ToString()));

        if (response == null) return false;

        return true;

    }

    /*
    public async Task<bool> LogInsertEntry(string Token, string Organization, string idLog, List<string> Columns )
    {

        if (Columns.Count != 3) {
            throw new InvalidOperationException("Expected three columns");
        }

        InitializeClient();
        SendLogEntryIDColumn columns = new SendLogEntryIDColumn();
        columns.col1 = Columns[0];
        columns.col2 = Columns[1];
        columns.col3 = Columns[2];

        var content = ToContentParamAndObj(Token, Organization, columns);

        var response = await ToPostResponse(content, ToStrUri(URIs_Log.LOG_GET_ID_ENTRY,idLog));

        if (response == null) return false;
            
        var statusCode = response.StatusCode;

        if ((int)statusCode != 200) return false;

        return true;

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

    public async Task<ResultLogEntryDTO> LogFetchData(string Token, string Organization,
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

    public async Task<bool> LogCSV(string Token, string Organization,
                                              string idLog, int days, string filename)
    {

        string Query = "?days=" + days.ToString();

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

        var response = await ToGetResponseFile(ToStrUri(URIs_Log.LOG_GET_ID_CSV, idLog) + Query, filename);

        var statusCode = response.StatusCode;

        if ((int)statusCode != 200) return false;

        return true;

    }
    */

}
