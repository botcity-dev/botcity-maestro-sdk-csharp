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
        public bool _verifySSLCert { get; set; } = true;
        
        private string _server = "";
        private string _login = "";

        private string _key = "";

        private string _accessToken = "";
        private string _taskId = "";

        /// <summary>
        /// Main class to interact with the BotMaestro web portal.
        /// </summary>
        /// <remarks>
        /// This class offers methods to send alerts, messages, create log entries, post artifacts and more.
        /// </remarks>
        /// <param name="server">The server IP or name</param>
        /// <param name="login">The username provided via server configuration. Available under `Dev. Environment`</param>
        /// <param name="key">The access key provided via server configuration. Available under `Dev. Environment`</param>
        /// <param name="taskId">The task ID associated with the current task.</param>
        /// <param name="notifiedDisconnect">Flag to indicate if a notification should be sent on disconnect.</param>
        /// <param name="raiseNotConnected">Flag to indicate if an exception should be raised when not connected.</param>
        /// <param name="verifySSLCert">Flag to indicate if SSL certificates should be verified.</param>
        public BotMaestroSDK(string server = "", string login = "", string key = "", string taskId = "", bool notifiedDisconnect = false, bool raiseNotConnected = true, bool verifySSLCert = true)
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

        public bool CheckAccessTokenAvailable() {
            if (string.IsNullOrEmpty(this._accessToken)) {
                if (this._raiseNotConnected) {
                    throw new InvalidOperationException("Access Token not available. Make sure to invoke login first.");
                } else {
                    if (!this.notifiedDisconnect) {
                        this.notifiedDisconnect = true;
                        Console.WriteLine("** WARNING BotCity Maestro is not logged in and RAISE_NOT_CONNECTED is False. Running in Offline mode. **");
                    }
                    return false;
                }
            }
            return true;
        }

        public bool GetVerifySSL() {
            return _verifySSLCert;
        }

        public void SetVerifySSL(bool newValue) {
            _verifySSLCert = newValue;
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

        /// <summary>
        /// Fetch the BotExecution object for a given task.
        /// </summary>
        /// <param name="taskId">The task ID. Defaults to an empty string.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the Execution information. See <see cref="Execution"/></returns>
        public async Task<Execution> GetExecutionAsync(string taskId = "") {
            string verifyTaskId = taskId;

            if (string.IsNullOrEmpty(taskId)) {
                verifyTaskId = this._taskId;
            }

            if (!this.CheckAccessTokenAvailable()) {
                return new Execution("", verifyTaskId, "", new Dictionary<string, object>{});
            }

            if (string.IsNullOrEmpty(verifyTaskId)) {
                throw new Exception("A task ID must be informed either via the parameter or the class property.");
            }

            AutomationTask task = await this.GetTaskAsync(verifyTaskId);
            JObject parametersObject = JObject.FromObject(task.Parameters);
            Dictionary<string, object> parametersDictionary = parametersObject.ToObject<Dictionary<string, object>>();

            return new Execution(_server, verifyTaskId, _accessToken, parametersDictionary);
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

        /// <summary>
        /// Obtain an access token with the configured BotMaestro portal.
        /// </summary>
        /// <remarks>
        /// Arguments are optional and can be used to configure or overwrite the object instantiation values.
        /// </remarks>
        /// <param name="server">The server IP or name</param>
        /// <param name="login">The username provided via server configuration. Available under `Dev. Environment`</param>
        /// <param name="key">The access key provided via server configuration. Available under `Dev. Environment`</param>
        public async Task LoginAsync(string server = "", string login = "", string key = "") {
            var handler = Handler.Get(this.GetVerifySSL());
            using (HttpClient client = new HttpClient(handler)) {
                
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
        private (int? totalItems, int? processedItems, int? failedItems) ValidateItems(int? totalItems, int? processedItems, int? failedItems)
        {
            if (totalItems == null && processedItems == null && failedItems == null)
            {
                string msg = @"
                    Attention: this task is not reporting items. Please inform the total, processed and failed items.
                    Reporting items is a crucial step to calculate the ROI, success rate and other metrics for your automation
                    via BotCity Insights.
                ";
                Console.WriteLine(msg);
                return (null, null, null);
            }

            if (totalItems == null && processedItems != null && failedItems != null)
            {
                totalItems = processedItems + failedItems;
            }

            if (totalItems != null && processedItems != null && failedItems == null)
            {
                failedItems = totalItems - processedItems;
            }

            if (totalItems != null && processedItems == null && failedItems != null)
            {
                processedItems = totalItems - failedItems;
            }

            if (totalItems == null || processedItems == null || failedItems == null)
            {
                throw new ArgumentException("You must inform at least two of the following parameters: totalItems, processedItems, failedItems.");
            }

            totalItems = Math.Max(0, totalItems.Value);
            processedItems = Math.Max(0, processedItems.Value);
            failedItems = Math.Max(0, failedItems.Value);

            if (totalItems != null && processedItems != null && failedItems != null)
            {
                if (totalItems != processedItems + failedItems)
                {
                    throw new ArgumentException("Total items is not equal to the sum of processed and failed items.");
                }
            }

            return (totalItems, processedItems, failedItems);
        }

        /// <summary>
        /// Creates a task to be executed on the BotMaestro portal.
        /// </summary>
        /// <param name="activityLabel">The activity unique identifier.</param>
        /// <param name="parameters">Dictionary with parameters and values for this task.</param>
        /// <param name="test">Whether or not the task is a test.</param>
        /// <param name="priority">An integer from 0 to 10 to refer to execution priority. Defaults to 0.</param>
        /// <param name="minExecutionDate">Minimum execution date for the task. Defaults to null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the automation task. See <see cref="AutomationTask"/>.</returns>
        /// <remarks>
        /// The BotCity Orchestrator time zone is in UTC-0. Therefore, consider the difference between
        /// time zones when using the <paramref name="minExecutionDate"/> parameter.
        /// </remarks>
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

            using (var client = new HttpClient(Handler.Get(this.GetVerifySSL())))
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);

                var response = await client.PostAsync(url, content);

                verifyResponse(response, "Error during create task");

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return AutomationTask.FromJson(jsonResponse);
            }
        }
        /// <summary>
        /// Return details about a given task.
        /// </summary>
        /// <param name="taskId">The task unique identifier.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the details of the automation task. See <see cref="AutomationTask"/>.</returns>
        public async Task<AutomationTask> GetTaskAsync(string taskId)
        {
            string url = $"{_server}/api/v2/task/{taskId}";

            using (var client = new HttpClient(Handler.Get(this.GetVerifySSL())))
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);

                var response = await client.GetAsync(url);

                verifyResponse(response, "Error during get task");

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return AutomationTask.FromJson(jsonResponse);
            }
        }

        /// <summary>
        /// Finishes a given task.
        /// </summary>
        /// <param name="taskId">The task unique identifier.</param>
        /// <param name="status">The condition in which the task must be finished. See <see cref="FinishStatusEnum"/>.</param>
        /// <param name="message">A message to be associated with this action.</param>
        /// <param name="totalItems">Total number of items processed by the task.</param>
        /// <param name="processedItems">Number of items processed successfully by the task.</param>
        /// <param name="failedItems">Number of items failed to be processed by the task.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the automation task See <see cref="AutomationTask"/>.</returns>
        /// <remarks>
        /// Starting from version 0.5.0, the parameters <paramref name="totalItems"/>, <paramref name="processedItems"/> and <paramref name="failedItems"/> are
        /// available to be used. It is important to report the correct values for these parameters as they are used
        /// to calculate the ROI, success rate and other metrics.
        /// 
        /// Keep in mind that the sum of <paramref name="processedItems"/> and <paramref name="failedItems"/> must be equal to <paramref name="totalItems"/>. If
        /// <paramref name="totalItems"/> is null, then the sum of <paramref name="processedItems"/> and <paramref name="failedItems"/> will be used as <paramref name="totalItems"/>.
        /// If you inform <paramref name="totalItems"/> and <paramref name="processedItems"/>, then <paramref name="failedItems"/> will be calculated as the difference.
        /// </remarks>
        public async Task<AutomationTask> FinishTaskAsync(string taskId, FinishStatusEnum status, string message = "", int? totalItems = null, int? processedItems = null, int? failedItems = null) {
            string url = $"{_server}/api/v2/task/{taskId}";
            (int? total, int? processed, int? failed) = this.ValidateItems(totalItems, processedItems, failedItems);
            var data = new Dictionary<string, object>
            {
                { "finishStatus", status },
                { "finishMessage", message },
                { "state", StateEnum.FINISHED },
                { "totalItems", total},
                { "processedItems", processed},
                { "failedItems", failed}
            };
            
            var content = HttpContentFactory.CreateJsonContent(data);

            using (var client = new HttpClient(Handler.Get(this.GetVerifySSL())))
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);

                var response = await client.PostAsync(url, content);

                verifyResponse(response, "Error during finish task");

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return AutomationTask.FromJson(jsonResponse);
            }
        }

        /// <summary>
        /// Request the interruption of a given task.
        /// </summary>
        /// <param name="taskId">The task unique identifier.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the interrupted automation task. See <see cref="AutomationTask"/>.</returns>
        public async Task<AutomationTask> InterruptTaskAsync(string taskId) {
            string url = $"{_server}/api/v2/task/{taskId}";
            var data = new Dictionary<string, object>
            {
                { "interrupted", true },
            };
            
            var content = HttpContentFactory.CreateJsonContent(data);

            using (var client = new HttpClient(Handler.Get(this.GetVerifySSL())))
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);

                var response = await client.PostAsync(url, content);

                verifyResponse(response, "Error during interrupt task");

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return AutomationTask.FromJson(jsonResponse);
            }
        }
        /// <summary>
        /// Restarts a given task.
        /// </summary>
        /// <param name="taskId">The task unique identifier.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the restarted automation task. See <see cref="AutomationTask"/>.</returns>
        public async Task<AutomationTask> RestartTaskAsync(string taskId) {
            string url = $"{_server}/api/v2/task/{taskId}";
            var data = new Dictionary<string, object>
            {
                { "state", StateEnum.START },
            };
            
            var content = HttpContentFactory.CreateJsonContent(data);

            using (var client = new HttpClient(Handler.Get(this.GetVerifySSL())))
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);

                var response = await client.PostAsync(url, content);

                verifyResponse(response, "Error during restart task");

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return AutomationTask.FromJson(jsonResponse);
            }
        }

        /// <summary>
        /// Register an alert message on the BotMaestro portal.
        /// </summary>
        /// <param name="taskId">The activity label.</param>
        /// <param name="title">A title associated with the alert message.</param>
        /// <param name="message">The alert message.</param>
        /// <param name="alertType">The alert type to be used. See <see cref="AlertTypeEnum"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the server response message. See <see cref="Alert"/>.</returns>
        public async Task<Alert> CreateAlertAsync(string taskId, string title, string message, AlertTypeEnum alertType)
        {
            var data = new Dictionary<string, object>
            {
                { "taskId", taskId },
                { "title", title },
                { "message", message },
                { "type", alertType }
            };            

            if (!this.CheckAccessTokenAvailable()) {
                return Alert.FromJson(JsonConvert.SerializeObject(data, Formatting.Indented));
            }
            string url = $"{_server}/api/v2/alerts";

            var content = HttpContentFactory.CreateJsonContent(data);

            using (var client = new HttpClient(Handler.Get(this.GetVerifySSL())))
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);

                var response = await client.PostAsync(url, content);

                verifyResponse(response, "Error during create alert");

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return Alert.FromJson(jsonResponse);
            }
        }
    
        /// <summary>
        /// Send an email message to the list of email and users given.
        /// </summary>
        /// <param name="emails">List of emails to receive the message.</param>
        /// <param name="logins">List of usernames registered on the BotMaestro portal to receive the message.</param>
        /// <param name="subject">The message subject.</param>
        /// <param name="body">The message body.</param>
        /// <param name="messageType">The message body type. See <see cref="MessageTypeEnum"/>.</param>
        /// <param name="groups">The message group information.</param>
        public async Task SendMessageAsync(List<string> emails, List<string> logins, string subject, string body, MessageTypeEnum messageType, List<string> groups = null)
        {
            string url = $"{_server}/api/v2/message";
            if (!this.CheckAccessTokenAvailable()) {
                return;
            };
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

            using (var client = new HttpClient(Handler.Get(this.GetVerifySSL())))
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);

                var response = await client.PostAsync(url, content);

                verifyResponse(response, "Error during send message");
            }
        }

        /// <summary>
        /// Retrieves the value associated with a key inside credentials.
        /// </summary>
        /// <param name="label">Credential set name.</param>
        /// <param name="key">Key name within the credential set.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the requested key value.</returns>
        public async Task<string> GetCredentialAsync(string label, string key) {
            if (!this.CheckAccessTokenAvailable()) {
                return "";
            }
            string url = $"{_server}/api/v2/credential/{label}/key/{key}";

            using (var client = new HttpClient(Handler.Get(this.GetVerifySSL())))
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);

                var response = await client.GetAsync(url);

                verifyResponse(response, "Error during get credential");

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return jsonResponse;
            }
        }

        private async Task<bool> CreateCredentialByLabel(string label, string key, string value) {
            if (!this.CheckAccessTokenAvailable()) {
                return true;
            }
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

            using (var client = new HttpClient(Handler.Get(this.GetVerifySSL())))
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);

                var response = await client.PostAsync(url, content);
                verifyResponse(response, "Error in teste create credential");
                return response.IsSuccessStatusCode;
            }
        }

        private async Task<bool> GetCredentialByLabel(string label) {
            if (!this.CheckAccessTokenAvailable()) {
                return true;
            }
            string url = $"{_server}/api/v2/credential/{label}";

            using (var client = new HttpClient(Handler.Get(this.GetVerifySSL())))
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);

                var response = await client.GetAsync(url);

                return response.IsSuccessStatusCode;
            }
        }
        /// <summary>
        /// Creates a new credential.
        /// </summary>
        /// <param name="label">Credential set name.</param>
        /// <param name="key">Key name within the credential set.</param>
        /// <param name="value">Credential value.</param>
        public async Task CreateCredentialAsync(string label, string key, string value) {
            if (!this.CheckAccessTokenAvailable()) {
                return;
            }
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
            using (var client = new HttpClient(Handler.Get(this.GetVerifySSL())))
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);
                var response = await client.PostAsync(url, content);
                verifyResponse(response, "Error in create credential");
                
            }
        }
        private Dictionary<string, string> MergeDictionaries(Dictionary<string, string> first, Dictionary<string, object> second)
        {
            Dictionary<string, string> result = new Dictionary<string, string>(first);

            foreach (var kvp in second)
            {
                if (!result.ContainsKey(kvp.Key))
                {
                    result.Add(kvp.Key, kvp.Value.ToString());
                }
            }

            return result;
        }

        /// <summary>
        /// Creates a new Error entry.
        /// </summary>
        /// <param name="exception">The exception object.</param>
        /// <param name="taskId">The task unique identifier.</param>
        /// <param name="screenshotPath">File path for a screenshot to be attached to the error. Defaults to an empty string.</param>
        /// <param name="tags">Dictionary with tags to be associated with the error entry. Defaults to null.</param>
        /// <param name="attachments">Additional files to be sent along with the error entry. Defaults to null.</param>
        public async Task CreateErrorAsync(Exception exception, string taskId, string screenshotPath = "", Dictionary<string, object> tags = null, List<string> attachments = null) {
            if (!this.CheckAccessTokenAvailable()) {
                return;
            }
            string url = $"{_server}/api/v2/error";
            Dictionary<string, string> defaultTags = this.GetDefaultErrorTags();
            if (tags != null) {
                defaultTags = this.MergeDictionaries(defaultTags, tags);
            }
            var data = new Dictionary<string, object>
            {
                { "taskId", taskId },
                { "type", exception.GetType().FullName },
                { "message", exception.Message },
                { "stackTrace", exception.StackTrace },
                { "language", "SHELL" },
                { "tags", defaultTags },
            };
            var content = HttpContentFactory.CreateJsonContent(data);
            string idError = "";
            using (var client = new HttpClient(Handler.Get(this.GetVerifySSL())))
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

            if (attachments != null && attachments.Count > 0) {
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
            if (!this.CheckAccessTokenAvailable()) {
                return;
            }
            string urlScreenshot = $"{_server}/api/v2/error/{errorId}/screenshot";
            filepath = Environment.ExpandEnvironmentVariables(filepath);
            filepath = Path.GetFullPath(filepath);
            try
            {
                using (var client = new HttpClient(Handler.Get(this.GetVerifySSL()))) {
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
            if (!this.CheckAccessTokenAvailable()) {
                return;
            }
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
                using (var client = new HttpClient(Handler.Get(this.GetVerifySSL()))) {
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
        
        /// <summary>
        /// Create a new log on the BotMaestro portal.
        /// </summary>
        /// <param name="label">The activity unique identifier.</param>
        /// <param name="columns">A list of columns for the new log.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the newly created log. See <see cref="Log"/>.</returns>
        public async Task<Log> NewLogAsync(string label, List<Column> columns) {
            
            string url = $"{_server}/api/v2/log";

            var data = new Dictionary<string, object>
            {
                { "label", label},
                { "columns", columns},
                { "repositoryLabel", "DEFAULT" }
            };
            
            if (!this.CheckAccessTokenAvailable()) {
                return Log.FromJson(JsonConvert.SerializeObject(data, Formatting.Indented));
            }

            var content = HttpContentFactory.CreateJsonContent(data);
            using (var client = new HttpClient(Handler.Get(this.GetVerifySSL())))
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);
                var response = await client.PostAsync(url, content);
                verifyResponse(response, "Error in New Log credential");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return Log.FromJson(jsonResponse);
            }
        }
        /// <summary>
        /// Creates a new log entry.
        /// </summary>
        /// <param name="label">The activity unique identifier.</param>
        /// <param name="values">Dictionary where the key is the column label and value is the entry value.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the newly created log entry. See <see cref="Log"/>.</returns>
        public async Task NewLogEntryAsync(string label, Dictionary<string, object> values) {
            if (!this.CheckAccessTokenAvailable()) {
                return;
            }
            string url = $"{_server}/api/v2/log/{label}/entry";

            var content = HttpContentFactory.CreateJsonContent(values);
            using (var client = new HttpClient(Handler.Get(this.GetVerifySSL())))
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);
                var response = await client.PostAsync(url, content);
                verifyResponse(response, "Error in New Log Entry");
            }
        }

        /// <summary>
        /// Fetches log information.
        /// </summary>
        /// <param name="label">The activity unique identifier.</param>
        /// <param name="date">Initial date for log information in the format DD/MM/YYYY. If empty, all information is retrieved.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of log entries. See <see cref="Entry"/>.</returns>
        public async Task<List<Entry>> GetLogAsync(string label, string date = null) {
            if (!this.CheckAccessTokenAvailable()) {
                return new List<Entry>();
            }
            string url = $"{_server}/api/v2/log/{label}";
            int days = 365;
            if (!string.IsNullOrEmpty(date)) {
                DateTime parsedDate = DateTime.ParseExact(date, "dd/MM/yyyy", null);
                days = (DateTime.Now - parsedDate).Days + 1;
            }

            var logData = new List<Dictionary<string, object>>();
            using (var client = new HttpClient(Handler.Get(this.GetVerifySSL())))
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

        /// <summary>
        /// Deletes log entries associated with the specified activity.
        /// </summary>
        /// <param name="label">The activity unique identifier.</param>
        public async Task DeleteLogAsync(string label) {
            if (!this.CheckAccessTokenAvailable()) {
                return;
            }
            string url = $"{_server}/api/v2/log/{label}";

            using (var client = new HttpClient(Handler.Get(this.GetVerifySSL())))
            {
                client.AddDefaultHeaders(_accessToken, _login, 30);
                var response = await client.DeleteAsync(url);
                verifyResponse(response, "Error in New Log Entry");
            }
        }

        /// <summary>
        /// Uploads a new artifact into the BotMaestro portal.
        /// </summary>
        /// <param name="taskId">The task unique identifier.</param>
        /// <param name="name">The name of the artifact to be displayed on the portal.</param>
        /// <param name="filepath">The file to be uploaded.</param>
        public async Task PostArtifactAsync(string taskId, string name, string filepath) {
            if (!this.CheckAccessTokenAvailable()) {
                return;
            }
            string artifact_id = await this.CreateArtifactAsync(taskId, name, filepath);
            string url = $"{_server}/api/v2/artifact/log/{artifact_id}";
            filepath = Environment.ExpandEnvironmentVariables(filepath);
            filepath = Path.GetFullPath(filepath);
            try
            {
                using (var client = new HttpClient(Handler.Get(this.GetVerifySSL()))) {
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

        private async Task<string> CreateArtifactAsync(string taskId, string name, string filename) {
            string url = $"{_server}/api/v2/artifact";
            var data = new Dictionary<string, object>
            {
                { "taskId", taskId },
                { "name", name },
                { "filename", filename },
            };
            if (!this.CheckAccessTokenAvailable()) {
                return "";
            }
            var content = HttpContentFactory.CreateJsonContent(data);
            string artifactId = "";
            using (var client = new HttpClient(Handler.Get(this.GetVerifySSL())))
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
        /// <summary>
        /// Retrieves an artifact from the BotMaestro portal.
        /// </summary>
        /// <param name="artifactId">The artifact unique identifier.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with the artifact name (string) and the binary content of the artifact (byte[]).</returns>
        public async Task<(string filename, byte[] fileContent)> GetArtifactAsync(string artifactId) {
            string url = $"{_server}/api/v2/artifact/{artifactId}";
            if (!this.CheckAccessTokenAvailable()) {
                return ("", new byte[0]);
            }
            using (var client = new HttpClient(Handler.Get(this.GetVerifySSL())))
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
        
        /// <summary>
        /// Lists all artifacts available for the organization.
        /// </summary>
        /// <param name="days">Number of days to filter artifacts. Defaults to 7.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of artifacts. See <see cref="Artifact"/>.</returns>
        public async Task<List<Artifact>> ListArtifactAsync(int days = 7) {
            if (!this.CheckAccessTokenAvailable()) {
                return new List<Artifact>();
            }
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
            if (!this.CheckAccessTokenAvailable()) {
                return (new List<Artifact>(), 0);
            }
            using (var client = new HttpClient(Handler.Get(this.GetVerifySSL())))
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

        /// <summary>
        /// Creates a new datapool on the BotMaestro portal.
        /// </summary>
        /// <param name="pool">The DataPool instance.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the created DataPool instance.</returns>
        public async Task<Datapool> CreateDatapoolAsync(Datapool pool)
        {
            if (!this.CheckAccessTokenAvailable()) {
                Datapool data = Datapool.FromJson("{}");
                data.Maestro = this;
                return data;
            }
            string url = $"{_server}/api/v2/datapool";

            using (var client = new HttpClient(Handler.Get(this.GetVerifySSL())))
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

        /// <summary>
        /// Retrieves a datapool from the BotMaestro portal.
        /// </summary>
        /// <param name="label">The label of the DataPool.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the DataPool instance.</returns>
        public async Task<Datapool> GetDatapoolAsync(string label)
        {
            if (!this.CheckAccessTokenAvailable()) {
                Datapool data = Datapool.FromJson("{}");
                data.Maestro = this;
                return data;
            }
            string url = $"{_server}/api/v2/datapool/{label}";

            using (var client = new HttpClient(Handler.Get(this.GetVerifySSL())))
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

        /// <summary>
        /// Revoke the access token used to communicate with the BotMaestro portal.
        /// </summary>
        public void Logoff()
        {
            _accessToken = null;
            
        }
    }
