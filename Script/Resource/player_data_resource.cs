using Godot;
using Godot.Collections;
using System;

public partial class player_data_resource : Resource
{
	// base
	[Export]
	public string playerName { get; set; }
    [Export]
    public int playerLevel { get; set; }

    // map
    [Export]
    public int mapID { get; set; }
    [Export]
    public Vector2 mapPos { get; set; }

    // TopViewData
    [Export]
    public float TopViewNormalSpeed { get; set; }
	[Export]
    public float TopViewRunSpeed { get; set; }

	// SideViewData
	[Export]
	public float SideViewSpeed { get; set; }
	[Export]
	public float SideViewJumpSpeed { get; set; }

	// Equipments
	[Export]
	public int equippedWeapon { get; set; }

	// Inventory
	[Export]
	public Array<item_resource> itemInventory { get; set; }
    [Export]
    public Array<Dictionary<int, int>> itemAmount { get; set; }
    [Export]
    public Array<Dictionary<int, int>> itemPosition { get; set; }
}
