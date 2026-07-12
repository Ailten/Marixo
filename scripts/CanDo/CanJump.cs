using Godot;

public class CanJump
{
    public CharacterBody2D owner;

    public float jumpStrength;

    public CanJump(CharacterBody2D owner, float jumpStrength = 600f)
    {
        this.owner = owner;
        this.jumpStrength = jumpStrength;
    }

    public virtual Vector2 jump(Vector2 velocity, Vector2? directionJump = null)
    {
        if (!owner.IsOnFloor())
            return velocity;
        
        return velocity + (directionJump ?? Vector2.Up) * jumpStrength;
    }

    public virtual Vector2 updateJump(Vector2 velocity, float delta)
    {
        return velocity;
    }
}