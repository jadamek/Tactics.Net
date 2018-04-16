using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using Tactics.Net.Animation;
using System.Collections;

namespace Tactics.Net.Sprites
{
    //========================================================================================================================
    // ** Sequence-Animated Spritesheet
    //========================================================================================================================
    public class SpriteAnimated : Spritesheet, IAnimated
    {
        //--------------------------------------------------------------------------------------------------------------------
        // - Animated Spritesheet Constructor
        //--------------------------------------------------------------------------------------------------------------------
        public SpriteAnimated(Texture texture, uint width = 0, uint height = 0, uint pages = 1, float framerate = Animations.DEFAULT_FRAMERATE )
            : base(texture, width, height, pages)
        {
            // The initial sequence is 0 .. paged-index limit
            Sequence = Enumerable.Range(0, (int)Indices).Select(number => (uint)Math.Max(0, number)).ToList();

            // Setup Animation Cycle: increment current sheet index, loop if necessary
            AnimationCycle = new Animator(framerate);
            AnimationCycle.Step += (s, e) => Step();            
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Dispose (Override)
        //--------------------------------------------------------------------------------------------------------------------
        public new void Dispose()
        {
            AnimationCycle.Dispose();
            base.Dispose();
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Play Animation
        //--------------------------------------------------------------------------------------------------------------------
        public void Play(bool loop = false)
        {
            if(sequence_.Any())
            {
                playing_ = true;
                Looping = loop;
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Stop Playing Animation
        //--------------------------------------------------------------------------------------------------------------------
        public void Stop()
        {
            if(Sequence.Any())
            {
                // Reset cursor position on Stop
                Cursor.Reset();
                Cursor.MoveNext();
                Index = (uint)Cursor.Current;
                playing_ = false;
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Step Through Animation
        //--------------------------------------------------------------------------------------------------------------------
        protected void Step()
        {
            if (playing_)
            {
                // Incremement to the index in the sheet
                if (!Cursor.MoveNext())
                {
                    // Return to the first index in the active sequence when the end of the animation has been reached
                    Cursor.Reset();
                    Cursor.MoveNext();

                    // If looping, continue playing
                    playing_ = Looping;
                }
                Index = (uint)Cursor.Current;
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Active Sequence (Property)
        //--------------------------------------------------------------------------------------------------------------------
        protected List<uint> sequence_;
        public List<uint> Sequence {
            get { return sequence_; }
            set {
                if (value.Any())
                {
                    sequence_ = value;
                    Cursor = value.GetEnumerator();
                    Stop();
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Sequence Cursor (Property)
        //--------------------------------------------------------------------------------------------------------------------
        protected IEnumerator Cursor { get; set; }

        // Members
        public bool Paused { get { return AnimationCycle.Frozen; } set { AnimationCycle.Frozen = value; } }
        public bool Looping { get; set; }

        private bool playing_;
        public bool Playing { get { return playing_; } }

        // Members - private
        protected Animator AnimationCycle { get; }
    }
}
