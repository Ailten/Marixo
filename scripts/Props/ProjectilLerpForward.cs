using Godot;

public partial class ProjectilLerpForward : Projectil
{
    protected override void setPosUpdate(float i)
    {
        Vector2 posDest = startPos.Lerp(endPos, i);
        posDest = posDest.Lerp(endPos, i);
        GlobalPosition = posDest;
    }
}