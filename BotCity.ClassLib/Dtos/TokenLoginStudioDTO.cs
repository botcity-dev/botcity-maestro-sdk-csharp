namespace BotCity.ClassLib.Dtos;

public class TokenLoginStudioDTO{

    public string userName { get; set; }
    public int IdUser { get; set; }
    public string Language { get; set; }
    public string Community { get; set; }
    public string OrganizationLabel { get; set; }
    public string New_Cookie { get; set; }
    public string Access_Token { get; set; }
    
    public List<OrganizationDTO> Organizations { get; set; }

    public TokenLoginStudioDTO()
    {
        Organizations = new List<OrganizationDTO>();
    }

}
