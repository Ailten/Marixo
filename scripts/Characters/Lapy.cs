using System;
using Godot;

public partial class Lapy : Character
{
    private StateLapy stateLapy = StateLapy.wait_to_jump_walk;
    private bool isStateBusy = false;

    private CanJump canJump;
    private CanWait canWait;

    private Vector2 posSpawn, lastPosValid;

    private RayCast2D rayCastIsGround;

    public override void _Ready()
    {
        base._Ready();
        canBeHit.cooldownDamaged = 0.3f;
        float jumpLength = GM.groundTileLenght * 0.3f;
        canJump = new CanLapyJump(this, jumpLength: jumpLength);
        canWait = new CanWait(timeToWait: 5.6f);

        posSpawn = GlobalPosition;
        lastPosValid = GlobalPosition;
        
        canBeHit = CanBeHitRepealToPos.evolvFrom(canBeHit);

        // TODO:
        // Colision to player, to make damage.
        // -- take damage player
        // -- respawn player
        // Colision from fist player, to take damage (?).
        // -- take damage lapy.
        // -- take damage lerp previous pos.
        // -- death.

        // snap to grid.
        GlobalPosition = (
            GlobalPosition.snapToGrid() +
            (GM.groundTileLenght * 0.15f * Vector2.Right) +  // center of a tile.
            (GM.groundTileLenght * 0.3f * 0.0625f * Vector2.Down)  // snap to grass.
        );

        rayCastIsGround = GetNode<RayCast2D>("RayCast2DIsGround");
        rayCastIsGround.Position = new Vector2(jumpLength, rayCastIsGround.Position.Y);  // set position X to dest jump.

        // set Character as the Monster for the AreaMonsterAtk.
        (GetNode<Area2D>("Area2DCollideHit") as AreaMonsterAtk).monsterWhoAtk = this;
    }

    public override void _Process(double delta)
    {
        switch (stateLapy)
        {
            case StateLapy.wait_to_jump_walk:
                if (!canWait.startWait())
                {
                    canWait.updateWait((float)delta);
                    if (!canWait.isWait)
                    {
                        stateLapy = StateLapy.jump_walk;
                        animatedSprite.Play(stateLapy.getSpriteAnime());
                    }
                }
                else  // set value before wait.
                {
                    lastPosValid = GlobalPosition;
                    if (!rayCastIsGround.IsColliding())
                    {
                        isLookAtRight = !isLookAtRight;
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
                        animatedSprite.Play(stateLapy.getSpriteAnime());
                    }
                }
                GlobalPosition = pos;
                break;

            case StateLapy.hited:
                float i = canBeHit.interpolateCooldownDamaged;
                if (!canBeHit.isCooldownDamaged)
                {
                    stateLapy = StateLapy.wait_to_jump_walk;
                    animatedSprite.Play(stateLapy.getSpriteAnime());
                    GlobalPosition = (canBeHit as CanBeHitRepealToPos).posDestRepeal;
                    break;
                }
                GlobalPosition = canBeHit.getRepealVelocityUpdate(Vector2.Zero);
                break;
        }
    }

	protected override void applyFlipH()
    {
        base.applyFlipH();
        rayCastIsGround.Position *= new Vector2(-1, 1);
    }

    // ------> 

    public override bool takeDamage(int damage, Character damageMaker = null)
    {
        if (!isLiving)
            return false;

        // switch state hited.
        stateLapy = StateLapy.hited;
        animatedSprite.Play(stateLapy.getSpriteAnime());
        (canBeHit as CanBeHitRepealToPos).posDestRepeal = lastPosValid;

        return true;
    }

}