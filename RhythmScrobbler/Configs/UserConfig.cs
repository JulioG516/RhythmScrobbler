using System.Collections.Generic;

namespace RhythmScrobbler.Configs;

public class UserConfig
{
    public string Username { get; set; }
    public string Password { get; set; }

    public List<GameConfig> GameConfigs { get; set; }
}