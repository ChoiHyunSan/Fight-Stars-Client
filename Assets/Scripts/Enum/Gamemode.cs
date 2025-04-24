public enum Gamemode
{
    Deathmatch, // 데스매치
    Occupation, // 점령전
}

public class GamemodeHelper
{
    public static string GetGamemodeName(Gamemode gamemode)
    {
        switch (gamemode)
        {
            case Gamemode.Deathmatch:
                return "Deathmatch";
            case Gamemode.Occupation:
                return "Occupation";
            default:
                return "Unknown";
        }
    }
}