using Godot;
using System;

public partial class event_manager : Node
{
	
	public override void _Ready()
	{
	}

	
	public override void _Process(double delta)
	{
	}

	public void EventCalled(int eventID)
	{
		GD.Print(eventID + "is Called!");
	}
}
