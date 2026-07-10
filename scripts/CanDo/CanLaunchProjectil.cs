using System;
using System.Buffers;
using Godot;

public class CanLaunchProjectil
{
    private CharacterBody2D owner;
    private float cooldownShot;
    private float timeUntilNextShot = 0f;
    private float timeTravel;

    public CanLaunchProjectil(CharacterBody2D owner, float cooldownShot = 1.2f, float timeTravel = 0.6f)
    {
        this.owner = owner;
        this.cooldownShot = cooldownShot;
        this.timeTravel = timeTravel;
    }


    public void launchProjectil(Vector2 startPos, Vector2 endPos, Node2D projectil = null)
    {
        if (timeUntilNextShot > 0f)
            return;
        timeUntilNextShot = cooldownShot;

        projectil ??= (Node2D)GD.Load<PackedScene>("res://customNode/fist.tscn").Instantiate();
        projectil.GetNode<Projectil>("").setData(startPos, endPos, timeTravel);

        owner.AddChild(projectil);
    }

    public void updateProjecti(float delta)
    {
        timeUntilNextShot = Math.Max(timeUntilNextShot - delta, 0f);
    }

}

public partial class Projectil : Node2D //RigidBody2D
{
    private Vector2 startPos, endPos;
    private float cumulTime = 0f;
    private float endTime;


    public void setData(Vector2 startPos, Vector2 endPos, float endTime)
    {
        this.startPos = startPos;
        this.endPos = endPos;
        this.endTime = endTime;

        this.GetNode<Sprite2D>("Sprite2D").FlipH = startPos.X > endPos.Y;
    }

    public override void _Process(double delta)
    {
        cumulTime += (float)delta;
        float i = cumulTime / endTime;
        if (i > 1f)
        {
            endTravel();
            return;
        }
        GlobalPosition = startPos.Lerp(endPos, i);
    }

    protected virtual void endTravel()
    {
        this.QueueFree();
    }

    public override void _PhysicsProcess(double delta)
    {
        // TODO: colide with element, make endTravel, and maybe call hit function character.
    }

}