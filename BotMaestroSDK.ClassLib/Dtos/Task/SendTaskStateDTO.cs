using NsBotCityMaestroSDK.ClassLib.Dtos.Task;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NsBotMaestroSDK.ClassLib.Dtos.Task
{
    public class SendTaskStateDTO
    {
        public string state { get; set; }
        public string finishStatus { get; set; }
        public string finishMessage { get; set; }
    }
}
