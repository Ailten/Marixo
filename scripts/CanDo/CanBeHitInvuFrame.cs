using Godot;

public class CanBeHitInvuFrame : CanBeHit
{
    public float timeDuringInvu = 2f;

    public bool isInvu
    {
        get => ((float)(Time.GetTicksMsec() - timeWorldWhenLastHit)) / 1000f < (cooldownDamaged + timeDuringInvu);
    }
    public bool isWasSetToInvu = false;

    public CanBeHitInvuFrame(Character owner) : base(owner)
    {
    }
    
    public static CanBeHitInvuFrame evolvFrom(CanBeHit cbh)
    {
        return new CanBeHitInvuFrame(
            cbh.owner
        );
    }
}