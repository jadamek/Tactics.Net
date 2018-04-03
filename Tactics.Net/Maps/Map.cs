using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using Tactics.Net.Extensions;

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

            for (int x = 0; x <Width; x++)
            {
                for(int y = 0; y < Length; y++)
                {
                    Tiles[x, y] = new List<Tile>();
                }
            }
            // Actors = new Actor[Width, Length];
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Dispose (Override)
        //--------------------------------------------------------------------------------------------------------------------
        public override void Dispose()
        {
            foreach(List<Tile> row in Tiles)
            {
                foreach(Tile tile in row)
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
            foreach(List<Tile> row in Tiles)
            {
                foreach(Tile tile in row)
                {
                    tile.Draw(target, states);
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Search & Retrieve Tile At [X,Y,Z]
        //--------------------------------------------------------------------------------------------------------------------
        public Tile At(int x, int y, float z = float.MaxValue)
        {
            Tile retrieved = null;

            // Confirm the requested position is within the bounds of the map
            if(x >= 0 && x < Width && y >= 0 && y < Length)
            {
                if(Tiles[x, y].Any())
                {
                    // retrieve the tile whose z -> height region contains z, or the top-most tile
                    int i = 0;
                    while(z > Tiles[x, y][i].Position.Z + Tiles[x, y][i].Height() && i < Tiles[x, y].Count - 1) { i++; }
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
                if(Tiles[x, y].Any())
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

                // Report a successful placement
                return true;
            }
            return false;
        }

        // Members
        public int Width { get; }
        public int Length { get; }
        public List<Tile>[,] Tiles { get; }
        // protected Actor[,] Actors { get; }
    }
}
