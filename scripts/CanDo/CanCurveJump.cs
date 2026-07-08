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
        if (!owner.IsOnFloor() || this.isJumping)
            return velocity;

        return makeJump(velocity, directionJump);
    }

    protected Vector2 makeJump(Vector2 velocity, Vector2? directionJump = null)
    {
        this.isJumping = true;
        this.timeFromJump = 0f;
        this.directionJump = directionJump ?? Vector2.Up;
        if (this.canFall != null)
        {
            this.gravityMult = this.canFall.gravityMult;
            this.canFall.gravityMult = 0f;
        }
        return velocity;
    }

    public virtual Vector2 updateJump(Vector2 velocity, float delta)
    {
        if (!this.isJumping)
            return velocity;

        this.timeFromJump += delta;
        if (this.timeFromJump >= this.timeJump)
        {
            this.endJump();
            return velocity;
        }

        if (
            !this.isStartingJump &&
            (this.owner.GetSlideCollisionCount() > 0 || this.owner.IsOnFloor())
        )
        {
            this.endJump();
            return velocity;
        }

        float i = this.timeFromJump / this.timeJump;  // interpolate linear.
        float i_curv;
        bool isFall = i > 0.5f;
        if (!isFall)
        {
            i *= 2f;  // 0~1.
            i_curv = Mathf.Lerp(this.jumpStrength, 0f, i);  // 1D bezier curve normalized.
            i_curv = Mathf.Lerp(this.jumpStrength, i_curv, i);
        }
        else
        {
            i = (i - 0.5f) * 2f;  // 0~1.
            i = 1f - i;
            //i_curv = Mathf.Lerp(0f, -this.jumpStrength, i);  // 1D bezier curve normalized.
            //i_curv = Mathf.Lerp(i_curv, -this.jumpStrength, i);
            //velocity += directionJump * i_curv;

            i_curv = Mathf.Lerp(0f, this.gravityMult, i);  // apply fall with gravity mult.
            i_curv = Mathf.Lerp(i_curv, this.gravityMult, i);
            this.canFall.gravityMult = i_curv;
            return velocity;

        }

        velocity += directionJump * i_curv;
        return velocity;
    }

    private void endJump()
    {
        this.isJumping = false;
        if (this.canFall != null)
        {
            this.canFall.gravityMult = this.gravityMult;
        }
    }
    
}