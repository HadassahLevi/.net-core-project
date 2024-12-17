using music.Interfaces;
using music.Services;
using FirstMiddlewares;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;



var builder = WebApplication.CreateBuilder(args);




// כל הקטע עם הטוקנים
// Add services to the container.
builder.Services
        .AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(cfg =>
        {
            cfg.RequireHttpsMetadata = false;
            cfg.TokenValidationParameters = TokenService.GetTokenValidationParameters();
        });

        builder.Services.AddAuthorization(cfg =>
        {
            cfg.AddPolicy("Admin", policy => policy.RequireClaim("type","Admin"));
            cfg.AddPolicy("User", policy => policy.RequireClaim("type", "User","Admin"));
        });

        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen(c =>
        {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tasks", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter JWT with Bearer into field",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                        { new OpenApiSecurityScheme
                                {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                                },
                            new string[] {}
                        }
        });
    });
// עד כאן הוספתי לטוקנים



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// מופע יחיד
builder.Services.AddSingleton<IMusicalInstruments, MusicalInstrumentsService>();
builder.Services.AddSingleton<IUsers, UsersService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

/*js*/
app.UseDefaultFiles();
app.UseStaticFiles();
/*js (remove "launchUrl" from Properties\launchSettings.json*/

// מידלוור
// app.UseFirstMiddleware();
// app.Use(async (c, n) => 
// {
//     await c.Response.WriteAsync("Our 3rd Middleware start\n");
//     await n();
//     await c.Response.WriteAsync("Our 3rd Middleware end\n");
// });
// app.Use(async (context, next) => 
// {
//     await context.Response.WriteAsync("Our 4th Middleware start\n");
//     await next();
//     await context.Response.WriteAsync("Our 4th Middleware end\n");
// });



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();