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

	// Manager
	private game_manager gameManager;
	private object_manager objectManager;
	private event_manager eventManager;

	public override void _Ready()
	{
		gameManager = GetNode<game_manager>("/root/game_manager");
		objectManager = GetNode<object_manager>("/root/object_manager");
		eventManager = GetNode<event_manager>("/root/event_manager");

		eventState = new EventState();

		Events = new List<EventFunction>()
		{
			Event0000, Event0001, Event0002, Event0003, Event0004,
			Event0005, Event0006,

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

	public void MapCaller(int mapID, Vector2 mapPos)
	{
		GetTree().CallDeferred("change_scene_to_file", "res://scene/maps/map" + mapID.ToString("D4") + ".tscn");
	}

	private void Event0000(int stage)
	{ // Event for Debug & Test 0
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
	{ // Event for Debug & Test 1
		
	}

	private void Event0002(int stage)
	{ // Event for Debug & Test 2
		
	}

	private void Event0003(int stage)
	{ // Event for Debug & Test 3
		
	}

	private void Event0004(int stage)
	{ // NewGame
		gameManager.NewPlayer();
		MapCaller(gameManager.playerDataResource.mapID, gameManager.playerDataResource.mapPos);

		return;
	}

	private void Event0005(int stage)
	{ // SaveGame
		gameManager.SavePlayer(stage);

		return;
	}

	private void Event0006(int stage)
	{ // LoadGame
		gameManager.LoadPlayer(stage);
		MapCaller(gameManager.playerDataResource.mapID, gameManager.playerDataResource.mapPos);

		return;
	}
}






