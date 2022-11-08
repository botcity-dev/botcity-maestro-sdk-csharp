using NUnit.Framework;
using BotCityMaestroSDK;
using BotCityMaestroSDK.Lib;
using System.Threading.Tasks;
using BotCityMaestroSDK.Dtos.Login;

namespace BotCity.TestsNUnit
{
    public class UnitTestLogin
    {
        string url = "https://developers.botcity.dev/api/v2/";
        string user = "edson.marcio7@gmail.com";
        string senha = ClassSenha.Password;
        private Maestro BotApi;
        
       

        [Test]
        public async Task LoginTest()
        {
            BotApi = new Maestro(url);
            var loginUser  = await BotApi.Login(user, senha);

            int result = (int)BotApi.ResponseMessage.StatusCode;

            Assert.AreEqual(result.ToString(), "200");
            Assert.AreEqual(user, loginUser.Email);


        }

        [Test]
        public async Task LoginStudio()
        {
            BotApi = new Maestro(url);
            var loginStudio = await BotApi.LoginStudio(user, senha);

            int result = (int)BotApi.ResponseMessage.StatusCode;

            Assert.AreEqual(result.ToString(), "200");
            Assert.AreEqual(ClassSenha.UserName, loginStudio.userName);

        }

        [Test]
        public async Task LoginCookieComParametro()
        {
            BotApi = new Maestro(url);
            var loginStudio = await BotApi.LoginStudio(user, senha);

            var loginCookie = await BotApi.LoginCookie();

            loginCookie = await BotApi.LoginCookie(loginCookie.New_Cookie);

            int result = (int)BotApi.ResponseMessage.StatusCode;

            Assert.AreEqual(result.ToString(), "200");
            Assert.AreEqual(ClassSenha.UserName, loginCookie.userName);

        }

        [Test]
        public async Task LoginCookieSemParametro()
        {
            BotApi = new Maestro(url);
            var loginStudio = await BotApi.LoginStudio(user, senha);

            var loginCookie = await BotApi.LoginCookie();

            int result = (int)BotApi.ResponseMessage.StatusCode;

            Assert.AreEqual(result.ToString(), "200");
            Assert.AreEqual(ClassSenha.UserName, loginCookie.userName);

        }

        [Test]
        public async Task LoginCli()
        {
            BotApi = new Maestro(url);
            var loginCli = await BotApi.LoginCli(user, senha);

            int result = (int)BotApi.ResponseMessage.StatusCode;

            Assert.AreEqual(result.ToString(), "200");
            Assert.AreEqual(ClassSenha.UserName, loginCli.userName);

        }

        [Test]
        public async Task LoginCliCookie()
        {
            BotApi = new Maestro(url);

            var loginCli1 = await BotApi.LoginCli(user, senha);
            var loginCli = await BotApi.LoginCookieCli();

            int result = (int)BotApi.ResponseMessage.StatusCode;

            Assert.AreEqual(result.ToString(), "200");
            Assert.AreEqual(ClassSenha.UserName, loginCli.userName);

        }
    }
}