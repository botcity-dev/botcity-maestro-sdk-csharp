using Newtonsoft.Json;

namespace BotCityMaestroSDK.Dtos;
public class Column
{
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("label")]
    public string Label { get; set; }

    [JsonProperty("width")]
    public int Width { get; set; }
    

}