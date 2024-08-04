using Godot;
using System;

public partial class item_resource : Resource
{
	[Export]
	public int itemID;
	[Export]
	public string itemName;
	[Export]
	public string itemDescription;
}
