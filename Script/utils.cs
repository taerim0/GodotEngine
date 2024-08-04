using Godot;
using System;

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
}
