namespace NsBotCityMaestroSDK.ClassLib.Dtos.Login;

public class ResultLoginCliDTO
{

    public string userName { get; set; }
    public int IdUser { get; set; }
    public string Language { get; set; }
    public string Community { get; set; }
    public string OrganizationLabel { get; set; }
    public string New_Cookie { get; set; }
    public string Access_Token { get; set; }

    public List<string> Organizations { get; set; }

    public ResultLoginCliDTO()
    {
        Organizations = new List<string>();
    }

}
