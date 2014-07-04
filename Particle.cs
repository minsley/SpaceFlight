using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame
{
	public class Particle
	{
		public Texture2D Texture { get; set; }
		public Vector2 Position { get; set; }
		public Vector2 Velocity { get; set; }
		public float Angle { get; set; }
		public float AngularVelocity { get; set; }
		public Color Color { get; set; }
		public Vector2 Size { get; set; }
		public float Shrink { get; set; }
		public int TTL { get; set; }

		public Particle (Texture2D texture, Vector2 position, Vector2? velocity, 
			float angle, float? angularVelocity, Color color, Vector2? size, float? shrink, int ttl)
		{
			Texture = texture;
			Position = position;
			Velocity = velocity ?? new Vector2(0f,0f);
			Angle = angle;
			AngularVelocity = angularVelocity ?? 0f;
			Color = color;
			Size = size ?? new Vector2(texture.Width, texture.Height);
			Shrink = shrink ?? 1f;
			TTL = ttl;
		}

		public void Update()
		{
			TTL--;
			Position += Velocity;
			Angle += AngularVelocity;
			Size *= Shrink;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			Rectangle sourceRect = new Rectangle (0, 0, Texture.Width, Texture.Height);
			Vector2 origin = new Vector2 (Texture.Width / 2, Texture.Height / 2);

			spriteBatch.Draw( Texture, Position, null, sourceRect, origin, Angle, Size, Color.White, SpriteEffects.None, 0f );
		}
	}
}

