using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.ECS.Components
{
	class DebugRectangle : Component
	{
		private Texture2D _debugTexture;

		public DebugRectangle(GraphicsDevice graphicsDevice, Rectangle boundingBox,
			int borderWidth, Color borderColor)
		{
			_debugTexture = new Texture2D(graphicsDevice, boundingBox.Width, boundingBox.Height);

			BuildDebugTexture(borderWidth, borderColor);
		}

		public void Draw(SpriteBatch spriteBatch, Rectangle boundingBox)
		{
			spriteBatch.Draw(_debugTexture, new Vector2(boundingBox.X, boundingBox.Y), Color.White);
		}

		private void BuildDebugTexture(int borderWidth, Color borderColor)
		{
			Color[] colors = new Color[_debugTexture.Width * _debugTexture.Height];

			for (int x = 0; x < _debugTexture.Width; x++)
			{
				for (int y = 0; y < _debugTexture.Height; y++)
				{
					bool isBorder = false;

					for (int i = 0; i <= borderWidth; i++)
					{
						if (x == i || y == i
							|| x == _debugTexture.Width - 1 - i
							|| y == _debugTexture.Height - 1 - i)
						{
							colors[x + y * _debugTexture.Width] = borderColor;
							isBorder = true;
							break;
						}
					}

					if (!isBorder)
					{
						colors[x + y * _debugTexture.Width] = Color.Transparent;
					}
				}
			}

			_debugTexture.SetData(colors);
		}

		public override void Dispose()
		{
			_debugTexture.Dispose();

			base.Dispose();
		}

	}
}
