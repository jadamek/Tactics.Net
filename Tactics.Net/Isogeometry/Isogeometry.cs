using SFML.System;

namespace Tactics.Net.Isogeometry
{
    //========================================================================================================================
    // ** Isogeometry Global Class
    //========================================================================================================================
    public class Isogeometry
    {
        //--------------------------------------------------------------------------------------------------------------------
        // - Convert Isometric Position to Global (Screen) Coordinate
        //--------------------------------------------------------------------------------------------------------------------
        public static Vector2f IsoToGlobal(Vector3f position)
        {
            float x = 0.5f * Scale.X * (position.X - position.Y);
            float y = 0.5f * Scale.Y * (position.X + position.Y) - Scale.Z * position.Z;

            return new Vector2f(x, y);
        }

        // Properties
        public static Vector3f Scale { get; } = new Vector3f(32.0f, 24.0f, 8.0f);
    }
}
