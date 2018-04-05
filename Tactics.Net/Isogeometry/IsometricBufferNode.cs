using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using Tactics.Net.Extensions;

namespace Tactics.Net.Isogeometry
{
    partial class IsometricBuffer
    {
        //====================================================================================================================
        // ** Isometric Buffer Node (Internal)
        //====================================================================================================================
        // Node class which is used to create a topological graph of Isometric objects within the buffer
        // Two nodes are connected when their bounding rectangles (using global coordinates) intersect
        // A node is the parent of another node when its object is to be drawn behind that node's object
        //====================================================================================================================
        private class IsometricBufferNode : Disposable
        {
            //----------------------------------------------------------------------------------------------------------------
            // - Isometric Buffer Node Constructor
            //----------------------------------------------------------------------------------------------------------------
            public IsometricBufferNode(IsometricObject target, IsometricBuffer container)
            {
                Target = target;

                // Add events to target to dispose this node when the target is disposed, and alert this node of any changes
                // in position
                if (Target != null)
                {
                    Target.Disposed += (s, e) => { Dispose(); };
                    Target.PositionChanged += (s, e) => { Alert(); };
                }

                Container = container;

                // New nodes immediately signal a re-sort call
                if(Target != null && Container != null)
                {
                    Alert();
                }
            }

            //----------------------------------------------------------------------------------------------------------------
            // - Compare Painter's Order Of Target Objects
            //----------------------------------------------------------------------------------------------------------------
            private bool Compare(IsometricObject a, IsometricObject b)
            {
                if (a != null && b != null)
                {
                    // If A.Z is above B.Z + B.Height relative to A, then A is on top
                    if (a.Position.Z >= b.Position.Z + b.Height(new Vector2f(a.Position.X - b.Position.X, a.Position.Y - b.Position.Y)))
                    {
                        return true;
                    }
                    // Else, if A.Z + A.Height relative to B is lower than B.Z, B is on top
                    else if (b.Position.Z >= a.Position.Z + a.Height(new Vector2f(a.Position.X - b.Position.X, a.Position.Y - b.Position.Y)))
                    {
                        return false;
                    }
                    // If neither, then compare isometric X+Y (closeness) relationship ...
                    else
                    {
                        return a.Position.X + a.Position.Y >= b.Position.X + b.Position.Y;
                    }
                }
                return false;
            }

            //----------------------------------------------------------------------------------------------------------------
            // - Attach Two Nodes In Order
            //----------------------------------------------------------------------------------------------------------------
            public void Attach(IsometricBufferNode node)
            {
                // Child nodes are drawn below (before) parent nodes
                if(Compare(Target, node.Target))
                {
                    Children.Add(node);                
                }
                else
                {
                    node.Children.Add(this);
                }
            }

            //----------------------------------------------------------------------------------------------------------------
            // - Alert Node Of Position Change
            //----------------------------------------------------------------------------------------------------------------
            private void Alert()
            {
                Dirty = true;

                // Pre-compute new bounding box to hasten sorting computation time
                if (Target != null && Container != null)
                {
                    FloatRect bounds = Target.GetGlobalBounds();
                    Vector2f isometricPosition = Target.GlobalPosition;
                    bounds.Left += isometricPosition.X;
                    bounds.Top += isometricPosition.Y;

                    Bounds = bounds;

                    // Alert container for need to re-sort
                    if(Container != null)
                    {
                        Container.Dirty = true;
                    }
                }
            }

            //----------------------------------------------------------------------------------------------------------------
            // - Dispose (Implementation)
            //----------------------------------------------------------------------------------------------------------------
            public override void Dispose()
            {
                // Remove events from target to dispose this node when the target is disposed, and alert this node of any
                // changes in position
                if (Target != null)
                {
                    Target.Disposed -= (s, e) => { Dispose(); };
                    Target.PositionChanged += (s, e) => { Alert(); };
                }
                base.Dispose();

                // Remove node from container and detach from all parents, unless the entire container is being disposed
                if(Container != null && !Container.Disposing)
                {
                    for(int i = Container.Nodes.Count - 1; i >= 0; i--)
                    {
                        if(this == Container.Nodes[i])
                        {
                            Container.Nodes.RemoveAt(i);
                        }
                        else
                        {
                            Container.Nodes[i].Children.Remove(this);
                        }
                    }
                }
            }

            // Members
            public IsometricObject Target { get; set; }
            public bool Visited { get; set; }
            public bool Dirty { get; set; } = true;
            public FloatRect Bounds { get; set; }
            public List<IsometricBufferNode> Children { get; } = new List<IsometricBufferNode>();

            // Members - private
            private IsometricBuffer Container { get; }

        }
    }
}
