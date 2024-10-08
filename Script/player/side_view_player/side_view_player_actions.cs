using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

public partial class side_view_player_actions : CharacterBody2D
{
	public int moveScale = 1;

	public bool canPlayerInput = true;

	private int gravity = 2000;
	private int jumpSpeed = -1300;
	private int speed = 250;
	private int dashSpeed = 350;
	private float acceleration = 0.25f;
	private float friction = 0.12f;

	private float curDir = 1f;

	private float dashCooltimeBase = 0.4f;
	private float dashCooltime = 0.15f;
	private float dashTime = 0f;
	private float dashDir = 1f;

	private float attackCooltimeBase = 0.15f;
	private float attackCooltime = 0.1f;
	private float attackTime = 0f;
	private float attackDir = 1f;

	int[] dy = { 0, 0, 1, -1 };
	int[] dx = { 1, -1, 0, 0 };

	// Inputs
	private class PlayerInputs
	{
		public float HorizontalAxis, VerticalAxis;
		public bool
			isPressJumpKey, isPressDashKey,
			isPressAttackKey, isPressSkill1Key,
			isPressSkill2Key, isPressUseSkillKey,
			isPressDownKey;

		public PlayerInputs()
		{
			this.HorizontalAxis = 0;
			this.VerticalAxis = 0;
			this.isPressJumpKey = false;
			this.isPressDashKey = false;
			this.isPressAttackKey = false;
			this.isPressSkill1Key = false;
			this.isPressSkill2Key = false;
			this.isPressUseSkillKey = false;
			this.isPressDownKey = false;
		}

		public void Update(
			float HorizontalAxis,
			float VerticalAxis,
			bool isPressJumpKey,
			bool isPressDashKey,
			bool isPressAttackKey,
			bool isPressSkill1Key,
			bool isPressSkill2Key,
			bool isPressUseSkillKey,
			bool isPressDownKey
		) {
			this.HorizontalAxis = HorizontalAxis;
			this.VerticalAxis = VerticalAxis;
			this.isPressJumpKey = isPressJumpKey;
			this.isPressDashKey = isPressDashKey;
			this.isPressAttackKey = isPressAttackKey;
			this.isPressSkill1Key = isPressSkill1Key;
			this.isPressSkill2Key = isPressSkill2Key;
			this.isPressUseSkillKey = isPressUseSkillKey;
			this.isPressDownKey = isPressDownKey;

			return;
		}
	}
	PlayerInputs playerInputs;

	private int JumpCnt = 0;

	private enum ActionState
	{
		Idle, Run, JumpUp, JumpDown,
		UpperDash, BottomDash, UpperSideDash, HorizontalDash, BottomSideDash,
	}ActionState actionState = ActionState.Idle;

	// Player

	// Attack
	private bool weaponChanged = true;

	private Area2D attackCollision;
	private CollisionShape2D attackCollisionShape;

	// Manager
	private game_manager gameManager;
	private object_manager objectManager;
	private event_manager eventManager;

	// InputMap
	private string moveLeft = "SideViewPlayerMovement_Left";
	private string moveRight = "SideViewPlayerMovement_Right";
	private string moveUp = "SideViewPlayerMovement_Up";
	private string moveDown = "SideViewPlayerMovement_Down";

	private string actionJump = "SideViewPlayerMovement_Jump";
	private string actionDash = "SideViewPlayerAction_Dash";
	private string actionAttack = "SideViewPlayerAction_Attack";
	private string actionSkill1 = "SideViewPlayerAction_Skill1";
	private string actionSkill2 = "SideViewPlayerAction_Skill2";
	private string actionUseSkill = "SideViewPlayerAction_UseSkill";

	// TileMap
	private TileMap physicTile;

	public override void _Ready()
	{
		gameManager = GetNode<game_manager>("/root/game_manager");
		objectManager = GetNode<object_manager>("/root/object_manager");
		eventManager = GetNode<event_manager>("/root/event_manager");

		attackCollision = GetNode<Area2D>("attack_collision");
		attackCollisionShape = GetNode<CollisionShape2D>("attack_collision/collision_shape_2d");

		physicTile = GetNode<TileMap>("../physic_tile");

		playerInputs = new PlayerInputs();
	}

	public override void _Process(double delta)
	{
		AttackCollisionUpdate();

		if (canPlayerInput)
		{
			playerInputs.Update(
				Input.GetAxis(moveLeft, moveRight),
				Input.GetAxis(moveUp, moveDown),
				Input.IsActionJustPressed(actionJump),
				Input.IsActionJustPressed(actionDash),
				Input.IsActionJustPressed(actionAttack),
				Input.IsActionJustPressed(actionSkill1),
				Input.IsActionJustPressed(actionSkill2),
				Input.IsActionJustPressed(actionUseSkill),
				Input.IsActionPressed(moveDown)
			);
		}

		PlayerMovements(delta);

		if (isPlayerCanAction())
		{
			if (playerInputs.isPressDashKey)
			{
				dashDir = curDir;
				dashTime = dashCooltimeBase;
				GD.Print("Dash Use!!");
			}
		}
		else if (dashTime > 0f)
		{
			if (playerInputs.isPressDashKey)
			{
				GD.Print("Cant Use Dash");
			}

			if (dashTime > dashCooltimeBase - dashCooltime)
				useDash();
			dashTime -= (float)delta;

			if (dashTime <= 0f)
				dashTime = 0f;
		}

		if (isPlayerCanAction())
		{
			if (playerInputs.isPressAttackKey)
			{
				curDir = GetGlobalMousePosition().X < GlobalPosition.X ? -1 : 1;
				attackDir = curDir;
				attackTime = attackCooltimeBase;
				GD.Print("Attack Use!!");
			}
		}
		else if (attackTime > 0f)
		{
			if (playerInputs.isPressAttackKey)
			{
				GD.Print("Cant Use Attack");
			}

			if (attackTime > attackCooltimeBase - attackCooltime)
				useAttack();
			attackTime -= (float)delta;

			if (attackTime <= 0f)
				attackTime = 0f;
		}

		return;
	}

	private void AttackCollisionUpdate()
	{
		attackCollision.Rotation = (GetGlobalMousePosition() - GlobalPosition).Normalized().Angle();

		if (weaponChanged)
		{
			weaponChanged = false;

			switch (gameManager.playerDataResource.equippedWeapon)
			{
				case 0:
					attackCollisionShape.Position = new Vector2(40, 0);
					attackCollisionShape.Scale = new Vector2(4, 5);
					break;
				case 1:
					attackCollisionShape.Position = new Vector2(60, 0);
					attackCollisionShape.Scale = new Vector2(6, 5);
					break;
				case 2:
					attackCollisionShape.Position = new Vector2(500, 0);
					attackCollisionShape.Scale = new Vector2(50, 3);
					break;
			}
		}
	}

	private bool isPlayerCanAction()
	{
		if (dashTime > 0f) return false;
		if (attackTime > 0f) return false;
		return true;
	}

	private void PlayerMovements(double delta)
	{
		Vector2 velocity = Velocity;

		if (playerInputs.HorizontalAxis != 0)
		{
			curDir = playerInputs.HorizontalAxis;

			velocity.X = Mathf.Lerp(velocity.X, playerInputs.HorizontalAxis * speed, acceleration);
		}
		else
		{
			velocity.X = Mathf.Lerp(velocity.X, 0.0f, friction);
		}

		Velocity = velocity;

		// Apply Movements to Character
		MoveAndSlide();

		if (IsOnFloor())
		{
			JumpCnt = 1;
			velocity.Y = 0;
		}

		velocity.Y = ApplyGravity(velocity.Y, delta);

		if (playerInputs.isPressJumpKey)
		{
			if (IsOnFloor())
			{
				if (playerInputs.isPressDownKey)
				{
					JumpThroughFloor();
				}
				else
				{
					velocity.Y = jumpSpeed;
				}
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

	private float ApplyGravity(float velocityY, double delta)
	{
		return utils.Clamp(velocityY + gravity * (float)delta, -500, 800);
	}

	private async void JumpThroughFloor()
	{
		List<Vector2I> floor = new List<Vector2I>();

		var tilePos = physicTile.LocalToMap(Position + new Vector2(0, 48));
		if (physicTile.GetCellAtlasCoords(0, tilePos) == new Vector2I(2, 0))
		{
			DefineFloor(floor, tilePos);
			foreach (Vector2I cell in floor)
				physicTile.SetCell(0, cell, 1, new Vector2I(3, 0));
			await ToSignal(GetTree().CreateTimer(0.2f), "timeout");
			foreach (Vector2I cell in floor)
				physicTile.SetCell(0, cell, 1, new Vector2I(2, 0));
		}
	}

	private void DefineFloor(List<Vector2I> floor, Vector2I start)
	{
		Dictionary<Vector2I, bool> cellCheck = new Dictionary<Vector2I, bool>();
		Queue<Vector2I> q = new Queue<Vector2I>();
		floor.Add(start); q.Enqueue(start);
		cellCheck[start] = true;

		while (q.Count > 0)
		{
			Vector2I nowPos = q.Dequeue();

			for (int i = 0; i < 2; i++)
			{
				Vector2I targetPos = nowPos + new Vector2I(dx[i], dy[i]);

				if (physicTile.GetCellAtlasCoords(0, targetPos) != new Vector2I(2, 0))
					continue;

				if (cellCheck.ContainsKey(targetPos))
					continue;

				cellCheck[targetPos] = true;
				floor.Add(targetPos); q.Enqueue(targetPos);
			}
		}
	}

	private void useDash()
	{
		Vector2 velocity = Velocity;

		velocity.X = dashDir * dashSpeed;

		Velocity = velocity;

		MoveAndSlide();
	}

	private void useAttack()
	{
		Vector2 velocity = Velocity;

		velocity.X = attackDir * 70;

		Velocity = velocity;

		MoveAndSlide();
	}
}
