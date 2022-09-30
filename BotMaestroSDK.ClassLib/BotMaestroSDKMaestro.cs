
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System;

using BotCityMaestroSDK.Dtos.Maestro;

namespace BotCityMaestroSDK.Lib;

public partial class BotMaestroSDK
{


    public async Task<TokenMaestroVersion> MaestroVersion(){

        await ToGetResponseURL( URIs_Maestro.MAESTRO_VERSION);

        return ToObjectMaestro<TokenMaestroVersion>();
        
    }

    public TokenMaestroVersion ToObjectMaestro<T>()
    {

        TokenMaestroVersion result = JsonConvert.DeserializeObject<TokenMaestroVersion>(ResultRaw);
        //this.TokenMaestroVersion = result;

        return result;


    }

}
