
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System;

using NsBotCityMaestroSDK.ClassLib.Dtos.Maestro;

namespace NsBotMaestroSDK.ClassLib;

public partial class BotMaestroSDK
{


    public async Task<TokenMaestroVersion> MaestroVersion(){

        await ToGetMaestroResponse( URIs_Maestro.MAESTRO_VERSION);

        return ToObjectMaestro<TokenMaestroVersion>();
        
    }


}
