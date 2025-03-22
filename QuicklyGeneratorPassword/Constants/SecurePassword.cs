namespace QuicklyGeneratorPassword.Constants;

public class SecurePassword
{
    public const string UPPER_CASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public const string LOWER_CASE = "abcdefghijklmnopqrstuvwxyz";
    public const string NUMBERS = "0123456789";
    public const string SPECIAL_CHARACTERS = "!@#$%^&*()-_=+[]{}|;:,.<>?/~";
    public const int MINIMUM_PASSWORD_LENGTH = 8;

    //Exception messages
    public const string PASSWORD_TOO_SHORT = "Password must be at least 8 characters long";
    public const string PASSWORD_GREATER_THAN_ZERO = "The length must be greater than 0";
    public const string NO_CHARACTER_SET_SELECTED = "You must select at least one character set";
    public const string INVALID_ENTROPY = "Entropy must be a positive number";
    public const string INVALID_CRACKING_TIME = "Cracking time must be a positive number";
    public const string INVALID_PASSWORD_STRENGTH = "Password strength must be a positive number";
    public const string INVALID_CHARACTER_SET = "You must select at least one character set";

    //Swagger
    public const string SWAGGER_URL = "/swagger/v1/swagger.json";
    public const string SWAGGER_TITLE = "Secure Password Generator API";
    public const string SWAGGER_DESCRIPTION = "API for generating secure and robust passwords";
    public const string SWAGGER_API_VERSION = "v1";
}
