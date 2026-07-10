using System;
using Godot;

public class CanShoot
{
	private CharacterBody2D owner;
	private float cooldownShot = 0.4f;
	private float timeUntilNextShot = 0f;
	private float timeTravel = 0.25f;
	private Node2D startMarker, endMarker;
	protected virtual Vector2 startPos
	{
		get => startMarker.GlobalPosition;
	}
	protected virtual Vector2 endPos
	{
		get => endMarker.GlobalPosition;
	}

	public CanShoot(CharacterBody2D owner, Node2D startMarker = null, Node2D endMarker = null)
	{
		this.owner = owner;
		this.startMarker = startMarker;
		this.endMarker = endMarker;
	}


	public void shoot(Vector2? startPos = null, Vector2? endPos = null, Node2D projectil = null)
	{
		if (timeUntilNextShot > 0f)
			return;
		timeUntilNextShot = cooldownShot;

		PackedScene sceneProjectil = GD.Load<PackedScene>("res://customNode/fist.tscn");
		projectil = sceneProjectil.Instantiate<Node2D>();
		(projectil as Projectil).setData(
			startPos ?? this.startPos,
			endPos ?? this.endPos,
			timeTravel,
			this
		);

		owner.AddChild(projectil);
	}

	public void updateShoot(float delta)
	{
		timeUntilNextShot = Math.Max(timeUntilNextShot - delta, 0f);
	}

	public void projectilEndTravel(Node2D projectil)
	{
        // destroy projectil.
		projectil.QueueFree();
	}

	public void projectilTrigger(Node2D projectil, Node2D body)
	{
		Console.WriteLine($"Touched : {body.Name}");
		this.projectilEndTravel(projectil);
	}

}
