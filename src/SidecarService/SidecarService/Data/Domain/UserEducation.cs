using Newtonsoft.Json;

namespace SidecarService.Data.Domain;

public class UserEducation
{
    public int Id { get; set; }
    public string DocumentId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    [JsonProperty("personal_subjects")]
    public List<PersonalSubject> PersonalSubjects { get; set; }
    public string Statuses { get; set; }
    public Education Education { get; set; }
}
