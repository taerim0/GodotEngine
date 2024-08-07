using Godot;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public partial class game_manager : Node
{
	// Data Resources
	public player_data_resource playerDataResource { get; set; }
	public player_data_resource tmpPlayerDataResource { get; set; }

	// File Path
	private const string PlayerFilePath = "user://save/data";
	private const string TmpPlayerFilePath = "user://save/data.tmp.tres";

	public override void _Ready()
	{
		if (!Directory.Exists(System.IO.Path.Combine(OS.GetUserDataDir(), "save")))
			Directory.CreateDirectory(System.IO.Path.Combine(OS.GetUserDataDir(), "save"));

		NewPlayer(64);
		SavePlayer(64);
		LoadPlayer(64);
		tmpPlayerDataResource = playerDataResource;
		SaveTmpPlayer();
		LoadTmpPlayer();
	}

	public void NewPlayer(int ID)
	{
		playerDataResource = new player_data_resource()
		{
			playerName = "",
			TopViewNormalSpeed = 400,
			TopViewRunSpeed = 600,
			SideViewSpeed = 250,
			SideViewJumpSpeed = -600
		};
	}

	public void SavePlayer(int ID)
	{
		string targetFile = PlayerFilePath + (ID > 9 ? "" : "0") + ".tres";

		ResourceSaver.Save(playerDataResource, targetFile);
		GD.Print("PlayerData Saved. data" + ID.ToString() + ".tres");
	}

	public bool LoadPlayer(int ID)
	{
		string targetFile = PlayerFilePath + (ID > 9 ? "" : "0") + ".tres";

		if (Godot.FileAccess.FileExists(targetFile))
		{
			playerDataResource = (player_data_resource)ResourceLoader.Load(targetFile);
			GD.Print("PlayerData Loaded. data" + ID.ToString() + ".tres");
			return true;
		}

		GD.Print("SaveFile doesn't exist");
		return false;
	}

	public void SaveTmpPlayer()
	{
		ResourceSaver.Save(tmpPlayerDataResource, TmpPlayerFilePath);
	}

	public void LoadTmpPlayer()
	{
		tmpPlayerDataResource = (player_data_resource)ResourceLoader.Load(TmpPlayerFilePath);
	}
}
