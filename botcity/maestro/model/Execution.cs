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

namespace Dev.BotCity.MaestroSdk.Model.Execution
{
    public partial class Execution
    {
        public string Server { get; set; }

        public string TaskId { get; set; }

        public string Token { get; set; }

        public Dictionary<string, object> Parameters { get; set; }

        public Execution(
            string server,
            string taskId,
            string token,
            Dictionary<string, object> parameters
        ) {
            Server = server;
            TaskId = taskId;
            Token = token;
            Parameters = parameters;
        }
    }
}