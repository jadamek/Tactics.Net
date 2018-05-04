using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using Tactics.Net.Isogeometry;
using Tactics.Net.Sprites;
using Tactics.Net.Movement;
using Tactics.Net.Sprites.Actor;
using SFML.System;
using Tactics.Net.Maps;

namespace Tactics.Net.Actors
{
    //========================================================================================================================
    // ** Scene Actor
    //========================================================================================================================
    // A general actor, which ranges from playable characters and NPC livestock to complex skill effects and composite objects
    //========================================================================================================================
    public class Actor : IsometricObject, IMobile
    {
        //--------------------------------------------------------------------------------------------------------------------
        // - Inflict or Recover Damage
        //--------------------------------------------------------------------------------------------------------------------
        public void Damage(int amount)
        {

        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Move To (X,Y)
        //--------------------------------------------------------------------------------------------------------------------
        public virtual void Move(Vector2f position)
        {
            Locomotion?.Move(position);
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Face Specific Position
        //--------------------------------------------------------------------------------------------------------------------
        public void Face(Vector2f focus)
        {
            if (Sprite != null)
            {
                double angle = Math.Atan2(focus.Y - Position.Y, focus.X - Position.X);

                // Add 2 PI (full circle) to the angle, if it's negative
                if (angle < 0)
                {
                    angle = 2 * Math.PI + angle;
                }

                // Set page to the radial angle normalized by page limit
                Sprite.Page = (uint)Math.Round(angle / 2 / Math.PI * Sprite.Pages) % Sprite.Pages;
            }
        }

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
        public SpriteActor Sprite { get; set; }
        public SpritePortrait Portrait { get; set; }
        public Map Environment { get; set; }
        public Mobility Locomotion { get; set; }
    }
}
