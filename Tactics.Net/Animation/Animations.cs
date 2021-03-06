﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tactics.Net.Animation
{
    //========================================================================================================================
    // ** Global Animations Collection
    //========================================================================================================================
    // Singular global and static handler and collection for all animations which pre-register to this collection, and
    // self-deregister on disposal
    //========================================================================================================================
    public static class Animations
    {
        //--------------------------------------------------------------------------------------------------------------------
        // - Add Animation
        //--------------------------------------------------------------------------------------------------------------------
        // Pushes a new animated object to the front of the list composed into a node
        //--------------------------------------------------------------------------------------------------------------------
        public static void Add(Animator animation)
        {
            if(animation != null)
            {
                AnimationNode node = new AnimationNode()
                {
                    Payload = animation,
                    Next = Front
                };

                // Update Front node
                if (Front != null)
                {
                    Front.Previous = node;
                }
                Front = node;
            }
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Update Animations
        //--------------------------------------------------------------------------------------------------------------------
        public static void Update(float elapsed)
        {

            if(!Frozen)
            {
                AnimationNode node = Front;

                while(node != null)
                {
                    node.Payload.Update(elapsed);
                    node = node.Next;
                }
            }
        }        

        // Global Members
        public const float DEFAULT_FRAMERATE = 60.0f;
        public static bool Frozen { get; set; }
        private static AnimationNode Front { get; set; }

        //====================================================================================================================
        // ** Animation Linked-List Node (Internal)
        //====================================================================================================================
        private class AnimationNode : IDisposable
        {
            //----------------------------------------------------------------------------------------------------------------
            // - Dispose (Implementation)
            //----------------------------------------------------------------------------------------------------------------
            public void Dispose()
            {
                // Update Current Front Node
                if(Animations.Front == this)
                {
                    Animations.Front = Next;
                }
                // Update Neighbors
                if(Previous != null)
                {
                    Previous.Next = Next;
                }
                if(Next != null)
                {
                    Next.Previous = Previous;
                }
            }

            //----------------------------------------------------------------------------------------------------------------
            // - Animated Object Payload (Property)
            //----------------------------------------------------------------------------------------------------------------
            private Animator payload_;
            public Animator Payload {
                get { return payload_; }
                set
                {
                    if (payload_ != value)
                    {
                        if (value != null)
                        {
                            // Add event for disposing this node on object disposal
                            value.Disposed += (s, e) => { Dispose(); };
                        }
                        payload_ = value;
                    }
                }
            }

            // Members
            public AnimationNode Next { get; set; }
            public AnimationNode Previous { get; set; }
        }
    }
}
