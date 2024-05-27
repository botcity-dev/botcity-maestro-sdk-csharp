namespace Dev.BotCity.MaestroSdk.Model.Artifact
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Artifact
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("taskId")]
        public string TaskId { get; set; }

        [JsonProperty("taskName")]
        public string TaskName { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("storageFileName")]
        public string StorageFileName { get; set; }

        [JsonProperty("storageFilePath")]
        public string StorageFilePath { get; set; }

        [JsonProperty("organizationId")]
        public string Organization { get; set; }

        [JsonProperty("user")]
        public string? User { get; set; }

        [JsonProperty("dateCreation")]
        public DateTime? DateCreation { get; set; }
    }

    public partial class Artifact
    {
        public static Artifact FromJson(string json) => JsonConvert.DeserializeObject<Artifact>(json, Dev.BotCity.MaestroSdk.Model.Artifact.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Artifact self) => JsonConvert.SerializeObject(self, Dev.BotCity.MaestroSdk.Model.Artifact.Converter.Settings);
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
