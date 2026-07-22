using Godot;

public partial class FxHPLost : Node2D, IPoolableProps
{
    private int HPLeft = 0;
    private static Color textColor = new Color(1, 0, 0, 1);
    private static Vector2 endDirectionMove = Vector2.Up * 80f;

    private float timeFromSpawn;
    private float timeMaxAnime;
    private float getInterpolation
    {
        get => timeFromSpawn / timeMaxAnime;
    }
    
    public static PoolProps<FxHPLost> pool = new PoolProps<FxHPLost>("fxHPLost", poolSize: 10);


    public void setData(Vector2 pos, int HPLeft, float timeMaxAnime = 1f)
    {
        GlobalPosition = pos;

        this.HPLeft = HPLeft;

        timeFromSpawn = 0;
        this.timeMaxAnime = timeMaxAnime;
    }

    public void endFx()
    {
        (this as IPoolableProps).setElementPoolSleep();
    }

    public override void _Process(double delta)
    {
        timeFromSpawn += (float)delta;
        if (timeFromSpawn >= timeMaxAnime)
        {
            endFx();
        }

        // force re draw.
        QueueRedraw();
    }

    public override void _Draw()
    {
        float i = getInterpolation;
        float opacity = Mathf.Min((1 - i) * 3f, 1f);

        DrawString(
            font: ThemeDB.FallbackFont,
            pos: Vector2.Zero.Lerp(endDirectionMove, i) + (Vector2.Left * 20f),
            text: $"- {HPLeft} ♥",

            //alignment: HorizontalAlignment.Center,  // not work.
            fontSize: 20,
            modulate: textColor * new Color(1, 1, 1, opacity)
        );
    }
}