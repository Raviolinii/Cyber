public static class PasswordVerification
{
    public static bool PasswordVerificationEnabled { get; set; } = true;
    public static bool DoesntHaveDoubles(string password)
    {
        for (int i = 0; i < password.Length; i++)
        {
            for (int j = 0; j < password.Length; j++)
            {
                if (i == j)
                    continue;

                if (password[i] == password[j])
                    return false;
            }
        }
        return true;
    }
    public static bool DateExpired(DateTime expirationDate)
    {
        var now = DateTime.Now;
        var result = DateTime.Compare(expirationDate, now);
        return result > 0;
    }
}
