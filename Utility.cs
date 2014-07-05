using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteFlight
{
	public static class Utility
	{
		public static Vector2 GetRandomDirection(Random randomGenerator)
		{
			return new Vector2 (
				GetRandomFloat(randomGenerator, 0f, 1f), 
				GetRandomFloat(randomGenerator, 0f, 1f)
			);
		}

		public static float GetRandomFloat(Random randomGenerator, float min, float max)
		{
			return (float)randomGenerator.NextDouble() * (max - min) + min;
		}
	}
}

