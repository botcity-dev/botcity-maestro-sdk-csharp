
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System;
using NsBotCityMaestroSDK.ClassLib.Dtos.Login;

namespace NsBotMaestroSDK.ClassLib;

public partial class BotMaestroSDK
{


    public async Task<ResultLoginDTO> Login(string UserName, string Password){

       
        var content = ToContent<SendLoginDTO>(UserName, Password);

        await ToPostResponse(content, URIs.LOGIN_POST);

        return ToObject<ResultLoginDTO>();
        
    }

    public async Task<ResultLoginStudioDTO> LoginStudio(string UserName, string Password){

        var content = ToContent<SendLoginStudioDTO>(UserName, Password);

        await ToPostResponse(content, URIs.LOGIN_STUDIO_POST);

        return ToObject<ResultLoginStudioDTO>();

    }

    public async Task<ResultLoginStudioDTO> LoginCookie(string Cookie){


        var cont = new {
             cookie = Cookie
        };

        var content = new StringContent(JsonConvert.SerializeObject(cont), Encoding.UTF8, "application/json");

        await ToPostResponse(content, URIs.LOGIN_COOKIE_POST);

        return ToObject<ResultLoginStudioDTO>();

    }

    public async Task<ResultLoginStudioDTO> LoginCookie(){

        
        string Cookie = this.TokenLoginStudioDTO.New_Cookie;

        if ( string.IsNullOrEmpty(Cookie) || string.IsNullOrWhiteSpace(Cookie) ){
            throw new Exception("Invalid Cookie");
        }
            

        var cont = new {
             cookie = Cookie
        };

        var content = new StringContent(JsonConvert.SerializeObject(cont), Encoding.UTF8, "application/json");

        await ToPostResponse(content, URIs.LOGIN_COOKIE_POST);

        return ToObject<ResultLoginStudioDTO>();
       

    }


    public async Task<ResultLoginCliDTO> LoginCli(string UserName, string Password)
    {

        var content = ToContent<SendLoginCliDTO>(UserName, Password);

        await ToPostResponse(content, URIs.LOGIN_CLI_POST);

        return ToObject<ResultLoginCliDTO>();

    }


    public async Task<ResultLoginCliDTO> LoginCookieCli(string Cookie)
    {


        var cont = new
        {
            cookie = Cookie
        };

        var content = new StringContent(JsonConvert.SerializeObject(cont), Encoding.UTF8, "application/json");

        await ToPostResponse(content, URIs.LOGIN_COOKIE_CLI_POST);

        return ToObject<ResultLoginCliDTO>();

    }

    public async Task<ResultLoginCliDTO> LoginCookieCli()
    {

        //Console.WriteLine("COOKIE:" + this.TokenLoginCliDTO.New_Cookie);
        string Cookie = this.TokenLoginCliDTO.New_Cookie;

        if (string.IsNullOrEmpty(Cookie) || string.IsNullOrWhiteSpace(Cookie))
        {
            throw new Exception("Invalid Cookie");
        }


        var cont = new
        {
            cookie = Cookie
        };

        var content = new StringContent(JsonConvert.SerializeObject(cont), Encoding.UTF8, "application/json");

        await ToPostResponse(content, URIs.LOGIN_COOKIE_CLI_POST);

        return ToObject<ResultLoginCliDTO>();

    }

}
