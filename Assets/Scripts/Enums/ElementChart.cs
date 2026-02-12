public static class ElementChart
{
    private static readonly float[,] chart =
    {
        { 1f, 2f, 0.5f, 1f },
        { 0.5f, 1f, 1f, 2f },
        { 2f, 1f, 1f, 0.5f },
        { 1f, 0.5f, 2f, 1f }
    };

    private static readonly string[,] text =
    {
        { "normal", "crítico", "poco eficaz", "normal" },
        { "poco eficaz", "normal", "normal", "crítico" },
        { "crítico", "normal", "normal", "poco eficaz" },
        { "normal", "poco eficaz", "crítico", "normal" }
    };

    public static float GetMultiplier(Element attacker, Element defender) => chart[(int)attacker, (int)defender];
    public static string GetText(Element attacker, Element defender) => text[(int)attacker, (int)defender];
}
