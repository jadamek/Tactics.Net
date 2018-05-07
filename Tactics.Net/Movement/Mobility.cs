using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using Tactics.Net.Animation;

namespace Tactics.Net.Movement
{
    //========================================================================================================================
    // ** Mobility Handler (Abstract)
    //========================================================================================================================
    // Handles the movement of an isometric mobile object in relative time, changing its position gradually within the bounds
    // of the object's environment, if present
    //========================================================================================================================
    public abstract partial class Mobility
    {
        //--------------------------------------------------------------------------------------------------------------------
        // - Mobility Constructor
        //--------------------------------------------------------------------------------------------------------------------
        public Mobility(IMobile target)
        {
            Target = target;

            // If the object is "grounded", set its initial z-position to the height of the grounding map at (x,y)
            if (Grounded)
            {
                target.Position = new Vector3f(Target.Position.X, Target.Position.Y, Target.Environment.Height(Target.Position.X, Target.Position.Y));
            }

            // Animate the extensible Step method, which defines how objects move under this mobility method
            MotionCycle.Step += (s, e) => Step();
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Move To (X, Y) (Abstract)
        //--------------------------------------------------------------------------------------------------------------------
        public abstract void Move(Vector2f destination);

        //--------------------------------------------------------------------------------------------------------------------
        // - Move Along A Set Path (Abstract)
        //--------------------------------------------------------------------------------------------------------------------
        public abstract void Move(LinkedList<Vector2f> path);

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
            Function = null;
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Go Along A Function-Based Movement
        //--------------------------------------------------------------------------------------------------------------------
        protected void GoAlong(MotionFunction function)
        {
            if((function?.Length() ?? 0) > 0)
            {
                Function = function;
                FunctionLength = (int)Math.Ceiling(function.Length() * MotionCycle.Framerate / Speed);
                FramesToArrive = FunctionLength;
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Stop Moving
        //--------------------------------------------------------------------------------------------------------------------
        public virtual void Stop()
        {
            FramesToArrive = 0;
            Function = null;
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Update Positioning & Pathing
        //--------------------------------------------------------------------------------------------------------------------
        protected virtual void Step()
        {
            // Implements the basics of the GoTo movement (gradual slide to destination)
            if(FramesToArrive > 0)
            {
                // Gradual progression = (Destination - Current) / Arrival--
                if (Function == null)
                {
                    float x = Target.Position.X + (Destination.X - Target.Position.X) / FramesToArrive;
                    float y = Target.Position.Y + (Destination.Y - Target.Position.Y) / FramesToArrive;
                    float z = 0;

                    // If "grounded" and the map has a valid height at the new x, y, set the z-coordinate to that height
                    // Otherwise, "levitate" (gradual Z to destination)
                    if (Grounded)
                    {
                        z = Math.Max(0, Target.Environment.Height(x, y));
                    }
                    else
                    {
                        z = Target.Position.Z + (Destination.Z - Target.Position.Z) / FramesToArrive;
                    }

                    Target.Position = new Vector3f(x, y, z);
                    FramesToArrive--;
                }
                // Function-based progression
                else
                {
                    Target.Position = Function.At(((float)FunctionLength - --FramesToArrive) / FunctionLength);
                }

                // When the destination has just been reached ...
                if(FramesToArrive == 0)
                {
                    Arrived();
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Current Destination Is Reached
        //--------------------------------------------------------------------------------------------------------------------
        protected virtual void Arrived() { }

        //--------------------------------------------------------------------------------------------------------------------
        // - Speed In Coordinates / Sec (Property)
        //--------------------------------------------------------------------------------------------------------------------
        private float speed_ = 1;
        public float Speed
        {
            get { return speed_; }
            set { if (value > 0) speed_ = value; }
        }

        // Members
        public virtual bool Moving { get { return FramesToArrive > 0; } }

        // Members - private
        protected IMobile Target { get; set; }
        protected int FramesToArrive { get; set; }
        protected Vector3f Destination { get; set; }
        protected MotionFunction Function { get; set; }
        protected int FunctionLength { get; set; }
        protected virtual bool Grounded { get { return Target.Environment != null; } }
        protected Animator MotionCycle { get; } = new Animator();        
    }
}