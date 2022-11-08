
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
using System.Drawing;
using System.Runtime.CompilerServices;

namespace BotCityMaestroSDK.Lib;

public partial class BotMaestroSDK
{
    public async Task<Artefact> ArtifactCreate(SendArtefact SendArtefact)
    {

        InitializeClient();

        var content = ToContentParamAndObj(SendArtefact);

        await ToPostResponse(content, URIs_ResultFiles.ARTIFACT_POST_CREATE);

       return ToObject<Artefact>();


    } 

    public async Task<bool> ArtifactSend(int ArtifactId, string FilePath)
    {

        InitializeClient();

        var response = await ToPostSendFile(FilePath, ArtifactId, ToStrUri(URIs_ResultFiles.ARTIFACT_POST_UPLOAD_ARTIFACT,ArtifactId.ToString()));

        if (response == null) return false;

        return true;

    }

    public async Task<string> ArtifactGetAll(Artefact Artifact, int Size = 50, int Page = 0, string Sort = "dateCreation", string OrderBy = "desc", int Days = 7 )
    {

        InitializeClient(ListParams);
        //example "?size=50&page=0&sort=dateCreation,desc&days=7"
        string options = "?size=SIZEX&page=PAGEX&sort=SORTX,ORDERBYX&days=DAYSX";
        options = options.Replace("SIZEX", Size.ToString());
        options = options.Replace("PAGEX", Page.ToString());
        options = options.Replace("SORTX", Sort.ToString());
        options = options.Replace("ORDERBYX", OrderBy.ToString());
        options = options.Replace("DAYSX", Days.ToString());
        var content = ToContentParamAndObj(Artifact);
        

        var response = await ToGetResponseURL(ToStrUri(URIs_ResultFiles.ARTIFACT_GET_ARTIFACT) + options );

        if (response == null) return "";

        return ResultRaw;

    }

    public async Task<bool> ArtifactGetFile(string ArtifactId, string FileName)
    {

        InitializeClient(ListParams);

        var response = await ToGetResponseFile(ToStrUri(URIs_ResultFiles.ARTIFACT_GET_ARTIFACT,ArtifactId), FileName );
 
        var statusCode = response.StatusCode;

        if ((int)statusCode != 200) return false;

        return true;

    }


}
