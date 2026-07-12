using Godot;

public static class GM
{
    public static float groundTileLenght
    {
        get => 427.645f;  // eval with markers.
        // get => 425f;  // rounded manually.
    }

    private static Vector2 origineWorld = new Vector2(510.005f, 764.915f);

    public static Vector2 snapToGrid(this Vector2 pos)
    {
        pos.X -= pos.X % origineWorld.X;
        pos.Y -= pos.Y % origineWorld.Y;
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
// lapy need using "canCurveJump" (ou canBezierLerpJump) -> minimum using a "can" for jump.
// player need a "canRespawn" -> override death.
// lapy need override "takedamage / death" -> switch stateLapy to hit. at the end of anime hit, check to call death (else, lerp to previous pos jump).
// character need something to print Live, or damage take when hit.