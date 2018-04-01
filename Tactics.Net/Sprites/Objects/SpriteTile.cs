using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Tactics.Net.Sprites.Objects
{
    //========================================================================================================================
    // ** Isometric Tile Sprite
    //========================================================================================================================
    // A sprite of a single (continuous or definite) tile column. Texture is expected to fit the following structure:
    // Top-left unit : tile top face (0, 0, width, length)
    // Right half : tile center body (0, length, width, length)
    // Unit under top-left : tile bottom face (width, 0, width, entire length of texture)
    //========================================================================================================================
    public class SpriteTile : Transformable, Drawable, IDisposable
    {
        //--------------------------------------------------------------------------------------------------------------------
        // Tile Sprite Constructor
        //--------------------------------------------------------------------------------------------------------------------
        public SpriteTile(Texture texture, int width = 0, int length = 0, int height = 0, bool continuous = false)
        {
            // Width|Length must be between (0, Texture.Width|Length / 2], or set to Texture.Width|Length / 2
            Width = width <= 0 || width > texture.Size.X / 2 ? (int)texture.Size.X / 2: width;
            Length = length <= 0 || length > texture.Size.Y  / 2? (int)texture.Size.Y / 2: length;
            Height = Math.Max(0, height);
            Continuous = continuous;

            // Repeat 'body' texture for heights exceeding the texture's
            texture.Repeated = true;

            // Setup 'Top' face sprite
            Top = new Sprite(texture)
            {
                Position = new Vector2f(Width / -2, Length / -2 - Height),
                TextureRect = new IntRect(0, 0, Width, Length),
            };

            // Setup 'Body' center column sprite - continuous describes a tile whose center column extends infinitely down
            if(Height > 0 || Continuous)
            {
                Body = new Sprite(texture)
                {
                    Position = new Vector2f(Width / -2, -1 * Height),
                    TextureRect = new IntRect(Width, 0, Width, (Continuous ? (int)Texture.MaximumSize : Height)),
                };
            }

            // Setup 'Bottom' face sprite - created if the tile has a definite height
            if(!Continuous)
            {
                Bottom = new Sprite(texture)
                {
                    Position = new Vector2f(Width / -2, Length / -2),
                    TextureRect = new IntRect(0, Length, Width, Length),
                };
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Set New Height
        //--------------------------------------------------------------------------------------------------------------------
        public void ResetHeight(int height)
        {
            Height = Math.Max(0, height);

            // Adjust the top face appropriately
            Top.Position = new Vector2f(Width / -2, Length / -2 - Height);

            // Elongate and adjust center body to match the new height if a positive height was given, or the height is set
            // as indefinite (continuous)
            if(Height > 0 || Continuous)
            {
                if (Body == null) Body = new Sprite(Top.Texture);

                Body.Position = new Vector2f(Width / -2, -1 * Height);
                Body.TextureRect = new IntRect(Width, 0, Width, (Continuous ? (int)Texture.MaximumSize : Height));
            }
            // Otherwise, if the height has just been set to zero and a center body sprite was present, remove it
            else
            {
                Body?.Dispose();
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Draw (Implementation)
        //--------------------------------------------------------------------------------------------------------------------
        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;

            Bottom?.Draw(target, states);
            Body?.Draw(target, states);
            Top.Draw(target, states);
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Dispose (Implementation)
        //--------------------------------------------------------------------------------------------------------------------
        public new void Dispose()
        {
            Top.Dispose();
            Body?.Dispose();
            Bottom?.Dispose();
            base.Dispose();
        }

        // Members
        protected int Width { get; set; }
        protected int Length { get; set; }
        protected int Height { get; set; }
        protected bool Continuous { get; set; }
        protected Sprite Top { get; }
        protected Sprite Body { get; set; }
        protected Sprite Bottom { get; }
    }
}
