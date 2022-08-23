using NsBotMaestroSDK.ClassLib;
using System.Net.Http.Formatting;

var url = "https://developers.botcity.dev/api/v2/";
var user = "edson.marcio7@gmail.com";
var senha = "suasenhaaqui!";


//CREATE Library Instance
var BotApi = new BotMaestroSDK(url);


 
//CALL LOGIN
var loginUser = await BotApi.Login(user,senha);
Console.WriteLine("LoginToken:" + loginUser.Token);


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


var b = Console.ReadKey();


