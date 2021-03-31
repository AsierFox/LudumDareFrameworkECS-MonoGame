using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Core.ECS.Components
{
	class Sprite : Component
	{
		public float Opacity { get; set; }

		public float Rotation { get; set; }

		public float Scale { get; set; }

		public Texture2D Texture { get; set; }

		public Vector2 Origin { get; set; }


		public Sprite(Texture2D texture)
		{
			Texture = texture;
		}

		public override void Dispose()
		{
			Texture.Dispose();

			base.Dispose();
		}

		public object Clone()
		{
			return MemberwiseClone();
		}

	}
}
