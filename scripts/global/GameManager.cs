using Godot;
using System;
using System.IO;

public static partial class GameManager
{
    public static bool IsPaused { get; set; } = false;
    public static int ScoreA { get; set; } = 0;
    public static int ScoreB { get; set; } = 0;

    public enum Player
    {
        A = 0,
        B = 1
    };

    public static Player StartingPlayer { get; set; } = Player.A;
}