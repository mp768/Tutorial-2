using Godot;
using System;

public partial class GameManager : Node2D
{
    [Export]
    public int playerCount = 1;

    [Export]
    public Label labelPlayer1;
    private int scorePlayer1 = 0;

    [Export]
    public Label labelPlayer2;
    private int scorePlayer2 = 0;

    public override void _Ready()
    {
        GameSignals.Instance.PlayerScore += OnScore;
    }

    private void OnScore(int _playerId)
    {
        var playerId = (PlayerId)_playerId;

        switch (playerId)
        {
            case PlayerId.Player1:
                scorePlayer1++;
                if (labelPlayer1 != null) labelPlayer1.Text = "Score: " + scorePlayer1;
                break;

            case PlayerId.Player2:
                scorePlayer2++;
                if (labelPlayer2 != null) labelPlayer2.Text = "Score: " + scorePlayer2;
                break;
        }
    }
}
