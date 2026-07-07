using Godot;

public interface ICanWalk
{

    public Vector2 walk(Vector2 velocity, float delta, Vector2? directionWalk = null);

    // ------> example.

    //public float speedWalk = 320f;

    //public Vector2 walk(Vector2 velocity, float delta, Vector2? directionWalk = null)
    //{
    //  velocity += (directionWalk ?? Vector2.Right) * speedWalk * delta;
    //  return velocity;
    //}

}