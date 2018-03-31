using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using Tactics.Net.Isogeometry;

namespace Tactics.Net.Map
{
    //========================================================================================================================
    // ** Isometric Tile
    //========================================================================================================================
    // Represents a single basic isometric map tile with a float height and single occupant (object directly above this one)
    //========================================================================================================================
    public class Tile : IsometricObject
    {
        public Tile(Sprite sprite = null, float height = 1.0f)
        {
            Sprite = sprite;

            // If the Z-coordinate is changed, appropriately adjust the Z-coordinate of any occupying objects 
            PositionChanged += (s, e) =>
            {
                if(e.NewPosition.Z != e.OldPosition.Z)
                {
                    Occupant.Position = new Vector3f(Occupant.Position.X, Occupant.Position.Y, Occupant.Position.Z + e.NewPosition.Z - e.OldPosition.Z);
                }
            };
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Set Height
        //--------------------------------------------------------------------------------------------------------------------
        // Set the flat height of this tile and appropriately adjust the Z-coordinate of any occupying objects
        //--------------------------------------------------------------------------------------------------------------------
        public virtual void SetHeight(float height)
        {
            if (height_ != height)
            {
                if(Occupant != null)
                {
                    Occupant.Position = new Vector3f(Occupant.Position.X, Occupant.Position.Y, Occupant.Position.Z + height - height_);
                }
                height_ = height;
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Draw Tile (Override)
        //--------------------------------------------------------------------------------------------------------------------
        public override void Draw(RenderTarget target, RenderStates states)
        {
            Sprite?.Draw(target, states);
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Dispose (Extension)
        //--------------------------------------------------------------------------------------------------------------------
        public override void Dispose()
        {
            Sprite?.Dispose();
            base.Dispose();
        }

        // Members
        public IsometricObject Occupant { get; set; }
        protected Sprite Sprite { get; set; }
        protected float height_;
    }
}
