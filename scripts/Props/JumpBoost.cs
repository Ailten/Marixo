using Godot;

public partial class JumpBoost : Node2D, IPoolableProps
{
    private JumpBoostType jumpBoostType = JumpBoostType.Appel;
    private Vector2 posSpawn;
    private const float highWaveFly = 8f;
    private const float timeWaveAnime = 2.5f;
    private float timeWaveAnimeInMilisec
    {
        get => timeWaveAnime * 1000f;
    }

    public static PoolProps<JumpBoost> pool = new PoolProps<JumpBoost>("jumpBoost", poolSize:4);

    public override void _Ready()
    {
        GetNode<Area2D>("Area2D").BodyEntered += (Node2D body) =>
        {
            if (body is Player playerScript)
            {
                upgradePlayerJump(playerScript);
                QueueFree();
            }
        };

        // snap to grid.
        GlobalPosition = GlobalPosition.snapToCenterGrid();

        posSpawn = GlobalPosition;
    }

    public void setData(JumpBoostType jumpBoostType, Vector2 pos)
    {
        this.jumpBoostType = jumpBoostType;
        GetNode<Sprite2D>("Sprite2D").Texture = GD.Load<Texture2D>($"res://sprites/{jumpBoostType.ToString().ToLower()}.png");

        // set pos (center tile map).
        GlobalPosition = pos.snapToCenterGrid();
        posSpawn = GlobalPosition;
    }

    public override void _Process(double delta)
    {
        // anime fly wave.
        float i_animeWaveFly = (Time.GetTicksMsec() % timeWaveAnimeInMilisec) / timeWaveAnimeInMilisec;
        float i_animeWaveFly_cos = Mathf.Cos(i_animeWaveFly * Mathf.Pi * 2f);
        GlobalPosition = posSpawn + (Vector2.Up * (i_animeWaveFly_cos * highWaveFly));
    }

    public void upgradePlayerJump(Player playerScript)
    {
        switch (jumpBoostType)
        {
            case JumpBoostType.Appel:
                playerScript.canJump = CanLowGravityCurveJump.evolvFrom(playerScript.canJump as CanCurveJump);
                break;
            case JumpBoostType.Banana:
                playerScript.canJump = CanHightCurveJump.evolvFrom(playerScript.canJump as CanCurveJump);
                break;
            case JumpBoostType.Cherry:
                playerScript.canJump = CanDoubleCurveJump.evolvFrom(playerScript.canJump as CanCurveJump);
                break;
        }
    }
}
