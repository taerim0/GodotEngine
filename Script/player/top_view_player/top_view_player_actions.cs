using Godot;
using System;
using System.Collections.Generic;

public partial class top_view_player_actions : CharacterBody2D
{
	private int speed;
	private bool isUiVisible = false;
	private bool isMenuVisible = false;
	private bool isSubMenuVisible = false;
	private bool isEscapeVisible = false;

	// Inputs
	private class PlayerInputs
	{
		public float HorizontalAxis, VerticalAxis;
		public bool isPressInteractionKey;
		public bool isPressMenuKey;
		public bool isPressEscapeKey;

		public PlayerInputs()
		{
			this.HorizontalAxis = 0;
			this.VerticalAxis = 0;
			this.isPressInteractionKey = false;
		}

		public void Update(
			float HorizontalAxis,
			float VerticalAxis,
			bool isPressInteractionKey,
			bool isPressMenuKey,
			bool isPressEscapeKey
			) {
			this.HorizontalAxis = HorizontalAxis;
			this.VerticalAxis = VerticalAxis;
			this.isPressInteractionKey = isPressInteractionKey;
			this.isPressMenuKey = isPressMenuKey;
			this.isPressEscapeKey = isPressEscapeKey;

			return;
		}
	}PlayerInputs playerInputs;

	// Manager
	private game_manager gameManager;
	private object_manager objectManager;
	private event_manager eventManager;

	// ChildNode
	private Area2D interactionArea;
	private Area2D encounterArea;
	private CollisionShape2D interactionAreaCollision;

	private menu_ui menuUi;

	// InputMap
	private string moveLeftKey = "TopViewPlayerMovement_Left";
	private string moveRightKey = "TopViewPlayerMovement_Right";
	private string moveUpKey = "TopViewPlayerMovement_Up";
	private string moveDownKey = "TopViewPlayerMovement_Down";

	// Interaction
	private string interactionKey = "TopViewPlayerInteraction";
	private Queue<int> detectedObject;

	// Action - Ui Call
	private string MenuKey = "TopViewPlayerAction_Menu";
	private string EscapeKey = "TopViewPlayerAction_Escape";

	// Encounter

	public override void _Ready()
	{
		gameManager = GetNode<game_manager>("/root/game_manager");
		objectManager = GetNode<object_manager>("/root/object_manager");
		eventManager = GetNode<event_manager>("/root/event_manager");

		interactionArea = GetNode<Area2D>("interaction_area");
		interactionArea.AreaEntered += InteractiveObjectEntered;
		interactionArea.AreaExited += InteractiveObjectExited;

		encounterArea = GetNode<Area2D>("encounter_area");
		encounterArea.AreaEntered += EncounterObjectEntered;

		Position = gameManager.playerDataResource.mapPos;
		
		playerInputs = new PlayerInputs();
		detectedObject = new Queue<int>();

		menuUi = GetNode<menu_ui>("menu_ui");
		menuUi.Visible = false;
		menuUi.SetProcessInput(false);

		return;
	}

	public override void _Process(double delta)
	{
		playerInputs.Update(
			Input.GetAxis(moveLeftKey, moveRightKey),
			Input.GetAxis(moveUpKey, moveDownKey),
			Input.IsActionJustPressed(interactionKey),
			Input.IsActionJustPressed(MenuKey),
			Input.IsActionJustPressed(EscapeKey)
		);

		isUiVisible = isUiVisibleUpdate();

		if (playerInputs.isPressMenuKey)
		{
			if (!isUiVisible)
			{
				menuUi.Visible = true;
				isMenuVisible = true;
				menuUi.SetProcessInput(true);
				isUiVisible = true;
			}
			else if (isMenuVisible)
			{
				isMenuVisible = false;
				menuUi.Close();
			}
		}

		if (playerInputs.isPressEscapeKey)
		{
			if (isSubMenuVisible)
			{
				isSubMenuVisible = false;
				menuUi.CloseSubUi();
			}
			else if (isMenuVisible)
			{
				isMenuVisible = false;
				menuUi.Close();
			}
			else if (isEscapeVisible)
			{
				isEscapeVisible = false;
			}
			else
			{
				isEscapeVisible = true;
			}
		}

		if (!isUiVisible)
		{
			PlayerMovements(delta);
			InteractionActions(delta);
		}
	}

	private bool isUiVisibleUpdate()
	{
		if (isMenuVisible) return true;
		if (isEscapeVisible) return true;
		return false;
	}

	private void PlayerMovements(double delta)
	{
		// isRun
		if (Input.IsKeyPressed(Key.Shift))
			speed = (int)gameManager.playerDataResource.TopViewRunSpeed; else speed = (int)gameManager.playerDataResource.TopViewNormalSpeed;

		// Apply Velocity
		MoveAndCollide(new Vector2(playerInputs.HorizontalAxis, 0) * speed * (float)delta);
		MoveAndCollide(new Vector2(0, playerInputs.VerticalAxis) * speed * (float)delta);

		return;
	}

	private void InteractionActions(double delta)
	{
		// InteractionArea Update
		utils.UpdateArea2DDir(interactionArea, new Vector2(playerInputs.HorizontalAxis, playerInputs.VerticalAxis));

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
			eventManager.EventCaller(eventArea.eventID, 0);
			return;
		}

		if (encounterObject is map_changer mapChanger)
		{
			eventManager.MapCaller(mapChanger.nextMapID, mapChanger.nextMapPos);
			return;
		}

		return;
	}
}
