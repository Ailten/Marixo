using Godot;

public class CanFall : ICanFall
{
    private CharacterBody2D owner;

    public bool isGravity = true;
    public float mass;
    public float fallingMultiplication = 1f;

    public CanFall(CharacterBody2D owner, float mass = 1.0f)
    {
        this.owner = owner;
        this.mass = mass;
    }

    public Vector2 fall(Vector2 velocity, float delta)
    {
        if (this.isGravity && !owner.IsOnFloor())
        {
            velocity += owner.GetGravity() * delta * mass * (velocity.Y < 0.8f? this.fallingMultiplication: 1f);
        }
        return velocity;
    }
}