using Godot;

public class CanWalk : ICanWalk
{
    public float speedWalk;
    public bool isWalking = false;

    public CanWalk(float speedWalk = 320f)
    {
        this.speedWalk = speedWalk;
    }

    public Vector2 walk(Vector2 velocity, float delta, Vector2? directionWalk = null)
    {
        velocity += (directionWalk ?? Vector2.Right) * speedWalk * delta;
        return velocity;
    }
}