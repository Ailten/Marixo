
using System;

public enum StateLapy
{
    wait_to_jump_walk,
    jump_walk,
    hited,
}

public static class StateLapyStatic
{
    public static float getTimeAnime(this StateLapy stateLapy)
    {
        switch (stateLapy)
        {
            case StateLapy.wait_to_jump_walk:
                return 0.2f;
            case StateLapy.jump_walk:
                return 0.3f;
            case StateLapy.hited:
                return 0f;

            default:
                throw new Exception("StateLapyStatic.getTimeAnime not implemented");
        }
    }
    public static string getSpriteAnime(this StateLapy stateLapy)
    {
        switch (stateLapy)
        {
            case StateLapy.wait_to_jump_walk:
                return "default";
            case StateLapy.jump_walk:
                return "jump";
            case StateLapy.hited:
                return "hit";

            default:
                return "default";
        }
    }
}