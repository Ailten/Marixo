
using Godot;

public partial class TestPos : Node2D
{
    public override void _Ready()
    {
        GD.Print(GlobalPosition);
        GlobalPosition = GlobalPosition.snapToCenterGrid();
        GD.Print(GlobalPosition);
    }

    public override void _Draw()
    {
        //Vector2 decal = new Vector2(-28, 50);

        //GD.Print($"center map pos : {Vector2.Zero + decal}");
        //GD.Print($"length tile world : {GM.groundTileLenght}");
        //
        //DrawCircle(Vector2.Zero + decal, 8f, new Color(1, 0, 0));
        //DrawCircle(Vector2.Right * GM.groundTileLenght + decal, 8f, new Color(1, 0, 0));
        //DrawCircle(Vector2.Down * GM.groundTileLenght + decal, 8f, new Color(1, 0, 0));
        //DrawCircle(Vector2.One * GM.groundTileLenght + decal, 8f, new Color(1, 0, 0));
        //
        //DrawLine(Vector2.Zero + decal, Vector2.Right * GM.groundTileLenght + decal, new Color(1, 0, 0));
        //DrawLine(Vector2.Zero + decal, Vector2.Down * GM.groundTileLenght + decal, new Color(1, 0, 0));
        //DrawLine(Vector2.Right * GM.groundTileLenght + decal, Vector2.One * GM.groundTileLenght + decal, new Color(1, 0, 0));
        //DrawLine(Vector2.Down * GM.groundTileLenght + decal, Vector2.One * GM.groundTileLenght + decal, new Color(1, 0, 0));
        
    }

    public override void _Process(double delta)
    {
    }
}