using Newtonsoft.Json;
using NsBotCityMaestroSDK.ClassLib.Dtos.Task;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NsBotMaestroSDK.ClassLib.Dtos.Task
{
    public class ResultTaskDTO
    {
        [JsonProperty("id")]
        public int? Id { get; set; }
        [JsonProperty("state")]
        public string? State { get; set; }

        [JsonProperty("Parameters")]
        public ListParameters Parameters { get; set; }

        [JsonProperty("InputFile")]
        public InputFileDTO? InputFile { get; set; }

        [JsonProperty("ActivityId")]
        public string? ActivityId { get; set; }

        [JsonProperty("AgentId")]
        public string? AgentId { get; set; }

        [JsonProperty("UserCreationId")]
        public int? UserCreationId { get; set; }

        [JsonProperty("UserCreationName")]
        public string? UserCreationName { get; set; }

        [JsonProperty("OrganizationCreationId")]
        public int? OrganizationCreationId { get; set; }

        [JsonProperty("DateCreation")]
        public DateTime? DateCreation { get; set; }

        [JsonProperty("DateLastModified")]
        public DateTime? DateLastModified { get; set; }

        [JsonProperty("FinishStatus")]
        public string? FinishStatus { get; set; }

        [JsonProperty("FinishMessage")]
        public string? FinishMessage { get; set; }

        [JsonProperty("Test")]
        public bool? Test { get; set; }

        [JsonProperty("ActivityLabel")]
        public string? ActivityLabel { get; set; }

        [JsonProperty("Days")]
        public int? Days { get; set; }

        [JsonProperty("StateFilter")]
        public string? StateFilter { get; set; }



       

    }
}
