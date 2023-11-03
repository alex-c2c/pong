using Godot;
using System;
using System.Collections.Generic;
using System.IO;
//using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;

public static partial class GameUtils
{
    public static bool IsPlayerAStart
    {
        get { return (GameManager.StartingPlayer == GameManager.Player.A); }
    }
}
