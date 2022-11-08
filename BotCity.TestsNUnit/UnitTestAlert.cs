using NUnit.Framework;
using BotCityMaestroSDK;
using BotCityMaestroSDK.Lib;
using System.Threading.Tasks;
using BotCityMaestroSDK.Dtos.Login;
using BotCityMaestroSDK.Dtos.Task;
using System.Linq;
using System;
using static NUnit.Framework.Constraints.Tolerance;
using BotCityMaestroSDK.Dtos;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.IO;
using BotCityMaestroSDK.Dtos.Alert;

namespace BotCity.TestsNUnit;

public class UnitTestAlert
{
    string url = "https://developers.botcity.dev/api/v2/";
    string user = "edson.marcio7@gmail.com";
    string senha = ClassSenha.Password;

    private SendAlert sendAlert;

    [SetUp]
    public void Setup()
    {


    }


    //ARRANGE
    private void Arrange()
    {
        sendAlert = new SendAlert();
        sendAlert.TaskId = 137985; // Need be a existent TASK ID
        sendAlert.Title = "Message Title";
        sendAlert.Message = "My message body";
        sendAlert.AlertType = AlertType.INFO;

    }

    [Test, Order(1)]
    public async Task CreateAlertTest()
    {
        //ARRANGE
        var BotApi = new BotMaestroSDK(url);
        var loginUser = await BotApi.Login(user, senha);

        Arrange();

        var alert = await BotApi.AlertCreate(sendAlert);
     

        int result = (int)BotApi.ResponseMessage.StatusCode;

        Assert.AreEqual("200", result.ToString());
        Assert.AreEqual("Automacao01Name", alert.ActivityName);

    }


}