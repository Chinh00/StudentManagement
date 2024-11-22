using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SidecarService.Data;
using SidecarService.Data.Domain;
using SidecarService.Services;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SidecarService.Controllers;
[ApiController]
[Route("/api/[controller]")]
public class EducationController(IConfiguration configuration, IRedisService redisService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Webhook(object webhook)
    {
        using var httpClient = new HttpClient();

        var userUrl = configuration.GetValue<string>("InternalService:UserServiceEducation") ??
                      throw new Exception("Missing Sidecar:Url");
        var tmp = JObject.Parse(webhook.ToString());
        if (tmp["event"]?.ToString() == "entry.create")
        {
            switch (tmp["model"].ToString())
            {
                case "education":
                {
                    try
                    {
                        var ttt = new StringContent(JsonConvert.SerializeObject(new
                        {
                            data = new
                            {
                                educationName = tmp["entry"]["educationName"],
                                educationCode = tmp["entry"]["educationCode"],
                                educationId = tmp["entry"]["id"],
                            }
                        }), Encoding.UTF8, "application/json");
                        await httpClient.PostAsync(userUrl + "/personal-educations", ttt);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    break;
                }
                case "subject":
                {
                    var ttt = new StringContent(JsonConvert.SerializeObject(new
                    {
                        data = new
                        {
                            subjectName = tmp["entry"]["SubjectName"],
                            subjectCode = tmp["entry"]["subjectCode"],
                            subjectId = tmp["entry"]["id"],
                            educationId = tmp["entry"]["education"]["id"],
                        }
                    }), Encoding.UTF8, "application/json");
                    Console.WriteLine(ttt);
                    await httpClient.PostAsync(userUrl + "/subjects", ttt);
                    break;    
                }
                case "course":
                {
                    var ttt = new StringContent(JsonConvert.SerializeObject(new
                    {
                        data = new
                        {
                            subjectId = tmp["entry"]["subject"]["id"],
                            courseId = tmp["entry"]["id"],
                            courseName = tmp["entry"]["SubjectName"] + " - " + tmp["entry"]["courseCode"],
                        }
                    }), Encoding.UTF8, "application/json");
                    Console.WriteLine(ttt);
                    await httpClient.PostAsync(userUrl + "/courses", ttt);
                    break;    
                }
            }

        }


        if (tmp["model"].ToString() == "history-enrollment")
        {
            Console.WriteLine(tmp);
            var key = $"{tmp["entry"]["documentId"]}";
            if (tmp["event"]?.ToString() == "entry.create" || tmp["event"]?.ToString() == "entry.update")
            {
                await redisService.StringSetAsync(key, new Enrollment()
                {
                    MaxStudent = int.Parse(tmp["entry"]["course"]["maxStudent"].ToString()),
                    StartDate = DateTime.Parse(tmp["entry"]["startDate"].ToString()),
                    EndDate = DateTime.Parse(tmp["entry"]["endDate"].ToString()),
                    IsEnable = bool.Parse(tmp["entry"]?["isEnable"]?.ToString() ?? "false"),
                    CourseId = int.Parse(tmp["entry"]["course"]["id"].ToString()),
                    DocumentId = tmp["entry"]["documentId"].ToString()
                });

                var res = await httpClient.GetStringAsync(userUrl +
                                                          "/users?populate[user_educations][populate][personal_subjects][populate][personal_courses][populate]=course&populate[user_educations][populate]=education");
                var data = JsonConvert.DeserializeObject<List<User>>(res);
                
                
                
                foreach (var user in data)
                {
                    foreach (var userUserEducation in user.UserEducations)
                    {
                        foreach (var personalSubject in userUserEducation.PersonalSubjects)
                        {
                            foreach (var personalSubjectPersonalCourse in personalSubject.PersonalCourses)
                            {
                                await redisService.StringSetAsync(user.Id.ToString(), JsonConvert.SerializeObject(personalSubject.PersonalCourses.Select(e => e.DocumentId)));
                            }
                        }
                    }
                }

                Console.WriteLine(JsonConvert.SerializeObject(data));
            }
            

                
            
            if (tmp["event"]?.ToString() == "entry.delete")
            {
                await redisService.RemoveAsync(key);
            }
        }
        
        
        
        
        
        
        
        httpClient.Dispose();
        
        return StatusCode(200);
    }

    [HttpPost("enjoy")]
    public async Task<IActionResult> Enjoy([FromBody] EnjoyBody enjoyBody)
    {
        var info = await redisService.StringGetAsync<string>(enjoyBody.UserId.ToString());
        if (info.Contains(enjoyBody.CourseDocumentId))
        {   
            var enrollment = await redisService.StringGetAsync<Enrollment>(enjoyBody.CourseDocumentId);
            if (enrollment.Students.Count() <= enrollment.MaxStudent)
            {
                enrollment.Students.Add(enjoyBody.UserId);
            }
            await redisService.StringSetAsync(enjoyBody.UserId.ToString(), enrollment);
        }

        Console.WriteLine();
        return Ok();
    }
    
    [HttpGet("{enrollmentId}")]
    public async Task<IActionResult> EnjoyGet(string enrollmentId)
    {
        var enrollment = await redisService.StringGetAsync<Enrollment>(enrollmentId);
        
        return Ok(enrollment);
    }
    
    
}