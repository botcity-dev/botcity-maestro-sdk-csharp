
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System;

using BotCity.ClassLib.Dtos;
namespace BotCity.ClassLib;

public partial class BotMaestroSDK
{


    public async Task<TokenLoginDTO> Login(string UserName, string Password){

       
        var content = ToContent<SendLoginDTO>(UserName, Password);

        await ToPostResponse(content, URIs_Login.LOGIN_POST);

        return ToObject<TokenLoginDTO>();
        
    }

    public async Task<TokenLoginStudioDTO> LoginStudio(string UserName, string Password){


        var content = ToContent<SendLoginStudioDTO>(UserName, Password);

        await ToPostResponse(content, URIs_Login.LOGIN_STUDIO_POST);

        return ToObject<TokenLoginStudioDTO>();


    }

    public async Task<TokenLoginStudioDTO> LoginCookie(string Cookie){


        var cont = new {
             cookie = Cookie
        };

        var content = new StringContent(JsonConvert.SerializeObject(cont), Encoding.UTF8, "application/json");

        await ToPostResponse(content, URIs_Login.LOGIN_COOKIE_POST);

        return ToObject<TokenLoginStudioDTO>();

    }

    public async Task<TokenLoginStudioDTO> LoginCookie(){

        
        string Cookie = this.TokenLoginStudioDTO.New_Cookie;

        if ( string.IsNullOrEmpty(Cookie) || string.IsNullOrWhiteSpace(Cookie) ){
            throw new Exception("Invalid Cookie");
        }
            

        var cont = new {
             cookie = Cookie
        };

        var content = new StringContent(JsonConvert.SerializeObject(cont), Encoding.UTF8, "application/json");

        await ToPostResponse(content, URIs_Login.LOGIN_COOKIE_POST);

        return ToObject<TokenLoginStudioDTO>();
       

    }


    public async Task<TokenLoginCliDTO> LoginCli(string UserName, string Password)
    {

        var content = ToContent<SendLoginCliDTO>(UserName, Password);

        await ToPostResponse(content, URIs_Login.LOGIN_CLI_POST);

        return ToObject<TokenLoginCliDTO>();

    }


    public async Task<TokenLoginCliDTO> LoginCookieCli(string Cookie)
    {


        var cont = new
        {
            cookie = Cookie
        };

        var content = new StringContent(JsonConvert.SerializeObject(cont), Encoding.UTF8, "application/json");

        await ToPostResponse(content, URIs_Login.LOGIN_COOKIE_CLI_POST);

        return ToObject<TokenLoginCliDTO>();

    }

    public async Task<TokenLoginCliDTO> LoginCookieCli()
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

        await ToPostResponse(content, URIs_Login.LOGIN_COOKIE_CLI_POST);

        return ToObject<TokenLoginCliDTO>();

    }

}
