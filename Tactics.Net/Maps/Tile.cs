using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using Tactics.Net.Isogeometry;
using Tactics.Net.Sprites;

namespace Tactics.Net.Maps
{
    //========================================================================================================================
    // ** Isometric Tile
    //========================================================================================================================
    // Represents a single basic isometric map tile with a float height and single occupant (object directly above this one)
    //========================================================================================================================
    public class Tile : IsometricObject
    {
        public Tile(ISprite sprite = null, float height = 1.0f)
        {
            Sprite = sprite;
            height_ = height;

            // If the Z-coordinate is changed, appropriately adjust the Z-coordinate of any occupying objects 
            PositionChanged += (s, e) =>
            {
                if(e.NewPosition.Z != e.OldPosition.Z && Occupant != null)
                {
                    Occupant.Position = new Vector3f(Occupant.Position.X, Occupant.Position.Y, Occupant.Position.Z + e.NewPosition.Z - e.OldPosition.Z);
                }
            };
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Get Height (Override)
        //--------------------------------------------------------------------------------------------------------------------
        // * offset : (x,y) position relative to the center of the x-y axis of object
        //--------------------------------------------------------------------------------------------------------------------
        public override float Height(Vector2f offset)
        {
            return height_;
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

        //--------------------------------------------------------------------------------------------------------------------
        // - Global Bounding Rectangle (Implementation)
        //--------------------------------------------------------------------------------------------------------------------
        public override FloatRect GetGlobalBounds()
        {
            return Sprite?.GetGlobalBounds() ?? new FloatRect();
        }

        // Members
        public IsometricObject Occupant { get; set; }

        // Members - private
        protected ISprite Sprite { get; set; }
        protected float height_;
    }
}
