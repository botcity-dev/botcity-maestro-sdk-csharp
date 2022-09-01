using Newtonsoft.Json;

namespace NsBotCityMaestroSDK.ClassLib.Dtos.Task;
public class Param
{
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("value")]
    public string Value { get; set; }

}