public enum Gamemode
{
    Deathmatch, // ������ġ
    Occupation, // ������
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