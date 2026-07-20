using System;
using Godot;

public partial class Lapy : Character, IPoolableRespawn
{
    private StateLapy stateLapy = StateLapy.wait_to_jump_walk;

    private CanJump canJump;
    private CanWait canWait;

    private Vector2 posSpawn, lastPosValid;

    private RayCast2D rayCastIsGround;

    public override void _Ready()
    {
        base._Ready();
        canBeHit.cooldownDamaged = 0.1f;
        float jumpLength = GM.groundTileLenght;
        canJump = new CanLapyJump(this, jumpLength: jumpLength);
        canWait = new CanWait(timeToWait: 2.5f);

        setMaxHp = 2;

        canBeHit = CanBeHitRepealToPos.evolvFrom(canBeHit);

        // snap to ground.
        GlobalPosition = GlobalPosition.snapToGround();

        posSpawn = GlobalPosition;
        lastPosValid = GlobalPosition;

        rayCastIsGround = GetNode<RayCast2D>("RayCast2DIsGround");
        rayCastIsGround.Position = new Vector2(jumpLength, rayCastIsGround.Position.Y);  // set position X to dest jump.

        // set Character as the Monster for the AreaMonsterAtk.
        (GetNode<Area2D>("Area2DCollideHit") as AreaMonsterAtk).monsterWhoAtk = this;


        // set spawn param.
        isLookAtRightSpawn = isLookAtRight;

        spawn();
        PoolRespawn.setInPool(this);
    }

    private bool isLookAtRightSpawn;
    public void spawn()
    {
        GlobalPosition = posSpawn;
        if (isLookAtRightSpawn ^ isLookAtRight)
            isLookAtRight = !isLookAtRight;
            
        refillLive();
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
                if (!canBeHit.isCooldownDamaged)
                {
                    stateLapy = StateLapy.wait_to_jump_walk;
                    animatedSprite.Play(stateLapy.getSpriteAnime());
                    GlobalPosition = (canBeHit as CanBeHitRepealToPos).posDestRepeal;
                    if (!isLiving)
                    {
                        death(canBeHit.characterWhoAtk);
                    }
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

    public override bool takeDamage(int damage, Character damageMaker = null, bool isCheckDeath = true)
    {
        bool isTakenDamage = base.takeDamage(damage, damageMaker, isCheckDeath: false);

        if (!isTakenDamage)
            return false;

        // reset all states cooldown.
        resetCooldown();

        // switch state hited.
        stateLapy = StateLapy.hited;
        animatedSprite.Play(stateLapy.getSpriteAnime());
        (canBeHit as CanBeHitRepealToPos).posDestRepeal = isLiving ? lastPosValid : GlobalPosition;

        return true;
    }

    private void resetCooldown()
    {
        if (canWait.isWait)
        {
            canWait.endWait();
        }
        if (canJump is CanLapyJump canLapyJump)
        {
            if (canLapyJump.isJump)
            {
                canLapyJump.endJump();
            }
        }
    }

    public override void death(Character killer = null)
    {
        // spawn explosion (from pool).
        Explo explo = Explo.pool.getNextElement();
        explo.initExplo(GlobalPosition + (Vector2.Up * 20f), Vector2.One * 0.5f);

        // unspawn.
        (this as IPoolableRespawn).unspawn();
    }

}