public static class StringExtensions
{
    /// <summary>
    /// Returns strings with specififc amount of tabs before it
    /// </summary>
    /// <param name="input">Input string</param>
    /// <param name="amount">Amount of tabs</param>
    /// <returns></returns>
    public static string CodeIndent(this string input, int amount)
    {
        if (amount < 0)
            throw new System.ArgumentOutOfRangeException($"{amount} amount is out of bounds");
        string output = "";
        for (int i = 0; i < amount; i++)
            output += "\t";
        output += input;
        return output;
    }
}
