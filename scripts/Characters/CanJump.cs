using Godot;

public class CanJump : ICanJump
{
    public CharacterBody2D owner;

    public float jumpStrength;

    public CanJump(CharacterBody2D owner, float jumpStrength = 600f)
    {
        this.owner = owner;
        this.jumpStrength = jumpStrength;
    }
    
    public Vector2 jump(Vector2 velocity, Vector2? directionJump = null)
    {
        if (owner.IsOnFloor())
        {
            velocity += (directionJump ?? Vector2.Up) * jumpStrength;
        }
        return velocity;
    }
}