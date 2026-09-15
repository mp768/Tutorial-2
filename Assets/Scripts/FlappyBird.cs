using Godot;
using System;

public partial class FlappyBird : RigidBody2D
{
	[Export]
	public float jumpForce = 500.0f;

	[Export]
	public PlayerId playerId;

	private bool isDead = false;
	private AnimatedSprite2D animatedSprite;

	// NOTE: These constants are simply for doing the little "tilt" that the original
	// flappy bird does when jumping. It just feels odd to me not having that.
	private const float ROTATION_START = -18.0f;
	private const float ROTATION_END = 15.0f;

	private bool hasToReset = false;
	private Vector2 resetPosition;

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		BodyEntered += OnBodyEntered;
		GameSignals.Instance.ResetGame += Reset;
		GameSignals.Instance.ResetPlayerPosition += PlayerResetPosition;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!isDead)
		{
			if (Input.IsActionJustPressed(playerId.GetUserInputAction()))
			{
				animatedSprite.Play("flappy");
				// Set the animation frame to 0 to force it to start at the "open wing" part
				// of the animation.
				animatedSprite.Frame = 0;

				// Apply the (upward) jump force to the flappy bird.
				// NOTE: Negative Y values are up in Godot 2D.
				// 2ND NOTE: Using the 'ApplyForce' method from 'RigidBody2D' doesn't seem
                // to work as well as applying the force through linear velocity.
                LinearVelocity = new Vector2(0, -jumpForce);

                // Start the "tilt" of the flappy bird.
                animatedSprite.Rotation = Mathf.DegToRad(ROTATION_START);
            }
        }

        animatedSprite.Rotation = Mathf.LerpAngle(animatedSprite.Rotation, Mathf.DegToRad(ROTATION_END), (float)(delta * 1.5));
    }

    public override void _IntegrateForces(PhysicsDirectBodyState2D state)
    {
        // NOTE: Can only reset the position properly in this method.
        if (hasToReset)
        {
            state.Transform = new Transform2D(0.0f, resetPosition);
            hasToReset = false;
        }
    }

    private void OnBodyEntered(Node node)
    {
        if (isDead) return;

        GD.Print("Collided with something! ", node.GetGroups(), " ", node.Name);

        if (node.IsInGroup(Constants.KILL_SURFACE_TAG))
        {
            isDead = true;
            animatedSprite.Play("dead");

            GameSignals.Instance.EmitSignal(GameSignals.SignalName.PlayerDied);
        }
    }

    private void PlayerResetPosition(int _playerId, Vector2 position)
    {
        var playerId = (PlayerId)_playerId;

        if (playerId == this.playerId)
        {
            hasToReset = true;
            resetPosition = position;
        }
    }

    private void Reset()
    {
        isDead = false;
        animatedSprite.Play("flappy");

        LinearVelocity = Vector2.Zero;
        AngularVelocity = 0.0f;

        animatedSprite.Rotation = 0.0f;
        Rotation = 0.0f;
    }
}
