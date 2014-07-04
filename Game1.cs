#region File Description
//-----------------------------------------------------------------------------
// MyGameGame.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

#endregion

namespace MyGame
{
	/// <summary>
	/// Default Project Template
	/// </summary>
	public class Game1 : Game
	{

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		Texture2D background;
		Texture2D earth;
		Shuttle shuttle;

		//SpriteFont font;
		//int score = 0;

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			
			Content.RootDirectory = "Assets";

			graphics.IsFullScreen = false;
		}

		protected override void Initialize ()
		{
			base.Initialize();
		}

		protected override void LoadContent ()
		{
			spriteBatch = new SpriteBatch (graphics.GraphicsDevice);
			Viewport viewport = graphics.GraphicsDevice.Viewport;
			viewport.Height = 800;
			viewport.Width = 1200;

			background = Content.Load<Texture2D>( "stars" );
			earth = Content.Load<Texture2D>( "earth" );

			shuttle = new Shuttle (Content.Load<Texture2D>("shuttle"), Content.Load<Texture2D>("JetExhaustParticle"));

			//font = Content.Load<SpriteFont>( "Kootenay" );

		}

		protected override void Update (GameTime gameTime)
		{
			if(Keyboard.GetState().IsKeyDown(Keys.Escape)){ this.Exit(); }

			shuttle.UpdateShuttle();

			// Let's pretend there's air resistance in space!
			shuttle.Velocity *= 0.98f;
            		
			base.Update( gameTime );
		}

		protected override void Draw (GameTime gameTime)
		{
			// Clear the backbuffer
			graphics.GraphicsDevice.Clear( Color.Black );

			spriteBatch.Begin();

			// draw the sprites
			spriteBatch.Draw( background, new Rectangle(0,0,800,480), Color.White );
			spriteBatch.Draw( earth, new Vector2 (400, 240), Color.White );

			shuttle.Draw( spriteBatch );

			//spriteBatch.DrawString( font, "<Insert Score Here>", new Vector2 (100, 100), Color.Black );

			spriteBatch.End();

			base.Draw( gameTime );
		}



	}
}
