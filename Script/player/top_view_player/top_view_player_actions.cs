using Godot;
using System;

public partial class top_view_player_actions : CharacterBody2D
{
	private int speed;

	// PlayerData
	private int NormalSpeed;
	private int RunSpeed;

	// Manager
	private game_manager gameManager;

	// ChildNode
	private Area2D interactionArea;
	private CollisionShape2D interactionAreaCollision;

	// InputMap
	private string moveLeftKey = "TopViewPlayerMovement_Left";
	private string moveRightKey = "TopViewPlayerMovement_Right";
	private string moveUpKey = "TopViewPlayerMovement_Up";
	private string moveDownKey = "TopViewPlayerMovement_Down";

	private string interactionKey = "TopViewPlayerInteraction";

	public override void _Ready()
	{
		gameManager = GetNode<game_manager>("../game_manager");

		interactionArea = GetNode<Area2D>("interaction_area");
	}

	public override void _Process(double delta)
	{

		UpdatePlayerData();
		PlayerMovements(delta);
	}

	private void UpdatePlayerData()
	{
		NormalSpeed = (int)gameManager.tmpPlayerDataResource.TopViewNormalSpeed;
		RunSpeed = (int)gameManager.tmpPlayerDataResource.TopViewRunSpeed;
	}

	private void PlayerMovements(double delta)
	{
		// Run
		if (Input.IsKeyPressed(Key.Shift))
			speed = RunSpeed; else speed = NormalSpeed;

		// 8-Way Movements
		float velocityX = Input.GetAxis(moveLeftKey, moveRightKey);
		float velocityY = Input.GetAxis(moveUpKey, moveDownKey);

		if (Input.IsActionPressed(interactionKey))
		{
			utils.UpdateInterationAreaDir(interactionArea, new Vector2(velocityX, velocityY));
			objectInteraction();
		}

		// Apply Movements to Character
		MoveAndCollide(new Vector2(velocityX, 0) * speed * (float)delta);
		MoveAndCollide(new Vector2(0, velocityY) * speed * (float)delta);
	}

	private void objectInteraction()
	{

	}
}
