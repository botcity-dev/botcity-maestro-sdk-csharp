namespace Dev.BotCity.MaestroSdk.Model.AutomationTask
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public enum StateEnum { START, RUNNING, FINISHED, CANCELED };
    public enum FinishStatusEnum { SUCCESS, FAILED, PARTIALLY_COMPLETED };

    public partial class AutomationTask
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("state")]
        public StateEnum State { get; set; }

        [JsonProperty("parameters")]
        public dynamic Parameters { get; set; }

        [JsonProperty("inputFile")]
        public dynamic InputFile { get; set; }

        [JsonProperty("agentId")]
        public string AgentId { get; set; }

        [JsonProperty("userEmail")]
        public dynamic UserEmail { get; set; }

        [JsonProperty("userCreationName")]
        public string UserCreationName { get; set; }

        [JsonProperty("organizationLabel")]
        public string OrganizationLabel { get; set; }

        [JsonProperty("dateCreation")]
        public DateTime DateCreation { get; set; }

        [JsonProperty("dateLastModified")]
        public DateTime? DateLastModified { get; set; }

        [JsonProperty("finishStatus")]
        public FinishStatusEnum? FinishStatus { get; set; }

        [JsonProperty("finishMessage")]
        public dynamic FinishMessage { get; set; }

        [JsonProperty("test")]
        public bool Test { get; set; }

        [JsonProperty("machineId")]
        public dynamic MachineId { get; set; }

        [JsonProperty("activityLabel")]
        public string ActivityLabel { get; set; }

        [JsonProperty("interrupted")]
        public bool Interrupted { get; set; }

        [JsonProperty("minExecutionDate")]
        public DateTime? MinExecutionDate { get; set; }

        [JsonProperty("killed")]
        public bool Killed { get; set; }

        [JsonProperty("dateStartRunning")]
        public DateTime? DateStartRunning { get; set; }

        [JsonProperty("priority")]
        public long Priority { get; set; }

        [JsonProperty("repositoryLabel")]
        public string RepositoryLabel { get; set; }

        [JsonProperty("processedItems")]
        public long? ProcessedItems { get; set; }

        [JsonProperty("failedItems")]
        public long? FailedItems { get; set; }

        [JsonProperty("totalItems")]
        public long? TotalItems { get; set; }

        [JsonProperty("activityName")]
        public string ActivityName { get; set; }

        public bool IsInterrupted() {
            return this.Interrupted;
        }
    }

    public partial class AutomationTask
    {
        public static AutomationTask FromJson(string json) => JsonConvert.DeserializeObject<AutomationTask>(json, Dev.BotCity.MaestroSdk.Model.AutomationTask.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this AutomationTask self) => JsonConvert.SerializeObject(self, Dev.BotCity.MaestroSdk.Model.AutomationTask.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
