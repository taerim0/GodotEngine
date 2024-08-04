using Godot;
using System;

public partial class player_data_resource : Resource
{
	// base
	[Export]
	public string playerName { get; set; }

	// TopViewData
	[Export]
	public float TopViewSpeed { get; set; }


	// SideViewData
	[Export]
	public float SideViewSpeed { get; set; }
	[Export]
	public float SideViewJumpSpeed { get; set; }
}
