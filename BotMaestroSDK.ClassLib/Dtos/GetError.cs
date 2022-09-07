using Newtonsoft.Json;

namespace BotCityMaestroSDK.Dtos;
public class GetError
{
    [JsonProperty("message")]
    public string Message { get; set; }
    public string ErroDetail { get; set; }
    public int ErrorNumber { get; set; }

}