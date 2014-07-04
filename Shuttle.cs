using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;

namespace MyGame
{
	public class Shuttle
	{
		public Texture2D Texture { get; set; }
		public Vector2 Position { get; set; }
		public Vector2 EnginePosition { get; set; } 
		public Vector2 Velocity { get; set; }
		public float Angle { get; set; }
		public float AngularVelocity { get; set; }
		public Vector2 Origin { get; set; }
		public Color Color { get; set; }

		List<Texture2D> particleTextures;
		ParticleEngine particleEngine;

		float thrust = 0.2f;
		float turnSpeed = 0.05f;

		public Shuttle (Texture2D texture, Texture2D exhaust)
		{
			Texture = texture;
			Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
			EnginePosition = new Vector2 (
				Position.X - (float)Math.Cos(Angle) * Texture.Width / 2,
				Position.Y - (float)Math.Sin(Angle) * Texture.Width / 2
			);

			particleTextures = new List<Texture2D> (){ exhaust };
			particleEngine = new ParticleEngine (particleTextures, Position, 5);
		}

		public void UpdateShuttle()
		{
			KeyboardState currentState = Keyboard.GetState();

			AngularVelocity = currentState.IsKeyDown( Keys.A ) ? -turnSpeed
				: currentState.IsKeyDown( Keys.D ) ? turnSpeed
				: 0f;

			Angle += AngularVelocity;

			Vector2 modelVelocityAdd = Vector2.Zero;

			modelVelocityAdd.X = (float) Math.Sin( Angle );
			modelVelocityAdd.Y = -(float) Math.Cos( Angle );

			modelVelocityAdd *= currentState.IsKeyDown( Keys.W ) ? thrust : 0f;

			Velocity += modelVelocityAdd;

			if(currentState.IsKeyDown(Keys.R))
			{
				Position = Vector2.Zero;
				Velocity = Vector2.Zero;
				Angle = 0.0f;
			}

			Position += Velocity;

			EnginePosition = new Vector2 (
				Position.X - (float)Math.Cos(Angle - Math.PI/2) * Texture.Height / 2,
				Position.Y - (float)Math.Sin(Angle - Math.PI/2) * Texture.Height / 2
			);

			particleEngine.EmitterLocation = EnginePosition;
			particleEngine.Update();
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw( Texture, Position, null, null, Origin, Angle, null, null, SpriteEffects.None, 0f );
			particleEngine.Draw( spriteBatch );
		}
	}

	class ParticleEngine
	{
		private System.Random random;
		public int Count { get; set; }
		public Vector2 EmitterLocation { get; set; }
		private List<Particle> particles;
		private List<Texture2D> textures;

		public ParticleEngine (List<Texture2D> textures, Vector2 location, int count)
		{
			Count = count;
			EmitterLocation = location;
			this.textures = textures;
			this.particles = new List<Particle>();
			random = new System.Random();
		}

		private Particle GenerateNewParticle()
		{
			Texture2D texture = textures [random.Next( textures.Count )];
			Vector2 position = EmitterLocation;
			Vector2 velocity = new Vector2 (
				0.5f * (float) (random.NextDouble() * 2 - 1),
				0.5f * (float) (random.NextDouble() * 2 - 1)
			);
			float angle = 0;
			float angularVelocity = 0.1f * (float) (random.NextDouble() * 2 - 1);
			Vector2 size = new Vector2(0.4f,0.4f);
			int ttl = 20 + random.Next( 40 );

			return new Particle (texture, position, velocity, angle, angularVelocity, Color.White, size, 0.95f, ttl);
		}

		public void Update()
		{
			if (Keyboard.GetState().IsKeyDown( Keys.W ))
			{
				for (int i = 0; i < Count; i++)
				{
					particles.Add( GenerateNewParticle() );
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

