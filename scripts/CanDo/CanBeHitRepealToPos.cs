using Godot;

public class CanBeHitRepealToPos : CanBeHit
{
    public Vector2 posDestRepeal;

    public CanBeHitRepealToPos(Character owner) : base(owner)
    {
    }

    public override Vector2 getRepealVelocityUpdate(Vector2 velocity)
    {
        float i = interpolateCooldownDamaged;
        return owner.GlobalPosition.bezierLerp(i, posDestRepeal);
    }

    public static CanBeHitRepealToPos evolvFrom(CanBeHit cbh)
    {
        return new CanBeHitRepealToPos(
            cbh.owner
        );
    }
}