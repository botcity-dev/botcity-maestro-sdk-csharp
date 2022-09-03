
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCityMaestroSDK.Dtos.Task
{
    public class ListParameters
    {
        public IList<Param> Parameters { get; set; }

        public ListParameters()
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
