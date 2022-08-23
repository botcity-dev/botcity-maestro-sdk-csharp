
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NsBotCityMaestroSDK.ClassLib.Dtos.Login;
using NsBotCityMaestroSDK.ClassLib.Dtos.Maestro;

namespace NsBotMaestroSDK.ClassLib;

public partial class BotMaestroSDK
{




    public async Task<HttpResponseMessage> ToGetMaestroResponse( string URI)
    {
        var response = BotMaestroSDK.ApiClient.GetAsync(
                ToStrUri(URI)).Result;

        ResponseMessage = response;
        //Console.WriteLine("response:" + response);
        var statusCode = response.StatusCode;
        ResultRaw = await response.Content.ReadAsStringAsync();
        if ((int)statusCode != 200) return null;


        return response;

    }


    public TokenMaestroVersion ToObjectMaestro<T>()
    {

        TokenMaestroVersion result = JsonConvert.DeserializeObject<TokenMaestroVersion>(ResultRaw);
        this.TokenMaestroVersion = result;

        return result;

     
    }

}
