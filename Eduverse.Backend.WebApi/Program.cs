using Eduverse.Backend.Entity.Repository;
using Eduverse.Backend.WebApi.Services;
using Eduverse.Backend.WebApi.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IEduverseRepository, EduverseRepository>();
builder.Services.AddScoped<ILoginService, LoginServices>();
builder.Services.AddScoped<IDirectoryService, DirectoryService>();
builder.Services.AddScoped<INotesService, NoteService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IInqueryService, InqueryService>();
// Add services to the container.

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(bearer =>
{
    bearer.RequireHttpsMetadata = false;
    bearer.SaveToken = true;
    bearer.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
        ValidateLifetime = true,
        ValidateAudience = false,
        ValidateIssuer = true,
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization(options=> options.AddPolicy("MyPolicy", policy => policy.RequireAuthenticatedUser()));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(obj => obj.AddPolicy("EduverseCorsPolicy",obj=>{ obj.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); })) ;

builder.Services.AddSwaggerGen();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
   
}
app.UseCors("EduverseCorsPolicy");


app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    if (context.Request.Path != null )
    {
       string endpoint = context.Request.Path;
        HttpContext userContext = context.Request.HttpContext;
        var user = userContext.User;
        Claim? roles = user?.Claims.Where(ele => ele.Type.ToString().Contains("role")).FirstOrDefault();
        if (roles != null)
        {

            if (endpoint.ToUpper().Contains("ADMIN") && !roles.Value.Contains("ADMIN"))
                return;

            if (endpoint.ToUpper().Contains("AUTHOR") && !roles.Value.Contains("EDU-AUTHOR"))
                return;

        }



    }

    await next();
});
app.MapControllers();

app.Run();
