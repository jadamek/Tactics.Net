using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using Tactics.Net.Extensions;

namespace Tactics.Net.Animation
{
    //========================================================================================================================
    // ** Animation Handler
    //========================================================================================================================
    // Executes a per-frame Step event in a relative framerate progressed by update(elapsed)
    //========================================================================================================================
    public class Animator : Disposable
    {
        //--------------------------------------------------------------------------------------------------------------------
        // - Animated Object Constructor
        //--------------------------------------------------------------------------------------------------------------------
        public Animator(float framerate = 0)
        {
            Framerate = framerate > 0 ? framerate : Animations.DEFAULT_FRAMERATE;

            // Self-register
            Animations.Add(this);
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Execute Frame Event
        //--------------------------------------------------------------------------------------------------------------------
        protected void OnStep()
        {
            Step?.Invoke(this, EventArgs.Empty);
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Update Animation In Local Time
        //--------------------------------------------------------------------------------------------------------------------
        public void Update(float elapsed)
        {
            // If not frozen, update the current clock according to the elapsed time (in seconds) since last update
            if(!Frozen)
            {
                Clock += elapsed * Framerate;

                // Step any number of times for every 1 / FRAMERATE amount of seconds that have passed
                while(Clock >= 1)
                {
                    OnStep();
                    Clock--;
                }
            }
        }

        // Members
        public event EventHandler<EventArgs> Step;
        public float Framerate { get { return framerate_; } set { if (value > 0) framerate_ = value; } }
        public bool Frozen { get; set; }

        // Members - private
        protected float framerate_;
        protected float Clock { get; set; }
    }
}
