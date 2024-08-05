using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

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

	// Interaction
	private string interactionKey = "TopViewPlayerInteraction";
	private Queue<int> detectedObject;

	public override void _Ready()
	{
		gameManager = GetNode<game_manager>("../game_manager");

		interactionArea = GetNode<Area2D>("interaction_area");
		interactionArea.AreaEntered += InteractiveObjectEntered;
		interactionArea.AreaExited += InteractiveObjectExited;

		detectedObject = new Queue<int>();

		return;
	}

	public override void _Process(double delta)
	{
		UpdatePlayerData();
		PlayerMovements(delta);

		return;
	}

	private void UpdatePlayerData()
	{
		NormalSpeed = (int)gameManager.tmpPlayerDataResource.TopViewNormalSpeed;
		RunSpeed = (int)gameManager.tmpPlayerDataResource.TopViewRunSpeed;

		return;
	}

	private void PlayerMovements(double delta)
	{
		// Run
		if (Input.IsKeyPressed(Key.Shift))
			speed = RunSpeed; else speed = NormalSpeed;

		// 8-Way Movements
		float velocityX = Input.GetAxis(moveLeftKey, moveRightKey);
		float velocityY = Input.GetAxis(moveUpKey, moveDownKey);

		if (Input.IsActionJustPressed(interactionKey))
		{
			utils.UpdateArea2DDir(interactionArea, new Vector2(velocityX, velocityY));
			if (detectedObject.Count > 0)
				objectInteraction();
		}

		// Apply Movements to Character
		MoveAndCollide(new Vector2(velocityX, 0) * speed * (float)delta);
		MoveAndCollide(new Vector2(0, velocityY) * speed * (float)delta);

		return;
	}

	private void objectInteraction()
	{
		GD.Print("now queue front : " + detectedObject.Peek());

		return;
	}

	private void InteractiveObjectEntered(Node interactiveObject)
	{
		GD.Print("Debug1");

		if (interactiveObject is interactive_object targetObject)
			detectedObject.Enqueue(targetObject.objectId);

		if (interactiveObject is interactive_object t)
			GD.Print("enqueue : " + t);

		return;
	}

	private void InteractiveObjectExited(Node interactiveObject)
	{
		GD.Print("Debug2");

		if (interactiveObject is interactive_object t)
			GD.Print("dequeue : " + t);

		if (interactiveObject is interactive_object targetObject)
		{
			int maxIter = detectedObject.Count;

			while (maxIter-- > 0)
			{
				if (targetObject.objectId == detectedObject.Peek())
				{
					detectedObject.Dequeue();
					return;
				}

				detectedObject.Enqueue(detectedObject.Dequeue());
			}

			throw new Exception("InteractiveObjectQueue Remove Error : targetObject doesnt exist in queue");
		}

		return;
	}
}
