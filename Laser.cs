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
	public class Laser : Particle
	{
		public Laser(Texture2D texture) : base(texture)
		{
			EdgeWrap = true;
			TTL = 20;
		}

		public void Update()
		{
			base.Update();
		}

		public void Update(GameObjects gameObjects)
		{
			base.Update(gameObjects.ScreenSize);
		}
	}
}
