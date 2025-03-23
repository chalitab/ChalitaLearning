using Amazon.S3;
using ChalitaLearning;
using ChalitaLearning.Extensions;
using ChalitaLearning.Services.AwsService;

var builder = WebApplication.CreateBuilder(args);
builder.AddConfigurations();

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Bind AWS config
builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("MockSetting"));
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());

// Register AWS SDK S3 Client
builder.Services.AddAWSService<IAmazonS3>();

// Register AWS SDK ¦ð¦ Client
builder.Services.AddAWSService<Amazon.SQS.IAmazonSQS>();

// Register your custom S3 service
builder.Services.AddScoped<IAwsS3Service, AwsS3Service>();

// For Consumer use
builder.Services.AddHttpClient();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
