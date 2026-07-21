
using Godot;

public class PoolProps<T> where T : Node2D, IPoolableProps
{
    private T[] pool;
    private int indexPool = 0;

    public PoolProps(string instanceName, int poolSize = 10)
    {
        pool = new T[poolSize];
        PackedScene sceneProjectil = GD.Load<PackedScene>($"res://customNode/{instanceName}.tscn");
        for (int i = 0; i < poolSize; i++)
        {
            Node2D element = sceneProjectil.Instantiate<Node2D>();
            if (element is T scriptElement)
            {
                pool[i] = scriptElement;
                pool[i].setElementPoolSleep();
                Callable.From(() => GM.root.AddChild(scriptElement)).CallDeferred();
            }
            else
                GD.PrintErr($"PoolProps, element has no script needed.");
        }
    }

    public T getNextElement()
    {
        T currentElement = pool[indexPool];
        indexPool = (indexPool + 1) % pool.Length;
        currentElement.setElementPoolAwaike();
        return currentElement;
    }

}

/// <summary>
/// Interface for props who can be spawn during the run, but not defaultly place at start (like fx).
/// </summary>
public interface IPoolableProps
{
    public void setElementPoolAwaike()
    {
        if (this is Node2D elementPool)
        {
            elementPool.setProcessModeDeferred(Node.ProcessModeEnum.Inherit);
        }
        else
            GD.PrintErr($"IPoolableProps, element is not Node2D.");
    }
    public void setElementPoolSleep()
    {
        if (this is Node2D elementPool)
        {
            elementPool.GlobalPosition = new Vector2(0, 9000);  // safe spot (out of map).
            elementPool.Scale = Vector2.One;  // reset scale.
            elementPool.setProcessModeDeferred(Node.ProcessModeEnum.Disabled);
        }
        else
            GD.PrintErr($"IPoolableProps, element is not Node2D.");
    }
}

/* --- demo ---

// demo of using pool.
public class A : IPoolableProps
{

    public static PoolProps<A> pool = new PoolProps<A>("fist", poolSize:10);

}

// instatiate.
A instance = A.pool.getNextElement()
instance.GlobalPosition = new Vector2(200, 200);

...

// free.
instance.setElementPoolSleep();

*/