using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

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
            // Create a tile sprite
            Texture grassTexture = new Texture("Resources/Graphics/Tiles/GrassTile_32x24.png");
            Sprites.Objects.SpriteTile grassTile = new Sprites.Objects.SpriteTile(grassTexture, 32, 24, 32, true)
            {
                Position = new Vector2f(100, 100)
            };
            Sprites.Objects.SpriteTile grassTile2 = new Sprites.Objects.SpriteTile(grassTexture, 32, 24,68, false)
            {
                Position = new Vector2f(132, 100)
            };

            // Instantiate the main rendering window
            RenderWindow window = new RenderWindow(new SFML.Window.VideoMode(640, 480), "Tactics!");
            window.Closed += (s, e) => { window.Close(); };

            while(window.IsOpen)
            {
                window.DispatchEvents();

                window.Clear();
                window.Draw(grassTile);
                window.Draw(grassTile2);
                window.Display();
            }
        }
    }
}
