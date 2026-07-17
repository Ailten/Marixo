using Godot;

public class CanHightCurveJump : CanCurveJump
{
    public float multiplyHight = 2f;

    protected override float getJumpStrength
    {
        get => jumpStrength * multiplyHight;
    }

    public CanHightCurveJump(CharacterBody2D owner, CanFall canFall, float jumpStrength = 10f, float timeJump = 1f) : base(owner, canFall, jumpStrength, timeJump)
    {
    }

    public static CanHightCurveJump evolvFrom(CanCurveJump ccj)
    {
        return new CanHightCurveJump(
            ccj.owner, ccj.canFall, ccj.jumpStrength, ccj.timeJump
        );
    }
}
