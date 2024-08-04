using Godot;
using System;
using System.IO;

public partial class game_manager : Node
{
	// Data Resources
	public player_data_resource playerDataResource { get; set; }

	// File Path
	private const string PlayerFilePath = "user://save/data";

	public override void _Ready()
	{
		if (!Directory.Exists(System.IO.Path.Combine(OS.GetUserDataDir(), "save")))
            Directory.CreateDirectory(System.IO.Path.Combine(OS.GetUserDataDir(), "save"));

		GD.Print(System.IO.Path.Combine(OS.GetUserDataDir(), "save"));

        NewPlayer(64);
		SavePlayer(64);
		LoadPlayer(64);
	}

	public void NewPlayer(int ID)
	{
		playerDataResource = new player_data_resource
		{
			playerName = "",
			TopViewSpeed = 400,
			SideViewSpeed = 250,
			SideViewJumpSpeed = -600
		};
	}

	public void SavePlayer(int ID)
	{
		if (ID > 9) ResourceSaver.Save(playerDataResource, PlayerFilePath +ID.ToString() + ".tres");
		else ResourceSaver.Save(playerDataResource, PlayerFilePath + "0" + ID.ToString() + ".tres");
		GD.Print("PlayerData Saved. data" + ID.ToString() + ".tres");
	}

	public bool LoadPlayer(int ID)
	{
		string targetFile;
		if (ID > 9) targetFile = PlayerFilePath + ID.ToString() + ".tres";
		else targetFile = PlayerFilePath + "0" + ID.ToString() + ".tres";

		if (Godot.FileAccess.FileExists(targetFile))
		{
			playerDataResource = (player_data_resource)ResourceLoader.Load(targetFile);
			GD.Print("PlayerData Loaded. data" + ID.ToString() + ".tres");
			return true;
		}

		GD.Print("SaveFile doesn't exist");
		return false;
	}
}
