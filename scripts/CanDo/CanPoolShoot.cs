using System;
using Godot;

public class CanPoolShoot : CanShoot
{
    private Node2D[] pool;
    private int indexPool = 0;

    public CanPoolShoot(CharacterBody2D owner, string pathProjectilInstance = "res://customNode/fist.tscn", int amountToLoad = 5, Node2D startMarker = null, Node2D endMarker = null) :
    base(owner, startMarker, endMarker)
    {
        pool = new Node2D[amountToLoad];
        PackedScene sceneProjectil = GD.Load<PackedScene>(pathProjectilInstance);
        for (int i = 0; i < amountToLoad; i++)
        {
            pool[i] = sceneProjectil.Instantiate<Node2D>();
            projectilEndTravel(pool[i]);  // reset pos and disabled.
            owner.GetParent<Node2D>().CallDeferred(Node.MethodName.AddChild, pool[i]);
        }
    }

    public override void shoot(Vector2? startPos = null, Vector2? endPos = null, Node2D projectil = null)
    {
        if (timeUntilNextShot > 0f)
            return;
        timeUntilNextShot = cooldownShot;

        // next projectil is not ready yet.
        projectil = pool[indexPool];
        if (projectil.ProcessMode != Node.ProcessModeEnum.Disabled)
            return;
        projectil.ProcessMode = Node.ProcessModeEnum.Inherit;  // unable it.

        // increase index pool (for next shoot).
        indexPool = (indexPool + 1) % pool.Length;

        // launch it.
        (projectil as Projectil).setData(
            startPos ?? this.startPos,
            endPos ?? this.endPos,
            timeTravel,
            this
        );
    }

    public override void projectilEndTravel(Node2D projectil)
    {
        projectil.GlobalPosition = new Vector2(0, 9000);  // safe spot (out of map).
        projectil.ProcessMode = Node.ProcessModeEnum.Disabled;  // disabled it.
    }

}
