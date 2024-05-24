namespace Dev.BotCity.MaestroSdk.Model.Log
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    public partial class Column
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("label")]
        public string Label;

        [JsonProperty("width")]
        public int Width;

        [JsonProperty("visible")]
        public bool Visible;        
        
        public Column(string name = null, string label = null, int width = 0, bool visible = true)
        {
            Name = name;
            Label = label;
            Width = width;
            Visible = visible;
        }
    }

    public class Entry{
        public Dictionary<string, object> Columns { get; set; }
    }

    public partial class Log
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("organizationLabel")]
        public string OrganizationLabel;

        [JsonProperty("label")]
        public string Label;

        [JsonProperty("dateCreation")]
        public DateTime DateCreation;

        [JsonProperty("columns")]
        public List<Column> Columns;

        [JsonProperty("repositoryLabel")]
        public string RepositoryLabel;

        [JsonProperty("activityLabel")]
        public object ActivityLabel;
    }

    public partial class Log
    {
        public static Log FromJson(string json) => JsonConvert.DeserializeObject<Log>(json, Dev.BotCity.MaestroSdk.Model.Log.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Log self) => JsonConvert.SerializeObject(self, Dev.BotCity.MaestroSdk.Model.Log.Converter.Settings);
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
