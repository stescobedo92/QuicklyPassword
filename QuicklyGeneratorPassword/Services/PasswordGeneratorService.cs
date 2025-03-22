namespace QuicklyGeneratorPassword.Services;

using QuicklyGeneratorPassword.Constants;
using System.Security.Cryptography;
using System.Text;

public class PasswordGeneratorService : IPasswordGeneratorService
{
    public int CalculateEntropy(string password)
    {
        bool hasUppercase = false;
        bool hasLowercase = false;
        bool hasDigit = false;
        bool hasSpecial = false;

        foreach (char c in password)
        {
            if (char.IsUpper(c)) hasUppercase = true;
            else if (char.IsLower(c)) hasLowercase = true;
            else if (char.IsDigit(c)) hasDigit = true;
            else hasSpecial = true;
        }

        int charsetSize = 0;
        if (hasUppercase) charsetSize += 26;
        if (hasLowercase) charsetSize += 26;
        if (hasDigit) charsetSize += 10;
        if (hasSpecial) charsetSize += 33;

        // Entropy = longitud * log2(set size)
        return (int)(password.Length * Math.Log2(charsetSize));
    }

    public string GenerateSecurePassword(int length, bool includeUppercase, bool includeLowercase, bool includeNumbers, bool includeSpecial)
    {
        StringBuilder charSet = new StringBuilder();

        if (includeUppercase)
            charSet.Append(SecurePassword.UPPER_CASE);

        if (includeLowercase)
            charSet.Append(SecurePassword.LOWER_CASE);

        if (includeNumbers)
            charSet.Append(SecurePassword.NUMBERS);

        if (includeSpecial)
            charSet.Append(SecurePassword.SPECIAL_CHARACTERS);

        if (charSet.Length == 0)
            throw new ArgumentException(SecurePassword.INVALID_CHARACTER_SET);

        char[] password = new char[length];
        string availableChars = charSet.ToString();

        // We use RandomNumberGenerator to generate cryptographically secure random bytes
        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] randomBytes = new byte[length * 4];
            rng.GetBytes(randomBytes);

            for (int i = 0; i < length; i++)
            {
                uint randomIndex = BitConverter.ToUInt32(randomBytes, i * 4) % (uint)availableChars.Length;
                password[i] = availableChars[(int)randomIndex];
            }
        }

        return new string(password);
    }

    public string GetCrackingTime(int entropy)
    {
        // Assuming 100 billion (10^11) attempts per second
        double possibleCombinations = Math.Pow(2, entropy);
        double secondsToBreak = possibleCombinations / Math.Pow(10, 11);

        if (secondsToBreak < 1) return "Less than a second";
        if (secondsToBreak < 60) return $"{secondsToBreak:F2} seconds";
        if (secondsToBreak < 3600) return $"{secondsToBreak / 60:F2} minutes";
        if (secondsToBreak < 86400) return $"{secondsToBreak / 3600:F2} hours";
        if (secondsToBreak < 31536000) return $"{secondsToBreak / 86400:F2} days";
        if (secondsToBreak < 315360000) return $"{secondsToBreak / 31536000:F2} years";

        // For astronomical times
        double millionYears = secondsToBreak / 31536000 / 1000000;
        if (millionYears < 1000) return $"{millionYears:F2} millions of years";

        double billionYears = millionYears / 1000;
        if (billionYears < 1000) return $"{billionYears:F2} billion years";

        return $"{billionYears / 1000:F2} billion years";
    }

    public string GetPasswordStrength(int entropy)
    {
        if (entropy < 28) return "Very weak";
        if (entropy < 36) return "Weak";
        if (entropy < 60) return "Reasonable";
        if (entropy < 128) return "Strong";
        return "Very Strong";
    }
}
