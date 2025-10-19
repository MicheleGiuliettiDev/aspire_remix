var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.AddServiceDefaults();

builder.Services.AddAuthentication()
  .AddKeycloakJwtBearer(serviceName: "keycloak", realm: "overflow", options =>
      {
          options.RequireHttpsMetadata = false;
          options.Audience = "overflow";
      });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapDefaultEndpoints();

app.Run();
