using Newtonsoft.Json;

namespace BotCityMaestroSDK.Dtos;
public class Sort
{
    [JsonProperty("sorted")]
    public bool Sorted { get; set; }


    [JsonProperty("unsorted")]
    public bool Unsorted { get; set; }


    [JsonProperty("empty")]
    public bool Empty { get; set; }
    

    

}