using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.System;

namespace Tactics.Net.Algorithms
{
    //========================================================================================================================
    // ** Djikstra's A-Star Algorithm
    //========================================================================================================================
    // Computes the shortest path between a source and destination position using Djikstra's A-Star algorithm within a static
    // functor object
    //========================================================================================================================
    public static class Djikstra
    {
        //--------------------------------------------------------------------------------------------------------------------
        // - Compute Shortest Path
        //--------------------------------------------------------------------------------------------------------------------
        // Uses a valid step predicate to determine if the 'next' (second argument) position can be reached from the 'current'
        // position (first argument), returning the shortest path from source to destination or an empty path if the
        // destination is unreachable
        //--------------------------------------------------------------------------------------------------------------------
        public static LinkedList<Vector2f> ShortestPath(Vector2f source, Vector2f destination,
            int length = Int32.MaxValue, Func<Vector2f, Vector2f, bool> validStep = null)
        {
            // Track all visited positions, eventually creating a directed graph where each position points to the next hop
            // which makes the most progress toward the destination
            BFSToken token = new BFSToken() { Position = source, Distance = length};
            HashSet<string> visited = new HashSet<string>
            {
                { token.Position.X.ToString() + "-" + token.Position.Y }
            };

            Queue<BFSToken> queue = new Queue<BFSToken>();
            queue.Enqueue(token);

            // Bread-first search of Euclidean (integer) grid-space starting from the source position
            while(queue.Any())
            {
                token = queue.Dequeue();

                // Once the destination is discovered, the search immediately ends
                if(token.Position == destination)
                {
                    break;
                }

                // The maximum length of the path may be specified, or default to an unbounded path length
                if (token.Distance != 0)
                {
                    foreach (Vector2f adjacent in new Vector2f[]{
                    // Search West (X + 1)
                    new Vector2f(token.Position.X + 1, token.Position.Y),
                    // Search East (X - 1)
                    new Vector2f(token.Position.X - 1, token.Position.Y),
                    // Search North (Y + 1)
                    new Vector2f(token.Position.X, token.Position.Y + 1),
                    // Search South (Y - 1)
                    new Vector2f(token.Position.X, token.Position.Y - 1),
                })
                    {
                        // If the adjacent space has not yet been visited and meets the valid step criteria ...
                        if (!visited.Contains(adjacent.X.ToString() + "-" + adjacent.Y) && validStep(token.Position, adjacent))
                        {
                            // Enqueue a token for this adjacent space, setting the current token as this next step's parent
                            queue.Enqueue(new BFSToken() { Position = adjacent, Distance = token.Distance - 1, Parent = token });
                            visited.Add(adjacent.X.ToString() + "-" + adjacent.Y);
                        }
                    }
                }
            }

            LinkedList<Vector2f> path = new LinkedList<Vector2f>();

            // If the destination was discovered, back-trace from the destination to the source (greatest ancestor)
            if(token.Position == destination)
            {
                while(token.Position != source)
                {
                    path.AddFirst(token.Position);
                    token = token.Parent;
                }
            }
            
            return path;
        }

        //====================================================================================================================
        // * BFS Search Token during shortest path computation
        //====================================================================================================================
        private class BFSToken
        {
            public Vector2f Position { get; set; }
            public BFSToken Parent { get; set; }
            public int Distance { get; set; }
        };
    }
}
