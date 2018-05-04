using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace Tactics.Net.Sprites.Battle
{
    //========================================================================================================================
    // ** Damage Counter Display
    //========================================================================================================================
    public class SpriteDamage : Transformable, Drawable
    {
        //--------------------------------------------------------------------------------------------------------------------
        // - Damage Counter Constructor
        //--------------------------------------------------------------------------------------------------------------------
        public SpriteDamage(int amount)
        {
            Amount = new Text(amount.ToString(), Courier, 24)
            {
                Style = Text.Styles.Bold,
                Color = Color.Red,
            };
            Amount.Origin = new Vector2f(Amount.GetLocalBounds().Width / 2, Amount.GetLocalBounds().Height / 2);
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Draw (Override)
        //--------------------------------------------------------------------------------------------------------------------
        public void Draw(RenderTarget target, RenderStates states)
        {
            Amount.Draw(target, states);
        }

        // Members - private
        private Text Amount { get; set; }
        private Font Courier { get; } = new Font("Resources/Fonts/cour.ttf");
    }
}
