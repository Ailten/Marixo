using System;
using Godot;

public partial class FruitsBox : Node2D, IShootable
{
    private Node2D baseNode;

    public override void _Ready()
    {
        // get base node fruits box.
        baseNode = GetNode<Node2D>("..");

        // snap ground.
        baseNode.GlobalPosition = baseNode.GlobalPosition.snapToGround();
    }

    public void takeShoot(Node2D projectil)
    {
        openTheBox();
    }

    private void openTheBox()
    {
        // get random fruit type.
        int jumpBoostTypeLength = Enum.GetValues(typeof(JumpBoostType)).Length;
        JumpBoostType randomJumpBoostType = (JumpBoostType)GM.rand.Next(jumpBoostTypeLength);

        // spawn fruit.
        JumpBoost instanceJumpBoost = JumpBoost.pool.getNextElement();
        instanceJumpBoost.setData(randomJumpBoostType, GlobalPosition + (Vector2.Up * 116f));

        // spawn FX explo.
        Explo explo = Explo.pool.getNextElement();
        explo.initExplo(GlobalPosition + (Vector2.Up * 58f), Vector2.One * 0.5f);

        // destroy box.
        baseNode.QueueFree();
    }
}