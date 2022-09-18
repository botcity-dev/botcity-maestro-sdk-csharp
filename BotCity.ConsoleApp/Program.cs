
using System;
using System.Net.Http.Formatting;
using BotCityMaestroSDK.Lib;
using BotCityMaestroSDK.Dtos;
using BotCityMaestroSDK.Dtos.Task;
using BotCityMaestroSDK.Dtos.Login;
using BotCityMaestroSDK.Dtos.Alert;
using BotCityMaestroSDK.Dtos.Message;

var url = "https://developers.botcity.dev/api/v2/";

//CREATE Library Instance
var BotApi = new BotMaestroSDK(url);



var user = "edson.marcio7@gmail.com";
var senha = "!";





 
//CALL LOGIN
var loginUser = await BotApi.Login(user,senha);
Console.WriteLine("LoginToken:" + loginUser.Token);
/*

//After called any API endpoint, the developer can use ResponseMessage 
Console.WriteLine(BotApi.ResponseMessage.StatusCode + " " + (int)BotApi.ResponseMessage.StatusCode);

//CALL LOGIN_STUDIO
var loginStudio = await BotApi.LoginStudio(user,senha);
Console.WriteLine("LoginStudioToken:" + loginStudio.Access_Token);


//CALL LOGIN_COOKIE_SEM_PARAMETRO (ELE USA O COOKIE INFORMADO PELO LOGIN_STUDIO)
var loginCookie = await BotApi.LoginCookie();
Console.WriteLine("LoginStudioToken:" + loginCookie.Access_Token);


//CALL LOGIN_COOKIE_COM_PARAMETRO (ELE USA O COOKIE INFORMADO PELO DESENVOLVEDOR)
loginCookie = await BotApi.LoginCookie(loginCookie.New_Cookie);
Console.WriteLine("LoginStudioTokenCOM PARAMETRO:" + loginCookie.Access_Token);



//CALL LOGIN_Cli
var loginCli = await BotApi.LoginCli(user, senha);
Console.WriteLine("LoginCliToken:" + loginCli.Access_Token);


//CALL LOGIN_COOKIE_SEM_PARAMETRO (ELE USA O COOKIE INFORMADO PELO LOGIN_STUDIO)
var loginCookieCli = await BotApi.LoginCookieCli();
Console.WriteLine("LoginCliToken:" + loginCookieCli.Access_Token);

//CALL LOGIN_COOKIE_COM_PARAMETRO (ELE USA O COOKIE INFORMADO PELO DESENVOLVEDOR)
loginCookieCli = await BotApi.LoginCookieCli(loginCookieCli.New_Cookie);
Console.WriteLine("LoginCliToken COM PARAMETRO:" + loginCookieCli.Access_Token);

var Version = await BotApi.MaestroVersion();
Console.WriteLine("MaestroVersion:" + Version.Version);


//TASK
var activity = new Activity();
activity.ActivityLabel = "LabelAutomacao01";
activity.Test = true;


activity.ParamAdd("ParametroAutomacao01","");

Console.WriteLine("ORGANIZATION LABEL:" + loginUser.Organizations.FirstOrDefault(x => x.Label != "").Label );
Console.WriteLine("loginUser.Token:" + loginUser.Token);
var task = await BotApi.TaskCreate(loginUser.Token, loginUser.Organizations.FirstOrDefault(x => x.Label != "").Label, activity); ;
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


//var taskId3 = await BotApi.TaskGetState(loginUser.Token, loginUser.Organizations.FirstOrDefault(x => x.Label != "").Label, task.Id); ;
var taskId3 = await BotApi.TaskGetState(loginUser.Token, "79af9981-8d3c-4ea9-ae81-d33c525fba73", 129984);
Console.WriteLine("TASK3:" + taskId3.ToString());
Console.WriteLine("TASK3:" + taskId3.Id);
Console.WriteLine("TASK3:" + taskId3.State);
Console.WriteLine("TASK3:" + taskId3.ActivityLabel);


Console.ReadKey();
*/

/*
SendLogDTO sendLogDTO = new SendLogDTO();

sendLogDTO.activityLabel = "log3" + DateTime.Now.ToString("yyyymmddss");

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

var Log = await BotApi.LogCreate(loginUser.Token, "79af9981-8d3c-4ea9-ae81-d33c525fba73" ,sendLogDTO);
Console.WriteLine("LOG:" + Log.ToString());
Console.WriteLine("LOG:" + Log.activityLabel);
Console.WriteLine("LOG:" + Log.organizationLabel);
Console.WriteLine("LOG:" + Log.id);

var listParam = new List<string>();

/*
Param param1 = new Param
{
    Name = "Column1",
    Value = "Value Colum1"
};

Param param2 = new Param
{
    Name = "Column2",
    Value = "Value Colum2"
};

Param param3 = new Param
{
    Name = "Column3",
    Value = "Value Colum3"
};
*/

/*
string param1, param2, param3;
param1 = "Coluna1";
param2 = "Coluna2";
param3 = "Coluna3";

listParam.Add(param1);
listParam.Add(param2);
//listParam.Add(param3);

bool LogEntry = await BotApi.LogInsertEntry(loginUser.Token, "79af9981-8d3c-4ea9-ae81-d33c525fba73", Log.id, listParam);
Console.WriteLine("LogEntry:" + LogEntry.ToString());


var Log2 = await BotApi.LogById(loginUser.Token, "79af9981-8d3c-4ea9-ae81-d33c525fba73", Log.id);
Console.WriteLine("LOG2:" + Log2.ToString());
Console.WriteLine("LOG2:" + Log2.activityLabel);
Console.WriteLine("LOG2:" + Log2.organizationLabel);



var list = new List<Param>();
Param param = new Param
{
    Name = "size",
    Value = "10"
};
list.Add(param);

SendLogEntryDTO sendLogEntry = new SendLogEntryDTO();
//sendLogEntry.

var Log3 = await BotApi.LogFetchData(loginUser.Token, "79af9981-8d3c-4ea9-ae81-d33c525fba73",
                                Log.id, list);
Console.WriteLine("LOG3:" + Log3.ToString());
Console.WriteLine("LOG3:" + Log3.TotalPages);
Console.WriteLine("LOG3:" + Log3.size);

var filename = @"d:\Programacao\" + Log.id + ".csv";
var LogCSV = await BotApi.LogCSV(loginUser.Token, "79af9981-8d3c-4ea9-ae81-d33c525fba73",
                               Log.id, 7,filename);

Console.WriteLine("LogCSV:" + LogCSV.ToString());

*/

/*
SendAlert sendAlert = new SendAlert();
sendAlert.TaskId = 137985;
sendAlert.Title = "Meu alerta alerta";
sendAlert.Message = "Minha mensagem maneira de alerta";
sendAlert.AlertType = AlertType.INFO;

var Alert = await BotApi.AlertCreate(loginUser.Token, "79af9981-8d3c-4ea9-ae81-d33c525fba73", sendAlert);
Console.WriteLine("Alert:" + Alert.ActivityName.ToString());
*/

/*
SendMessage sendMessage = new SendMessage();
sendMessage.Emails.Add("edson.marcio7@gmail.com");
//sendMessage.Logins.Add("");
sendMessage.Subject = "Subject123 1247 BotCity";
sendMessage.Body = "Corpo do e-mail.. bora billll";
sendMessage.TypeMail = TypeMail.TEXT;

var Message = await BotApi.MessageCreate(loginUser.Token, "79af9981-8d3c-4ea9-ae81-d33c525fba73", sendMessage);
Console.WriteLine("Message:" + Message.ToString());
*/
