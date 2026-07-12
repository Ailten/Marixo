using System;
using Godot;

public partial class Lapy : Character
{
    private StateLapy stateLapy = StateLapy.wait_to_jump_walk;
    private bool isStateBusy = false;

    private CanJump canJump;
    private CanWait canWait;

    private Vector2 posSpawn, lastPosValid;

    public override void _Ready()
    {
        base._Ready();
        cooldownDamaged = 0.3f;
        canJump = new CanLapyJump(this, jumpLength: GM.groundTileLenght);
        canWait = new CanWait(timeToWait: 0.3f);

        posSpawn = GlobalPosition;
        lastPosValid = GlobalPosition;

        // TODO:
        // jump to height,
        // no cooldown wait.
        // desapear in border map ? (or continue at infinity).
        // jump speed to fast.

        // snap to grid.
        GlobalPosition = GlobalPosition.snapToGrid() + (GM.groundTileLenght * 0.5f * Vector2.Right);
    }

    public override void _Process(double delta)
    {
        switch (stateLapy)
        {
            case StateLapy.wait_to_jump_walk:
                if (!canWait.startWait())
                {
                    canWait.updateWait((float)delta);
                    if (canWait.isWait)
                    {
                        stateLapy = StateLapy.jump_walk;
                    }
                }
                break;

            case StateLapy.jump_walk:
                Vector2 pos = canJump.jump(
                    GlobalPosition,
                    directionJump: isLookAtRight ? Vector2.Right : Vector2.Left
                );
                pos = canJump.updateJump(pos, (float)delta);
                if (canJump is CanLapyJump canLapyJump)
                {
                    if (!canLapyJump.isJump)
                    {
                        stateLapy = StateLapy.wait_to_jump_walk;
                    }
                }
                GlobalPosition = pos;
                break;

            case StateLapy.hited:
                // TODO.
                break;
        }
    }

    // ------> 

    public override bool takeDamage(int damage, Character damageMaker = null)
    {
        if (!isLiving)
            return false;

        

        return true;
    }

}