using Godot;

public partial class GameSignals : Node
{
    public static GameSignals Instance { get; private set; }

    // NOTE: Godot has this concept of a "signal", which is essentially just a callback. For the C# version of
    // Godot, you need to have the "EventHandler" suffix for any "signal" you want to declare. 
    //
    // When accessing the signal, declared as "XYZEventHandler" for example, you would use "SignalName.XYZ" to 
    // get it's string name. To then "listen" to it and "invoke" the callback, you would use either 
    // 
    // "XYZ += MethodName" to set up the listener and "XYZ -= MethodName" to disconnect it
    // or 
    // "EmitSignal(SignalName.XYZ, ..any args go here)" to invoke the callback, calling all connected listeners.
    //

    [Signal]
    public delegate void PlayerDiedEventHandler();

    [Signal]
    public delegate void PlayerScoreEventHandler(int playerId);

    [Signal]
    public delegate void ResetGameEventHandler();

    [Signal]
    public delegate void GameOverEventHandler();

    [Signal]
    public delegate void ResetPlayerPositionEventHandler(int playerId, Vector2 position);

    public override void _Ready()
    {
        Instance = this;
    }
}