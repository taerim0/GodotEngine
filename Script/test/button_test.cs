using Godot;
using System;

public partial class button_test : Node
{
	CharacterBody2D TopView;
	CharacterBody2D SideView;

	Camera2D TopViewCamera;
	Camera2D SideViewCamera;

	Button button;

	public override void _Ready()
	{
		TopView = GetNode<CharacterBody2D>("../character_body_2d");
		GD.Print(TopView);
		TopViewCamera = GetNode<Camera2D>("../character_body_2d/camera_2d");
		SideView = GetNode<CharacterBody2D>("../character_body_2d2");
		SideViewCamera = GetNode<Camera2D>("../character_body_2d2/camera_2d");

		button = GetNode<Button>("../button");

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
