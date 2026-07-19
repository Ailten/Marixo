using System;
using Godot;

public class CanShoot
{
	private CharacterBody2D owner;
	protected float cooldownShot = 0.4f;
	protected float timeUntilNextShot = 0f;
	protected float timeTravel = 0.25f;
	private Node2D startMarker, endMarker;
	protected virtual Vector2 startPos
	{
		get => startMarker.GlobalPosition;
	}
	protected virtual Vector2 endPos
	{
		get => endMarker.GlobalPosition;
	}
    public int damageMaking = 1;

	public CanShoot(CharacterBody2D owner, Node2D startMarker = null, Node2D endMarker = null)
	{
		this.owner = owner;
		this.startMarker = startMarker;
		this.endMarker = endMarker;
	}


    public virtual void shoot(Vector2? startPos = null, Vector2? endPos = null, Node2D projectil = null)
    {
        if (timeUntilNextShot > 0f)
            return;
        timeUntilNextShot = cooldownShot;

        PackedScene sceneProjectil = GD.Load<PackedScene>("res://customNode/fist.tscn");
        projectil ??= sceneProjectil.Instantiate<Node2D>();
        (projectil as Projectil).setData(
            startPos ?? this.startPos,  // FIXME: both can be null, in this case, force the shoot call to get parameters values.
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

	public virtual void projectilEndTravel(Node2D projectil)
	{
		// destroy projectil.
		projectil.QueueFree();
	}

	public void projectilTrigger(Node2D projectil, Node2D bodyColliding)
	{
        if (bodyColliding is Character characterColliding)
        {
            (owner as Character).makeDamage(damageMaking, characterColliding);
        }
	}

}
