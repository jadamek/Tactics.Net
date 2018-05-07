using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using Tactics.Net.Movement;

namespace Tactics.Net.Movement
{
    public partial class Mobility
    {
        //====================================================================================================================
        // ** Jumping Arc
        //====================================================================================================================
        // Implements a parabolic curve which represents an object jumping from start to destination
        //====================================================================================================================
        protected class JumpArc : MotionFunction
        {
            //----------------------------------------------------------------------------------------------------------------
            // - Jump Arc Constructor
            //----------------------------------------------------------------------------------------------------------------
            public JumpArc(Vector3f start, Vector3f destination) : base(start, destination)
            {
            }

            //----------------------------------------------------------------------------------------------------------------
            // - Compute Position In Curve At t (Override)
            //----------------------------------------------------------------------------------------------------------------
            protected override Vector3f Compute(float t)
            {
                Vector3f result = new Vector3f(
                    Start.X + (Destination.X - Start.X) * t,
                    Start.Y + (Destination.Y - Start.Y) * t,
                    Start.Z + (Destination.Z - Start.Z) * t
                    );
                return result;
            }

            //----------------------------------------------------------------------------------------------------------------
            // - Compute Path Length (Override)
            //----------------------------------------------------------------------------------------------------------------
            public override float Length()
            {
                // The "length" of the curve is the X-Y plane distance between start and destination; the vertical (z)
                // component of the curve does not affect this
                return (float)Math.Sqrt(Math.Pow(Destination.X - Start.X, 2) + Math.Pow(Destination.Y - Start.Y, 2));
            }
        }
    }
}
