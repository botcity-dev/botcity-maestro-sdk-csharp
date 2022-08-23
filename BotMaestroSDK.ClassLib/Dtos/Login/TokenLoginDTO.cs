namespace NsBotCityMaestroSDK.ClassLib.Dtos.Login;

public class TokenLoginDTO
{

    public string userName { get; set; }
    public int IdUser { get; set; }
    public string Token { get; set; }
    public string Role { get; set; }
    public bool FirstLogin { get; set; }
    public string Email { get; set; }
    public string Language { get; set; }
    public List<OrganizationDTO> Organizations { get; set; }

    public TokenLoginDTO()
    {
        Organizations = new List<OrganizationDTO>();
    }

}
