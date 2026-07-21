using Godot;

public class CanLowGravityCurveJump : CanCurveJump
{
    public const float timeJumpMult = 0.35f;

    public override bool isStartingJump
    {
        get => getInterpolationTime < 0.1f;
    }
    protected override float getInterpolationTime
    {
        get => base.getInterpolationTime * timeJumpMult;
    }
    protected override float getJumpStrength{
		get => jumpStrength * 0.2f;
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
