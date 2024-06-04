namespace Dev.BotCity.MaestroSdk.Model.Summary
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Summary
    {
        [JsonProperty("countPending")]
        public int CountPending { get; set; }

        [JsonProperty("countProcessing")]
        public string CountProcessing { get; set; }

        [JsonProperty("countDone")]
        public string CountDone { get; set; }

        [JsonProperty("avgDone")]
        public string AvgDone { get; set; }

        [JsonProperty("countTimeout")]
        public int CountTimeout { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }
    }

    public partial class Summary
    {
        public static Summary FromJson(string json) => JsonConvert.DeserializeObject<Summary>(json, Dev.BotCity.MaestroSdk.Model.Summary.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Summary self) => JsonConvert.SerializeObject(self, Dev.BotCity.MaestroSdk.Model.Summary.Converter.Settings);
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
