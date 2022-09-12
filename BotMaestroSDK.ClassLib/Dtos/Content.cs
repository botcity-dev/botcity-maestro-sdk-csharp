using Newtonsoft.Json;

namespace BotCityMaestroSDK.Dtos;
public class Content
{


    [JsonProperty("empty")]
    public int Empty { get; set; }

    [JsonProperty("additionalProp1")]
    public object additionalProp1 { get; set; }

    [JsonProperty("additionalProp2")]
    public object additionalProp2 { get; set; }

    [JsonProperty("additionalProp3")]
    public object additionalProp3 { get; set; }
    
    


}