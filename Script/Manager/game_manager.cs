using Godot;
using System;
using System.IO;

public partial class game_manager : Node
{
	// Data Resources
	public player_data_resource playerDataResource { get; set; }

	// File Path
	private const string PlayerFilePath = "user://save/data";
	private const string TmpPlayerFilePath = "user://save/data.tmp.tres";

	public override void _Ready()
	{
		if (!Directory.Exists(System.IO.Path.Combine(OS.GetUserDataDir(), "save")))
			Directory.CreateDirectory(System.IO.Path.Combine(OS.GetUserDataDir(), "save"));

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
		string targetFile = PlayerFilePath + ID.ToString("D2") + ".tres";

		ResourceSaver.Save(playerDataResource, targetFile);
		GD.Print("PlayerData Saved. data" + ID.ToString() + ".tres");
	}

	public bool LoadPlayer(int ID)
	{
		string targetFile = PlayerFilePath + ID.ToString("D2") + ".tres";

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
		ResourceSaver.Save(playerDataResource, TmpPlayerFilePath);
	}

	public void LoadTmpPlayer()
	{
		playerDataResource = (player_data_resource)ResourceLoader.Load(TmpPlayerFilePath);
	}
}
