using NUnit.Framework;
using BotCityMaestroSDK;
using BotCityMaestroSDK.Lib;
using System.Threading.Tasks;
using BotCityMaestroSDK.Dtos.Login;
using BotCityMaestroSDK.Dtos.Task;
using System.Linq;
using System;
using static NUnit.Framework.Constraints.Tolerance;

namespace BotCity.TestsNUnit;

public class UnitTestTask
{
    string url = "https://developers.botcity.dev/api/v2/";
    string user = "edson.marcio7@gmail.com";
    string senha = ClassSenha.Password;
    //private BotMaestroSDK BotApi;
    private Activity activity = new Activity();
    //private ResultLoginDTO loginUser;
    private SendTaskStateDTO sendTaskState;
    private int? TaskId;

    [SetUp]
    public void Setup()
    {
        /*
        activity = new Activity();
        activity.ActivityLabel = "LabelAutomacao01";
        activity.Test = true;
        activity.ParamAdd("ParametroAutomacao01", "");

        Login();

        sendTaskState = new SendTaskStateDTO();
        sendTaskState.state = "FINISHED";
        sendTaskState.SendStatus = FinishedStatus.SUCCESS;
        sendTaskState.finishMessage = "MINHA MENSAGEM SUPER MANEIRA : " + DateTime.Now.ToString();
        */


    }

    //ARRANGE

    private async void Login()
    {
        //BotApi = new BotMaestroSDK(url);
        //var loginUser = await BotApi.Login(user, senha);
    }

    //ARRANGE
    private void Arrange()
    {
        activity = new Activity();
        activity.ActivityLabel = "LabelAutomacao01";
        activity.Test = true;
        activity.ParamAdd("ParametroAutomacao01", "");

        var sendTaskState = new SendTaskStateDTO();
        sendTaskState.state = "FINISHED";
        sendTaskState.SendStatus = FinishedStatus.SUCCESS;
        sendTaskState.finishMessage = "MINHA MENSAGEM SUPER MANEIRA : " + DateTime.Now.ToString();
    }

    [Test, Order(1)]
    public async Task CreateTaskTest()
    {
        //ARRANGE
        var BotApi = new BotMaestroSDK(url);
        var loginUser = await BotApi.Login(user, senha);
        Arrange();


        

        //Console.WriteLine(loginUser.Organizations.FirstOrDefault(x => x.Label != "").Label);
        var task = await BotApi.Task(loginUser.Token, loginUser.Organizations.FirstOrDefault(x => x.Label != "").Label, activity);

        int result = (int)BotApi.ResponseMessage.StatusCode;

        Assert.AreEqual(result.ToString(),"200");
        Assert.AreEqual(task.ActivityLabel, activity.ActivityLabel);
        TaskId = task.Id;
        Console.WriteLine("TaskId:" + TaskId + " - Date:" + DateTime.Now.ToString());

    }

    
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

    
}