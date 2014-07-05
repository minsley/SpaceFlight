using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFlight
{
	public class Sprite
	{
		private Texture2D _texture;
		public float Height { get{return (float)_texture.Height;} }
		public float Width { get{return (float)_texture.Width;} }

		public Vector2 Location { get; set; }
		public Vector2 Velocity { get; set; }
		public float Angle { get; set; }
		public float AngularVelocity { get; set; }

		public Vector2 Origin { get; set; }
		public Color Filter { get; set; }
		public Vector2 Scale { get; set; }
		public Boolean EdgeWrap { get; set; }
		public Boolean Active { get; set; }

		public Rectangle HitBox {get{
				if (Active) {
					return new Rectangle (
						(int) Location.X, 
						(int) Location.Y, 
						(int) (Width * Scale.X), 
						(int) (Height * Scale.Y)
					);
				} else {
					return new Rectangle ();
				}
			}
		}

		public Sprite (Texture2D texture) : this(texture, null, null, null, null, null, null, null, null, null)
		{

		}

		public Sprite (Texture2D texture, Vector2? location, Vector2? velocity, float? angle, 
			float? angularVelocity, Vector2? origin, Color? filter, Vector2? scale, Boolean? edgeWrap, Boolean? active)
		{
			_texture = texture;
			Location = location ?? Vector2.Zero;
			Velocity = velocity ?? Vector2.Zero;
			Angle = angle ?? 0f;
			AngularVelocity = angularVelocity ?? 0f;
			Origin = origin ?? new Vector2 (Width / 2, Height / 2);
			Filter = filter ?? Color.White;
			Scale = scale ?? new Vector2(1f);
			EdgeWrap = edgeWrap ?? false;
			Active = active ?? true;
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw( _texture, Location, null, null, Origin, Angle, Scale, Filter, SpriteEffects.None, 0f );
		}

		public virtual void Update()
		{
			Angle += AngularVelocity;
			Location += Velocity;
		}

		public virtual void Update(Rectangle screenBounds)
		{
			Update();
			if (EdgeWrap)
				WrapEdgeLocation( screenBounds );
		}

		public bool IsOutOfBounds(Rectangle screenBounds)
		{
			if (
				Location.X > screenBounds.Width + Width || 
				Location.X < 0f - Width || 
				Location.Y > screenBounds.Height + Height || 
				Location.Y < 0f - Height
			)
				return true;
			return false;
		}

		public void SetRandomLocation(Rectangle screenBounds, Random randGenerator)
		{
			Location = new Vector2 (
				randGenerator.Next( screenBounds.Width ),
				randGenerator.Next( screenBounds.Height )
			);
		}

		public void WrapEdgeLocation(Rectangle screenBounds)
		{
			var newX = Location.X;
			if (Location.X > screenBounds.Width)
			{
				newX = 0;
			} else if (Location.X < 0){
				newX = screenBounds.Width;
			}

			var newY = Location.Y;
			if (Location.Y > screenBounds.Height)
			{
				newY = 0;
			} else if (Location.Y < 0){
				newY = screenBounds.Height;
			}

			Location = new Vector2 (newX, newY);
		}

		public void Activate()
		{
			Active = true;
			Filter = Color.White;
		}

		public void Deactivate()
		{
			Active = false;
			Filter = Color.Transparent;
		}

		public void Remove()
		{
			Location = new Vector2 (-1f * Width, -1f * Height);
			Deactivate();
		}
	}
}

