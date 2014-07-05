using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFlight
{
	public class Particle : Sprite
	{
		public float? Shrink { get; set; }
		public int? TTL { get; set; }

		public Particle(Texture2D texture) : this(texture, null, null, null, null, null, null, null, null, null)
		{

		}

		public Particle (Texture2D texture, Vector2? location, Vector2? velocity, float? angle, 
			float? angularVelocity, Color? filter, Vector2? scale, bool? edgeWrap, float? shrink, int? ttl) 
			: base(texture, location, velocity, angle, angularVelocity, null, filter, scale, edgeWrap, null)
		{
			Shrink = shrink;
			TTL = ttl;
		}

		public override void Update()
		{
			if(TTL.HasValue)TTL--;
			if(Shrink.HasValue)Scale *= Shrink.Value;

			base.Update();
		}

		public override void Update(Rectangle screenBounds)
		{
			base.Update(screenBounds);
		}
	}
}

