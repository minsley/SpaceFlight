using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace SpriteFlight
{
	public class Asteroid : Sprite
	{
		public Asteroid (Texture2D texture, Vector2 location) : base(texture)
		{
			Location = location;
			EdgeWrap = true;
		}

		public void Update(GameObjects gameObjects)
		{
			base.Update(gameObjects.ScreenSize);
		}
	}
}

