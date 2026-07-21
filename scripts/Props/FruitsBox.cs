using System;
using Godot;

public partial class FruitsBox : Node2D, IPoolableRespawn
{

    public override void _Ready()
    {
        // snap ground.
        GlobalPosition = GlobalPosition.snapToGround();

        posSpawn = GlobalPosition;
        spawn();
        PoolRespawn.setInPool(this);
    }

    private Vector2 posSpawn;
    public void spawn()
    {
        GlobalPosition = posSpawn;
    }

    public void openTheBox()
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
        (this as IPoolableRespawn).unspawn();
    }
}