using NsBotCityMaestroSDK.ClassLib.Dtos.Task;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NsBotMaestroSDK.ClassLib.Dtos.Task
{
    public class SendTaskDTO
    {
        public string activityLabel { get; set; }
        public bool test { get; set; }
        
        public List<Param> Parameters { get; set; }

        public SendTaskDTO()
        {
            Parameters = new List<Param>();
        }

        public void ParamAdd(string Name, string Value)
        {
            Param param = new Param
            {
                Name = Name,
                Value = Value
            };
            Parameters.Add(param);
        }

        public void ParamClear(string Name, string Value)
        {
            Parameters = new List<Param>();
        }

        public void ParamDelete(string Name)
        {

            foreach (Param param in Parameters)
            {
                if (param.Name == Name)
                {
                    Parameters.Remove(param);
                    break;
                }
            }

        }

    }
}
