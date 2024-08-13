using Godot;
using System;

public partial class map_changer : Area2D
{
    [Export]
    public int nextMapID { get; set; }
    [Export]
    public Vector2 nextMapPos { get; set; }
}
