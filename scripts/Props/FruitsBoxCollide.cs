using Godot;

public partial class FruitsBoxCollide : StaticBody2D, IShootable
{
    private FruitsBox fruitBox;

    public override void _Ready()
    {
        // get base node fruits box.
        fruitBox = GetNode<FruitsBox>("..");
    }

    public void takeShoot(Node2D projectil)
    {
        fruitBox.openTheBox();
    }
}