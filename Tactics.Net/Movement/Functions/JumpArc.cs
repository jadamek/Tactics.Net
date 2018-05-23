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
                Distance = Math.Sqrt(Math.Pow(Destination.X - Start.X, 2) + Math.Pow(Destination.Y - Start.Y, 2));
                Floor = Math.Min(Start.Z, Destination.Z);
                // Height is the change in height from the lower of the z-coordinate of the Start or Destination to the higher
                Height = Math.Max(Start.Z, Destination.Z) - Floor;
                // The curve's summit (highest point, c) solves the following boundary conditions of curve z = f(t):
                // 1) f(c) = Height + Offset
                // 2) f(0) = 0
                // 3) f(2c) = 0
                // 4) f(Distance) = Height
                // 5) Distance > c
                Summit = (Distance * (Height + Offset) - Math.Sqrt(Offset * (Height + Offset))) / Height;
            }

            //----------------------------------------------------------------------------------------------------------------
            // - Compute Position In Curve At t (Override)
            //----------------------------------------------------------------------------------------------------------------
            protected override Vector3f Compute(float t)
            {
                // Time relative to the total distance being traversed; also flips the curve downward if Start > Floor
                double dt = (Start.Z > Floor ? Distance - t * Distance : t * Distance);
                
                // Arc equation to compute the z position (relative to the "Floor" z-coordinate) as a function of time (0, 1)
                double z = -(Height + Offset) / (Summit * Summit) * Math.Pow((dt * Distance - Summit), 2) + Height + Offset;

                Vector3f result = new Vector3f(
                    Start.X + (Destination.X - Start.X) * t,
                    Start.Y + (Destination.Y - Start.Y) * t,
                    (float)(z + Floor)
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
                return (float)Distance;
            }

            // Members - private
            private double Distance { get; set; }
            private double Summit { get; set; }
            private double Height { get; set; }
            private double Floor { get; set; }

            // Static member - offset from jump height to destination height
            private static double Offset { get; set; } = 1;
        }
    }
}
