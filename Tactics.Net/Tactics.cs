using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tactics.Net
{
    class Tactics
    {
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
