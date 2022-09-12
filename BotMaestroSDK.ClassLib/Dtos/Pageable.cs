using Newtonsoft.Json;

namespace BotCityMaestroSDK.Dtos;
public class Pageable
{
    [JsonProperty("pageNumber")]
    public int PageNumber { get; set; }

    [JsonProperty("pageSize")]
    public int PageSize { get; set; }

    [JsonProperty("sort")]
    public Sort Sort { get; set; }

    [JsonProperty("paged")]
    public bool Paged { get; set; }

    [JsonProperty("unpaged")]
    public bool UnPaged { get; set; }

    [JsonProperty("offset")]
    public int OffSet { get; set; } 

}