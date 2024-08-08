using Godot;
using System;
using System.Runtime.Serialization;

public partial class side_view_player_actions : CharacterBody2D
{
	private int gravity = 2000;
	private int jumpSpeed = -1500;
	private int speed = 400;
	private float acceleration = 0.25f;
	private float friction = 0.12f;

	// Inputs
	private class PlayerInputs
	{
		public float HorizontalAxis, VerticalAxis;
		public bool isPressJumpKey;

		public PlayerInputs()
		{
			this.HorizontalAxis = 0;
			this.VerticalAxis = 0;
			this.isPressJumpKey = false;
		}

		public void Update(float HorizontalAxis, float VerticalAxis, bool isPressJumpKey)
		{
			this.HorizontalAxis = HorizontalAxis;
			this.VerticalAxis = VerticalAxis;
			this.isPressJumpKey = isPressJumpKey;

			return;
		}
	}
	PlayerInputs playerInputs;

	private int JumpCnt = 0;

	private bool CanAction = true;

	// Manager
	private game_manager gameManager;
	private object_manager objectManager;
	private event_manager eventManager;

	// InputMap
	private string moveLeft = "SideViewPlayerMovement_Left";
	private string moveRight = "SideViewPlayerMovement_Right";

	public override void _Ready()
	{
		gameManager = GetNode<game_manager>("/root/game_manager");
		objectManager = GetNode<object_manager>("/root/object_manager");
		eventManager = GetNode<event_manager>("/root/event_manager");

		playerInputs = new PlayerInputs();
	}

	public override void _Process(double delta)
	{
		playerInputs.Update(
			Input.GetAxis(moveLeft, moveRight),
			0,
			Input.IsActionJustPressed("SideViewPlayerMovement_Jump")
		);

		if (CanAction)
		{
            PlayerMovements(delta);
        }

		return;
	}

	private void PlayerMovements(double delta)
	{
		Vector2 velocity = Velocity;

		if (playerInputs.HorizontalAxis != 0) 
			velocity.X = Mathf.Lerp(velocity.X, playerInputs.HorizontalAxis * speed, acceleration);
		else velocity.X = Mathf.Lerp(velocity.X, 0.0f, friction);

		Velocity = velocity;

		// Apply Movements to Character
		MoveAndSlide();

		if (IsOnFloor())
		{
			JumpCnt = 1;
			velocity.Y = 0;
		}

		velocity.Y = utils.Clamp(velocity.Y + gravity * (float)delta, -500, 800);

		if (playerInputs.isPressJumpKey)
		{
			if (IsOnFloor())
			{
				velocity.Y = jumpSpeed;
			}
			else if (JumpCnt > 0)
			{
				velocity.Y = jumpSpeed;
				JumpCnt--;
			}
		}

		Velocity = velocity;

		MoveAndSlide();

		return;
	}
}
