using System;
using Godot;

public class CanDoubleCurveJump : CanCurveJump
{
    private bool isHasDoubleJump = true;

    public CanDoubleCurveJump(CharacterBody2D owner, float jumpStrength = 10f, float timeJump = 1f) : base(owner, jumpStrength, timeJump)
    {
    }

    public override Vector2 jump(Vector2 velocity, Vector2? directionJump = null)
    {
        if (!owner.IsOnFloor() && isHasDoubleJump && !isStartingJump)
        {
            isHasDoubleJump = false;
            CanFall cf = canFall;  // disable canFall for not erase gravityMult.
            canFall = null;
            velocity = makeJump(velocity, directionJump);
            canFall = cf;
            canFall.gravityMult = 0f;
            return velocity;
        }

        return base.jump(velocity, directionJump);
    }

    public override Vector2 updateJump(Vector2 velocity, float delta)
    {
        if (!isHasDoubleJump && owner.IsOnFloor())
        {
            isHasDoubleJump = true;
        }

        return base.updateJump(velocity, delta);
    }

    public static CanDoubleCurveJump evolvFrom(CanCurveJump ccj)
    {
        return new CanDoubleCurveJump(
            ccj.owner, ccj.jumpStrength, ccj.timeJump
        );
    }
}
