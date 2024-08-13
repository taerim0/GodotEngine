using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class loadgame_ui : Control
{
	private delegate void LoadFunction();

	private Button ExitButton;
	private List<Button> slot;
	private List<bool> isDataExist;
	private List<LoadFunction> loadFunctions;

	private Control ParentMenu;

	// File Path
	private string PlayerFilePath;

	// Manager
	private game_manager gameManager;
	private event_manager eventManager;

	public override void _Ready()
	{
		PlayerFilePath = OS.GetUserDataDir() + "/save/data";

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
		loadFunctions = new List<LoadFunction>(new LoadFunction[20]);

		for (int i = 0; i < 8; i++)
		{
			int currentIndex = i;
			slot[i] = GetNode<Button>("scroll_container/control/button" + (i + 1).ToString());
			slot[i].Connect("pressed", Callable.From(() => DataLoad(currentIndex)));

			if (File.Exists(PlayerFilePath + i.ToString("D2") + ".tres"))
			{
				slot[i].Text = "File Exist!";
			}
			else
			{
				slot[i].Text = "File doesnt exist";
			}
		}
	}

	public void UpdateSaveSlotText()
	{
		for (int i = 0; i < 8; i++)
		{
			if (File.Exists(PlayerFilePath + (i + 1).ToString("D2") + ".tres"))
			{
				slot[i].Text = "File Exist!";
			}
			else
			{
				slot[i].Text = "File doesnt exist";
			}
		}
	}

	private void DataLoad(int slot)
	{
		gameManager.LoadPlayer(slot);
		eventManager.EventCaller(6, slot);
	}

	public void Close()
	{
		Visible = false;
		SetProcessInput(false);
	}
}
