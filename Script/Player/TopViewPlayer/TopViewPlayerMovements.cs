using Godot;
using System;

public partial class TopViewPlayerMovements : CharacterBody2D
{
	private int speed = 400;
	
	public TopViewPlayerMovements() {}

	public override void _Process(double delta)
	{
		PlayerMovements(delta);
	}

	private void PlayerMovements(double delta)
	{
		Vector2 velocity = new Vector2();

		// Run
		if (Input.IsKeyPressed(Key.Shift))
			speed = 600; else speed = 400;

		// 8-Way Movements
		if (Input.IsActionPressed("TopViewPlayerMovement_Left"))
			velocity.X--;
		if (Input.IsActionPressed("TopViewPlayerMovement_Right"))
			velocity.X++;
		if (Input.IsActionPressed("TopViewPlayerMovement_Up"))
			velocity.Y--;
		if (Input.IsActionPressed("TopViewPlayerMovement_Down"))
			velocity.Y++;

		// Normalize
		if (velocity != Vector2.Zero)
			velocity.Normalized();

		// Apply Movements to Character
		var collision = MoveAndCollide(velocity * speed * (float)delta);
	}
}
