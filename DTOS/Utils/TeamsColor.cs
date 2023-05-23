namespace BreachersAPI.DTOS.Utils;

public class TeamsColor
{
    public const string Enforcers = "4589E9";
    public const string Revolters = "F1C824";
    
    public static string GetTeamNameFromColor(string colorHex)
    {
        switch (colorHex)
        {
            case Enforcers:
                return "Enforcers";
            case Revolters:
                return "Revolters";
            default:
                return string.Empty;
        }
    }
}