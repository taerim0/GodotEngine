using Godot;
using System;

public partial class object_manager : Node
{
	
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	public void ObjectInteractive(int objectID)
	{
		GD.Print(objectID + " Interaction");
	}
}
