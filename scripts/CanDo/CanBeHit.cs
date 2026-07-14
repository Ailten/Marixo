using Godot;

public class CanBeHit
{
    public float cooldownDamaged = 0.7f;
    private ulong timeWorldWhenLastHit = 0;
    public bool isCooldownDamaged
    {
        get => ((float)(Time.GetTicksMsec() - timeWorldWhenLastHit)) / 1000f < cooldownDamaged;
    }
    public float interpolateCooldownDamaged
    {
        get => (((float)(Time.GetTicksMsec() - timeWorldWhenLastHit)) / 1000f) / cooldownDamaged;
    }

    public Vector2 directionHitBackDash;
    public float intencityRepeal = 7f;

    public Character owner;
    public Character characterWhoAtk;


    public CanBeHit(Character owner)
    {
        this.owner = owner;
    }

    public void beHit(Character characterWhoAtk = null)
    {
        timeWorldWhenLastHit = Time.GetTicksMsec();
        this.characterWhoAtk = characterWhoAtk;
		directionHitBackDash = ((characterWhoAtk ?? owner).GlobalPosition - owner.GlobalPosition).Normalized();
    }

    public virtual Vector2 getRepealVelocityUpdate(Vector2 velocity)
    {
        float i = interpolateCooldownDamaged;
        velocity += directionHitBackDash.bezierLerp(i, Vector2.Zero);
        return velocity;
    }


}