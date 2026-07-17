using Godot;

public class CanLowGravityCurveJump : CanCurveJump
{
    public float timeJumpMult = 0.5f;

    public override bool isStartingJump
    {
        get => getInterpolationTime < 0.1f;
    }
    protected override float getInterpolationTime
    {
        get => base.getInterpolationTime * timeJumpMult;
    }

    public CanLowGravityCurveJump(CharacterBody2D owner, CanFall canFall, float jumpStrength = 10f, float timeJump = 1f) : base(owner, canFall, jumpStrength, timeJump)
    {
    }

    public static CanLowGravityCurveJump evolvFrom(CanCurveJump ccj)
    {
        return new CanLowGravityCurveJump(
            ccj.owner, ccj.canFall, ccj.jumpStrength, ccj.timeJump
        );
    }
}
