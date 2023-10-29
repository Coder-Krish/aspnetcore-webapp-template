using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "Template APIS'",
        Version = "V1.0.0",
        Description = "aspnetcore-webapp-template Api Collections"
    });

    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",

    });
    config.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }
    });
});
builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", config =>
{
    config.AllowAnyOrigin()
          .AllowAnyHeader()
          .AllowAnyMethod()
          .SetIsOriginAllowed(origin => true);

}));

/*Start Adding Connection String here*/
var connectionString = builder.Configuration.GetConnectionString("AppDb");
builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(connectionString));
/*End of Adding Connection string section*/

//Below code should be used if angular is used as a client
//builder.Services.AddSpaStaticFiles(configuration =>
//{
//    configuration.RootPath = "Client/dist/client";
//});

//Inject Dependency
builder.Services.AddInfrastructureService();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.RequireAuthenticatedSignIn = false;
})
       // Adding Jwt Bearer
       .AddJwtBearer(options =>
       {
           options.SaveToken = true;
           options.RequireHttpsMetadata = false;
           options.TokenValidationParameters = new TokenValidationParameters()
           {
               // The signing key must match!
               ValidateIssuerSigningKey = true,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Tokens:JwtKey"])),
               ValidateIssuer = false,
               ValidateAudience = false,
               ValidAudience = builder.Configuration["Tokens:JwtAudience"],
               ValidIssuer = builder.Configuration["Tokens:JwtIssuer"]
           };
       });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(a =>
    {
        a.SwaggerEndpoint("v1/swagger.json", "aspnetcore-webapp-template Api Collections' V1.0.0");
    });
}

//Use below code if static files are to be used in production environment (if angular is used as a client)
//app.UseStaticFiles();
//if (!app.Environment.IsDevelopment())
//{
//    app.UseSpaStaticFiles();
//}

app.UseRouting();
app.UseCors("CorsPolicy");

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

//Below code should be used if angular is used as a client
//app.UseSpa(spa =>
//{

//    spa.Options.SourcePath = "Client/dist/client";
//    if (app.Environment.IsDevelopment())
//    {
//        spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
//    }
//});

app.Run();
