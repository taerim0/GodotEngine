using Godot;
using System;
using static Godot.TextServer;

public partial class utils : Node
{
	public static int Clamp(int value, int min, int max)
	{
		if (min > max) (min, max) = (max, min);
		if (value < min) return min;
		if (value > max) return max;
		return value;
	}

	public static float Clamp(float value, float min, float max)
	{
		if (min > max) (min, max) = (max, min);
		if (value < min) return min;
		if (value > max) return max;
		return value;
	}

	public static void UpdateRayCastDir(RayCast2D rayCast, Vector2 curDir, int length)
	{
		if (curDir.Normalized() == Vector2.Zero)
			return;
		rayCast.TargetPosition = curDir.Normalized() * length;
		return;
	}

	public static void UpdateInterationAreaDir(Area2D area, Vector2 curDir)
	{
		if (curDir.Normalized() == Vector2.Zero)
			return;
		area.Rotation = curDir.Angle() - MathF.PI / 2;
		return;
	}
}
