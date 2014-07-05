using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using System;
using System.Collections.Generic;

namespace SpriteFlight
{
	public class Shuttle : Sprite
	{
		public SoundEffect LaserFire;

		public Vector2 EnginePosition { get; set; }
		public Vector2 GunPosition { get; set; }

		List<Texture2D> particleTextures;
		ParticleEngine particleEngine;

		Texture2D laserTexture;

		float thrust = 0.2f;
		float turnSpeed = 0.05f;
		public int TimeToFire { get; protected set; }

		public Shuttle (Texture2D texture, Vector2 location, Texture2D exhaust, Texture2D laser, SoundEffect laserFire) 
			: base(texture, location, null, null, null, null, null, null, true, null)
		{
			Origin = new Vector2 (Width / 2, Height / 2);
			Scale = new Vector2 (0.5f);

			particleTextures = new List<Texture2D> (){ exhaust };
			particleEngine = new ParticleEngine (particleTextures, Location, 3, Scale);

			laserTexture = laser;
			LaserFire = laserFire;
			TimeToFire = 0;
		}

		public void Update(GameObjects gameObjects)
		{
			// Update Shuttle

			KeyboardState currentState = Keyboard.GetState();

			if(Active)
			{
				AngularVelocity = currentState.IsKeyDown( Keys.A ) ? -turnSpeed
				: currentState.IsKeyDown( Keys.D ) ? turnSpeed
				: 0f;

				Angle += AngularVelocity;

				Vector2 modelVelocityAdd = Vector2.Zero;

				modelVelocityAdd.X = (float) Math.Sin( Angle );
				modelVelocityAdd.Y = -(float) Math.Cos( Angle );

				modelVelocityAdd *= currentState.IsKeyDown( Keys.W ) ? thrust : 0f;

				Velocity += modelVelocityAdd;

				Location += Velocity;

				if (EdgeWrap)
					WrapEdgeLocation( gameObjects.ScreenSize );
			}

			// Update Engine

			EnginePosition = new Vector2 (
				Location.X - (float)Math.Cos(Angle - Math.PI/2) * Height / 2 * Scale.X,
				Location.Y - (float)Math.Sin(Angle - Math.PI/2) * Height / 2 * Scale.Y
			);

			particleEngine.EmitterLocation = EnginePosition;
			particleEngine.Update();

			// Update Lasers

			GunPosition = new Vector2 (
				Location.X + (float)Math.Cos(Angle - Math.PI/2) * Height / 2 * Scale.X,
				Location.Y + (float)Math.Sin(Angle - Math.PI/2) * Height / 2 * Scale.Y
			);

			TimeToFire--;
			TimeToFire = MathHelper.Clamp( TimeToFire, 0, 30 );

			if (Active && currentState.IsKeyDown( Keys.Space ) && TimeToFire == 0) 
			{
				var newLaser = new Laser (laserTexture);
				newLaser.Location = GunPosition;
				newLaser.Angle = Angle;
				newLaser.Velocity = new Vector2((float)Math.Cos(Angle - Math.PI/2),(float)Math.Sin(Angle - Math.PI/2)) * 30f;
				newLaser.Scale = Scale;

				gameObjects.Lasers.Add( newLaser );
				TimeToFire = 20;
				LaserFire.Play();
			}
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			particleEngine.Draw( spriteBatch );
			base.Draw( spriteBatch );
		}
	}

	class ParticleEngine
	{
		private System.Random random;
		public int Count { get; set; }
		public Vector2 EmitterLocation { get; set; }
		public Vector2 Scale { get; set; }
		private List<Particle> particles;
		private List<Texture2D> textures;

		public ParticleEngine (List<Texture2D> textures, Vector2 location, int count, Vector2? scale)
		{
			Count = count;
			EmitterLocation = location;
			Scale = scale ?? new Vector2 (1f);

			this.textures = textures;
			this.particles = new List<Particle>();
			random = new System.Random();
		}

		private Particle GenerateNewParticle()
		{
			Texture2D texture = textures [random.Next( textures.Count )];
			Vector2 position = EmitterLocation;
			Vector2 velocity = new Vector2 (
				0.2f * (float) (random.NextDouble() * 2 - 1),
				0.2f * (float) (random.NextDouble() * 2 - 1)
			);
			float angularVelocity = 0.1f * (float) (random.NextDouble() * 2 - 1);
			var particleScale = Scale * 0.5f;
			int ttl = 20 + random.Next( 40 );

			return new Particle (texture, position, velocity, 0f, angularVelocity, Color.White, particleScale, true, 0.95f, ttl);
		}

		public void Update()
		{
			if (Keyboard.GetState().IsKeyDown( Keys.W ))
			{
				for (int i = 0; i < Count; i++)
				{
					var newParticle = GenerateNewParticle();
					particles.Add( newParticle );
				}
			}

			for (int j = 0; j < particles.Count; j++)
			{
				particles [j].Update();
				if(particles[j].TTL <= 0)
				{
					particles.RemoveAt( j );
					j--;
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < particles.Count; i++)
			{
				particles [i].Draw( spriteBatch );
			}
		}

	}
}

