using Godot;

public partial class PlayerSence : Area2D
{
    private Player playerScript;

    public override void _Ready()
    {
        playerScript = GetNode<Player>("..");

        BodyEntered += (Node2D body) =>
        {
            // killed by Sea.
            if (body is TileMapLayer tileMapLayer)
            {
                playerScript.death();
                return;
            }

            // get layer collide.
            //if (body is CollisionObject2D bodyCollide)
            //{
            //    uint collisionLayer = bodyCollide.CollisionLayer;
            //    switch (collisionLayer)
            //    {
            //        default:
            //            GD.Print($"collide Player sence -> layer : {collisionLayer}");
            //            break;
            //    }
            //}

        };
    }

}
