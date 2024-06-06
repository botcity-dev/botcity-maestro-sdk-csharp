using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using Dev.BotCity.MaestroSdk.Model.AutomationTask;
using Dev.BotCity.MaestroSdk.Model.Alert;
using Dev.BotCity.MaestroSdk.Model.Log;
using Dev.BotCity.MaestroSdk.Model.Datapool;
using Dev.BotCity.MaestroSdk.Model.Message;
using Dev.BotCity.MaestroSdk.Model.Artifact;
using Dev.BotCity.MaestroSdk.Model.Summary;
using Dev.BotCity.MaestroSdk.Model.DatapoolEntry;
using Dev.BotCity.MaestroSdk.Model.Execution;
using System.Text.Json.Serialization;
using Dev.BotCity.MaestroSdk.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.StaticFiles;
using System.Globalization;


public class BotMaestroSDK {
        public bool notifiedDisconnect { get; set; } = false;
        public bool _raiseNotConnected { get; set; } = true;
        public bool _verifySSLCert { get; set; } = false;
        
        private string _server = "";
        private string _login = "";

        private string _key = "";

        private string _accessToken = "";
        private string _taskId = "";

        public BotMaestroSDK(string server = "", string login = "", string key = "", string taskId = "", bool notifiedDisconnect = false, bool raiseNotConnected = true, bool verifySSLCert = false)
        {
            notifiedDisconnect = notifiedDisconnect;
            _raiseNotConnected = raiseNotConnected;
            _verifySSLCert = verifySSLCert;
            _server = server;
            _login = login;
            _key = key;
            _taskId = taskId;
        }

        public string GetLogin()
        {
            return _login;
        }

        public void SetLogin(string newValue)
        {
            _login = newValue;
        }

        public string GetServer()
        {
            return _server;
        }

        public void SetServer(string newValue)
        {
            if (!string.IsNullOrEmpty(newValue) && newValue.EndsWith("/")){
                newValue = newValue.Substring(0, newValue.Length - 1);
            }
            _server = newValue;
        }
        
        public string GetKey()
        {
            return _key;
        }

        public void SetKey(string newValue)
        {
            _key = newValue;
        }

        public string GetAccessToken()
        {
            return _accessToken;
        }

        public void SetAccessToken(string newValue)
        {
            _accessToken = newValue;
        }
        private StringContent GetContent(object data) {
            string jsonBody = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            return content; 
        }
        
        public string GetTaskId()
        {
            return _taskId;
        }

        public void SetTaskId(string newValue)
        {
            _taskId = newValue;
        }
    
        public static BotMaestroSDK FromSysArgs(string defaultServer = "", string defaultLogin = "", string defaultKey = "")
        {
            string[] args = Environment.GetCommandLineArgs();
            BotMaestroSDK maestro;

            if (args.Length >= 4) {
                string server = args[1];
                string taskId = args[2];
                string token = args[3];
                string organization = args.Length > 4 ? args[4] : string.Empty;
                maestro = new BotMaestroSDK();
                maestro.SetServer(server);
                maestro.SetTaskId(taskId);
                maestro.SetAccessToken(token);
                maestro.SetLogin(organization);
            } else {
                maestro = new BotMaestroSDK(defaultServer, defaultLogin, defaultKey);
            }
            return maestro;
        }

        public async Task<Execution> GetExecutionAsync(string taskId = "") {
            taskId = taskId ?? this._taskId;
            if (string.IsNullOrEmpty(_accessToken)) {
                return new Execution("", taskId, "", new Dictionary<string, object>{});
            }

            if (taskId == null) {
                throw new Exception("A task ID must be informed either via the parameter or the class property.");
            }

            AutomationTask task = await this.GetTaskAsync(taskId);
            JObject parametersObject = JObject.FromObject(task.Parameters);
            Dictionary<string, object> parametersDictionary = parametersObject.ToObject<Dictionary<string, object>>();

            return new Execution(_server, taskId, _accessToken, parametersDictionary);
        }

        private Dictionary<string, object> ToDictionary(dynamic item)
        {
            var dictionary = new Dictionary<string, object>();
            foreach (var property in (IDictionary<string, object>)item)
            {
                dictionary.Add(property.Key, property.Value);
            }
            return dictionary;
        }

        public async Task Login(string server = "", string login = "", string key = "") {
            using (HttpClient client = new HttpClient()) {
                try {
                    var data = new { login = _login, key = _key };
                    HttpResponseMessage response = await client.PostAsync($"{_server}/api/v2/workspace/login", GetContent(data));
                    if (response.IsSuccessStatusCode){
                        string responseBody = await response.Content.ReadAsStringAsync();
                        _accessToken = System.Text.Json.JsonDocument.Parse(responseBody).RootElement.GetProperty("accessToken").GetString();
                    } else {
                        string error = await response.Content.ReadAsStringAsync();
                        throw new InvalidOperationException($"Error during login. Server returned {response.StatusCode}. {error}");
                    }    
                } catch (HttpRequestException error) {
                    Console.WriteLine($"Error during request: {error.Message}");
                }
            }
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
        public async Task<AutomationTask> CreateTaskAsync(string activityLabel, Dictionary<string, object> parameters = null,
            bool test = false, int priority = 0, DateTime? minExecutionDate = null)
        {
            string url = $"{_server}/api/v2/task";
            var data = new Dictionary<string, object>
            {
                { "activityLabel", activityLabel },
                { "test", test },
                { "parameters", parameters },
                { "priority", priority }
            };

            data["minExecutionDate"] = Validator.ValidateDateTime(minExecutionDate, "minExecutionDate");
            

            var content = HttpContentFactory.CreateJsonContent(data);

            using (var client = new HttpClient())
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);

                var response = await client.PostAsync(url, content);

                verifyResponse(response, "Error during create task");

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return AutomationTask.FromJson(jsonResponse);
            }
        }

        public async Task<AutomationTask> GetTaskAsync(string taskId)
        {
            string url = $"{_server}/api/v2/task/{taskId}";

            using (var client = new HttpClient())
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);

                var response = await client.GetAsync(url);

                verifyResponse(response, "Error during get task");

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return AutomationTask.FromJson(jsonResponse);
            }
        }

        public async Task<AutomationTask> FinishTaskAsync(string taskId, FinishStatusEnum status, string message = "") {
            string url = $"{_server}/api/v2/task/{taskId}";
            var data = new Dictionary<string, object>
            {
                { "finishStatus", status },
                { "finishMessage", message },
                { "state", StateEnum.FINISHED },
            };
            
            var content = HttpContentFactory.CreateJsonContent(data);

            using (var client = new HttpClient())
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);

                var response = await client.PostAsync(url, content);

                verifyResponse(response, "Error during finish task");

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return AutomationTask.FromJson(jsonResponse);
            }
        }

        public async Task<AutomationTask> InterruptTask(string taskId) {
            string url = $"{_server}/api/v2/task/{taskId}";
            var data = new Dictionary<string, object>
            {
                { "interrupted", true },
            };
            
            var content = HttpContentFactory.CreateJsonContent(data);

            using (var client = new HttpClient())
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);

                var response = await client.PostAsync(url, content);

                verifyResponse(response, "Error during interrupt task");

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return AutomationTask.FromJson(jsonResponse);
            }
        }

        public async Task<AutomationTask> RestartTask(string taskId) {
            string url = $"{_server}/api/v2/task/{taskId}";
            var data = new Dictionary<string, object>
            {
                { "state", StateEnum.START },
            };
            
            var content = HttpContentFactory.CreateJsonContent(data);

            using (var client = new HttpClient())
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);

                var response = await client.PostAsync(url, content);

                verifyResponse(response, "Error during restart task");

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return AutomationTask.FromJson(jsonResponse);
            }
        }

        public async Task<Alert> CreateAlert(string taskId, string title, string message, AlertTypeEnum alertType)
        {
            string url = $"{_server}/api/v2/alerts";
            var data = new Dictionary<string, object>
            {
                { "taskId", taskId },
                { "title", title },
                { "message", message },
                { "type", alertType }
            };            

            var content = HttpContentFactory.CreateJsonContent(data);

            using (var client = new HttpClient())
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);

                var response = await client.PostAsync(url, content);

                verifyResponse(response, "Error during create alert");

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return Alert.FromJson(jsonResponse);
            }
        }

        public async Task SendMessage(List<string> emails, List<string> logins, string subject, string body, MessageTypeEnum messageType, List<string> groups = null)
        {
            string url = $"{_server}/api/v2/message";
            var data = new Dictionary<string, object>
            {
                { "emails", emails },
                { "logins", logins },
                { "subject", subject },
                { "body", body },
                { "type", messageType },
                { "groups", groups }
            };            

            var content = HttpContentFactory.CreateJsonContent(data);

            using (var client = new HttpClient())
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);

                var response = await client.PostAsync(url, content);

                verifyResponse(response, "Error during send message");
            }
        }

        public async Task<string> GetCredential(string label, string key) {
            string url = $"{_server}/api/v2/credential/{label}/key/{key}";

            using (var client = new HttpClient())
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);

                var response = await client.GetAsync(url);

                verifyResponse(response, "Error during get credential");

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return jsonResponse;
            }
        }

        private async Task<bool> CreateCredentialByLabel(string label, string key, string value) {
            string url = $"{_server}/api/v2/credential";
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "label", label },
                { "repositoryLabel", "DEFAULT"},
                { "secrets", new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object>
                        {
                            { "key", key },
                            { "value", value },
                        }
                    }
                }
            };
            
            var content = HttpContentFactory.CreateJsonContent(data);

            using (var client = new HttpClient())
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);

                var response = await client.PostAsync(url, content);
                verifyResponse(response, "Error in teste create credential");
                return response.IsSuccessStatusCode;
            }
        }

        private async Task<bool> GetCredentialByLabel(string label) {
            string url = $"{_server}/api/v2/credential/{label}";

            using (var client = new HttpClient())
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);

                var response = await client.GetAsync(url);

                return response.IsSuccessStatusCode;
            }
        }
        
        public async Task CreateCredential(string label, string key, string value) {
            string url = $"{_server}/api/v2/credential/{label}/key";
            bool existCredential = await this.GetCredentialByLabel(label);
            if (!existCredential) {
                await this.CreateCredentialByLabel(label, key, value);
                return;
            }
            var data = new Dictionary<string, object>
            {
                { "key", key },
                { "value", value }
            };
            var content = HttpContentFactory.CreateJsonContent(data);
            using (var client = new HttpClient())
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);
                var response = await client.PostAsync(url, content);
                verifyResponse(response, "Error in create credential");
                
            }
        }
        
        public async Task CreateError(Exception exception, string taskId, string screenshotPath = "", List<string> attachments = null) {
            string url = $"{_server}/api/v2/error";
            Dictionary<string, string> tags = this.GetDefaultErrorTags();
            var data = new Dictionary<string, object>
            {
                { "taskId", taskId },
                { "type", exception.GetType().FullName },
                { "message", exception.Message },
                { "stackTrace", exception.StackTrace },
                { "language", "SHELL" },
                { "tags", tags },
            };
            var content = HttpContentFactory.CreateJsonContent(data);
            string idError = "";
            using (var client = new HttpClient())
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);
                var response = await client.PostAsync(url, content);
                verifyResponse(response, "Error in send error credential");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(jsonResponse);
                idError = json["id"].ToString();   
            }
            if (screenshotPath != "") {
                await this.CreateScreenshotAsync(idError, screenshotPath);
            }

            if (attachments.Count > 0) {
                foreach (string attachment in attachments){
                    await this.CreateAttachment(idError, attachment);
                }
            }
        }
        
        private Dictionary<string, string> GetDefaultErrorTags() {
            var tags = new Dictionary<string, string>();

            try{
                tags["user_name"] = Environment.UserName;
            } catch (Exception) {
                tags["user_name"] = string.Empty;
            }

            tags["host_name"] = Environment.MachineName;
            tags["os_name"] = GetOSName();
            tags["os_version"] = RuntimeInformation.OSDescription;
            tags["dotnet_version"] =  Environment.Version.ToString();
            return tags;
        }
        
        private async Task CreateScreenshotAsync(string errorId, string filepath) {
            string urlScreenshot = $"{_server}/api/v2/error/{errorId}/screenshot";
            filepath = Environment.ExpandEnvironmentVariables(filepath);
            filepath = Path.GetFullPath(filepath);
            try
            {
                using (var client = new HttpClient()) {
                    using (var form = new MultipartFormDataContent())
                    {
                        var fileContent = new ByteArrayContent(File.ReadAllBytes(filepath));
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                        form.Add(fileContent, "file", Path.GetFileName(filepath));

                        client.AddDefaultHeaders(_accessToken, _login, 30);
                        var response = await client.PostAsync(urlScreenshot, form);

                        if (!response.IsSuccessStatusCode){
                            string responseContent = await response.Content.ReadAsStringAsync();
                            throw new Exception($"Error during send screenshot. Server returned {response.StatusCode}. {responseContent}");
                        }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine($"An error occurred while creating screenshot: {ex.Message}");
                throw;
            }
        }
        
        private async Task CreateAttachment(string errorId, string filepath) {
            string urlScreenshot = $"{_server}/api/v2/error/{errorId}/attachments";
            filepath = Environment.ExpandEnvironmentVariables(filepath);
            filepath = Path.GetFullPath(filepath);
            string fileName = Path.GetFileName(filepath);
            var provider = new FileExtensionContentTypeProvider();
            string contentType;

            if (!provider.TryGetContentType(filepath, out contentType)){
                contentType = "application/octet-stream";
            }

            try
            {
                using (var client = new HttpClient()) {
                    using (var form = new MultipartFormDataContent())
                    {
                        var fileContent = new ByteArrayContent(File.ReadAllBytes(filepath));
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
                        form.Add(fileContent, "file", Path.GetFileName(filepath));

                        client.AddDefaultHeaders(_accessToken, _login, 30);
                        var response = await client.PostAsync(urlScreenshot, form);

                        if (!response.IsSuccessStatusCode){
                            string responseContent = await response.Content.ReadAsStringAsync();
                            throw new Exception($"Error during send attachments. Server returned {response.StatusCode}. {responseContent}");
                        }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine($"An error occurred while creating screenshot: {ex.Message}");
                throw;
            }
        }

        private static string GetOSName()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return "Windows";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return "Linux";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return "macOS";
            }
            else
            {
                return "Unknown";
            }
        }
        
        public async Task<Log> NewLogAsync(string label, List<Column> columns) {
            string url = $"{_server}/api/v2/log";

            var data = new Dictionary<string, object>
            {
                { "label", label},
                { "columns", columns},
                { "repositoryLabel", "DEFAULT" }
            };
            var content = HttpContentFactory.CreateJsonContent(data);
            using (var client = new HttpClient())
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);
                var response = await client.PostAsync(url, content);
                verifyResponse(response, "Error in New Log credential");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return Log.FromJson(jsonResponse);
            }
        }

        public async Task NewLogEntryAsync(string label, Dictionary<string, object> values) {
            string url = $"{_server}/api/v2/log/{label}/entry";

            var content = HttpContentFactory.CreateJsonContent(values);
            using (var client = new HttpClient())
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);
                var response = await client.PostAsync(url, content);
                verifyResponse(response, "Error in New Log Entry");
            }
        }

        public async Task<List<Entry>> GetLog(string label, string date = null) {
            string url = $"{_server}/api/v2/log/{label}";
            int days = 365;
            if (!string.IsNullOrEmpty(date)) {
                DateTime parsedDate = DateTime.ParseExact(date, "dd/MM/yyyy", null);
                days = (DateTime.Now - parsedDate).Days + 1;
            }

            var logData = new List<Dictionary<string, object>>();
            using (var client = new HttpClient())
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);
                var response = await client.GetAsync(url);
                verifyResponse(response, "Error in New Log credential");
                var log = Log.FromJson(await response.Content.ReadAsStringAsync());
                if (log.Columns == null || log.Columns.Count == 0) {
                    throw new Exception("Malformed log. No columns available.");
                }
                var namesForLabels = new Dictionary<string, string>();
                foreach (var column in log.Columns){
                    namesForLabels.Add(column.Label, column.Name);
                }
                var requestData = new Dictionary<string, object>{
                    { "days", days.ToString() }
                };
                url = $"{_server}/api/v2/log/{label}/entry-list";
                var responseEntries = await client.GetAsync(url);
                verifyResponse(responseEntries, "Error in New Log credential");
                var entriesString = await responseEntries.Content.ReadAsStringAsync();
                var entries =  Newtonsoft.Json.JsonConvert.DeserializeObject<List<Entry>>(entriesString);
                return entries;
            }
        }

        public async Task DeleteLogAsync(string label) {
            string url = $"{_server}/api/v2/log/{label}";

            using (var client = new HttpClient())
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);
                var response = await client.DeleteAsync(url);
                verifyResponse(response, "Error in New Log Entry");
            }
        }

        public async Task PostArtifact(string taskId, string name, string filepath) {
            string artifact_id = await this.CreateArtifact(taskId, name, filepath);
            string url = $"{_server}/api/v2/artifact/log/{artifact_id}";
            filepath = Environment.ExpandEnvironmentVariables(filepath);
            filepath = Path.GetFullPath(filepath);
            try
            {
                using (var client = new HttpClient()) {
                    using (var form = new MultipartFormDataContent())
                    {
                        var fileContent = new ByteArrayContent(File.ReadAllBytes(filepath));
                        var provider = new FileExtensionContentTypeProvider();
                        string contentType;

                        if (!provider.TryGetContentType(filepath, out contentType)){
                            contentType = "application/octet-stream";
                        }
                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);
                        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "file",
                            FileName = name
                        };
                        form.Add(fileContent);
                        client.AddDefaultHeaders(_accessToken, _login, 30);
                        var response = await client.PostAsync(url, form);

                        if (!response.IsSuccessStatusCode){
                            string responseContent = await response.Content.ReadAsStringAsync();
                            throw new Exception($"Error during send attachment. Server returned {response.StatusCode}. {responseContent}");
                        }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine($"An error occurred while creating attachment: {ex.Message}");
                throw;
            }
        }

        private async Task<string> CreateArtifact(string taskId, string name, string filename) {
            string url = $"{_server}/api/v2/artifact";
            var data = new Dictionary<string, object>
            {
                { "taskId", taskId },
                { "name", name },
                { "filename", filename },
            };
            var content = HttpContentFactory.CreateJsonContent(data);
            string artifactId = "";
            using (var client = new HttpClient())
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);
                var response = await client.PostAsync(url, content);
                verifyResponse(response, "Error in create attachment credential");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(jsonResponse);
                artifactId = json["id"].ToString();
            }
            return artifactId;
        }
        
        public async Task<(string filename, byte[] fileContent)> GetArtifact(string artifactId) {
            string url = $"{_server}/api/v2/artifact/{artifactId}";

            using (var client = new HttpClient())
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);

                var response = await client.GetAsync(url);
                verifyResponse(response, "Error in get artifact");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(jsonResponse);
                string fileName = json["fileName"].ToString();
                string urlGetFile = $"{_server}/api/v2/artifact/{artifactId}/file";
                var file = await client.GetAsync(urlGetFile);
                verifyResponse(file, "Error in get file artifact");
                var fileContent = await file.Content.ReadAsByteArrayAsync();

                return (fileName, fileContent);
            }
        }
        
        public async Task<List<Artifact>> ListArtifact(int days = 7) {
            string url = $"{_server}/api/v2/artifact?size=5&page=0&sort=dateCreation,desc&days={days}";
            var artifacts = new List<Artifact>();
            
            var (content, totalPages) = await FetchArtifactPageAsync(url);
            artifacts.AddRange(content);
            for (int page = 1; page < totalPages; page++) {
                url = $"{_server}/api/v2/artifact?size=5&page={page}&sort=dateCreation,desc&days={days}";
                var (pageContent, _) = await FetchArtifactPageAsync(url);
                artifacts.AddRange(pageContent);
            }
            return artifacts;
        }

        private async Task<(List<Artifact>, int)> FetchArtifactPageAsync(string url)
        {
            using (var client = new HttpClient())
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);
                var response = await client.GetAsync(url);
                var responseString = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(responseString);
                var content = json["content"].ToObject<List<Artifact>>();
                int totalPages = (int)json["totalPages"];

                return (content, totalPages);
            }
        }

        public async Task<Datapool> CreateDatapool(Datapool pool)
        {
            string url = $"{_server}/api/v2/datapool";

            using (var client = new HttpClient())
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);
                var content = HttpContentFactory.CreateJsonContent(pool.ToJson());
                var response = await client.PostAsync(url, content);
                verifyResponse(response, "Error in get artifact");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Datapool data = Datapool.FromJson(jsonResponse);
                data.Maestro = this;
                return data;
            }
        }

        public async Task<Datapool> GetDatapool(string label)
        {
            string url = $"{_server}/api/v2/datapool/{label}";

            using (var client = new HttpClient())
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);
                var response = await client.GetAsync(url);
                verifyResponse(response, "Error in get Datapool");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Datapool data = Datapool.FromJson(jsonResponse);
                data.Maestro = this;
                return data;
            }
        }
        
        public void Logoff()
        {
            _accessToken = null;
        }
    }