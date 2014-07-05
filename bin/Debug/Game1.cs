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

namespace SpriteFlight
{
	/// <summary>
	/// Default Project Template
	/// </summary>
	public class Game1 : Game
	{

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		GameObjects gameObjects;
		public Random randomGenerator;

		Texture2D background;
		Texture2D earth;
		Song song;
		SoundEffect shipExplosion;
		SoundEffect asteroidExplosion;
		SoundEffect laserFire;

		SpriteFont font;
		int score;
		int asteroidCount;

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			
			Content.RootDirectory = "Assets";

			graphics.IsFullScreen = true;

			gameObjects = new GameObjects ();
			gameObjects.ScreenSize = new Rectangle (
				0, 0, 
				GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, 
				GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height
			);

			randomGenerator = new Random ();
			asteroidCount = 12;
			score = 0;
		}

		protected override void Initialize ()
		{
			base.Initialize();
		}

		protected override void LoadContent ()
		{
			spriteBatch = new SpriteBatch (graphics.GraphicsDevice);
			Viewport viewport = graphics.GraphicsDevice.Viewport;

			background = Content.Load<Texture2D>( "stars" );
			earth = Content.Load<Texture2D>( "earth" );
			song = Content.Load<Song>("Ambient I Tibet Groove");
			MediaPlayer.Play(song);

			laserFire = Content.Load<SoundEffect>("Laser_Shoot10");
			shipExplosion = Content.Load<SoundEffect>( "Explosion5" );
			asteroidExplosion = Content.Load<SoundEffect>( "Explosion6" );

			gameObjects.Shuttle = new Shuttle (
				Content.Load<Texture2D>("shuttle"),
				new Vector2(gameObjects.ScreenSize.Width/2, gameObjects.ScreenSize.Height/2),
				Content.Load<Texture2D>("JetExhaustParticle2"),
				Content.Load<Texture2D>("Laser"),
				laserFire
			);

			gameObjects.Asteroids = new List<Asteroid> ();
			for( var i=0; i<asteroidCount; i++)
			{
				var randLocation = new Vector2 (randomGenerator.Next(gameObjects.ScreenSize.Width), randomGenerator.Next(gameObjects.ScreenSize.Height));
				var newAsteroid = new Asteroid (Content.Load<Texture2D>( "Asteroid01" ), randLocation);
				newAsteroid.Scale = new Vector2(0.5f);
				newAsteroid.Velocity = Utility.GetRandomDirection(randomGenerator) * 2f;
				newAsteroid.AngularVelocity = Utility.GetRandomFloat(randomGenerator, -0.1f, 0.1f);
				gameObjects.Asteroids.Add( newAsteroid );
			}

			gameObjects.Lasers = new List<Laser> ();

			font = Content.Load<SpriteFont>( "GameFont" );
		}

		protected override void Update (GameTime gameTime)
		{
			// Game Controls

			if(Keyboard.GetState().IsKeyDown(Keys.Escape)){ this.Exit(); }

			// Update Ship

			gameObjects.Shuttle.Velocity *= 0.99f; // Let's pretend there's air resistance in space!
			gameObjects.Shuttle.Update(gameObjects);

			// Update Lasers

			for (var i=0; i < gameObjects.Lasers.Count; i++){
				gameObjects.Lasers[i].Update(gameObjects);
				if (!gameObjects.Lasers[i].EdgeWrap && gameObjects.Lasers [i].IsOutOfBounds(gameObjects.ScreenSize))
					gameObjects.Lasers.RemoveAt( i );
				if (gameObjects.Lasers [i].TTL <= 0)
					gameObjects.Lasers.RemoveAt( i );
			}

			// Update Asteroids

			for (var i=0; i < gameObjects.Asteroids.Count; i++){
				var a = gameObjects.Asteroids [i];
				a.Update(gameObjects);
			}

			// Process Collisions

			for (var i=0; i < gameObjects.Asteroids.Count; i++){
				var a = gameObjects.Asteroids [i];

				if(a.HitBox.Intersects(gameObjects.Shuttle.HitBox))
				{
					gameObjects.Shuttle.Remove();
					gameObjects.Asteroids.RemoveAt( i );
					shipExplosion.Play();
				}

				for(var j=0; j < gameObjects.Lasers.Count; j++)
				{
					if(a.HitBox.Intersects(gameObjects.Lasers[j].HitBox))
					{
						gameObjects.Lasers.RemoveAt( j );
						gameObjects.Asteroids.RemoveAt( i );
						score++;
						asteroidExplosion.Play();
					}
				}
			}
            		
			base.Update( gameTime );
		}

		protected override void Draw (GameTime gameTime)
		{
			// Clear the backbuffer
			graphics.GraphicsDevice.Clear( Color.Black );

			spriteBatch.Begin();

			// draw the sprites
			spriteBatch.Draw( background, new Rectangle(0,0,2400,1440), Color.White );
			spriteBatch.Draw( earth, new Vector2 (400, 240), Color.White );

			foreach(var laser in gameObjects.Lasers)
			{
				laser.Draw( spriteBatch );
			}

			foreach(var asteroid in gameObjects.Asteroids){
				asteroid.Draw( spriteBatch );
			}

			gameObjects.Shuttle.Draw( spriteBatch );

			spriteBatch.DrawString( font, score.ToString(), new Vector2 (100, 100), Color.Red );

			spriteBatch.End();

			base.Draw( gameTime );
		}
	}

	public class GameObjects
	{
		public Rectangle ScreenSize { get; set; }
		public int Score { get; set; }

		public Shuttle Shuttle { get; set; }
		public List<Asteroid> Asteroids { get; set; }
		public List<Laser> Lasers { get; set; }
	}
}
