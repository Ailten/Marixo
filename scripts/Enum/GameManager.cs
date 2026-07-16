using Godot;

public static class GM
{
    public static float groundTileLenght
    {
        //get => 213.8225f;  // scale by tilemap transform.
        get => 427.645f;  // eval with markers.
        // get => 425f;  // rounded manually.
    }

    private static Vector2 origineWorld = new Vector2(510.005f, 764.915f);

    public static Vector2 snapToGrid(this Vector2 pos, float scale = 0.25f)
    {
        pos.X -= pos.X % (origineWorld.X * scale);
        pos.Y -= pos.Y % (origineWorld.Y * scale);
        return pos;
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


// TODO:
// remove colision from tile border water and deep water (usless colision, who take vertex).
// character need something to print Live, or damage take when hit.
// need a pannel (blaca ?) as tutoriel indicator (maybe PNJ with scripted scenario).
// HUD player HP, + VN dialogue pop up.

// ? explosion anime (for death lapy).
// ? collectible coin or other props (shrimp).
