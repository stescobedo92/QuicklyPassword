namespace QuicklyGeneratorPassword.Services;

public interface IPasswordGeneratorService
{
    string GenerateSecurePassword(int length, bool includeUppercase, bool includeLowercase, bool includeNumbers, bool includeSpecial);
    int CalculateEntropy(string password);
    string GetPasswordStrength(int entropy);
    string GetCrackingTime(int entropy);
}
