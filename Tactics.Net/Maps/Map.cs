using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using Tactics.Net.Extensions;
using Tactics.Net.Isogeometry;
using Tactics.Net.Actors;

namespace Tactics.Net.Maps
{
    //========================================================================================================================
    // ** Isometric Tilemap
    //========================================================================================================================
    // Represents an isometric tilemap, consisting of a 2-D jagged array of 3-D isometric map objects
    //========================================================================================================================
    public class Map : Disposable, Drawable
    {
        //--------------------------------------------------------------------------------------------------------------------
        // - Isometric Map Constructor
        //--------------------------------------------------------------------------------------------------------------------
        public Map(int width = 10, int length = 10)
        {
            Width = Math.Max(1, width);
            Length = Math.Max(1, length);
            Tiles = new List<Tile>[Width, Length];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Length; y++)
                {
                    Tiles[x, y] = new List<Tile>();
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Dispose (Override)
        //--------------------------------------------------------------------------------------------------------------------
        public override void Dispose()
        {
            foreach (List<Tile> row in Tiles)
            {
                foreach (Tile tile in row)
                {
                    tile.Dispose();
                }
            }
            base.Dispose();
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Draw Tilemap (Implementation)
        //--------------------------------------------------------------------------------------------------------------------
        public void Draw(RenderTarget target, RenderStates states)
        {
            ObjectBuffer.Draw(target, states);
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Search & Retrieve Tile At [X,Y,Z]
        //--------------------------------------------------------------------------------------------------------------------
        public Tile At(int x, int y, float z = float.MaxValue)
        {
            Tile retrieved = null;

            // Confirm the requested position is within the bounds of the map
            if (x >= 0 && x < Width && y >= 0 && y < Length)
            {
                if (Tiles[x, y].Any())
                {
                    // retrieve the tile whose z -> height region contains z, or the top-most tile
                    int i = 0;
                    while (z > Tiles[x, y][i].Position.Z + Tiles[x, y][i].Height() && i < Tiles[x, y].Count - 1) { i++; }
                    retrieved = Tiles[x, y][i];
                }
            }

            return retrieved;
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Place Tile At [X,Y]
        //--------------------------------------------------------------------------------------------------------------------
        // Places a tile on top of the top-most tile already located ay (X, Y), if one is present
        //--------------------------------------------------------------------------------------------------------------------
        public bool Place(Tile tile, int x, int y)
        {
            // Confirm the requested position is within the bounds of the map
            if (x >= 0 && x < Width && y >= 0 && y < Length)
            {
                float z = 0;

                // Find the top-most tile at [X, Y] ...
                if (Tiles[x, y].Any())
                {
                    Tile top = Tiles[x, y].Last();

                    // Set the incoming tile's Z-coodinate to the Z + Height of the top-most tile
                    z = top.Position.Z + top.Height();

                    // Acquire the top-most tile's occupant, if present, which should be an non-tile; then, set the top-most
                    // tile's occupant to this new tile
                    tile.Occupant = top.Occupant;
                    top.Occupant = tile;

                    // If this tile did gain an occupant, raise it's Z-coordinate appropriately
                    if (tile.Occupant != null)
                    {
                        tile.Occupant.Position = new Vector3f(tile.Occupant.Position.X, tile.Occupant.Position.Y, tile.Occupant.Position.Z + tile.Height());
                    }
                }

                tile.Position = new Vector3f(x, y, z);
                Tiles[x, y].Add(tile);
                ObjectBuffer.Add(tile);

                // Report a successful placement
                return true;
            }
            return false;
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Map Height At (X, Y)
        //--------------------------------------------------------------------------------------------------------------------
        public float Height(float x, float y)
        {
            float height = -1;

            // non-integral x and y signify a partial position: off-center from the actual (X, Y) position on the map
            int X = (int)Math.Round(x);
            int Y = (int)Math.Round(y);

            // If a valid tile is located at X, Y
            if (At(X, Y) is Tile tile)
            {
                // Get the height of the top-most tile at the position relative to the center of the tile
                height = tile.Position.Z + tile.Height(new Vector2f(x - X, y - Y));
            }

            return height;
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Spawn Actor Onto Map
        //--------------------------------------------------------------------------------------------------------------------
        public bool Join(Actor actor, int x, int y)
        {
            // Cannot join an occupied space
            if(!Occupied(x, y))
            {
                ObjectBuffer.Add(actor);
                actor.Environment = this;
                actor.Position = new Vector3f(x, y, Height(x, y));
            }

            return false;
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Remove Actor From Map
        //--------------------------------------------------------------------------------------------------------------------
        public void Leave(Actor actor)
        {
            ObjectBuffer.Remove(actor);
            actor.Environment = null;
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Map Space Is Occupied
        //--------------------------------------------------------------------------------------------------------------------
        public bool Occupied(int x, int y)
        {
            // Confirm the requested position is within the bounds of the map and that an object is currently present there
            if (At(x, y) is Tile tile)
            {
                return tile.Occupant != null;
            }
            return false;
        }

        // Members
        public int Width { get; }
        public int Length { get; }
        public List<Tile>[,] Tiles { get; }

        // Members - private
        private IsometricBuffer ObjectBuffer { get; } = new IsometricBuffer();
    }
}
