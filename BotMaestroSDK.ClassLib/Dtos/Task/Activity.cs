
namespace BotCityMaestroSDK.Dtos.Task;
public class Activity
{
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public string ActivityLabel { get; set; }
    public string AgentId { get; set; }
    public int UserCreationId { get; set; }
    public string UserCreationName { get; set; }
    public int OrganizationCreationId { get; set; }
    public DateTime DateCreation { get; set; }
    public DateTime DateLastModified { get; set; }
    public string FinishStatus { get; set; }
    public string FinishMessage { get; set; }
    public bool Test { get; set; }
    public string StateFilter { get; set; }
    public string State { get; set; }

    public InputFileDTO InputFile {get; set;}

    public List<Param> Parameters { get; set; }

    public Activity()
    {
        Parameters = new List<Param>();
    }

    public void ParamAdd(string Name, string Value){
        Param param = new Param{
            Name = Name,
            Value = Value
        };
        Parameters.Add(param);
    }

    public void ParamClear(string Name, string Value){
        Parameters = new List<Param>();
    }

    public void ParamDelete(string Name){

        foreach (Param param in Parameters){
            if (param.Name == Name){
                Parameters.Remove(param);
                break;
            }
        }
        
    }


}