using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using Tactics.Net.Isogeometry;
using Tactics.Net.Animation;
using Tactics.Net.Maps;

namespace Tactics.Net.Movement
{    
    //========================================================================================================================
    // ** Mobility Handler (Abstract)
    //========================================================================================================================
    // Handles the mobility of an isometric object in relative time, changing its position gradually; optionally, this gradual
    // movement may be "grounded": z-position is restricted to a height map
    //========================================================================================================================
    public abstract class Mobility
    {
        //--------------------------------------------------------------------------------------------------------------------
        // - Mobility Constructor
        //--------------------------------------------------------------------------------------------------------------------
        public Mobility(IsometricObject target, Map ground = null)
        {
            Target = target;
            Ground = ground;

            // If the object is "grounded", set its initial z-position to the height of the grounding map at (0,0)
            if (Grounded)
            {
                target.Position = new Vector3f(0, 0, Ground.Height(0, 0));
            }

            // Animate the extensible Step method, which defines how objects move under this mobility method
            MotionCycle.Step += (s, e) => Step();
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Move To (X, Y) (Abstract)
        //--------------------------------------------------------------------------------------------------------------------
        public abstract void Move(Vector2f destination);

        //--------------------------------------------------------------------------------------------------------------------
        // - Compute Reachable Spaces (Abstract)
        //--------------------------------------------------------------------------------------------------------------------
        public abstract List<Vector2f> Reach();

        //--------------------------------------------------------------------------------------------------------------------
        // - Compute Frames To Arive at (X, Y, Z)
        //--------------------------------------------------------------------------------------------------------------------
        protected int ArrivalTime(Vector3f destination)
        {
            // Compute Euclidean distance to (X, Y) - change in Z does not affect arrival time
            double distance = Math.Sqrt(Math.Pow(destination.X - Target.Position.X, 2) + Math.Pow(destination.Y - Target.Position.Y, 2));
            return (int)Math.Ceiling(distance * MotionCycle.Framerate / Speed);
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Go To (X, Y, Z) Linearly
        //--------------------------------------------------------------------------------------------------------------------
        protected void GoTo(Vector3f position)
        {
            Destination = position;
            FramesToArrive = ArrivalTime(position);
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Update Positioning & Pathing
        //--------------------------------------------------------------------------------------------------------------------
        protected virtual void Step()
        {
            if(FramesToArrive > 0)
            {
                // Gradual progression = (Destination - Current) / Arrival--
                float x = Target.Position.X + (Destination.X - Target.Position.X) / FramesToArrive;
                float y = Target.Position.Y + (Destination.Y - Target.Position.Y) / FramesToArrive;

                // If "grounded" and the map has a valid height at the new x, y, set the z-coordinate to that height
                // Otherwise, "levitate" (retain Z)
                float z = Math.Max(0, Ground?.Height(x, y) ?? Target.Position.Z);

                Target.Position = new Vector3f(x, y, z);
                FramesToArrive--;
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Speed In Coordinates / Sec (Property)
        //--------------------------------------------------------------------------------------------------------------------
        private float speed_ = 1;
        public float Speed
        {
            get { return speed_; }
            set { if (value > 0) speed_ = value; }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // Target In-Motion (Property)
        //--------------------------------------------------------------------------------------------------------------------
        public virtual bool Moving {
            get { return FramesToArrive > 0; }
            set
            {
                // Setting this flag will halt/resume the target's current motion
                if (Moving != value)
                {
                    FramesToArrive = (value && Destination != null ? ArrivalTime(Destination) : 0);
                }
            }
        }

        // Members - private
        protected IsometricObject Target { get; set; }
        protected Map Ground { get; set; }
        protected int FramesToArrive { get; set; }
        protected Vector3f Destination { get; set; }
        protected virtual bool Grounded { get { return Ground != null; } }
        protected Animator MotionCycle { get; } = new Animator();
    }    
}