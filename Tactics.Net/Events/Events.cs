using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tactics.Net.Animation;

namespace Tactics.Net.Events
{
    //========================================================================================================================
    // ** Event Scheduler
    //========================================================================================================================
    // Handles scheduling and tracking for all synchronous events
    //========================================================================================================================
    public static partial class Events
    {
        //--------------------------------------------------------------------------------------------------------------------
        // - Event Scheduler Constructor
        //--------------------------------------------------------------------------------------------------------------------
        static Events()
        {
            EventCycle.Step += (s, e) => Step();
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Schedule Event
        //--------------------------------------------------------------------------------------------------------------------
        public static void Schedule(Action action, float delay, Func<bool> trigger = null, object sender = null)
        {
            ScheduledEvents.AddLast(new Event()
            {
                Action = action,
                // Convert the delay in seconds to frames
                Delay = (int)Math.Round(delay * EventCycle.Framerate),
                Trigger = trigger,
                Sender = sender,
            });
        }

        //--------------------------------------------------------------------------------------------------------------------
        // - Event Scheduler Main Routine
        //--------------------------------------------------------------------------------------------------------------------
        private static void Step()
        {
            LinkedListNode<Event> next;

            for(LinkedListNode<Event> scheduled = ScheduledEvents.First; scheduled != null;)
            {
                next = scheduled.Next;

                // For any event whose delay has expired or their triggering event has fired TRUE, execute and remove it
                if (scheduled.Value.Countdown() || (scheduled.Value.Trigger?.Invoke() ?? false))
                {
                    scheduled.Value.Action();
                    ScheduledEvents.Remove(scheduled);
                }
                scheduled = next;
            }
        }

        // Members - private
        private static LinkedList<Event> ScheduledEvents { get; } = new LinkedList<Event>();
        private static Animator EventCycle { get; } = new Animator();

        //====================================================================================================================
        // ** Synchronous Event
        //====================================================================================================================
        // An event that may be scheduled to occur in a number of seconds or by a specific Boolean trigger condition
        //====================================================================================================================
        private class Event
        {
            //--------------------------------------------------------------------------------------------------------------------
            // - Update & Return Countdown Status
            //--------------------------------------------------------------------------------------------------------------------
            public bool Countdown()
            {
                if (Delay > 0) Delay--;
                return Delay == 0;
            }

            // Members
            public Action Action { get; set; }
            public int Delay { get; set; }
            public Func<bool> Trigger { get; set; }
            public object Sender { get; set; }
        }
    }
}
