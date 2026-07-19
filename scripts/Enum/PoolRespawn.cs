using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public static class PoolRespawn
{
    private static List<IPoolableRespawn> pool = new();

    public static void clean()
    {
        pool.Clear();
    }

    public static void setInPool(IPoolableRespawn element)
    {
        pool.Add(element);
    }

    public static void respawnAllDisabled()
    {
        pool.ForEach(e =>
        {
            if (e is Node2D eNode)
            {
                if (eNode.ProcessMode != Node.ProcessModeEnum.Disabled)
                    return;

                eNode.setProcessModeDeferred(Node.ProcessModeEnum.Inherit);
                e.spawn();
            }
            else
                GD.PrintErr($"IPoolableRespawn, element is not Node2D.");
        });
    }
}

public interface IPoolableRespawn
{
    public void spawn();
    public void unspawn()
    {
        if (this is Node2D elementPool)
        {
            elementPool.GlobalPosition = new Vector2(0, 9000);  // safe spot (out of map).
            elementPool.setProcessModeDeferred(Node.ProcessModeEnum.Disabled);
        }
        else
            throw new Exception($"IPoolableRespawn, is not Node2D.");
    }
}

/* --- demo ---

class A : Character, IPoolableRespawn{

    public override void _Ready(){
        posSpawn = GlobalPosition;
        isLookAtRightSpawn = isLookAtRight;
        spawn();
    }

    private Vector2 posSpawn;
    private bool isLookAtRightSpawn;
    public void spawn()
    {
        GlobalPosition = posSpawn;
        if (isLookAtRightSpawn ^ isLookAtRight)
            isLookAtRight = !isLookAtRight;
            
        refillLive();
    }

    public override void death()
    {
        (this is IPoolableRespawn).unspawn();
    }

}

*/