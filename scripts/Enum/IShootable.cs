
/// <summary>
/// Interface for node collision solide, wish can collide with projectil.
/// </summary>
public interface IShootable
{
    public void takeShoot(Godot.Node2D projectil);
}