using Godot;

public class CanSlideCurveJump : CanCurveJump
{
    public override bool isStartingJump
    {
        get => getInterpolationTime < 0.1f;
    }
    protected override float getInterpolationTime
    {
        get => base.getInterpolationTime * 0.4f;
    }
    protected override float getJumpStrength{
		get => jumpStrength * 0.5f;
    }


    public CanSlideCurveJump(CharacterBody2D owner, CanFall canFall, float jumpStrength = 10f, float timeJump = 1f) : base(owner, canFall, jumpStrength, timeJump)
    {
    }
    

	public override Vector2 updateJump(Vector2 velocity, float delta)
    {
        if (!isJumping)
            return velocity;

        timeFromJump += delta;
        float i = getInterpolationTime;  // interpolate linear.

        if (i >= 1f)
        {
            endJump();
            return velocity;
        }

        if (i > 0.25f && i < 0.75f)  // slide.
        {
            velocity -= canFall.fall(Vector2.Zero, delta);  // inverted gravity.
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

        float i_curv;
        bool isFall = i > 0.5f;
        if (!isFall)
        {
            i *= 4f;  // 0~1.
            i_curv = Mathf.Lerp(getJumpStrength, 0f, i);  // 1D bezier curve normalized.
            //i_curv = Mathf.Lerp(getJumpStrength, i_curv, i);
        }
        else
        {
            i = (i - 0.75f) * 4f;  // 0~1.
            i = 1f - i;

            i_curv = Mathf.Lerp(0f, gravityMult, i);  // apply fall with gravity mult.
            //i_curv = Mathf.Lerp(i_curv, gravityMult, i);
            canFall.gravityMult = i_curv;
            return velocity;
        }

        velocity += directionJump * i_curv;
        return velocity;
    }


    public static CanSlideCurveJump evolvFrom(CanCurveJump ccj)
    {
        return new CanSlideCurveJump(
            ccj.owner, ccj.canFall, ccj.jumpStrength, ccj.timeJump
        );
    }
}
