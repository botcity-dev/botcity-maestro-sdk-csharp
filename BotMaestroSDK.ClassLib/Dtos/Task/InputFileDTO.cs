namespace BotCityMaestroSDK.Dtos.Task;
public class InputFileDTO
{
    public int Id { get; set; }
    public string Type { get; set; } //INPUT
    public int TaskId { get; set; }
    public string Name { get; set; }
    public string FileName { get; set; }
    public string StorageFileName { get; set; }
    public string StorageFilePath { get; set; }
    public int OrganizationId { get; set; }
    public int UserId { get; set; }
    public string TaskName { get; set; }
    public int Days { get; set; }

}