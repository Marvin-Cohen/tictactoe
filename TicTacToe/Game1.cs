// Created by Marvin Cohen
// Tile assets are from https://opengameart.org/content/boardgame-tiles
// Note: This may be incomplete.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text;
using System;
using System.Collections.Generic;

namespace TicTacToe
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        //ButtonState button_state;

        const int TILE_WIDTH = 150;
        const int TILE_HEIGHT = 150;
        const int WindowWidth = 3 * TILE_WIDTH;
        const int WindowHeight = 3 * TILE_HEIGHT;
        const int NUM_OF_MARKERS = 9;
        const int NUM_OF_TILES = 9;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //MouseState curr_mouse_state;
        //MouseState prev_mouse_state;
        //int mouseX;
        //int mouseY;

        // Tiles
        Texture2D[] tile = new Texture2D[1];
        Rectangle[] drawRectangle = new Rectangle[NUM_OF_TILES];
        bool[] tileIsUsed = new bool[NUM_OF_TILES];

        // Marker Pieces
        Texture2D[] tictacPiece = new Texture2D[2];
        
        //Rectangle marker;
        Rectangle[] marker = new Rectangle[NUM_OF_MARKERS];
        bool[] isOnBoard = new bool[NUM_OF_MARKERS]; // Maximum number of pieces if game continues
        string[] type = new string[NUM_OF_MARKERS];

        int selected_index = 0;
        int selected_marker_X = 0; //original, NOT current coordinates of any marker
        int selected_marker_Y = 0;
        int player = 0;            //by default, X's turn first
        int result = 0;            //result is -1 for keep going, 0 for tie, 1 for O's win, 2 for X's win
        bool spaceClicked = false;
        bool toggle = false;
        Dictionary<int, string> TileMarkerPairing = new Dictionary<int, string>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            //IsMouseVisible = true;
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = WindowWidth;
            graphics.PreferredBackBufferHeight = WindowHeight;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            tile[0] = Content.Load<Texture2D>(@"C:\Users\marvi\source\repos\TicTacToe\TicTacToe\Content\Graphics\lighttile");
            tictacPiece[0] = Content.Load<Texture2D>(@"C:\Users\marvi\source\repos\TicTacToe\TicTacToe\Content\Graphics\close");
            tictacPiece[1] = Content.Load<Texture2D>(@"C:\Users\marvi\source\repos\TicTacToe\TicTacToe\Content\Graphics\2000px-Letter_o.svg");
            
            for (int i = 0; i < NUM_OF_MARKERS; i++)
            {
                tileIsUsed[i] = false;
                isOnBoard[i] = false;
                marker[i].Width = TILE_WIDTH;
                marker[i].Height = TILE_HEIGHT;
                TileMarkerPairing.Add(i, "");
                if (i == 0)
                {
                    marker[i].X = 0;
                    marker[i].Y = 0;
                }
                else if (i == 1)
                {
                    marker[i].X = TILE_WIDTH;
                    marker[i].Y = 0;
                }
                else if (i == 2)
                {
                    marker[i].X = 2 * TILE_WIDTH;
                    marker[i].Y = 0;
                }
                else if (i == 3)
                {
                    marker[i].X = 0;
                    marker[i].Y = TILE_HEIGHT;
                }
                else if (i == 4)
                {
                    marker[i].X = TILE_WIDTH;
                    marker[i].Y = TILE_HEIGHT;
                }
                else if (i == 5)
                {
                    marker[i].X = 2 * TILE_WIDTH;
                    marker[i].Y = TILE_HEIGHT;
                }
                else if (i == 6)
                {
                    marker[i].X = 0;
                    marker[i].Y = 2 * TILE_HEIGHT;
                }
                else if (i == 7)
                {
                    marker[i].X = TILE_WIDTH;
                    marker[i].Y = 2 * TILE_HEIGHT;
                }
                else if (i == 8)
                {
                    marker[i].X = 2 * TILE_WIDTH;
                    marker[i].Y = 2 * TILE_HEIGHT;
                }
            }

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            MouseState curr_mouse_state = Mouse.GetState();
            ButtonState button_state = curr_mouse_state.LeftButton;
            int mouseX = curr_mouse_state.X;
            int mouseY = curr_mouse_state.Y;

            if (curr_mouse_state.LeftButton.ToString() == "Pressed")
            {
                if (spaceClicked == false)
                {
                    for (int i = 0; i < NUM_OF_MARKERS; i++)
                    {
                        if (marker[i].X == (mouseX / TILE_WIDTH) * TILE_WIDTH && marker[i].Y == (mouseY / TILE_HEIGHT) * TILE_HEIGHT)
                        {
                            selected_index = i;
                            if (player == 0)
                            {
                                TileMarkerPairing[selected_index] = "X";
                                //result = Check(selected_index, TileMarkerPairing[selected_index]);
                                ///////////////////////////////////////
                                // first row
                                if (TileMarkerPairing[0] == "X" && TileMarkerPairing[1] == "X" 
                                    && TileMarkerPairing[2] == "X")
                                {
                                    break;
                                }
                                // second row
                                else if (TileMarkerPairing[3] == "X" && TileMarkerPairing[4] == "X"
                                    && TileMarkerPairing[5] == "X")
                                {
                                    break;
                                }
                                // third row
                                else if (TileMarkerPairing[6] == "X" && TileMarkerPairing[7] == "X" 
                                    && TileMarkerPairing[8] == "X")
                                {
                                    break;
                                }
                                // first column
                                else if (TileMarkerPairing[0] == "X" && TileMarkerPairing[3] == "X" 
                                    && TileMarkerPairing[6] == "X")
                                {
                                    break;
                                }
                                // second column
                                else if (TileMarkerPairing[1] == "X" && TileMarkerPairing[4] == "X" 
                                    && TileMarkerPairing[7] == "X")
                                {
                                    break;
                                }
                                // third column
                                else if (TileMarkerPairing[2] == "X" && TileMarkerPairing[5] == "X" 
                                    && TileMarkerPairing[8] == "X")
                                {
                                    break;
                                }
                                // left diagonal
                                else if (TileMarkerPairing[0] == "X" && TileMarkerPairing[4] == "X" 
                                    && TileMarkerPairing[8] == "X")
                                {
                                    break;
                                }
                                // right diagonal
                                else if (TileMarkerPairing[2] == "X" && TileMarkerPairing[4] == "X" 
                                    && TileMarkerPairing[6] == "X")
                                {
                                    break;
                                }
                                //else if (TileMarkerPairing[8] == "X")
                                //{
                                //    break;
                                //}
                                //isOnBoard[selected_index] = true;
                                //spaceClicked = true;
                                //player = 1;
                            }
                            else if (player == 1)
                            {
                                TileMarkerPairing[selected_index] = "O";
                                //result = Check(selected_index, TileMarkerPairing[selected_index]);
                                // first row
                                if (TileMarkerPairing[0] == "O" && TileMarkerPairing[1] == "O"
                                    && TileMarkerPairing[2] == "O")
                                {
                                    break;
                                }
                                // second row
                                else if (TileMarkerPairing[3] == "O" && TileMarkerPairing[4] == "O"
                                    && TileMarkerPairing[5] == "O")
                                {
                                    break;
                                }
                                // third row
                                else if (TileMarkerPairing[6] == "O" && TileMarkerPairing[7] == "O"
                                    && TileMarkerPairing[8] == "O")
                                {
                                    break;
                                }
                                // first column
                                else if (TileMarkerPairing[0] == "O" && TileMarkerPairing[3] == "O"
                                    && TileMarkerPairing[6] == "O")
                                {
                                    break;
                                }
                                // second column
                                else if (TileMarkerPairing[1] == "O" && TileMarkerPairing[4] == "O"
                                    && TileMarkerPairing[7] == "O")
                                {
                                    break;
                                }
                                // third column
                                else if (TileMarkerPairing[2] == "O" && TileMarkerPairing[5] == "O"
                                    && TileMarkerPairing[8] == "O")
                                {
                                    break;
                                }
                                // left diagonal
                                else if (TileMarkerPairing[0] == "O" && TileMarkerPairing[4] == "O"
                                    && TileMarkerPairing[8] == "O")
                                {
                                    break;
                                }
                                // right diagonal
                                else if (TileMarkerPairing[2] == "O" && TileMarkerPairing[4] == "O"
                                    && TileMarkerPairing[6] == "O")
                                {
                                    break;
                                }
                                //isOnBoard[selected_index] = true;
                                //spaceClicked = true;
                                //player = 0;
                            }
                        }
                    }
                }
                toggle = true;
            }

            if (curr_mouse_state.LeftButton.ToString() != "Pressed" && toggle == true)
            {
                player = (player + 1) % 2;
                toggle = false;
            }

            for (int i = 0; i < NUM_OF_TILES; i++)
            {
                drawRectangle[i].Width = TILE_WIDTH;
                drawRectangle[i].Height = TILE_HEIGHT;
                drawRectangle[i].X = (i % 3) * TILE_WIDTH;   // the rhs of these expressions will need to  be changed
                drawRectangle[i].Y = i / 3 * TILE_HEIGHT;  // to equal the appropriate functions of "i"
            }
            ///////////////////////////////////

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();


            for (int i = 0; i < NUM_OF_TILES; i++)
            {
                spriteBatch.Draw(tile[0], drawRectangle[i], Color.White);
                string type = TileMarkerPairing[i];
                if (type == "X")
                {
                    spriteBatch.Draw(tictacPiece[0], drawRectangle[i], Color.White);
                    if (result == 1 || result == 2)
                    {
                        break;
                    }
                    
                }
                else if (type == "O")
                {
                    spriteBatch.Draw(tictacPiece[1], drawRectangle[i], Color.White);
                    if (result == 1 || result == 2)
                    {
                        break;
                    }
                }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
   
        /*
        public static int Check(string type)
        {
            if (type == "X")
            {
                // First horizontal row
                if (Tile && index == 1 && index == 2)
                {
                    return 2;
                }
                // Second horizontal row
                else if (index == 3 && index == 4 && index == 5)
                {
                    return 2;
                }
                // Third horizontal row
                else if (index == 6 && index == 7 && index == 8)
                {
                    return 2;
                }
                // First vertical column
                else if (index == 0 && index == 3 && index == 6)
                {
                    return 2;
                }
                // Second vertical column
                else if (index == 1 && index == 4 && index == 7)
                {
                    return 2;
                }
                // Third vertical column
                else if (index == 2 && index == 5 && index == 8)
                {
                    return 2;
                }
                // Left diagonal
                else if (index == 0 && index == 4 && index == 8)
                {
                    return 2;
                }
                // Right diagonal
                else if (index == 2 && index == 4 && index == 6)
                {
                    return 2;
                }
            }
            else if (type == "O")
            {
                // First horizontal row
                if (index == 0 && index == 1 && index == 2)
                {
                    return 1;
                }
                // Second horizontal row
                else if (index == 3 && index == 4 && index == 5)
                {
                    return 1;
                }
                // Third horizontal row
                else if (index == 6 && index == 7 && index == 8)
                {
                    return 1;
                }
                // First vertical column
                else if (index == 0 && index == 3 && index == 6)
                {
                    return 1;
                }
                // Second vertical column
                else if (index == 1 && index == 4 && index == 7)
                {
                    return 1;
                }
                // Third vertical column
                else if (index == 2 && index == 5 && index == 8)
                {
                    return 1;
                }
                // Left diagonal
                else if (index == 0 && index == 4 && index == 8)
                {
                    return 1;
                }
                // Right diagonal
                else if (index == 2 && index == 4 && index == 6)
                {
                    return 1;
                }
            }
            return 0;
        }
        */
        
    }
}
