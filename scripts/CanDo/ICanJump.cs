using Godot;

public interface ICanJump
{

    public Vector2 jump(Vector2 velocity, Vector2? directionJump = null);

    // ------> example.

    //public float jumpStrength = 1000f;

    //public Vector2 jump(Vector2 velocity, Vector2? directionJump = null){
    //  if (IsOnFloor()){
    //      velocity += (directionJump ?? Vector2.Up) * jumpStrength;
    //  }
    //  return velocity;
    //}
}