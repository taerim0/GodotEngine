using Godot;
using System;

public partial class ButtonTest : Node
{
	CharacterBody2D TopView;
	CharacterBody2D SideView;

	Camera2D TopViewCamera;
	Camera2D SideViewCamera;

	Button button;

	public override void _Ready()
	{
		TopView = GetNode<CharacterBody2D>("../CharacterBody2D");
		GD.Print(TopView);
		TopViewCamera = GetNode<Camera2D>("../CharacterBody2D/Camera2D");
		SideView = GetNode<CharacterBody2D>("../CharacterBody2D2");
		SideViewCamera = GetNode<Camera2D>("../CharacterBody2D2/Camera2D");

		button = GetNode<Button>("../Button");

		SideView.SetProcess(false);
		SideViewCamera.Enabled = false;

		button.Connect("pressed", new Callable(this, nameof(pressButton)));
	}

	private void pressButton()
	{
		SideView.SetProcess(!SideView.IsProcessing());
		SideViewCamera.Enabled = SideView.IsProcessing();
		TopView.SetProcess(!TopView.IsProcessing());
		TopViewCamera.Enabled = TopView.IsProcessing();
	}
}
