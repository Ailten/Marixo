using Godot;

public partial class Explo : Node2D
{
    private ulong timeWorldWhenSpawn;
    private const float timeToLive = 0.2f * 3;

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
        QueueFree();
    }
}