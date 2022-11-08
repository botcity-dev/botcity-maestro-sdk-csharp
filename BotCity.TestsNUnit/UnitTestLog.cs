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

namespace BotCity.TestsNUnit;

public class UnitTestLog
{
    string url = "https://developers.botcity.dev/api/v2/";
    string user = "edson.marcio7@gmail.com";
    string senha = ClassSenha.Password;

    
    private SendLogDTO sendLogDTO;
    private ResultLogDTO resultLogDTO;

    [SetUp]
    public void Setup()
    {


    }


    //ARRANGE
    private void Arrange()
    {
        Random random = new Random();

        sendLogDTO = new SendLogDTO();
        sendLogDTO.activityLabel = "log2" + DateTime.Now.ToString("yyyymmddss");

        Column column1 = new Column
        {
            Name = "Column1",
            Label = "Label 1",
            Width = 100
        };

        Column column2 = new Column
        {
            Name = "Column2",
            Label = "Label 2",
            Width = 100
        };

        Column column3 = new Column
        {
            Name = "Column3",
            Label = "Label 3",
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

        var log = await BotApi.LogCreate(sendLogDTO);
        resultLogDTO = log;

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
        var log = await BotApi.LogInsertEntry(resultLogDTO.activityLabel, listColunas);
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

        var log = await BotApi.LogById(resultLogDTO.activityLabel);
        Console.WriteLine("AGORA:" + DateTime.Now.ToString());
        int result = (int)BotApi.ResponseMessage.StatusCode;

        Assert.AreEqual("200", result.ToString());
        Assert.AreEqual(loginUser.Organizations.FirstOrDefault(x => x.Label != "").Label, log.organizationLabel);
        
    }

    [Test, Order(4)]
    public async Task TestFetchData()
    {
        //ARRANGE
        var BotApi = new BotMaestroSDK(url);
        var loginUser = await BotApi.Login(user, senha);

        var list = new List<Param>();
        Param param = new Param
        {
            Name = "size",
            Value = "10"
        };
        list.Add(param);


        //ACTION
        var Log3 = await BotApi.LogFetchData(resultLogDTO.id, list);

        int result = (int)BotApi.ResponseMessage.StatusCode;

        //ASSERT
        Assert.AreEqual("200", result.ToString());
        Assert.AreEqual(10, Log3.size);

    }

    [Test, Order(5)]
    public async Task TestCSV()
    {
        //ARRANGE

        var BotApi = new BotMaestroSDK(url);
        var loginUser = await BotApi.Login(user, senha);

        Arrange();

        var Log1 = await BotApi.LogCreate(sendLogDTO);

        var listColunas = new List<string>();
        string sParam1, sParam2, sParam3;
        sParam1 = "Coluna1";
        sParam2 = "Coluna2";
        sParam3 = "Coluna3";

        listColunas.Add(sParam1);
        listColunas.Add(sParam2);
        listColunas.Add(sParam3);

        var log = await BotApi.LogInsertEntry(Log1.activityLabel, listColunas);

        //ACTION
        var filename = @"d:\Programacao\" + Log1.id + ".csv";

        var LogCSV = await BotApi.LogCSV(Log1.id, 7, filename);

        int result = (int)BotApi.ResponseMessage.StatusCode;

        //ASSERT
        Assert.AreEqual("200", result.ToString());
        Assert.AreEqual(true, LogCSV);

    }

}