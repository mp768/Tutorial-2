
using System.Runtime.CompilerServices;

public enum PlayerId
{
    Player1,
    Player2,
}

public static class PlayerIdEnumExtension
{
    public static string GetUserInputAction(this PlayerId playerId)
    {
        return playerId switch
        {
            PlayerId.Player1 => "jump1",
            PlayerId.Player2 => "jump2",
            _ => "jump1",
        };
    }
}