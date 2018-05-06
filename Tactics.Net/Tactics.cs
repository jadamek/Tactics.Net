using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using Tactics.Net.Maps;
using Tactics.Net.Sprites.Map;

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
            Map map = new Map(15, 15);

            Tile[] tiles = new Tile[5];

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Length; y++)
                {
                    Tile dirt = new Tile(new SpriteTile(dirtTexture, 32, 24, 16), 2);
                    map.Place(dirt, x, y);
                    Tile grass = new Tile(new SpriteTile(grassTexture, 32, 24, 8), 1);
                    map.Place(grass, x, y);                    

                    if (y == 0 && x < 5)
                    {
                        tiles[x] = grass;
                    }
                }
            }
            map.Place(new Tile(new SpriteTile(grassTexture, 32, 24, 32), 4), 1, 5);
            map.Place(new Tile(new SpriteTile(grassTexture, 32, 24, 8), 1), 2, 5);
            map.Place(new Tile(new SpriteTile(grassTexture, 32, 24, 16), 2), 3, 5);
            map.Place(new Tile(new SpriteTile(grassTexture, 32, 24, 48), 6), 6, 5);
            map.Place(new Tile(new SpriteTile(grassTexture, 32, 24, 32), 4), 6, 6);

            CircleShape origin = new CircleShape(2)
            {
                FillColor = Color.Red,
                Origin = new Vector2f(2, 2),
            };

            Texture assassinTexture = new Texture("Resources/Graphics/Characters/Assassin.png");
            Sprites.Actor.SpriteActor sheet = new Sprites.Actor.SpriteActor(assassinTexture, 48, 48, 4, 12)
            {
                Origin = new Vector2f(24, 39),
            };
            
            sheet.SetSequence(Sprites.Actor.SpriteActor.BasicAnimations.Stand, new List<uint>{ 0, 0, 0, 1, 1, 1, 2, 2, 2, 3, 3, 3});
            sheet.SetSequence(Sprites.Actor.SpriteActor.BasicAnimations.Walk, new List<uint>{ 0, 0, 1, 1, 2, 2, 3, 3});
            sheet.Play(Sprites.Actor.SpriteActor.BasicAnimations.Stand, 3);

            Actors.Actor assassin = new Actors.Actor()
            {
                Sprite = sheet,
            };
            assassin.Locomotion = new Movement.Walking(assassin) { JumpScore = 5, Speed = 1f };

            map.Join(assassin, 0, 5);

            assassin.Move(new Vector2f(3, 9));

            Sprites.Battle.SpriteDamage damage = new Sprites.Battle.SpriteDamage(32);

            Clock gameclock = new Clock();

            // Instantiate the main rendering window
            RenderWindow window = new RenderWindow(new SFML.Window.VideoMode(640, 480), "Tactics!");
            window.Closed += (s, e) => { window.Close(); };
            View view = window.DefaultView;
            view.Center = new Vector2f(0, 150);
            window.SetView(view);
            
            while(window.IsOpen)
            {
                Animation.Animations.Update(gameclock.Restart().AsSeconds());
                window.DispatchEvents();
                window.Clear();
                window.Draw(map);
                window.Draw(origin);
                window.Draw(damage);
                window.Display();
            }
        }
    }
}
