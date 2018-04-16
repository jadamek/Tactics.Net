using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace Tactics.Net.Sprites
{
    //========================================================================================================================
    // ** Indexed Spritesheet
    //========================================================================================================================
    public class Spritesheet : Sprite
    {
        //--------------------------------------------------------------------------------------------------------------------
        // - Spritesheet Constructor
        //--------------------------------------------------------------------------------------------------------------------
        public Spritesheet(Texture texture, uint width = 0, uint height = 0, uint pages = 1) : base(texture)
        {
            // Default frame dimensions are the entire source dimensions
            if(Texture != null)
            {
                Width = Texture.Size.X;
                Height = Texture.Size.Y;
            }

            Width = width;
            Height = height;
            Index = 0;
            Pages = pages;
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Update Sprite Frame By Index
        //--------------------------------------------------------------------------------------------------------------------
        protected virtual void UpdateFrame()
        {
            if(Texture != null)
            {

                // Directions are effectively a group of frame rows (vertical)
                uint effectiveIndex = Index + Page * IndexLimit;

                // Frames are indexed left-to-right, then top-to-bottom (reading order) within their page
                TextureRect = new IntRect(
                    (int)(Width * (effectiveIndex % (Texture.Size.X / Width))),
                    (int)(Height * (effectiveIndex / (Texture.Size.X / Width))),
                    (int)Width,
                    (int)Height
                    );
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Update Number of Frames
        //--------------------------------------------------------------------------------------------------------------------
        // The limits of the total number of indices in the sheet are directly determined by the source texture of the and the
        // current frame dimensions; these values changes seldomly and the computation may vary depending on the definition of
        // a frame in the sheet owing to a stored extensible computation
        //--------------------------------------------------------------------------------------------------------------------
        protected void UpdateIndexLimit()
        {
            if(Texture != null)
            {
                IndexLimit = Texture.Size.X / Width * Texture.Size.Y / Height / Pages;
            }
        }

        // Properties

        //--------------------------------------------------------------------------------------------------------------------
        // - Sheet Frame Index (Property)
        //--------------------------------------------------------------------------------------------------------------------
        private uint index_;
        public uint Index
        {
            get { return index_; }
            set
            {
                index_ = Math.Min(value, IndexLimit - 1);
                UpdateFrame();
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Sheet Frame Width (Property)
        //--------------------------------------------------------------------------------------------------------------------
        private uint width_ = 1;
        public uint Width
        {
            get { return width_; }
            set
            {
                if(value > 0)
                {
                    width_ = Math.Min(value, WidthLimit);
                    UpdateIndexLimit();
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Sheet Height Width (Property)
        //--------------------------------------------------------------------------------------------------------------------
        private uint height_ = 1;
        public uint Height
        {
            get { return height_; }
            set
            {
                if (value > 0)
                {
                    height_ = Math.Min(value, HeightLimit);
                    UpdateIndexLimit();
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Sheet Source Texture (Override)
        //--------------------------------------------------------------------------------------------------------------------
        public new Texture Texture
        {
            get { return base.Texture; }
            set
            {
                // Need to update dependent properties when the texture is changed: frame dimensions and index limits
                if(Texture != value)
                {
                    base.Texture = value;
                    Width = Width;
                    Height = Height;
                    Index = Index;
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Sheet Page (Property)
        //--------------------------------------------------------------------------------------------------------------------
        private uint page_;
        public uint Page
        {
            get { return page_; }
            set
            {
                page_ = Math.Min(value, Pages - 1);
                UpdateFrame();
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Page Limit (Property)
        //--------------------------------------------------------------------------------------------------------------------
        private uint pages_ = 1;
        public uint Pages
        {
            get { return pages_; }
            set
            {
                if (pages_ != value && value > 0 && value <= Texture.Size.X * Texture.Size.Y)
                {
                    // Need to update dependent properties when the texture is changed: frame dimensions and index limits
                    pages_ = value;
                    Width = Width;
                    Height = Height;
                    Index = Index;
                }
            }
        }

        // Members
        public uint Indices { get { return IndexLimit; } }

        // Members - private
        protected uint IndexLimit { get; set; }
        protected uint WidthLimit { get { return Texture.Size.X; } }
        protected uint HeightLimit { get { return Texture.Size.Y / Pages; } }
    }
}
