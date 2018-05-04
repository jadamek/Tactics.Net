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
            GoTo(new Vector3f(destination.X, destination.Y, Target.Environment.Height(destination.X, destination.Y)));
        }

        //----------------------------------------------------------------------------------------------------------------
        // - Move Along A Set Path (Implementation)
        //----------------------------------------------------------------------------------------------------------------
        public override void Move(LinkedList<Vector2f> path)
        {
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
    }
}
