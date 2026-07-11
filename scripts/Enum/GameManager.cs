using Godot;

public static class GM
{
    public static float groundTileLenght
    {
        get => 427.645f;  // eval with markers.
        // get => 425f;  // rounded manually.
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