using Godot;

public static class GM
{
    public static TileMapLayer tileMapLayer;
    public static Vector2 groundTileSize;
    public static float groundTileLenght
    {
        get => groundTileSize.X;
    }
    public static float sixteenFraction
    {
        get => 0.0625f;
    }

    public static Vector2 snapToGrid(this Vector2 pos)
    {
        // self made (less opti).
        Vector2 posReduced = pos - (pos % groundTileSize);
        Vector2 dif = (pos - posReduced).Abs();
        if (dif.X > groundTileSize.X * 0.5f)
            posReduced.X += (pos.X < groundTileSize.X * 0.5f ?
                -groundTileSize.X :
                groundTileSize.X
            );
        if (dif.Y > groundTileSize.Y * 0.5f)
            posReduced.Y += (pos.Y < groundTileSize.Y * 0.5f ?
                -groundTileSize.Y :
                groundTileSize.Y
            );
        return posReduced;

        // get index of cell map (from pos world).
        //Vector2I cellMapIndex = tileMapLayer.LocalToMap(
        //    tileMapLayer.ToLocal(pos)
        //);
        //// get pos world (from index cell map).
        //Vector2 snapped = tileMapLayer.ToGlobal(
        //    tileMapLayer.MapToLocal(cellMapIndex)
        //);
        //// correct position.
        //snapped += new Vector2(-28, 50);
        //return snapped;
    }

    public static Vector2 snapToCenterGrid(this Vector2 pos)
    {
        Vector2 posSnapped = pos.snapToGrid();
        Vector2 dif = pos - posSnapped;
        Vector2 difNormalised = new Vector2(dif.X < 0 ? -0.5f : 0.5f, dif.Y < 0 ? -0.5f : 0.5f);
        posSnapped += difNormalised * groundTileSize;
        return posSnapped;
    }

    public static Vector2 bezierLerp(this Vector2 startingPos, float i, params Vector2[] otherPos)
    {
        if (otherPos.Length == 0)
            return startingPos;

        Vector2 nextStartingPos = startingPos.Lerp(otherPos[0], i);

        if (otherPos.Length == 1)
            return nextStartingPos;

        Vector2[] nextOtherPos = new Vector2[otherPos.Length - 1];
        for (int j = 0; j < nextOtherPos.Length; j++)
        {
            nextOtherPos[j] = otherPos[j].Lerp(otherPos[j + 1], i);
        }
        return nextStartingPos.bezierLerp(i, nextOtherPos);
    }
}

// z-index print sprite (Ordering):
//     << the back >>
// ground :              0
// fx :                 70
// boost :              80
// mob :                90
// player :            100
// player projectil :  110
//     << the front >>

// TODO:
// remove colision from tile border water and deep water (usless colision, who take vertex).
// character need something to print Live, or damage take when hit.
// need a pannel (blaca ?) as tutoriel indicator (maybe PNJ with scripted scenario).
// HUD player HP, + VN dialogue pop up.
// add pnj talkable (for tuto) -> blaca.
// add boost (shoot).
// add menu.
// add background level (paralax ?).
// add a groundPownd.
// add an "echel".
// debug lowGravityCurveJump.
// make slower the lapy jump.
// make speeder the lapy lerp damage.
// add effect when take a boost.
// add check point.
// make Explo static pool (on a generic way, for can be re-use. help with canPoolShot script).
// make lapy spawn shrimp.

// ? explosion anime (for death lapy).
// ? collectible coin or other props (shrimp).
