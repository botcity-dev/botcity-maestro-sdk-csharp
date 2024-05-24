namespace Dev.BotCity.MaestroSdk.Model.Datapool
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public enum DatapoolConsumptionPolicyEnum { FIFO, LIFO }
    public enum DatapoolTriggerEnum { ACTIVE, INACTIVE }

    public partial class Schema
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class Datapool
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("defaultActivity")]
        public string DefaultActivity { get; set; }

        [JsonProperty("consumptionPolicy")]
        public DatapoolConsumptionPolicyEnum ConsumptionPolicy { get; set; }

        [JsonProperty("schema")]
        public Schema[] Schema { get; set; }

        [JsonProperty("trigger")]
        public DatapoolTriggerEnum Trigger { get; set; }

        [JsonProperty("autoRetry")]
        public bool AutoRetry { get; set; }

        [JsonProperty("maxAutoRetry")]
        public long MaxAutoRetry { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("abortOnError")]
        public bool AbortOnError { get; set; }

        [JsonProperty("maxErrorsBeforeInactive")]
        public long MaxErrorsBeforeInactive { get; set; }

        [JsonProperty("itemMaxProcessingTime")]
        public long ItemMaxProcessingTime { get; set; }

        [JsonProperty("dateTimeout")]
        public string DateTimeout { get; set; }

        [JsonProperty("repositoryLabel")]
        public string RepositoryLabel { get; set; }
    }

    public partial class Datapool
    {
        public static Datapool FromJson(string json) => JsonConvert.DeserializeObject<Datapool>(json, Dev.BotCity.MaestroSdk.Model.Datapool.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Datapool self) => JsonConvert.SerializeObject(self, Dev.BotCity.MaestroSdk.Model.Datapool.Converter.Settings);
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
