using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Core.ECS.Components
{
	class TiledMapRectCollider : Component
	{

		private RectangleF _colliderF;

		public RectangleF ColliderF {
			get { return _colliderF; }
			set
			{
				_colliderF = value;

				Collider = new Rectangle(
					(int) value.X, (int) value.Y,
					(int) value.Width, (int) value.Height);
			}
		}

		public Rectangle Collider { get; private set; }

	}
}
