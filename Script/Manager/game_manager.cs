using Godot;
using Godot.Collections;
using System;
using System.IO;

public partial class game_manager : Node
{
	// Data Resources
	public player_data_resource playerDataResource { get; set; }
	public setting_resource settingResource { get; set; }

	// File Path
	private const string PlayerFilePath = "user://save/data";
	private const string TmpPlayerFilePath = "user://save/data.tmp.tres";

	public override void _Ready()
	{
		if (!Directory.Exists(System.IO.Path.Combine(OS.GetUserDataDir(), "save")))
			Directory.CreateDirectory(System.IO.Path.Combine(OS.GetUserDataDir(), "save"));

		NewPlayer();
	}

   	public void NewPlayer()
	{
		playerDataResource = new player_data_resource()
		{
			playerName = "",
			playerLevel = 1,
			mapID = 0,
			mapPos = new Vector2(0, 0),
			TopViewNormalSpeed = 400,
			TopViewRunSpeed = 600,
			SideViewSpeed = 250,
			SideViewJumpSpeed = -600,
			equippedWeapon = 0,
			itemInventory = new Array<item_resource>(),
			itemAmount = new Array<Dictionary<int, int>>(),
			itemPosition = new Array<Dictionary<int, int>>(),
		};
	}

	public void SavePlayer(int ID)
	{
		string targetFile = PlayerFilePath + ID.ToString("D2") + ".tres";

		ResourceSaver.Save(playerDataResource, targetFile);
		GD.Print("PlayerData Saved. data" + ID.ToString("D2") + ".tres");
	}

	public bool LoadPlayer(int ID)
	{
		string targetFile = PlayerFilePath + ID.ToString("D2") + ".tres";

		if (Godot.FileAccess.FileExists(targetFile))
		{
			playerDataResource = (player_data_resource)ResourceLoader.Load(targetFile);
			GD.Print("PlayerData Loaded. data" + ID.ToString("D2") + ".tres");
			return true;
		}

		GD.Print("SaveFile doesn't exist");
		return false;
	}

	public void SaveTmpPlayer()
	{
		ResourceSaver.Save(playerDataResource, TmpPlayerFilePath);
	}

	public void LoadTmpPlayer()
	{
		playerDataResource = (player_data_resource)ResourceLoader.Load(TmpPlayerFilePath);
	}
}
