using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Core.ECS.Components;

namespace Core.ECS.Entities
{
	class Particle : Entity
	{
		public Vector2 Position { get; set; }
		
		public Vector2 Velocity { get; set; }

		public Sprite Prefab { get; private set; }

		
		public Particle(Sprite prefab)
		{
			Prefab = prefab;
		}

		public Particle(Texture2D texture)
		{
			Prefab = new Sprite(texture);
		}

		public Particle(GraphicsDevice graphicsDevice, int pixelSize, Color color)
		{
			if (pixelSize % 2 != 0)
			{
				throw new Exception("pixelSize must be even!");
			}

			Texture2D texture = new Texture2D(graphicsDevice, pixelSize, pixelSize);

			Color[] colors = new Color[pixelSize * pixelSize];
			Array.Fill(colors, color);
			texture.SetData(colors);
			Prefab = new Sprite(texture);
		}

	}
}
