using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using Tactics.Net.Maps;
using Tactics.Net.Sprites.Objects;

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
            Texture dirtTexture = new Texture("Resources/Graphics/Tiles/DirtTile_32x24.png");
            Texture grassTexture = new Texture("Resources/Graphics/Tiles/GrassTile_32x24.png");
            Isogeometry.IsometricBuffer buffer = new Isogeometry.IsometricBuffer();
            Map map = new Map(25, 25);
            for(int x = 0; x < map.Width; x++)
            {
                for(int y = 0; y < map.Length; y++)
                {
                    Tile dirt = new Tile(new SpriteTile(dirtTexture, 32, 24, 16), 2);
                    map.Place(dirt, x, y);
                    buffer.Add(dirt);
                    Tile grass = new Tile(new SpriteTile(grassTexture, 32, 24, 8), 1);
                    map.Place(grass, x, y);
                    buffer.Add(grass);
                }
            }

            Clock clock = new Clock();
            Time elapsed = new Time();

            clock.Restart();
            buffer.Sort();
            elapsed = clock.Restart();

            Console.WriteLine(elapsed.AsMicroseconds());

            for(int i = 0; i < 5; i++)
            {
                map.Tiles[i, 0][0].Position = new Vector3f(i, 0, map.Tiles[i, 0][0].Position.Z + 3);
            }

            clock.Restart();
            buffer.PartialSort();
            elapsed = clock.Restart();

            Console.WriteLine(elapsed.AsMicroseconds());

            // Instantiate the main rendering window
            RenderWindow window = new RenderWindow(new SFML.Window.VideoMode(640, 480), "Tactics!");
            window.Closed += (s, e) => { window.Close(); };
            View view = window.DefaultView;
            view.Center = new Vector2f(0, 0);
            window.SetView(view);

            while(window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear();
                window.Draw(buffer);
                window.Display();
            }
        }
    }
}
