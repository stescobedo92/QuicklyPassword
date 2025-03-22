using Microsoft.OpenApi.Models;
using QuicklyGeneratorPassword.Constants;
using QuicklyGeneratorPassword.Services;

var builder = WebApplication.CreateBuilder(args);

// Swagger  Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(SecurePassword.SWAGGER_API_VERSION, new OpenApiInfo
    {
        Title = SecurePassword.SWAGGER_TITLE,
        Description = SecurePassword.SWAGGER_DESCRIPTION,
        Version = SecurePassword.SWAGGER_API_VERSION
    });
});

builder.Services.AddSingleton<IPasswordGeneratorService, PasswordGeneratorService>();

var app = builder.Build();

// Middlewares
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint(SecurePassword.SWAGGER_URL, SecurePassword.SWAGGER_TITLE);
});

app.MapGet("/quickpass", async (int length, bool? includeUppercase, bool? includeLowercase, bool? includeNumbers, bool? includeSpecial, IPasswordGeneratorService passwordService) =>
{
    try
    {

        if (length <= 0)
            return Results.BadRequest(SecurePassword.PASSWORD_GREATER_THAN_ZERO);

        includeUppercase ??= true;
        includeLowercase ??= true;
        includeNumbers ??= true;
        includeSpecial ??= true;

        if (!includeUppercase.Value && !includeLowercase.Value && !includeNumbers.Value && !includeSpecial.Value)
            return Results.BadRequest(SecurePassword.INVALID_CHARACTER_SET);

        string password = passwordService.GenerateSecurePassword(length, includeUppercase.Value, includeLowercase.Value, includeNumbers.Value, includeSpecial.Value);

        // Calculate security metrics
        int entropy = passwordService.CalculateEntropy(password);
        string strength = passwordService.GetPasswordStrength(entropy);
        string crackingTime = passwordService.GetCrackingTime(entropy);

        var response = new
        {
            Password = password,
            Length = length,
            SecurityInfo = new
            {
                Entropy = entropy,
                Strength = strength,
                EstimatedCrackingTime = crackingTime
            },
            GeneratedOn = DateTime.UtcNow
        };

        return Results.Ok(response);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.WithName("GeneratePassword")
.WithDescription("Generates a secure password with the specified parameters")
.WithOpenApi(operation =>
{
    operation.Summary = "Generate a secure password";
    operation.Parameters[0].Description = "Password length";
    operation.Parameters[1].Description = "Include capital letters";
    operation.Parameters[2].Description = "Include lowercase letters";
    operation.Parameters[3].Description = "Include numbers";
    operation.Parameters[4].Description = "Include special characters";
    return operation;
});

app.Run();