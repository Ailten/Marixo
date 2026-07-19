using Godot;

public partial class Explo : Node2D, IPoolableProps
{
    private ulong timeWorldWhenSpawn;
    private const float timeToLive = 0.2f * 3;
    
    public static PoolProps<Explo> pool = new PoolProps<Explo>("explo", poolSize: 6);

    public override void _Ready()
    {
    }

    public void initExplo(Vector2 globalPos, Vector2? scale = null)
    {
        GlobalPosition = globalPos;
        Scale = scale ?? Vector2.One;

        timeWorldWhenSpawn = Time.GetTicksMsec();
        GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("explo");
    }

    public override void _Process(double delta)
    {
        float timeFromSpawn = (float)(Time.GetTicksMsec() - timeWorldWhenSpawn) / 1000f;
        if (timeFromSpawn >= timeToLive)
        {
            endExplo();
        }
    }

    public void endExplo()
    {
        (this as IPoolableProps).setElementPoolSleep();
    }
}