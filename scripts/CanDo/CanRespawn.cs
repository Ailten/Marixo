using Godot;

public class CanRespawn
{
    private Character owner;
    public Vector2 posToRespawn;

    public CanRespawn(Character owner, Vector2? posToRespawn = null)
    {
        this.owner = owner;
        this.posToRespawn = posToRespawn ?? owner.GlobalPosition;
    }

    public void teleportRespawn()
    {
        owner.GlobalPosition = posToRespawn;
    }
}