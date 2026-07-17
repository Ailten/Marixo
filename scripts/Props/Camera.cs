using Godot;
using System;

public partial class Camera : Camera2D
{
	public Node2D target;
	public float lerpCamera = 0.05f;
	public bool isMoveUnable = true;  // for debuging in pos [0:0].

	public override void _Ready()
	{
		target = GetParent().GetNode<Node2D>("Player").GetNode<Node2D>("CameraMarker");
		if (isMoveUnable)
			focusTarget();
	}

	public override void _Process(double delta)
	{
		if (target != null && isMoveUnable)
		{
			GlobalPosition = GlobalPosition.Lerp(
				target.GlobalPosition,
				lerpCamera
			);
		}
	}

	public void focusTarget()
	{
		if (target != null)
		{
			GlobalPosition = target.GlobalPosition;
		}
	}
}
