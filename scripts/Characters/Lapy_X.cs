using System;
using Godot;

public partial class Lapy_X : Character
{
    private StateLapy stateLapy = StateLapy.wait_to_jump_walk;
    private float totalTimeAnime = 0f;
    private Vector2 startPosAnime, endPosAnime;
    private const float ratioUpJump = 0.5f;
    private float distanceJumpWalk
    {
        get => GM.groundTileLenght;
    }
    private float heightJumpWalk
    {
        get => Math.Abs(endPosAnime.X - startPosAnime.X) * ratioUpJump;
    }
    private RayCast2D rayCastIsGround;

    public override void _Ready()
    {
        base._Ready();
        rayCastIsGround = GetNode<RayCast2D>("RayCast2DIsGround");
        rayCastIsGround.Position = new Vector2(  // set position X to dest jump.
            distanceJumpWalk,
            rayCastIsGround.Position.Y
        );
    }

    public override void _Process(double delta)
    {
        totalTimeAnime += (float)delta;

        // loop to eval new state when change.
        bool isStillEvalState = true;
        while (isStillEvalState)
        {
            float i = totalTimeAnime / stateLapy.getTimeAnime();

            switch (stateLapy)
            {
                case StateLapy.wait_to_jump_walk:
                    if (totalTimeAnime == 0f)
                    {
                        if (!rayCastIsGround.CollideWithBodies)
                        {
                            isLookAtRight = !isLookAtRight;
                        }
                    }
                    if (i > 1f)
                    {
                        setNewState(StateLapy.jump_walk);
                        setPosStartEndJump(
                            startPosAnime: GlobalPosition,
                            endPosAnime: GlobalPosition + (
                                (isLookAtRight ? Vector2.Right : Vector2.Left) * distanceJumpWalk
                            )
                        );
                        continue;
                    }
                    isStillEvalState = false;
                    continue;

                case StateLapy.jump_walk:
                    if (i > 1f)
                    {
                        GlobalPosition = endPosAnime;
                        setNewState(StateLapy.wait_to_jump_walk);
                        continue;
                    }

                    GlobalPosition = startPosAnime.bezierLerp(i,
                        startPosAnime + (Vector2.Up * heightJumpWalk),
                        endPosAnime + (Vector2.Up * heightJumpWalk),
                        endPosAnime
                    );

                    isStillEvalState = false;
                    continue;

                case StateLapy.hited:

                    // TODO. (implement hit anime, and HP, death and damage method in character).

                    isStillEvalState = false;
                    continue;
            }
        }
        
    }

    private void setNewState(StateLapy stateLapy)
    {
        this.stateLapy = stateLapy;
        animatedSprite.Play(
            stateLapy.getSpriteAnime()
        );
        totalTimeAnime = 0f;
    }

    private void setPosStartEndJump(Vector2 startPosAnime, Vector2 endPosAnime)
    {
        this.startPosAnime = startPosAnime;
        this.endPosAnime = endPosAnime;
    }

	protected override void applyFlipH()
    {
        base.applyFlipH();
        GetNode<Marker2D>("RigidBody2DTrigger").Position *= new Vector2(-1, 1);
    }

    // ------>

    public override bool takeDamage(int damage, Character damageMaker = null)
    {
        if (!isLiving)
            return false;

        // TODO:.

        return true;
    }
}
