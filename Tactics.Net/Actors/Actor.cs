using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using Tactics.Net.Isogeometry;
using Tactics.Net.Sprites;
using Tactics.Net.Movement;

namespace Tactics.Net.Actors
{
    //========================================================================================================================
    // ** Scene Actor
    //========================================================================================================================
    // A general actor, which ranges from playable characters and NPC livestock to complex skill effects and composite objects
    //========================================================================================================================
    public class Actor : IsometricObject
    {
        //--------------------------------------------------------------------------------------------------------------------
        // - Draw Actor (Override)
        //--------------------------------------------------------------------------------------------------------------------
        public override void Draw(RenderTarget target, RenderStates states)
        {
            Sprite?.Draw(target, states);
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Compute Global Bounding Rectangle (Override)
        //--------------------------------------------------------------------------------------------------------------------
        public override FloatRect GetGlobalBounds()
        {
            return Sprite?.GetGlobalBounds() ?? new FloatRect();
        }

        // Members
        public ISprite Sprite { get; set; }
        public Mobility Locomotion { get; set; }
    }
}
