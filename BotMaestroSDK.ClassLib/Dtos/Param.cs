using Newtonsoft.Json;

namespace BotCityMaestroSDK.Dtos;
public class Param
{
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("value")]
    public string Value { get; set; }

}