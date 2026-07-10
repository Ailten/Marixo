using Godot;

public class CanFall
{
    private CharacterBody2D owner;

    public bool isGravity = true;
    public float mass;
    public float fallingMultiplication = 1f;
    public float gravityMult = 1f;

    public CanFall(CharacterBody2D owner, float mass = 1.0f)
    {
        this.owner = owner;
        this.mass = mass;
    }

    public Vector2 fall(Vector2 velocity, float delta)
    {
        if (isGravity && !owner.IsOnFloor())
        {
            velocity += owner.GetGravity() * delta * mass * (velocity.Y < 0.8f? fallingMultiplication: 1f) * gravityMult;
        }
        return velocity;
    }
}