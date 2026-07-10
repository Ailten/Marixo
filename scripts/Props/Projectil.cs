using Godot;

public partial class Projectil : Node2D
{
    protected Vector2 startPos, endPos;
    private float cumulTime = 0f;
    private float endTime;
    private Area2D triggerArea;
    private CanShoot launcher;
    private bool isStarting
    {
        get => cumulTime < endTime * 0.1f;
    }


    public void setData(Vector2 startPos, Vector2 endPos, float endTime, CanShoot launcher)
    {
        this.startPos = startPos;
        this.endPos = endPos;
        this.endTime = endTime;
        this.launcher = launcher;

        triggerArea = GetNode<Area2D>("Area2D");
        triggerArea.BodyEntered += onTriggerEnter;

        GetNode<Sprite2D>("Sprite2D").FlipH = startPos.X > endPos.X;
    }

    public override void _Process(double delta)
    {
        // eval interpolation time travel.
        cumulTime += (float)delta;
        float i = cumulTime / endTime;
        if (i > 1f)
        {
            endTravel();
            return;
        }

        // execute lerp pos.
        setPosUpdate(i);
    }

    protected virtual void setPosUpdate(float i)
    {
        GlobalPosition = startPos.Lerp(endPos, i);
    }

    // ------>

    protected void endTravel()
    {
        launcher.projectilEndTravel((this as Node2D));
    }

    protected void onTriggerEnter(Node2D body)
    {
        // skip first update, just spawn in ground.
        if (this.isStarting)
            return;

        launcher.projectilTrigger((this as Node2D), body);
    }

}