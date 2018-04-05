using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using Tactics.Net.Animation;

namespace Tactics.Net.Isogeometry
{
    //========================================================================================================================
    // ** Isometric Depth Buffer
    //========================================================================================================================
    // Represents a depth buffer of drawable isometric objects, using a topological sort on a separating sorting thread to
    // implement an isometric Painter's algorithm
    //========================================================================================================================
    partial class IsometricBuffer : AnimatedObject, Drawable
    {
        //--------------------------------------------------------------------------------------------------------------------
        // - Add New Object To Buffer
        //--------------------------------------------------------------------------------------------------------------------
        public void Add(IsometricObject obj)
        {
            // Will signal this buffer to re-sort by setting Dirty
            Nodes.Add(new IsometricBufferNode(obj, this));
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Remove Object From Buffer
        //--------------------------------------------------------------------------------------------------------------------
        public void Remove(IsometricObject obj)
        {
            IsometricBufferNode node = Nodes.FirstOrDefault(n => n.Target == obj);
            node?.Dispose();
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Depth-first Topological Traversal (Recursive)
        //--------------------------------------------------------------------------------------------------------------------
        private void TopologicalTraverse(IsometricBufferNode current)
        {
            // Mark the current node as visited
            current.Visited = true;

            // Traverse each non-visited child
            foreach(IsometricBufferNode child in current.Children)
            {
                if(!child.Visited)
                {
                    TopologicalTraverse(child);
                }
            }

            // Push current node's target object onto the back of the sorted drawing queue
            SortedObjects.Add(current.Target);
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Topological Sort
        //--------------------------------------------------------------------------------------------------------------------
        // Executes a depth-first search to topologically sort nodes according a Painter's ordering
        //--------------------------------------------------------------------------------------------------------------------
        private void TopologicalSort()
        {
            // Clear any previous sorting
            SortedObjects.Clear();

            // Clear all 'Visited' flags from any previous sorting
            foreach (IsometricBufferNode node in Nodes)
            {
                node.Visited = false;
            }

            // Initiate a traversal at every non-visited node
            foreach(IsometricBufferNode node in Nodes)
            {
                if(!node.Visited)
                {
                    TopologicalTraverse(node);
                }
            }

            // Clear signal to sort
            Dirty = false;
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Isometric Full Sort
        //--------------------------------------------------------------------------------------------------------------------
        // Sorts all isometric objects, regardless of "dirty" status, into Painter's drawing order using a topological sort on
        // a directed bounding box intersection graph
        //--------------------------------------------------------------------------------------------------------------------
        public void Sort()
        {
            // Disconnect any pre-existing graph
            foreach(IsometricBufferNode node in Nodes)
            {
                node.Children.Clear();
            }

            // Connect any two nodes with intersecting bounding boxes
            for(int i = 0; i < Nodes.Count; i++)
            {
                for(int j = i + 1; j < Nodes.Count; j++)
                {
                    if(Nodes[i].Bounds.Intersects(Nodes[j].Bounds))
                    {
                        Nodes[i].Attach(Nodes[j]);
                    }
                }
                // Piggyback Dirty flag reset
                Nodes[i].Dirty = false;
            }
           
            // Topologically sort all nodes
            TopologicalSort();
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Isometric Sort (Partial)
        //--------------------------------------------------------------------------------------------------------------------
        // Isometrically sorts only a subset of nodes which are labeled as 'Dirty', signifying their positions have changed
        // since the last sorting
        //--------------------------------------------------------------------------------------------------------------------
        public void PartialSort()
        {
            foreach(IsometricBufferNode node in Nodes)
            {
                // Remove all outgoing edges from nodes
                if (node.Dirty)
                {
                    node.Children.Clear();
                }
                // Remove all edges to Dirty nodes
                else
                {
                    node.Children.RemoveAll(child => child.Dirty);
                }
            }

            // Reconnect all dirty nodes to their proper neighbors
            foreach(IsometricBufferNode dirtyNode in Nodes.Where(node => node.Dirty))
            {
                foreach(IsometricBufferNode candidate in Nodes)
                {
                    if(dirtyNode != candidate && dirtyNode.Bounds.Intersects(candidate.Bounds))
                    {
                        dirtyNode.Attach(candidate);
                    }
                }
                // Piggy-back Dirty flag clear for all dirty nodes
                dirtyNode.Dirty = false;
            }

            // Topologically sort all nodes
            TopologicalSort();
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Draw (Implementation)
        //--------------------------------------------------------------------------------------------------------------------
        public void Draw(RenderTarget target, RenderStates states)
        {
            // Draw each sorted isometric object, isometrically projected to screen coordinates
            foreach(IsometricObject sortedObject in SortedObjects)
            {
                RenderStates state = states;
                state.Transform.Translate(sortedObject.GlobalPosition);

                sortedObject.Draw(target, state);
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Execute Sort Check (Implementation)
        //--------------------------------------------------------------------------------------------------------------------
        protected override void Step()
        {
            // If the number of dirty (recently added or updated) objects is greather than the total number of objects,
            // execute a full sort; otherwise, execute a partial sort on only those dirty objects
            if(Dirty)
            {
                if(Nodes.Count(node => node.Dirty) > Nodes.Count / 2)
                {
                    Sort();
                }
                else
                {
                    PartialSort();
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Dispose (Implementation)
        //--------------------------------------------------------------------------------------------------------------------
        public override void Dispose()
        {
            Disposing = true;
            foreach(IsometricBufferNode node in Nodes)
            {
                node.Dispose();
            }
            base.Dispose();
        }

        // Members - private
        private List<IsometricBufferNode> Nodes { get; } = new List<IsometricBufferNode>();
        private List<IsometricObject> SortedObjects { get; } = new List<IsometricObject>();
        private bool Dirty { get; set; }
        private bool Disposing { get; set; }
    }
}
