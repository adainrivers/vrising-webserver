using ProjectM.Network;

namespace VRising.WebServer.Models;

public class UserModel
{
    public UserModel() { }

    public static UserModel FromUser(User user)
    {
        var model = new UserModel
        {
            Index = user.Index,
            PlatformId = user.PlatformId,
            CharacterName = user.CharacterName.ToString(),
            IsAdmin = user.IsAdmin,
            IsConnected = user.IsConnected,
            TimeLastConnected = user.TimeLastConnected
        };
        return model;
    }

    public long TimeLastConnected { get; set; }

    public bool IsConnected { get; set; }

    public bool IsAdmin { get; set; }

    public string CharacterName { get; set; }

    public ulong PlatformId { get; set; }

    public int Index { get; set; }
}