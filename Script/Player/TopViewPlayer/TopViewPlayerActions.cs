using Godot;
using System;

public partial class TopViewPlayerActions : CharacterBody2D
{
	private int speed = 400;

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		PlayerMovements(delta);
	}

	private void PlayerMovements(double delta)
	{
		// Run
		if (Input.IsKeyPressed(Key.Shift))
			speed = 600; else speed = 400;

		// 8-Way Movements
		Vector2 velocity = Input.GetVector("TopViewPlayerMovement_Left", "TopViewPlayerMovement_Right", "TopViewPlayerMovement_Up", "TopViewPlayerMovement_Down");

		// Apply Movements to Character
		var collision = MoveAndCollide(velocity * speed * (float)delta);
	}
}
