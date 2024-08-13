using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class savegame_ui : Control
{
	private Button ExitButton;
	private List<Button> slot;
	private List<bool> isDataExist;

	private Control ParentMenu;

	// File Path
	private string PlayerFilePath;

	// Manager
	private game_manager gameManager;
	private event_manager eventManager;

	public override void _Ready()
	{
		PlayerFilePath = OS.GetUserDataDir() + "/save/data";
		GD.Print(PlayerFilePath);

		if (HasNode("../../main_ui"))
			ParentMenu = GetNode<Control>("../../main_ui");
		else
			ParentMenu = GetNode<Control>("../../menu_ui");

		gameManager = GetNode<game_manager>("/root/game_manager");
		eventManager = GetNode<event_manager>("/root/event_manager");

		if (HasNode("exit_button"))
		{
			ExitButton = GetNode<Button>("exit_button");
			ExitButton.Connect("pressed", new Callable(this, nameof(Close)));
		}
		slot = new List<Button>(new Button[20]);

		for (int i = 0; i < 8; i++)
		{
			int currentIndex = i + 1;
			slot[i] = GetNode<Button>("scroll_container/control/button" + (i + 1).ToString());
			slot[i].Connect("pressed", Callable.From(() => DataSave(currentIndex)));
		}

		UpdateSaveSlotText();
	}

	public void UpdateSaveSlotText()
	{
		GD.Print("save slot text update : 1");

		for (int i = 0; i < 8; i++)
		{
			if (File.Exists(PlayerFilePath + (i + 1).ToString("D2") + ".tres"))
			{
				GD.Print("File exist");
				slot[i].Text = "File Exist!";
			}
			else
			{
				GD.Print("File doesnt exist");
				slot[i].Text = "File doesnt exist";
			}
		}
	}

	private void DataSave(int slot)
	{
		GD.Print("esegseg");
		eventManager.EventCaller(5, slot);
		UpdateSaveSlotText();
	}

	public void Close()
	{
		Visible = false;
		SetProcessInput(false);
	}
}
