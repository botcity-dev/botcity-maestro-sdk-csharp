
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCityMaestroSDK.Dtos.Task
{
    public class ResultTaskStateDTO
    {
        public string state { get; set; }
        public string finishStatus { get; set; }
        public string finishMessage { get; set; }
    }
}
