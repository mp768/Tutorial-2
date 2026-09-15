using Godot;
using System;

public partial class Column : Area2D
{
	[Export]
	public float speed = 45.0f;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	public override void _PhysicsProcess(double delta)
	{
		// NOTE: It's negative X because we're moving left.
		Position += new Vector2(-speed * (float)delta, 0.0f);
	}

	private void OnBodyEntered(Node node)
	{
		if (node is FlappyBird bird)
		{
			GameSignals.Instance.EmitSignal(GameSignals.SignalName.PlayerScore, (int)bird.playerId);
		}
	}
}
