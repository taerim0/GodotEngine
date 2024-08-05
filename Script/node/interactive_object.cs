using Godot;
using System;

public partial class interactive_object : Node
{
	[Export]
	public int objectId { get; set; }
	[Export]
	public int eventId { get; set; }
}
