using Godot;

public interface ICanFall
{

    public Vector2 fall(Vector2 velocity, float delta);


    // ------> example.

    //public bool isGravity = true;
    //public float mass = 1.0f;

    //public Vector2 fall(Vector2 velocity, float delta) {
    //	if (this.isGravity && !IsOnFloor())
    //	{
    //		velocity += GetGravity() * delta * mass;
    //	}
	//	return velocity;
    //}

}