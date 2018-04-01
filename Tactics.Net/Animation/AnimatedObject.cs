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
    // ** Animated Object (Abstract)
    //========================================================================================================================
    // Represents an abstract object animated in a relative time-space; calls abstract step() per frame based on a local
    // framerate progressed by update(elapsed)
    //========================================================================================================================
    public abstract class AnimatedObject : Disposable
    {
        //--------------------------------------------------------------------------------------------------------------------
        // - Animated Object Constructor
        //--------------------------------------------------------------------------------------------------------------------
        public AnimatedObject(float framerate = 0)
        {
            Framerate = framerate > 0 ? framerate : Animations.DEFAULT_FRAMERATE;

            // Self-register
            Animations.Add(this);
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Execute Frame (Abstract)
        //--------------------------------------------------------------------------------------------------------------------
        protected abstract void Step();

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
                while(Clock-- >= 1)
                {
                    Step();
                }
            }
        }

        // Members
        protected float framerate_;
        public float Framerate { get { return framerate_; } set { if (value > 0) framerate_ = value; } }
        public bool Frozen { get; set; }
        protected float Clock { get; set; }
    }
}
