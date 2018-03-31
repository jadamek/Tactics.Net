using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tactics.Net
{
    //========================================================================================================================
    // ** Main Program
    //========================================================================================================================
    // Sandbox for testing the functionality of the Tactics.Net engine
    //========================================================================================================================
    class Tactics
    {
        //--------------------------------------------------------------------------------------------------------------------
        // - Main Game Loop
        //--------------------------------------------------------------------------------------------------------------------
        static void Main()
        {
            SFML.Graphics.RenderWindow window = new SFML.Graphics.RenderWindow(new SFML.Window.VideoMode(640, 480), "Tactics!");
            window.Closed += (s, e) => { window.Close(); };

            while(window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear();
                window.Display();
            }
        }
    }
}
