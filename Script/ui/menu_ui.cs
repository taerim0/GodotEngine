using Godot;
using System;

public partial class menu_ui : Control
{
	private Button SaveButton;
	private Button LoadButton;
	private Button InventoryButton;
	private Button OptionButton;

	public bool isSubUiVisible = false;

	// Sub-Ui   
	public savegame_ui saveGameUi;
	public loadgame_ui loadGameUi;

	public override void _Ready()
	{
		GD.Print("Ready function executed");

		SaveButton = GetNode<Button>("save_game_button");
		LoadButton = GetNode<Button>("load_game_button");
		InventoryButton = GetNode<Button>("inventory_button");
		OptionButton = GetNode<Button>("option_button");

		loadGameUi = GetNode<loadgame_ui>("menu_loadgame_ui");
		loadGameUi.Visible = false;
		loadGameUi.SetProcessInput(false);

		saveGameUi = GetNode<savegame_ui>("menu_savegame_ui");
		saveGameUi.Visible = false;
		saveGameUi.SetProcessInput(false);

		SaveButton.Connect("pressed", new Callable(this, nameof(SaveGame)));
		LoadButton.Connect("pressed", new Callable(this, nameof(LoadGame)));
		InventoryButton.Connect("pressed", new Callable(this, nameof(Inventory)));
		OptionButton.Connect("pressed", new Callable(this, nameof(Option)));
	}

	public void SaveGame()
	{
		GD.Print("debug1");
		saveGameUi.UpdateSaveSlotText();
		saveGameUi.Visible = true;
		saveGameUi.SetProcessInput(true);
		return;
	}

	public void LoadGame()
	{
		GD.Print("debug2");
		loadGameUi.UpdateSaveSlotText();
		loadGameUi.Visible = true;
		loadGameUi.SetProcessInput(true);
		return;
	}

	public void Inventory()
	{

	}

	public void Option()
	{

	}

	public void Close()
	{
		if (isSubUiVisible)
		{
			CloseSubUi();
		}
		else
		{
			Visible = false;
			SetProcessInput(false);
		}
	}

	public void CloseSubUi()
	{
		saveGameUi.Close();
		loadGameUi.Close();
	}
}
