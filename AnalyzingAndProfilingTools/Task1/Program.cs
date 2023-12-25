using System.Security.Cryptography;

public class Program
{
    public static void Main()
    {
        var text = "TestText";
        var byteArray = new byte[36];
        var test = GeneratePasswordHashUsingSalt(text, byteArray);
        var test2 = GeneratePasswordHashUsingSaltPerformanceIncreased(text, byteArray);
        if (test == test2) { Console.WriteLine("Equal"); }

    }

    static string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
    {
        var iterate = 10000;
        var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
        byte[] hash = pbkdf2.GetBytes(20);
        byte[] hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);
        var passwordHash = Convert.ToBase64String(hashBytes);
        return passwordHash;
    }

    static string GeneratePasswordHashUsingSaltPerformanceIncreased(string text, byte[] salt)
    {
        var iterate = 10000;
        var plainText = Rfc2898DeriveBytes.Pbkdf2(text, salt, iterate, HashAlgorithmName.SHA1, 20); // I have added images ImagesForStackOverflow.jpg project folder why I choose algorithm SHA1

        byte[] plainTextWithSaltBytes = new byte[36];

        for (int i = 0; i < 16; i++)
        {
            plainTextWithSaltBytes[i] = salt[i];
        }
        for (int i = 0; i < 20; i++)
        {
            plainTextWithSaltBytes[16 + i] = plainText[i];
        }

        var temp = Convert.ToBase64String(plainTextWithSaltBytes);
        return temp;
    }

}