using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tactics.Net.Animation
{
    //========================================================================================================================
    // ** Animation Interface
    //========================================================================================================================
    // Describes a playable animation, with Play/Stop/Pause functionality
    //========================================================================================================================
    public interface IAnimated
    {
        void Play(bool loop = false);
        void Stop();
        bool Paused { get; set; }
        bool Looping { get; set; }
    }
}
