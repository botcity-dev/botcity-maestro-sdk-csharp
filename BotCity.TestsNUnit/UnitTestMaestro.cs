using NUnit.Framework;
using BotCityMaestroSDK;
using BotCityMaestroSDK.Lib;
using System.Threading.Tasks;
using BotCityMaestroSDK.Dtos.Login;

namespace BotCity.TestsNUnit;

public class UnitTestMaestro
{
    string url = "https://developers.botcity.dev/api/v2/";
    string user = "edson.marcio7@gmail.com";
    string senha = ClassSenha.Password;
    private Maestro BotApi;


    [Test]
    public async Task MaestroVersionTest()
    {
        BotApi = new Maestro(url);
        var version = await BotApi.MaestroVersion();

        int result = (int)BotApi.ResponseMessage.StatusCode;

        Assert.AreEqual(result.ToString(),"200");
        Assert.NotNull(version);

    }

}