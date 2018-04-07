using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace Tactics.Net.Sprites
{
    //========================================================================================================================
    // ** Directed Spritesheet
    //========================================================================================================================
    // Extends the spritesheet to include directionality
    //========================================================================================================================
    public class SpriteDirected : Spritesheet
    {
        //--------------------------------------------------------------------------------------------------------------------
        // - Directed Spritesheet Constructor
        //--------------------------------------------------------------------------------------------------------------------
        public SpriteDirected(Texture texture, uint width = 0, uint height = 0, uint directions = 4) : base(texture, width, height)
        {
            Directions = directions;
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Update Sprite Frame By Index & Direction (Override)
        //--------------------------------------------------------------------------------------------------------------------
        protected override void UpdateFrame()
        {
            if (Texture != null)
            {
                // Directions are effectively a group of frame rows (vertical)
                uint effectiveIndex = Index + Direction * IndexLimit;

                // Frames are indexed left-to-right, then top-to-bottom (reading order) within their direction
                TextureRect = new IntRect(
                    (int)(Width * (effectiveIndex % (Texture.Size.X / Width))),
                    (int)(Height * (effectiveIndex / (Texture.Size.X / Width))),
                    (int)Width,
                    (int)Height
                    );
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Update Number of Frames (Override)
        //--------------------------------------------------------------------------------------------------------------------
        protected override void UpdateIndexLimit()
        {
            if (Texture != null)
            {
                IndexLimit = Texture.Size.X / Width * Texture.Size.Y / Height / Directions;
            }
        }

        // Properties

        //--------------------------------------------------------------------------------------------------------------------
        // - Sheet Direction (Property)
        //--------------------------------------------------------------------------------------------------------------------
        private uint direction_;
        public uint Direction
        {
            get { return direction_; }
            set
            {
                direction_ = Math.Min(value, Directions - 1);
                UpdateFrame();
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Possible Directions (Property)
        //--------------------------------------------------------------------------------------------------------------------
        private uint directions_ = 1;
        public uint Directions
        {
            get { return directions_; }
            set
            {
                if (directions_ != value && value > 0 && value <= Texture.Size.X * Texture.Size.Y)
                {
                    // Need to update dependent properties when the texture is changed: frame dimensions and index limits
                    directions_ = value;
                    Width = Width;
                    Height = Height;
                    Index = Index;
                }
            }
        }

        // Members
        protected override uint HeightLimit { get { return Texture.Size.Y / Directions; } }
    }
}
