using Godot;

public class CanDoubleJump : CanJump
{
    public bool isCanDoubleJump = true;

    public CanDoubleJump(CharacterBody2D owner, float jumpStrength = 600f) : base(owner, jumpStrength)
    {
    }

    public override Vector2 jump(Vector2 velocity, Vector2? directionJump = null)
    {
        if (!owner.IsOnFloor() && isCanDoubleJump)
        {
            isCanDoubleJump = false;
            velocity += (directionJump ?? Vector2.Up) * jumpStrength;
            return velocity;
        }

        return base.jump(velocity, directionJump);
    }

    public void checkIsTouchingFloor()
    {
        if (owner.IsOnFloor() && !isCanDoubleJump)
            isCanDoubleJump = true;
    }
}