using Godot;
using System;
using System.Collections.Generic;

public partial class event_manager : Node
{
	delegate void EventFunction(int stage);

	private class EventState
	{
		public int eventID, stageNumber;
		public EventState()
		{
			this.eventID = -1;
			this.stageNumber = -1;
		}

		public void Update(int eventID, int stageNumber)
		{
			this.eventID = eventID;
			this.stageNumber = stageNumber;

			return;
		}
	}

	private EventState eventState;

	private List<EventFunction> Events;

	public override void _Ready()
	{
		eventState = new EventState();

		Events = new List<EventFunction>()
		{
			Event0000,
			Event0001
		};
	}


	public override void _Process(double delta)
	{
	}

	public void EventCaller(int eventID, int stageNumber)
	{
		GD.Print("EventCaller : " + eventID + " is Called!");

		if (eventID >= Events.Count || eventID < 0)
			throw new Exception("EventCaller Error : eventID out of range");

		eventState.Update(eventID, stageNumber);
		Events[eventID](stageNumber);
	}

	public void BattleCaller(int battleID)
	{
		GetTree().CallDeferred("change_scene_to_file", "res://scene/battles/battle" + battleID.ToString("D4") + ".tscn");
	}

	private void Event0000(int stage)
	{
		switch (stage)
		{
			case 0:
				BattleCaller(0);
				break;
			default:
				return;
		}

		return;
	}

	private void Event0001(int stage)
	{

	}

	
}
