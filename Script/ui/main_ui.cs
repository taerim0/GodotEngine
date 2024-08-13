using Godot;
using System;

public partial class main_ui : Control
{
	private Button newGameButton, loadGameButton, optionButton, exitButton;

	// Sub-UI
	private loadgame_ui loadGameUi;

	// Manager
	private game_manager gameManager;
	private object_manager objectManager;
	private event_manager eventManager;

	public override void _Ready()
	{
		gameManager = GetNode<game_manager>("/root/game_manager");
		objectManager = GetNode<object_manager>("/root/object_manager");
		eventManager = GetNode<event_manager>("/root/event_manager");

		loadGameUi = GetNode<loadgame_ui>("loadgame_ui");

		loadGameUi.Visible = false;
		loadGameUi.SetProcessInput(false);

		newGameButton = GetNode<Button>("new_game");
		loadGameButton = GetNode<Button>("load_game");
		optionButton = GetNode<Button>("option");
		exitButton = GetNode<Button>("exit");

		newGameButton.Connect("pressed", new Callable(this, nameof(PressNewGame)));
		loadGameButton.Connect("pressed", new Callable(this, nameof(PressLoadGame)));
		optionButton.Connect("pressed", new Callable(this, nameof(PressOption)));
		exitButton.Connect("pressed", new Callable(this, nameof(PressExit)));
	}

	public void PressNewGame()
	{
		eventManager.EventCaller(4, 0);
		return;
	}

	public void PressLoadGame()
	{
		SetProcessInput(false);
		loadGameUi.Visible = true;
		loadGameUi.SetProcessInput(true);
		return;
	}

	public void PressOption()
	{
		eventManager.EventCaller(6, 0);
		return;
	}

	public void PressExit()
	{
		eventManager.EventCaller(7, 0);
		return;
	}
}
