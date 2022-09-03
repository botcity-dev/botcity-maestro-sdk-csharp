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
    private BotMaestroSDK BotApi;


    [Test]
    public async Task MaestroVersionTest()
    {
        BotApi = new BotMaestroSDK(url);
        var version = await BotApi.MaestroVersion();

        int result = (int)BotApi.ResponseMessage.StatusCode;

        Assert.AreEqual(result.ToString(),"200");
        Assert.NotNull(version);

    }



    /*
    var Version = await BotApi.MaestroVersion();
    Console.WriteLine("MaestroVersion:" + Version.Version);


    //TASK
    var activity = new Activity();
    activity.ActivityLabel = "LabelAutomacao01";
    activity.Test = true;


    activity.ParamAdd("ParametroAutomacao01","");

    Console.WriteLine("ORGANIZATION LABEL:" + loginUser.Organizations.FirstOrDefault(x => x.Label != "").Label );
    Console.WriteLine("loginUser.Token:" + loginUser.Token);
    var task = await BotApi.Task(loginUser.Token, loginUser.Organizations.FirstOrDefault(x => x.Label != "").Label, activity); ;
    Console.WriteLine("TASK:" + task.ToString());
    Console.WriteLine("TASK:" + task.Id);
    Console.WriteLine("TASK STATE:" + task.State);
    Console.WriteLine("TASK:" + task.ActivityLabel);

    Console.WriteLine(":");



    //TASK FINISH
    var sendTaskState = new SendTaskStateDTO();
    sendTaskState.state = "FINISHED";
    sendTaskState.SendStatus = FinishedStatus.SUCCESS;
    sendTaskState.finishMessage = "MINHA MENSAGEM SUPER MANEIRA";


    activity.ParamAdd("ParametroAutomacao01", "");

    var taskId2 = await BotApi.TaskSetState(loginUser.Token, loginUser.Organizations.FirstOrDefault(x => x.Label != "").Label, sendTaskState, task.Id); ;
    Console.WriteLine("TASK2:" + taskId2.ToString());
    Console.WriteLine("TASK2:" + taskId2.Id);
    Console.WriteLine("TASK2:" + taskId2.State);
    Console.WriteLine("TASK2:" + taskId2.ActivityLabel);
    */
    /*
    //var taskId3 = await BotApi.TaskGetState(loginUser.Token, loginUser.Organizations.FirstOrDefault(x => x.Label != "").Label, task.Id); ;
    var taskId3 = await BotApi.TaskGetState(loginUser.Token, "79af9981-8d3c-4ea9-ae81-d33c525fba73", 129984);
        Console.WriteLine("TASK3:" + taskId3.ToString());
    Console.WriteLine("TASK3:" + taskId3.Id);
    Console.WriteLine("TASK3:" + taskId3.State);
    Console.WriteLine("TASK3:" + taskId3.ActivityLabel);


    Console.ReadKey();
    */

}