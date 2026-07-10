using System;
using Godot;

public class CanCurveJump : CanJump
{
    public bool isJumping = false;
    private float timeFromJump;
    public float timeJump;
    private Vector2 directionJump;
    public CanFall canFall = null;
    private float gravityMult;

    public bool isStartingJump
    {
        get => timeFromJump < (timeJump * 0.1f);
    }

    public CanCurveJump(CharacterBody2D owner, float jumpStrength = 10f, float timeJump = 1f) : base(owner, jumpStrength)
    {
        this.timeJump = timeJump;
    }

    public override Vector2 jump(Vector2 velocity, Vector2? directionJump = null)
    {
        if (!owner.IsOnFloor() || isJumping)
            return velocity;

        return makeJump(velocity, directionJump);
    }

    protected Vector2 makeJump(Vector2 velocity, Vector2? directionJump = null)
    {
        isJumping = true;
        timeFromJump = 0f;
        this.directionJump = directionJump ?? Vector2.Up;
        if (canFall != null)
        {
            gravityMult = canFall.gravityMult;
            canFall.gravityMult = 0f;
        }
        return velocity;
    }

    public virtual Vector2 updateJump(Vector2 velocity, float delta)
    {
        if (!isJumping)
            return velocity;

        timeFromJump += delta;
        if (timeFromJump >= timeJump)
        {
            endJump();
            return velocity;
        }

        if (
            !isStartingJump &&
            (owner.GetSlideCollisionCount() > 0 || owner.IsOnFloor())
        )
        {
            endJump();
            return velocity;
        }

        float i = timeFromJump / timeJump;  // interpolate linear.
        float i_curv;
        bool isFall = i > 0.5f;
        if (!isFall)
        {
            i *= 2f;  // 0~1.
            i_curv = Mathf.Lerp(jumpStrength, 0f, i);  // 1D bezier curve normalized.
            i_curv = Mathf.Lerp(jumpStrength, i_curv, i);
        }
        else
        {
            i = (i - 0.5f) * 2f;  // 0~1.
            i = 1f - i;
            //i_curv = Mathf.Lerp(0f, -jumpStrength, i);  // 1D bezier curve normalized.
            //i_curv = Mathf.Lerp(i_curv, -jumpStrength, i);
            //velocity += directionJump * i_curv;

            i_curv = Mathf.Lerp(0f, gravityMult, i);  // apply fall with gravity mult.
            i_curv = Mathf.Lerp(i_curv, gravityMult, i);
            canFall.gravityMult = i_curv;
            return velocity;

        }

        velocity += directionJump * i_curv;
        return velocity;
    }

    private void endJump()
    {
        isJumping = false;
        if (canFall != null)
        {
            canFall.gravityMult = gravityMult;
        }
    }
    
}