using Godot;
using System;
using System.Collections.Generic;

public partial class top_view_player_actions : CharacterBody2D
{
	private int speed;

	// Inputs
	private class PlayerInputs
	{
		public float velocityX, velocityY;
		public bool isPressInteractionKey;

		public PlayerInputs()
		{
			this.velocityX = 0;
			this.velocityY = 0;
			this.isPressInteractionKey = false;
		}

		public void Update(float velocityX, float velocityY, bool isPressInteractionKey)
		{
			this.velocityX = velocityX;
			this.velocityY = velocityY;
			this.isPressInteractionKey = isPressInteractionKey;
		}
	}PlayerInputs playerInputs;

	// PlayerData
	private int NormalSpeed;
	private int RunSpeed;

	// Manager
	private game_manager gameManager;
	private object_manager objectManager;
	private event_manager eventManager;

	// ChildNode
	private Area2D interactionArea;
	private Area2D encounterArea;
	private CollisionShape2D interactionAreaCollision;

	// InputMap
	private string moveLeftKey = "TopViewPlayerMovement_Left";
	private string moveRightKey = "TopViewPlayerMovement_Right";
	private string moveUpKey = "TopViewPlayerMovement_Up";
	private string moveDownKey = "TopViewPlayerMovement_Down";

	// Interaction
	private string interactionKey = "TopViewPlayerInteraction";
	private Queue<int> detectedObject;

	// Encounter

	public override void _Ready()
	{
		gameManager = GetNode<game_manager>("../game_manager");
		objectManager = GetNode<object_manager>("../object_manager");
		eventManager = GetNode<event_manager>("../event_manager");

		interactionArea = GetNode<Area2D>("interaction_area");
		interactionArea.AreaEntered += InteractiveObjectEntered;
		interactionArea.AreaExited += InteractiveObjectExited;

		encounterArea = GetNode<Area2D>("encounter_area");
		encounterArea.AreaEntered += EncounterObjectEntered;
		
		playerInputs = new PlayerInputs();
		detectedObject = new Queue<int>();
	}

	public override void _Process(double delta)
	{
		playerInputs.Update(
			Input.GetAxis(moveLeftKey, moveRightKey),
			Input.GetAxis(moveUpKey, moveDownKey),
			Input.IsActionJustPressed(interactionKey)
		);
		UpdatePlayerData();
		PlayerMovements(delta);
		InteractionActions(delta);
	}

	private void UpdatePlayerData()
	{
		NormalSpeed = (int)gameManager.tmpPlayerDataResource.TopViewNormalSpeed;
		RunSpeed = (int)gameManager.tmpPlayerDataResource.TopViewRunSpeed;

		return;
	}

	private void PlayerMovements(double delta)
	{
		// isRun
		if (Input.IsKeyPressed(Key.Shift))
			speed = RunSpeed; else speed = NormalSpeed;

		// Apply Velocity
		MoveAndCollide(new Vector2(playerInputs.velocityX, 0) * speed * (float)delta);
		MoveAndCollide(new Vector2(0, playerInputs.velocityY) * speed * (float)delta);

		return;
	}

	private void InteractionActions(double delta)
	{
		// InteractionArea Update
		utils.UpdateArea2DDir(interactionArea, new Vector2(playerInputs.velocityX, playerInputs.velocityY));

		if (playerInputs.isPressInteractionKey && detectedObject.Count > 0)
			objectInteraction();

		return;
	}

	private void objectInteraction()
	{
		GD.Print("now queue front : " + detectedObject.Peek());

		objectManager.ObjectInteractive(detectedObject.Peek());

		return;
	}

	private void InteractiveObjectEntered(Node interactiveObject)
	{
		GD.Print("Debug1");

		if (interactiveObject is interactive_object targetObject)
			detectedObject.Enqueue(targetObject.objectID);

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
				if (targetObject.objectID == detectedObject.Peek())
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

	private void EncounterObjectEntered(Node encounterObject)
	{
		GD.Print("Debug3");

		if (encounterObject is event_area eventArea)
		{
			eventManager.EventCalled(eventArea.eventID);
		}

		return;
	}
}
