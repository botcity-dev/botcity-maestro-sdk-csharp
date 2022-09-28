
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCityMaestroSDK.Dtos.Task
{
    public class ResultLogEntryDTO
    {
        [JsonProperty("totalPages")]
        public int TotalPages { get; set; }

        [JsonProperty("totalElements")]
        public int TotalElements { get; set; }

        [JsonProperty("pageable")]
        public Pageable Pageable { get; set; }

        [JsonProperty("sort")]
        public Sort Sort { get; set; }

        [JsonProperty("first")]
        public bool first { get; set; }

        [JsonProperty("last")]
        public bool last { get; set; }

        [JsonProperty("numberOfElements")]
        public int numberOfElements { get; set; }

        [JsonProperty("size")]
        public int size { get; set; }

        [JsonProperty("content")]
        public List<Content> Content { get; set; }
        

        [JsonProperty("number")]
        public int number { get; set; }

        [JsonProperty("empty")]
        public bool empty { get; set; }

        public ResultLogEntryDTO()
        {
     
        }

      
    }

}
