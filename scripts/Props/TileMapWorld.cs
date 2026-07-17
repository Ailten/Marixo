using Godot;

public partial class TileMapWorld : TileMapLayer
{
    public override void _Ready()
    {
        GM.tileMapLayer = this;
        GM.groundTileSize = TileSet.TileSize * Scale;
    }
}