using Newtonsoft.Json;

namespace SidecarService.Data.Domain;

public class User
{
    public int Id { get; set; }
    public string DocumentId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Provider { get; set; }
    public bool Confirmed { get; set; }
    public bool Blocked { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    [JsonProperty("user_educations")]
    public List<UserEducation> UserEducations { get; set; } = [];
}
