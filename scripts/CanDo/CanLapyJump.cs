using System;
using Godot;

public class CanLapyJump : CanJump
{
    private Vector2 posA, posB, posC, posD;
    // B---C  // pos for bezierLerp jump.
    // |   |
    // A   D
    private bool isJumping = false;
    public bool isJump
    {
        get => isJumping;
    }
    private float jumpLength
    {
        get => jumpStrength;
    }
    private const float ratioUpJump = 0.5f;
    private float heightJump
    {
        get => jumpLength * ratioUpJump;
    }
    private float totalTimeJump = 0f;
    private float speedJump;
    private float timeToJump
    {
        get => (jumpLength / 425f) * speedJump;
    }


    public CanLapyJump(CharacterBody2D owner, float jumpLength, float speedJump = 0.3f) : base(owner, jumpLength)
    {
        this.speedJump = speedJump;
    }

    public override Vector2 jump(Vector2 velocity, Vector2? directionJump = null)
    {
        if (isJumping)
            return velocity;

        // make jump.
        makeJump(velocity, directionJump ?? Vector2.Right);

        return velocity;
    }

    private void makeJump(Vector2 posStart, Vector2 directionJump)
    {
        isJumping = true;
        totalTimeJump = 0f;

        posA = posStart;
        posD = posStart + (jumpLength * directionJump);
        posB = posA + (Vector2.Up * heightJump);
        posC = posD + (Vector2.Up * heightJump);
    }

    public override Vector2 updateJump(Vector2 velocity, float delta)
    {
        totalTimeJump += delta;
        float i = totalTimeJump / timeToJump;
        if (i >= 1f)
        {
            endJump();
            return posD;
        }
        return posA.bezierLerp(i, posB, posC, posD);
    }

    private void endJump()
    {
        isJumping = false;
    }
}