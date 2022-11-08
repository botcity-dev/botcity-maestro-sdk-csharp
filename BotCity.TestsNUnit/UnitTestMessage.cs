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
using BotCityMaestroSDK.Dtos.Message;

namespace BotCity.TestsNUnit;

public class UnitTestMessage
{
    string url = "https://developers.botcity.dev/api/v2/";
    string user = "edson.marcio7@gmail.com";
    string senha = ClassSenha.Password;


    private SendMessage sendMessage;

    [SetUp]
    public void Setup()
    {


    }


    //ARRANGE
    private void Arrange()
    {
        sendMessage = new SendMessage();
        sendMessage.Emails.Add("edson.marcio7@gmail.com");
        sendMessage.Subject = $"Subject BotCity {DateTime.Now.ToString("yyyyMMddHHmmss")}";
        sendMessage.Body = "Mail body.. ";
        sendMessage.TypeMail = TypeMail.TEXT;

    }

    [Test, Order(1)]
    public async Task CreateAlertTest()
    {
        //ARRANGE
        var BotApi = new BotMaestroSDK(url);
        var loginUser = await BotApi.Login(user, senha);

        Arrange();

        var message = await BotApi.MessageCreate(sendMessage);

        int result = (int)BotApi.ResponseMessage.StatusCode;

        Assert.AreEqual("200", result.ToString());
        Assert.AreEqual(true, message);

    }


}