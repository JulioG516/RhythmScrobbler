﻿namespace RhythmScrobbler.Models;

public class Game
{
    public string Name { get; set; }
    public string Path { get; set; }

    public EnumGameType Type { get; set; }
}