using SidecarService.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IRedisService, RedisService>();
builder.Services.AddHttpClient();
var app = builder.Build();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();



app.Run();