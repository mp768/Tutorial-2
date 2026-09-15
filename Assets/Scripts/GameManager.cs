using Godot;
using System;

public partial class GameManager : Node2D
{
	[Export]
	private FlappyBird[] players;
	private Vector2[] initialPlayerPositions;
	private int playersDead = 0;

	[Export]
	private Label labelPlayer1;
	private int scorePlayer1 = 0;

	[Export]
	private Label labelPlayer2;
	private int scorePlayer2 = 0;

	private bool isGameOver = false;

	private Control GameOverStuff;

	public override void _Ready()
	{
		initialPlayerPositions = new Vector2[players.Length];
		for (int i = 0; i < players.Length; i++)
		{
			initialPlayerPositions[i] = players[i].Position;
		}

		GameSignals.Instance.PlayerScore += OnScore;
		GameSignals.Instance.PlayerDied += OnDie;

		GameOverStuff = GetNode<Control>("GameOver Stuff");
		GameOverStuff.Visible = false;
	}

	public override void _Process(double _delta)
	{
		if (
			isGameOver
			&& (
			Input.IsActionJustPressed(PlayerId.Player1.GetUserInputAction()) ||
			Input.IsActionJustPressed(PlayerId.Player2.GetUserInputAction())
			)
		)
		{
			GameOverStuff.Visible = false;
			isGameOver = false;

			scorePlayer1 = 0;
			scorePlayer2 = 0;

			playersDead = 0;

			UpdateScorePlayerLabels();

			GameSignals.Instance.EmitSignal(GameSignals.SignalName.ResetGame);

			// Re-enable everything when we restart.
			foreach (var node in GetChildren())
			{
				node.CallDeferred("set_physics_process", true);
				node.CallDeferred("set_physics_process_internal", true);
				node.CallDeferred("set_process_internal", true);
			}

			// Have to do the positional setting here since the player's don't know where they started initially.
			for (int i = 0; i < players.Length; i++)
			{
				GameSignals.Instance.EmitSignal(GameSignals.SignalName.ResetPlayerPosition, (int)players[i].playerId, initialPlayerPositions[i]);
			}
		}
	}

	private void GameOver()
	{
		isGameOver = true;

		GameOverStuff.Visible = true;

		GameSignals.Instance.EmitSignal(GameSignals.SignalName.GameOver);

		// Disable all children nodes so we don't continue the game any further whilst we're in a 
		// game over state.
		foreach (var node in GetChildren())
		{
			node.SetPhysicsProcess(false);
			node.SetPhysicsProcessInternal(false);
			node.SetProcessInternal(false);
		}
	}

	private void OnDie()
	{
		playersDead += 1;

		// Match how many players have died to how many there are to determine gameover.
		if (playersDead >= players.Length)
		{
			GameOver();
		}
	}

	private void OnScore(int _playerId)
	{
		var playerId = (PlayerId)_playerId;

		switch (playerId)
		{
			case PlayerId.Player1:
				scorePlayer1++;
				break;

			case PlayerId.Player2:
				scorePlayer2++;
				break;
		}

		UpdateScorePlayerLabels();
	}

	private void UpdateScorePlayerLabels()
	{
		if (labelPlayer1 != null)
		{
			labelPlayer1.Text = "Score: " + scorePlayer1;
		}

		if (labelPlayer2 != null)
		{
			labelPlayer2.Text = "Score: " + scorePlayer2;
		}
	}
}
