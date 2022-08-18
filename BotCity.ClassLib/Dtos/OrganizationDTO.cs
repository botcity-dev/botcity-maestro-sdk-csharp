namespace BotCity.ClassLib.Dtos;
public class OrganizationDTO{
    public int Id { get; set; }
    public string Label { get; set; }
    public string Name { get; set; }
    public string country { get; set; }
    public string login { get; set; }
    public string key { get; set; }
    public string googleDataStudioToken { get; set; }
    public List<Activitie> Activities { get; set; }
    
    public OrganizationDTO()
    {
        Activities = new List<Activitie>();
    }
}