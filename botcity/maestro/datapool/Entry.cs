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
using System.Globalization;
using Dev.BotCity.MaestroSdk.Utils;

namespace Dev.BotCity.MaestroSdk.Model.DatapoolEntry
{  
    public enum StateEntryEnum {
        PENDING,
        PROCESSING,
        TIMEOUT,
        DONE,
        ERROR
    }
    public partial class DatapoolEntry
    {
        public BotMaestroSDK Maestro { get; set; } = null;

        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("datapoolLabel")]
        public string DatapoolLabel { get; set; }

        [JsonProperty("values")]
        public Dictionary<string, object> Values { get; set; }
        
        [JsonProperty("state")]
        public StateEntryEnum? State { get; set;}

        [JsonProperty("id")]
        public string EntryId { get; set; }


        [JsonProperty("taskId")]
        public string TaskId { get; set; }

        [JsonProperty("parent")]
        public string Parent { get; set; }

        [JsonProperty("child")]
        public string Child { get; set; }

        [JsonProperty("dateRegister")]
        public DateTime? DateRegister { get; set; }

        [JsonProperty("dateProcessing")]
        public DateTime? DateProcessing { get; set; }

        [JsonProperty("dateFinished")]
        public DateTime? DateFinished { get; set; }

        public DatapoolEntry(
            int priority = 0, 
            Dictionary<string, object> values = null,
            BotMaestroSDK maestro = null, 
            StateEntryEnum? state = null,
            string entryId = null,
            string taskId = null,
            string parent = null,
            string child = null,
            DateTime? dateRegister = null,
            DateTime? dateProcessing = null,
            DateTime? dateFinished = null
        ) {
                Priority = priority;
                Values = values;
                if (values == null) {
                    Values = new Dictionary<string, object>{};
                }
                Maestro = maestro;
                State = state;
                EntryId = entryId;
                TaskId = taskId;
                Parent = parent;
                Child = child;
                DateRegister = dateRegister;
                DateProcessing = dateProcessing;
                DateFinished = dateFinished;
            }

        public void UpdateFromJson(string payload)
        {
            var values = DatapoolEntry.FromJson(payload);
            Priority = values.Priority;
            Values = values.Values;
            Maestro = values.Maestro;
            State = values.State;
            EntryId = values.EntryId;
            TaskId = values.TaskId;
            Parent = values.Parent;
            Child = values.Child;
            DateRegister = values.DateRegister;
            DateProcessing = values.DateProcessing;
            DateFinished = values.DateFinished;
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
                { "priority", this.Priority },
                { "values", this.Values },
            };
            return data;
        }

        public Dictionary<string, object> JsonToUpdate() {
            var data = new Dictionary<string, object>
            {
                { "priority", this.Priority },
                { "values", this.Values },
                { "dataPoolLabel", this.DatapoolLabel },
                { "state", this.State.ToString() },
                { "taskId", this.TaskId },
                { "parent", this.Parent },
                { "child", this.Child },
            };
            return data;
        }

        /// <summary>
        /// Updates an entry in the DataPool.
        /// </summary>
        /// <param name="taskId">Optional task ID associated with this entry.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the updated DataPoolEntry.</returns>
        public async Task<DatapoolEntry> SaveAsync(string? taskId = null) {
            if (!this.Maestro.CheckAccessTokenAvailable()) {
                return DatapoolEntry.FromJson("{}");
            }
            string url = $"{this.Maestro.GetServer()}/api/v2/datapool/{this.DatapoolLabel}/entry/{this.EntryId}";
            var data = JsonConvert.SerializeObject(this.JsonToUpdate());
            var dataDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
            var updatedData = JsonConvert.SerializeObject(dataDict);

            using (var client = new HttpClient(Handler.Get(this.Maestro.GetVerifySSL()))) {
                client.AddDefaultHeaders(this.Maestro.GetAccessToken(), this.Maestro.GetLogin(), 30);
                var content = new StringContent(updatedData, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);
                verifyResponse(response, "Error in save entry");
                var payload = await response.Content.ReadAsStringAsync();
                UpdateFromJson(payload);
                return this;
            }
        }
        
        private async Task Report(StateEntryEnum state) {
            this.State = state;
            await this.SaveAsync();
        }

        /// <summary>
        /// Reports the state DONE to a DataPool Entry.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ReportDoneAsync() {
            await this.Report(StateEntryEnum.DONE);
        }

        /// <summary>
        /// Reports the state ERROR to a DataPool Entry.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task ReportErrorAsync() {
            await this.Report(StateEntryEnum.ERROR);
        }

        public string GetValue(string key, string defaultValue = null) {
            if (this.Values.TryGetValue(key, out var value)) {
                return value as string ?? defaultValue;
            }
            return defaultValue;
        }

        public string this[string key]
        {
            get {
                if (this.Values.ContainsKey(key))
                {
                    return this.Values[key]?.ToString();
                }
                var property = this.GetType().GetProperty(key);
                if (property != null)
                {
                    return (string)property.GetValue(this);
                }
                throw new KeyNotFoundException($"The key '{key}' was not found.");
            }
            set {
                if (this.Values.ContainsKey(key))
                {
                    this.Values[key] = value;
                }
                else
                {
                    var property = this.GetType().GetProperty(key);
                    if (property != null)
                    {
                        property.SetValue(this, value);
                    }
                    else
                    {
                        this.Values[key] = value;
                    }
                }
            }
        }
    
        private void VerifyState(StateEntryEnum? state)
        {
            if (State == null)
            {
                return;
            }

            if (State == state)
            {
                return;
            }

            if (State == StateEntryEnum.PENDING)
            {
                var allowedStates = new List<StateEntryEnum> { StateEntryEnum.PROCESSING };
                if (!allowedStates.Contains(state.Value))
                {
                    throw new ArgumentException($"In state {this.State}, only change to states {string.Join(", ", allowedStates)} is allowed.");
                }
            }

            if (State == StateEntryEnum.PROCESSING)
            {
                var allowedStates = new List<StateEntryEnum> { StateEntryEnum.TIMEOUT, StateEntryEnum.DONE, StateEntryEnum.ERROR };
                if (!allowedStates.Contains(state.Value))
                {
                    throw new ArgumentException($"In state {this.State}, only change to states {string.Join(", ", allowedStates)} is allowed.");
                }
            }

            if (State == StateEntryEnum.TIMEOUT)
            {
                var allowedStates = new List<StateEntryEnum> { StateEntryEnum.DONE, StateEntryEnum.ERROR };
                if (!allowedStates.Contains(state.Value))
                {
                    throw new ArgumentException($"In state {this.State}, only change to states {string.Join(", ", allowedStates)} is allowed.");
                }
            }
        }
    }

    public partial class DatapoolEntry
    {
        public static DatapoolEntry FromJson(string json) => JsonConvert.DeserializeObject<DatapoolEntry>(json, Dev.BotCity.MaestroSdk.Model.DatapoolEntry.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this DatapoolEntry self) => JsonConvert.SerializeObject(self, Dev.BotCity.MaestroSdk.Model.DatapoolEntry.Converter.Settings);
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
