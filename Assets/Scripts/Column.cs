using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Column : Area2D
{
	[Export]
	public float speed = 45.0f;

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		GameSignals.Instance.GameOver += GameOver;
		GameSignals.Instance.ResetGame += Reset;
	}

	public override void _PhysicsProcess(double delta)
	{
		// NOTE: It's negative X because we're moving left.
		Position += new Vector2(-speed * (float)delta, 0.0f);
	}

	#region game over and restart stuff
	private void GameOver()
	{
		SetPhysicsProcess(false);
	}

	private void Reset()
	{
		SetPhysicsProcess(true);
	}
	#endregion

	public void Disable()
	{
		var topColumnCollisionShape = GetNode<CollisionShape2D>("Top Column/CollisionShape2D");
		var bottomColumnCollisionShape = GetNode<CollisionShape2D>("Bottom Column/CollisionShape2D");

		topColumnCollisionShape.SetDeferred("disabled", true);
		bottomColumnCollisionShape.SetDeferred("disabled", true);

		ProcessMode = ProcessModeEnum.Disabled;
	}

	public void Enable()
	{
		var topColumnCollisionShape = GetNode<CollisionShape2D>("Top Column/CollisionShape2D");
		var bottomColumnCollisionShape = GetNode<CollisionShape2D>("Bottom Column/CollisionShape2D");

		topColumnCollisionShape.SetDeferred("disabled", false);
		bottomColumnCollisionShape.SetDeferred("disabled", false);

		ProcessMode = ProcessModeEnum.Inherit;
	}

	private void OnBodyEntered(Node node)
	{
		if (node is FlappyBird bird)
		{
			GameSignals.Instance.EmitSignal(GameSignals.SignalName.PlayerScore, (int)bird.playerId);
		}
	}
}
