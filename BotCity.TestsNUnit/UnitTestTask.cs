using NUnit.Framework;
using BotCityMaestroSDK;
using BotCityMaestroSDK.Lib;
using System.Threading.Tasks;
using BotCityMaestroSDK.Dtos.Login;
using BotCityMaestroSDK.Dtos.Task;
using System.Linq;
using System;
using static NUnit.Framework.Constraints.Tolerance;
using BotCityMaestroSDK.Dtos.Artefact;
using System.IO;

namespace BotCity.TestsNUnit;

public class UnitTestTask
{
    string url = "https://developers.botcity.dev/api/v2/";
    string user = "edson.marcio7@gmail.com";
    string senha = ClassSenha.Password;
    
    private Activity _activity = new Activity();
    private SendTaskStateDTO _sendTaskState;
    private SendArtefact _sendArtefact;
    private int _artifactId = 0;
    private int? _TaskId;
    private Artefact _artefact = new Artefact();

    [SetUp]
    public void Setup()
    {
      

    }

    //ARRANGE
    private void Arrange()
    {
        _activity = new Activity();
        _activity.ActivityLabel = "LabelAutomacao01";
        _activity.Test = true;
        _activity.ParamAdd("ParametroAutomacao01", "");

        _sendTaskState = new SendTaskStateDTO();
        _sendTaskState.state = "FINISHED";
        _sendTaskState.SendStatus = FinishedStatus.SUCCESS;
        _sendTaskState.finishMessage = "MINHA MENSAGEM SUPER MANEIRA : " + DateTime.Now.ToString();

        _sendArtefact = new SendArtefact();
        _sendArtefact.taskId = 129984; 
        _sendArtefact.Name = "User Facing Name.txt";
        _sendArtefact.FileName = "arquivoTeste.txt";
    }

    [Test, Order(1)]
    public async Task CreateTaskTest()
    {
        //ARRANGE
        var BotApi = new BotMaestroSDK(url);
        var loginUser = await BotApi.Login(user, senha);
        Arrange();


        

        //Console.WriteLine(loginUser.Organizations.FirstOrDefault(x => x.Label != "").Label);
        var task = await BotApi.TaskCreate( _activity);

        int result = (int)BotApi.ResponseMessage.StatusCode;

        Assert.AreEqual(result.ToString(),"200");
        Assert.AreEqual(task.ActivityLabel, _activity.ActivityLabel);
        _TaskId = task.Id;
        Console.WriteLine("TaskId:" + _TaskId + " - Date:" + DateTime.Now.ToString());

    }

    
    [Test, Order(2)]
    public async Task TaskSetStateTest()
    {
        Console.WriteLine("TaskSetStateTest:" + _TaskId + " - Date:" + DateTime.Now.ToString());
        //ARRANGE
        var BotApi = new BotMaestroSDK(url);
        var loginUser = await BotApi.Login(user, senha);
        Arrange();


        //var task = await BotApi.Task(loginUser.Token, loginUser.Organizations.FirstOrDefault(x => x.Label != "").Label, activity);

        var taskId2 = await BotApi.TaskSetState(_sendTaskState, _TaskId);

        int result = (int)BotApi.ResponseMessage.StatusCode;


        //ASSERT
        Assert.AreEqual(result.ToString(), "200");
        Assert.AreEqual(taskId2.Id, _TaskId);

    }


    [Test, Order(3)]
    public async Task TaskGetStateTest()
    {
        Console.WriteLine("TaskGetStateTest:" + _TaskId + " - Date:" + DateTime.Now.ToString());
        //ARRANGE
        var BotApi = new BotMaestroSDK(url);
        var loginUser = await BotApi.Login(user, senha);
        Arrange();

        var taskId3 = await BotApi.TaskGetState(_TaskId); 

        int result = (int)BotApi.ResponseMessage.StatusCode;

        Assert.AreEqual(result.ToString(), "200");
        Assert.AreEqual(taskId3.Id, _TaskId);

    }

    [Test, Order(4)]
    public async Task TaskCreateArtifactTest()
    {
        Console.WriteLine("TaskGetStateTest:" + _TaskId + " - Date:" + DateTime.Now.ToString());
        //ARRANGE
        var BotApi = new BotMaestroSDK(url);
        var loginUser = await BotApi.Login(user, senha);

        Arrange();

        Artefact artifact = await BotApi.ArtifactCreate(_sendArtefact);
        Console.WriteLine("Message:" + artifact.ToString());
        Console.WriteLine("Message:" + artifact.Type);
        Console.WriteLine("Message:" + artifact.userId);

        int result = (int)BotApi.ResponseMessage.StatusCode;

        _artifactId = artifact.id;
        _artefact = artifact;
        Assert.AreEqual(result.ToString(), "200");
        Assert.AreEqual(_sendArtefact.taskId, artifact.taskId);

    }

    [Test, Order(5)]
    public async Task TaskSendArtifactTest()
    {
        if (_artifactId == 0)
        {
            Console.WriteLine("artifactId:0");
            Assert.AreEqual(true, false); ;
        }

        Console.WriteLine("TaskGetStateTest:" + _TaskId + " - Date:" + DateTime.Now.ToString());
        //ARRANGE
        var BotApi = new BotMaestroSDK(url);
        var loginUser = await BotApi.Login(user, senha);

        Arrange();

        var path = @"d:\arquivoTeste.txt";
        var send = await BotApi.ArtifactSend(_artefact.id, path); //179
  
        int result = (int)BotApi.ResponseMessage.StatusCode;

        Assert.AreEqual(result.ToString(), "200");
        Assert.AreEqual(true, send);

    }

    [Test, Order(6)]
    public async Task TaskSendArtifactGetAll()
    {
        if (_artefact == null || _artefact.id == 0)
        {
            Console.WriteLine("artifactId:0");
            Assert.AreEqual(true, false); ;
        }

        //ARRANGE
        var BotApi = new BotMaestroSDK(url);
        var loginUser = await BotApi.Login(user, senha);

        Arrange();

        Console.WriteLine("ARTI:" + _artefact.id);

        Artefact artifact = await BotApi.ArtifactCreate(_sendArtefact);

        var path = @"d:\arquivoTeste.txt";
        var send = await BotApi.ArtifactSend(artifact.id, path); //179
        
        var artifactAll = await BotApi.ArtifactGetAll(artifact);
        Console.WriteLine("artifactAll:" + artifactAll.ToString());
        int result = (int)BotApi.ResponseMessage.StatusCode;

        Assert.AreNotEqual("", artifactAll);

    }

    [Test, Order(7)]
    public async Task TaskSendArtifactGetFile()
    {
        if (_artifactId == 0)
        {
            Console.WriteLine("artifactId:0");
            Assert.AreEqual(true, false); ;
        }

        //ARRANGE
        var BotApi = new BotMaestroSDK(url);
        var loginUser = await BotApi.Login(user, senha);

        Arrange();

        Artefact artifact = await BotApi.ArtifactCreate(_sendArtefact);

        var path = @"d:\arquivoTeste.txt";
        var send = await BotApi.ArtifactSend(artifact.id, path); //179

        var filename = @"d:\Programacao\Artifact" + artifact.id.ToString() + ".txt";
        Console.WriteLine("ID:" + artifact.id.ToString());
        var getFile = await BotApi.ArtifactGetFile(artifact.id.ToString(), filename); //179

        int result = (int)BotApi.ResponseMessage.StatusCode;

        Assert.AreEqual(result.ToString(), "200");
        Assert.AreEqual(true, getFile);

    }



}