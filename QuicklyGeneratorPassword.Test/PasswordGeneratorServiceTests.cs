using QuicklyGeneratorPassword.Services;

namespace QuicklyGeneratorPassword.Test;

public class PasswordGeneratorServiceTests
{
    private readonly IPasswordGeneratorService _passwordService;

    public PasswordGeneratorServiceTests()
    {
        _passwordService = new PasswordGeneratorService();
    }

    [Fact]
    public void GenerateSecurePassword_WithDefaultParameters_ReturnsPasswordWithCorrectLength()
    {
        // Arrange
        int expectedLength = 16;

        // Act
        string password = _passwordService.GenerateSecurePassword(expectedLength, true, true, true, true);

        // Assert
        Assert.Equal(expectedLength, password.Length);
    }

    [Theory]
    [InlineData(8)]
    [InlineData(16)]
    [InlineData(32)]
    [InlineData(64)]
    public void GenerateSecurePassword_WithDifferentLengths_ReturnsPasswordWithCorrectLength(int length)
    {
        // Arrange & Act
        string password = _passwordService.GenerateSecurePassword(length, true, true, true, true);

        // Assert
        Assert.Equal(length, password.Length);
    }

    [Fact]
    public void GenerateSecurePassword_WithUppercaseOnly_ReturnsPasswordWithOnlyUppercase()
    {
        // Arrange
        int length = 20;

        // Act
        string password = _passwordService.GenerateSecurePassword(length, true, false, false, false);

        // Assert
        Assert.Equal(length, password.Length);
        Assert.True(password.All(char.IsUpper));
    }

    [Fact]
    public void GenerateSecurePassword_WithLowercaseOnly_ReturnsPasswordWithOnlyLowercase()
    {
        // Arrange
        int length = 20;

        // Act
        string password = _passwordService.GenerateSecurePassword(length, false, true, false, false);

        // Assert
        Assert.Equal(length, password.Length);
        Assert.True(password.All(char.IsLower));
    }

    [Fact]
    public void GenerateSecurePassword_WithNumbersOnly_ReturnsPasswordWithOnlyNumbers()
    {
        // Arrange
        int length = 20;

        // Act
        string password = _passwordService.GenerateSecurePassword(length, false, false, true, false);

        // Assert
        Assert.Equal(length, password.Length);
        Assert.True(password.All(char.IsDigit));
    }

    [Fact]
    public void GenerateSecurePassword_WithSpecialCharsOnly_ReturnsPasswordWithOnlySpecialChars()
    {
        // Arrange
        int length = 20;
        string specialChars = "!@#$%^&*()-_=+[]{}|;:,.<>?/~";

        // Act
        string password = _passwordService.GenerateSecurePassword(length, false, false, false, true);

        // Assert
        Assert.Equal(length, password.Length);
        Assert.True(password.All(c => specialChars.Contains(c)));
    }

    [Fact]
    public void GenerateSecurePassword_GeneratesTwoDifferentPasswords()
    {
        // Arrange
        int length = 16;

        // Act
        string password1 = _passwordService.GenerateSecurePassword(length, true, true, true, true);
        string password2 = _passwordService.GenerateSecurePassword(length, true, true, true, true);

        // Assert
        Assert.NotEqual(password1, password2);
    }
}