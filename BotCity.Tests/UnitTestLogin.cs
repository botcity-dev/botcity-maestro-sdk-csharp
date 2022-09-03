using BotCityMaestroSDK.Lib;

namespace BotCity.Tests;

public class UnitTestLogin
{
    string url = "https://developers.botcity.dev/api/v2/";
    string user = "edson.marcio7@gmail.com";
    string senha = ClassSenha.Password;
    private BotMaestroSDK BotApi;
    

    [Fact]
    public async void LoginTest()
    {
        BotApi = new BotMaestroSDK(url);
        var loginUser = await BotApi.Login(user, senha);

        int result = (int)BotApi.ResponseMessage.StatusCode;

        Assert.Matches(result.ToString(),"200");
        Assert.Matches(user,loginUser.Email);

    }

    [Fact]
    public async void LoginStudio()
    {
        BotApi = new BotMaestroSDK(url);
        var loginStudio = await BotApi.LoginStudio(user, senha);

        int result = (int)BotApi.ResponseMessage.StatusCode;

        Assert.Matches(result.ToString(), "200");
        Assert.Matches(ClassSenha.UserName, loginStudio.userName);

    }

    [Fact]
    public async void LoginCookieComParametro()
    {
        BotApi = new BotMaestroSDK(url);
        var loginStudio = await BotApi.LoginStudio(user, senha);

        var loginCookie = await BotApi.LoginCookie();

        loginCookie = await BotApi.LoginCookie(loginCookie.New_Cookie);

        int result = (int)BotApi.ResponseMessage.StatusCode;

        Assert.Matches(result.ToString(), "200");
        Assert.Matches(ClassSenha.UserName, loginCookie.userName);

    }

    [Fact]
    public async void LoginCookieSemParametro()
    {
        BotApi = new BotMaestroSDK(url);
        var loginStudio = await BotApi.LoginStudio(user, senha);

        var loginCookie = await BotApi.LoginCookie();

        int result = (int)BotApi.ResponseMessage.StatusCode;

        Assert.Matches(result.ToString(), "200");
        Assert.Matches(ClassSenha.UserName, loginCookie.userName);

    }

    [Fact]
    public async void LoginCli()
    {
        BotApi = new BotMaestroSDK(url);
        var loginCli = await BotApi.LoginCli(user, senha);

        int result = (int)BotApi.ResponseMessage.StatusCode;

        Assert.Matches(result.ToString(), "200");
        Assert.Matches(ClassSenha.UserName, loginCli.userName);

    }

    [Fact]
    public async void LoginCliCookie()
    {
        BotApi = new BotMaestroSDK(url);
        
        var loginCli1 = await BotApi.LoginCli(user, senha);
        var loginCli = await BotApi.LoginCookieCli();

        int result = (int)BotApi.ResponseMessage.StatusCode;

        Assert.Matches(result.ToString(), "200");
        Assert.Matches(ClassSenha.UserName, loginCli.userName);

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