namespace SidecarService.Data.Domain;

public class Education
{
    public int Id { get; set; }
    public string DocumentId { get; set; }
    public string EducationName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string EducationCode { get; set; }
    public int EducationId { get; set; }
}
