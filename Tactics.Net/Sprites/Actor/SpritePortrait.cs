using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;

namespace Tactics.Net.Sprites.Actor
{
    //========================================================================================================================
    // ** Actor Portrait Sprite
    //========================================================================================================================
    class SpritePortrait : Spritesheet
    {
        //--------------------------------------------------------------------------------------------------------------------
        // - Portrait Sprite Constructor
        //--------------------------------------------------------------------------------------------------------------------
        public SpritePortrait(Texture texture) : base(texture, 64, 104)
        {
            // Fill all basic moods with the first frame index
            EmotionIndex = Enumerable.Repeat((uint)0, Enum.GetValues(typeof(Emotions)).Length).ToArray();
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Set Emotion
        //--------------------------------------------------------------------------------------------------------------------
        public void SetEmotionIndex(Emotions emotion, uint index)
        {
            // If the current mood is being replaced, replace it actively
            if (Emotion == emotion) Index = index;
            EmotionIndex[(int)emotion] = index;
        }

        // Properties

        //--------------------------------------------------------------------------------------------------------------------
        // - Actor Emotion (Portrait Index)
        //--------------------------------------------------------------------------------------------------------------------
        private Emotions emotion_ = Emotions.Normal;
        public Emotions Emotion
        {
            get { return emotion_; }
            set { emotion_ = value; Index = EmotionIndex[(int)value]; }
        }

        // Members - private
        protected uint[] EmotionIndex { get; }

        //====================================================================================================================
        // * Portrait Sprite Emotions (Named Indices)
        //====================================================================================================================
        public enum Emotions
        {
            Normal,
        };
    }
}
