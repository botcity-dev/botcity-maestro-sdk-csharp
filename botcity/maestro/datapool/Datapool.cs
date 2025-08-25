using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Dev.BotCity.MaestroSdk.Utils;

namespace Dev.BotCity.MaestroSdk.Model.Datapool
{
    using Dev.BotCity.MaestroSdk.Model.Summary;
    using Dev.BotCity.MaestroSdk.Model.DatapoolEntry;

    public enum DatapoolConsumptionPolicyEnum { FIFO, LIFO }
    public enum DatapoolTriggerEnum { ALWAYS, NEVER, NO_TASK_ACTIVE}

    public partial class Schema
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class Datapool
    {
        public BotMaestroSDK Maestro { get; set; } = null;

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }


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
        
        public Datapool(
            string label, 
            string defaultActivity, 
            BotMaestroSDK maestro = null, 
            DatapoolConsumptionPolicyEnum consumptionPolicy = DatapoolConsumptionPolicyEnum.FIFO, 
            DatapoolTriggerEnum trigger = DatapoolTriggerEnum.NEVER, 
            Schema[] schema = null, 
            bool autoRetry = true, 
            int maxAutoRetry = 0, 
            bool abortOnError = true, 
            int maxErrorsBeforeInactive = 0, 
            int itemMaxProcessingTime = 0, 
            string id = null, 
            bool active = true, 
            string repositoryLabel = "DEFAULT"
        ) {
                Label = label;
                DefaultActivity = defaultActivity;
                ConsumptionPolicy = consumptionPolicy;
                Trigger = trigger;
                Schema = schema;
                AutoRetry = autoRetry;
                MaxAutoRetry = maxAutoRetry;
                AbortOnError = abortOnError;
                MaxErrorsBeforeInactive = maxErrorsBeforeInactive;
                ItemMaxProcessingTime = itemMaxProcessingTime;
                Id = id;
                Maestro = maestro;
                Active = active;
                RepositoryLabel = repositoryLabel;
            }

        public void UpdateFromJson(string payload)
        {
            var values = Datapool.FromJson(payload);

            Id = values.Id;
            Label = values.Label;
            DefaultActivity = values.DefaultActivity;
            ConsumptionPolicy = values.ConsumptionPolicy;
            Schema = values.Schema;
            Trigger = values.Trigger;
            AutoRetry = values.AutoRetry;
            MaxAutoRetry = values.MaxAutoRetry;
            ItemMaxProcessingTime = values.ItemMaxProcessingTime;
            MaxErrorsBeforeInactive = values.MaxErrorsBeforeInactive;
            AbortOnError = values.AbortOnError;
            Active = values.Active;
            RepositoryLabel = values.RepositoryLabel;
        }

        /// <summary>
        /// Enables the DataPool in Maestro.
        /// </summary>
        public async Task<bool> ActivateAsync() {
            return await this.ActiveAsync(true);
        }

        private async Task<bool> ActiveAsync(bool active) {
            var data = JsonConvert.SerializeObject(this);
            
            var dataDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            dataDict["active"] = active;
            var updatedData = JsonConvert.SerializeObject(dataDict);
            if (!this.Maestro.CheckAccessTokenAvailable()) {
                UpdateFromJson(updatedData);
                return true;
            }
            string url = $"{this.Maestro.GetServer()}/api/v2/datapool/{this.Label}";

            using (var client = new HttpClient()) {
                client.DefaultRequestHeaders.UserAgent.ParseAdd($"csharp-requests/{System.Environment.Version}");
                client.AddDefaultHeaders(this.Maestro.GetAccessToken(), this.Maestro.GetLogin(), 30);
                var content = new StringContent(updatedData, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var payload = await response.Content.ReadAsStringAsync();
                    UpdateFromJson(payload);
                    return true;
                }
                else
                {
                    response.EnsureSuccessStatusCode();
                    return false;
                }
            }
        }

        /// <summary>
        /// Disable the DataPool in Maestro.
        /// </summary>
        public async Task<bool> DeactivateAsync() {
            return await this.ActiveAsync(false);
        }
        
        /// <summary>
        /// Check if the DataPool is active.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result is a boolean indicating if the DataPool is active.</returns>
        public async Task<bool> IsActiveAsync() {
            string url = $"{this.Maestro.GetServer()}/api/v2/datapool/{this.Label}";
            if (!this.Maestro.CheckAccessTokenAvailable()) {
                return this.Active;
            }
            using (var client = new HttpClient(Handler.Get(this.Maestro.GetVerifySSL()))) {
                client.DefaultRequestHeaders.UserAgent.ParseAdd($"csharp-requests/{System.Environment.Version}");
                client.AddDefaultHeaders(this.Maestro.GetAccessToken(), this.Maestro.GetLogin(), 30);

                var response = await client.GetAsync(url);
                verifyResponse(response, "Error in get datapool");

                var payload = await response.Content.ReadAsStringAsync();
                UpdateFromJson(payload);
                return this.Active;
            }
        }

        /// <summary>
        /// Retrieves the DataPool counters.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result is a Summary object containing the DataPool counters.</returns>
        public async Task<Summary> GetSummaryAsync() {
            string url = $"{this.Maestro.GetServer()}/api/v2/datapool/{this.Label}/summary";
            if (!this.Maestro.CheckAccessTokenAvailable()) {
                return Summary.FromJson("{}");
            }
            using (var client = new HttpClient(Handler.Get(this.Maestro.GetVerifySSL()))) {
                client.DefaultRequestHeaders.UserAgent.ParseAdd($"csharp-requests/{System.Environment.Version}");
                client.AddDefaultHeaders(this.Maestro.GetAccessToken(), this.Maestro.GetLogin(), 30);

                var response = await client.GetAsync(url);
                verifyResponse(response, "Error in get summary");

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return Summary.FromJson(jsonResponse);
            }
        }

        /// <summary>
        /// Checks if the DataPool is empty.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the DataPool is empty, false otherwise.</returns>
        public async Task<bool> IsEmptyAsync() {
            Summary summary = await this.GetSummaryAsync();
            if(summary.CountPending == 0) {
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// Checks if there are pending items in the DataPool.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result is true if there are pending items, false otherwise.</returns>
        public async Task<bool> HasNextAsync() {
            return !(await this.IsEmptyAsync());
        }

        private async void verifyResponse(HttpResponseMessage data, string error) {
            if (data.IsSuccessStatusCode) {
                return;
            }
            var errorMessage = $"{error}. Server returned {data.StatusCode}.";
            try {
                var jsonResponse = await data.Content.ReadAsStringAsync();
            } catch (System.Text.Json.JsonException) {
                errorMessage += $" {await data.Content.ReadAsStringAsync()}";
            }
            throw new Exception(errorMessage);
 
        }

        public Dictionary<string, object> ToJson() {
            var data = new Dictionary<string, object>
            {
                { "label", this.Label },
                { "defaultAutomation", this.DefaultActivity },
                { "consumptionPolicy", this.ConsumptionPolicy },
                { "schema", this.Schema },
                { "trigger", this.Trigger },
                { "autoRetry", this.AutoRetry },
                { "maxAutoRetry", this.MaxAutoRetry },
                { "abortOnError", this.AbortOnError },
                { "maxErrorsBeforeInactive", this.MaxErrorsBeforeInactive },
                { "itemMaxProcessingTime", this.ItemMaxProcessingTime },
                { "active", this.Active },
                { "repositoryLabel", "DEFAULT" },
            };
            return data;
        }

        /// <summary>
        /// Fetches the next pending entry.
        /// </summary>
        /// <param name="taskId">Optional task ID to be associated with this entry.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the next pending DataPoolEntry, or null if there are no pending entries.</returns>
        public async Task<DatapoolEntry> NextAsync(string? taskId = null) {
            string url = $"{this.Maestro.GetServer()}/api/v2/datapool/{this.Label}/pull";
            if (!this.Maestro.CheckAccessTokenAvailable()) {
                return DatapoolEntry.FromJson("{}");
            }
            using (var client = new HttpClient(Handler.Get(this.Maestro.GetVerifySSL()))) {
                client.DefaultRequestHeaders.UserAgent.ParseAdd($"csharp-requests/{System.Environment.Version}");
                client.AddDefaultHeaders(this.Maestro.GetAccessToken(), this.Maestro.GetLogin(), 30);

                var response = await client.GetAsync(url);
                verifyResponse(response, "Error in get next entry");

                var jsonResponse = await response.Content.ReadAsStringAsync();
                if (jsonResponse == "") {
                    return null;
                }
                var entry = JsonConvert.DeserializeObject<DatapoolEntry>(jsonResponse);
                entry.TaskId = taskId;
                entry.Maestro = this.Maestro;
                return entry;
            }
        }

        /// <summary>
        /// Fetches an entry from the DataPool by its ID.
        /// </summary>
        /// <param name="entryId">The ID of the entry to fetch.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the DataPoolEntry that was fetched.</returns>
        public async Task<DatapoolEntry> GetEntryAsync(string entryId) {
            string url = $"{this.Maestro.GetServer()}/api/v2/datapool/{this.Label}/entry/{entryId}";
            if (!this.Maestro.CheckAccessTokenAvailable()) {
                return DatapoolEntry.FromJson("{}");
            }
            using (var client = new HttpClient(Handler.Get(this.Maestro.GetVerifySSL()))) {
                client.DefaultRequestHeaders.UserAgent.ParseAdd($"csharp-requests/{System.Environment.Version}");
                client.AddDefaultHeaders(this.Maestro.GetAccessToken(), this.Maestro.GetLogin(), 30);

                var response = await client.GetAsync(url);
                verifyResponse(response, "Error in get entry");

                var jsonResponse = await response.Content.ReadAsStringAsync();
                if (jsonResponse == "") {
                    return null;
                }
                var entry = JsonConvert.DeserializeObject<DatapoolEntry>(jsonResponse);
                entry.Maestro = this.Maestro;
                return entry;
            }
        }

        /// <summary>
        /// Creates an entry in the DataPool.
        /// </summary>
        /// <param name="entry">The DataPoolEntry instance to create.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the DataPoolEntry that was created.</returns>
        public async Task<DatapoolEntry> CreateEntryAsync(DatapoolEntry entry) {
            if (!this.Maestro.CheckAccessTokenAvailable()) {
                return DatapoolEntry.FromJson("{}");
            }
            string url = $"{this.Maestro.GetServer()}/api/v2/datapool/{this.Label}/push";
            var data = JsonConvert.SerializeObject(entry.ToJson());
            using (var client = new HttpClient(Handler.Get(this.Maestro.GetVerifySSL()))) {
                client.DefaultRequestHeaders.UserAgent.ParseAdd($"csharp-requests/{System.Environment.Version}");
                client.AddDefaultHeaders(this.Maestro.GetAccessToken(), this.Maestro.GetLogin(), 30);
                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                if (jsonResponse == "") {
                    return null;
                }
                var entryResponse = JsonConvert.DeserializeObject<DatapoolEntry>(jsonResponse);
                entryResponse.Maestro = this.Maestro;
                return entryResponse;
            }
        }
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
