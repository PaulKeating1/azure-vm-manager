using AzureVmManager.Services.Implementations;
using AzureVmManager.WebApi.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(x =>
{
    x.Filters.Add<UserActionFilter>();
    x.Filters.Add<ExceptionFilter>();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(options =>
    {
        builder.Configuration.Bind("AzureAd", options);
        options.Events = new JwtBearerEvents();

        /// <summary>
        /// Below you can do extended token validation and check for additional claims, such as:
        ///
        /// - check if the caller's tenant is in the allowed tenants list via the 'tid' claim (for multi-tenant applications)
        /// - check if the caller's account is homed or guest via the 'acct' optional claim
        /// - check if the caller belongs to right roles or groups via the 'roles' or 'groups' claim, respectively
        ///
        /// Bear in mind that you can do any of the above checks within the individual routes and/or controllers as well.
        /// For more information, visit: https://docs.microsoft.com/azure/active-directory/develop/access-tokens#validate-the-user-has-permission-to-access-this-data
        /// </summary>

        //options.Events.OnAuthenticationFailed = async context =>
        //{

        //};

        //options.Events.OnMessageReceived = async context =>
        //{

        //};

        //options.Events.OnTokenValidated = async context =>
        //{

        //};

        //options.Events.OnChallenge = async context =>
        //{

        //};

        //options.Events.OnTokenValidated = async context =>
        //{
        //    string[] allowedClientApps = { /* list of client ids to allow */ };

        //    string clientappId = context?.Principal?.Claims
        //        .FirstOrDefault(x => x.Type == "azp" || x.Type == "appid")?.Value;

        //    if (!allowedClientApps.Contains(clientappId))
        //    {
        //        throw new System.Exception("This client is not authorized");
        //    }
        //};
    }, options => { builder.Configuration.Bind("AzureAd", options); });

builder.Services.Scan(s =>
{
    s.FromAssemblyOf<ArmClientFactory>().AddClasses().AsMatchingInterface().WithScopedLifetime();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
