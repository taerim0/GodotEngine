using Godot;
using System;

public partial class item_resource : Resource
{
	[Export]
	public int itemID { get; set; }
	[Export]
	public string itemName { get; set; }
	[Export]
	public string itemDescription { get; set; }
}
