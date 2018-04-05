using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Tactics.Net.Sprites
{
    //========================================================================================================================
    // ** Sprite Interface
    //========================================================================================================================
    // Combines Drawable, IDisposable and the interface of the SFML.Transformable class into a single interface
    //========================================================================================================================
    public interface ISprite : Drawable, IDisposable
    {
        // Methods
        FloatRect GetGlobalBounds();

        // Members
        Color Color { get; set; }
        Transform InverseTransform { get; }
        Transform Transform { get; }
        float Rotation { get; set; }
        Vector2f Origin { get; set; }
        Vector2f Position { get; set; }
        Vector2f Scale { get; set; }
    }
}
