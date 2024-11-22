using Newtonsoft.Json;

namespace SidecarService.Data.Domain;

public class PersonalSubject
{
    public int Id { get; set; }
    public string DocumentId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    [JsonProperty("personal_courses")]
    public List<PersonalCourse> PersonalCourses { get; set; }
}
