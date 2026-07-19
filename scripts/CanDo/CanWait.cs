using Godot;

public class CanWait
{
    private float totalTimeWait;
    private float timeToWait;
    private bool isWaiting = false;
    public bool isWait
    {
        get => isWaiting;
    }

    public CanWait(float timeToWait)
    {
        this.timeToWait = timeToWait;
    }

    public bool startWait()
    {
        if (isWaiting)
            return false;
        totalTimeWait = 0f;
        isWaiting = true;
        return true;
    }

    public void updateWait(float delta)
    {
        totalTimeWait += delta;
        if (totalTimeWait >= timeToWait)
            endWait();
    }

    public void endWait()
    {
        isWaiting = false;
    }
}