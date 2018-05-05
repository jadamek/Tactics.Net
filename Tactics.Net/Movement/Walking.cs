using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;
using Tactics.Net.Actors;

namespace Tactics.Net.Movement
{
    //========================================================================================================================
    // ** Walking Movement Handler
    //========================================================================================================================
    // Implements mobility by foot, whereby the pedestrian walks over tiles which are near level, hops up or down slight
    // inclines, and leaps up or down bordering tiles with major height differences; this mobility is available only to Actors
    //========================================================================================================================
    public class Walking : Mobility
    {
        //----------------------------------------------------------------------------------------------------------------
        // - Walking Movement Handler Constructor
        //----------------------------------------------------------------------------------------------------------------
        public Walking(Actor target) : base(target)
        {
        }

        //----------------------------------------------------------------------------------------------------------------
        // - Move To (X, Y) (Implementation)
        //----------------------------------------------------------------------------------------------------------------
        public override void Move(Vector2f destination)
        {
            // Move along the shortest path to the destination
            Move(Algorithms.Djikstra.ShortestPath(
                new Vector2f(Target.Position.X, Target.Position.Y),
                destination,
                MoveScore,
                (current, next) =>
                {
                    // If the target is present within a height map environment, valid steps must be destined for a valid
                    // tile, and the difference in height between the current and next tile must not exceed the JumpScore
                    if (Target.Environment != null)
                    {
                        float z = Target.Environment.Height(current.X, current.Y);
                        // Will result in -1 if the next tile is invalid
                        float dz = Target.Environment.Height(next.X, next.Y);

                        return dz >= 0 && Math.Abs(z - dz) <= JumpScore;
                    }
                    // Otherwise, the path has no real bounds other than length
                    else
                    {
                        return true;
                    }
                }));
        }

        //----------------------------------------------------------------------------------------------------------------
        // - Move Along A Set Path (Implementation)
        //----------------------------------------------------------------------------------------------------------------
        public override void Move(LinkedList<Vector2f> path)
        {
            if (path?.Any() ?? false)
            {
                Path = path;

                // Go to the first step in the path (X, Y, Height) by invoking Arrive at the source position
                Arrived();
            }
        }

        //----------------------------------------------------------------------------------------------------------------
        // - Compute Valid Reach Destinations
        //----------------------------------------------------------------------------------------------------------------
        // Uses a breadth-first traversal of the target's immediate area to compute all possible destinations the target
        // may walk to given their environment, Move and Jump scores
        //----------------------------------------------------------------------------------------------------------------
        public override List<Vector2f> Reach()
        {
            throw new NotImplementedException();
        }

        //----------------------------------------------------------------------------------------------------------------
        // - Current Step Has Been Traversed (Override)
        //----------------------------------------------------------------------------------------------------------------
        protected override void Arrived()
        {
            // Move onto the next step in the path (X, Y, Height), if present
            if (Path.Any())
            {
                float z = Target.Environment?.Height(Path.First.Value.X, Path.First.Value.Y) ?? Target.Position.Z;
                Vector3f next = new Vector3f(Path.First.Value.X, Path.First.Value.Y, z);

                // If the difference in height between the current and next step is ...               
                float dz = Math.Abs(Target.Position.Z - z);
                if (dz <= 1)
                {
                    // Less than 1, just walk normally
                    GoTo(next);
                }
                else if(dz <= 2)
                {
                    // Within 2, hop to the next step
                    Jump(next, true);
                }
                else
                {
                    // More than 2, full jump to then next step
                    Jump(next);
                }

                Path.RemoveFirst();
            }
        }

        //----------------------------------------------------------------------------------------------------------------
        // - Jump To (X, Y)
        //----------------------------------------------------------------------------------------------------------------
        protected void Jump(Vector3f destination, bool hop = false)
        {
            Console.WriteLine("BOING! " + destination.X + " " + destination.Y + (hop ? " BUNNY HOP!" : ""));
            GoTo(destination);
        }

        // Members
        public int MoveScore { get; set; } = 10;
        public int JumpScore { get; set; } = 10;

        // Members - private
        protected LinkedList<Vector2f> Path { get; set; } = new LinkedList<Vector2f>();
    }
}
