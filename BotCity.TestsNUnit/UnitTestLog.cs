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
        sendLogDTO.activityLabel = "LabelLogName" + sendLogDTO.Id;

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

        sendLogDTO.Columns.Add(column1);
        sendLogDTO.Columns.Add(column2);
        sendLogDTO.Columns.Add(column3);

    }

    [Test, Order(1)]
    public async Task CreateLogTest()
    {
        //ARRANGE
        var BotApi = new BotMaestroSDK(url);
        var loginUser = await BotApi.Login(user, senha);

        Arrange();


        

        Console.WriteLine(loginUser.Organizations.FirstOrDefault(x => x.Label != "").Label);
        var log = await BotApi.LogCreate(loginUser.Token, loginUser.Organizations.FirstOrDefault(x => x.Label != "").Label, sendLogDTO);

        int result = (int)BotApi.ResponseMessage.StatusCode;

        Assert.AreEqual(result.ToString(),"200");
        Assert.AreEqual(log.organizationLabel, loginUser.Organizations.FirstOrDefault(x => x.Label != "").Label);
        LogId = log.id;



    }

    [Test, Order(2)]
    public async Task TestLogById()
    {
        //ARRANGE
        var BotApi = new BotMaestroSDK(url);
        var loginUser = await BotApi.Login(user, senha);

        Console.WriteLine(loginUser.Organizations.FirstOrDefault(x => x.Label != "").Label);
        var log = await BotApi.LogById(loginUser.Token, loginUser.Organizations.FirstOrDefault(x => x.Label != "").Label, LogId);

        int result = (int)BotApi.ResponseMessage.StatusCode;

        Assert.AreEqual(result.ToString(), "200");
        Assert.AreEqual(log.organizationLabel, loginUser.Organizations.FirstOrDefault(x => x.Label != "").Label);
        Console.WriteLine("LOG ID:" + LogId);
    }
    /*
    [Test, Order(2)]
    public async Task TaskSetStateTest()
    {
        Console.WriteLine("TaskSetStateTest:" + TaskId + " - Date:" + DateTime.Now.ToString());
        //ARRANGE
        var BotApi = new BotMaestroSDK(url);
        var loginUser = await BotApi.Login(user, senha);
        Arrange();


        //var task = await BotApi.Task(loginUser.Token, loginUser.Organizations.FirstOrDefault(x => x.Label != "").Label, activity);

        var taskId2 = await BotApi.TaskSetState(loginUser.Token, loginUser.Organizations.FirstOrDefault(x => x.Label != "").Label, sendTaskState, TaskId);

        int result = (int)BotApi.ResponseMessage.StatusCode;


        //ASSERT
        Assert.AreEqual(result.ToString(), "200");
        Assert.AreEqual(taskId2.Id, TaskId);

    }
    /*

    [Test, Order(3)]
    public async Task TaskGetStateTest()
    {
        Console.WriteLine("TaskGetStateTest:" + TaskId + " - Date:" + DateTime.Now.ToString());
        //ARRANGE
        var BotApi = new BotMaestroSDK(url);
        var loginUser = await BotApi.Login(user, senha);
        Arrange();

        var taskId3 = await BotApi.TaskGetState(loginUser.Token, loginUser.Organizations.FirstOrDefault(x => x.Label != "").Label, TaskId); 

        int result = (int)BotApi.ResponseMessage.StatusCode;

        Assert.AreEqual(result.ToString(), "200");
        Assert.AreEqual(taskId3.Id, TaskId);

    }

    */
}