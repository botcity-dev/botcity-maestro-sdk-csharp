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

namespace BotCity.TestsNUnit;

public class UnitTestLog
{
    string url = "https://developers.botcity.dev/api/v2/";
    string user = "edson.marcio7@gmail.com";
    string senha = ClassSenha.Password;

    
    private SendLogDTO sendLogDTO;
    private string? LogId;

    [SetUp]
    public void Setup()
    {


    }


    //ARRANGE
    private void Arrange()
    {
        Random random = new Random();

        sendLogDTO = new SendLogDTO();

        sendLogDTO.Id = random.Next(0, 9999).ToString();
        sendLogDTO.activityLabel = "LabelLogName" + random.Next(0, 9999).ToString();
        LogId = sendLogDTO.Id;

        Column column1 = new Column
        {
            Name = "Column1",
            Label = "Label 1",
            Width = 100
        };

        Column column2 = new Column
        {
            Name = "Column1",
            Label = "Label 1",
            Width = 100
        };

        Column column3 = new Column
        {
            Name = "Column1",
            Label = "Label 1",
            Width = 100
        };

        sendLogDTO.columns.Add(column1);
        sendLogDTO.columns.Add(column2);
        sendLogDTO.columns.Add(column3);

    }

    [Test, Order(1)]
    public async Task CreateLogTest()
    {
        //ARRANGE
        var BotApi = new BotMaestroSDK(url);
        var loginUser = await BotApi.Login(user, senha);

        Arrange();

        Console.WriteLine(loginUser.Organizations.FirstOrDefault(x => x.Label != "").Label);
        var log = await BotApi.LogCreate(sendLogDTO);
        
        int result = (int)BotApi.ResponseMessage.StatusCode;

        Assert.AreEqual("200", result.ToString());
        Assert.AreEqual(log.organizationLabel, loginUser.Organizations.FirstOrDefault(x => x.Label != "").Label);
       

    }

    [Test, Order(2)]
    public async Task CreateLogEntry()
    {
        //ARRANGE
        var BotApi = new BotMaestroSDK(url);
        var loginUser = await BotApi.Login(user, senha);

        var listColunas = new List<string>();
        string sParam1, sParam2, sParam3;
        sParam1 = "Coluna1";
        sParam2 = "Coluna2";
        sParam3 = "Coluna3";

        listColunas.Add(sParam1);
        listColunas.Add(sParam2);
        listColunas.Add(sParam3);


        //ACT
        Console.WriteLine(loginUser.Organizations.FirstOrDefault(x => x.Label != "").Label);
        Console.WriteLine("LogId:" + LogId);


        var log = await BotApi.LogInsertEntry(sendLogDTO.activityLabel, listColunas);

        int result = (int)BotApi.ResponseMessage.StatusCode;


        //ASSERT
        Assert.AreEqual("200", result.ToString());
       

    }

    [Test, Order(3)]
    public async Task TestLogById()
    {
        //ARRANGE
        var BotApi = new BotMaestroSDK(url);
        var loginUser = await BotApi.Login(user, senha);

        Console.WriteLine(loginUser.Organizations.FirstOrDefault(x => x.Label != "").Label);
        Console.WriteLine("LOG ID:" + sendLogDTO.Id);
        Console.WriteLine("LOG ID:" + LogId);
        var log = await BotApi.LogById(sendLogDTO.activityLabel);

        int result = (int)BotApi.ResponseMessage.StatusCode;
        Console.WriteLine("Result:" + result.ToString());
        Assert.AreEqual("200", result.ToString());
        Assert.AreEqual(loginUser.Organizations.FirstOrDefault(x => x.Label != "").Label, log.organizationLabel);
        
    }
   
}