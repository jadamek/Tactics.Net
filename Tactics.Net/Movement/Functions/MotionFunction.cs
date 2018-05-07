using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Tactics.Net.Movement
{
    public abstract partial class Mobility
    {
        //====================================================================================================================
        // ** Function-of-Motion (Abstract)
        //====================================================================================================================
        // Implements movement over time as p = f(t), where p is the 3-D isometric coordinate of some object, and
        // f(t) is a function of relative time
        //====================================================================================================================
        protected abstract class MotionFunction
        {
            //----------------------------------------------------------------------------------------------------------------
            // - Motion Function Constructor
            //----------------------------------------------------------------------------------------------------------------
            public MotionFunction(Vector3f start, Vector3f destination)
            {
                Start = start;
                Destination = destination;
            }

            //----------------------------------------------------------------------------------------------------------------
            // - Compute Position At Time t (With Bounds)
            //----------------------------------------------------------------------------------------------------------------
            // Computes the position of some object at time t, where t ranges from [0 ... 1]
            //----------------------------------------------------------------------------------------------------------------
            public Vector3f At(float t)
            {
                // At t <= 0, the object is at its start position
                if (t <= 0)
                {
                    return Start;
                }
                // At t = 1, the object is at its destination
                else if (t >= 1)
                {
                    return Destination;
                }
                // At t = (0, 1), the position of the object is determined by an extended method
                else
                {
                    return Compute(t);
                }
            }

            //----------------------------------------------------------------------------------------------------------------
            // - Compute Position At Time t (Without Bounds, Abstract)
            //----------------------------------------------------------------------------------------------------------------
            protected abstract Vector3f Compute(float t);

            //----------------------------------------------------------------------------------------------------------------
            // - Compute Path Length (Abstract)
            //----------------------------------------------------------------------------------------------------------------
            public abstract float Length();

            // Members - private
            protected Vector3f Start { get; set; }
            protected Vector3f Destination { get; set; }
        }
    }
}
