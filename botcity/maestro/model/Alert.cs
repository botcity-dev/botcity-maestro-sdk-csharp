namespace Dev.BotCity.MaestroSdk.Model.Alert
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public enum AlertTypeEnum { INFO, WARN, ERROR }

    public partial class Alert
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("organizationLabel")]
        public string OrganizationLabel { get; set; }

        [JsonProperty("taskId")]
        public int TaskId { get; set; }

        [JsonProperty("activityName")]
        public string ActivityName { get; set; }

        [JsonProperty("activityLabel")]
        public string ActivityLabel { get; set; }

        [JsonProperty("botId")]
        public string BotId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("type")]
        public AlertTypeEnum Type { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("repositoryLabel")]
        public string RepositoryLabel { get; set; }
    }

    public partial class Alert
    {
        public static Alert FromJson(string json) => JsonConvert.DeserializeObject<Alert>(json, Dev.BotCity.MaestroSdk.Model.Alert.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Alert self) => JsonConvert.SerializeObject(self, Dev.BotCity.MaestroSdk.Model.Alert.Converter.Settings);
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
