using Godot;
using System;

public partial class side_view_player_actions : CharacterBody2D
{
	private int gravity = 2000;
	private int jumpSpeed = -2500;
	private int speed = 400;
	private float acceleration = 0.25f;
	private float friction = 0.12f;

	public override void _Process(double delta)
	{
		PlayerMovements(delta);
	}

	private void PlayerMovements(double delta)
	{
		Vector2 velocity = Velocity;
		float dir = Input.GetAxis("SideViewPlayerMovement_Left", "SideViewPlayerMovement_Right");

		if (dir != 0)
			velocity.X = Mathf.Lerp(velocity.X, dir * speed, acceleration);
		else velocity.X = Mathf.Lerp(velocity.X, 0.0f, friction);

		velocity.Y = utils.Clamp(velocity.Y + gravity * (float)delta, -500, 500);

		if (Input.IsActionJustPressed("SideViewPlayerMovement_Jump") && IsOnFloor())
			velocity.Y = jumpSpeed;

		// Apply Movements to Character
		MoveAndSlide();

		Velocity = velocity;
	}
}
