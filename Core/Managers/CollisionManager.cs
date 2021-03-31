using Microsoft.Xna.Framework;

namespace Core.Managers
{
	class CollisionManager
	{
		public enum CollisionDirection
		{
			UP,
			DOWN,
			RIGHT,
			LEFT
		}

		public bool CheckCollision(Rectangle a, Rectangle b)
		{
			return a.X + a.Width > b.X
				&& b.X + b.Width > a.X
				&& a.Y + a.Height > b.Y
				&& b.Y + b.Height > a.Y;
		}

		public CollisionDirection GetRectDepthSideCollision(Rectangle a, Rectangle b)
		{
			int dx = (a.X + (a.Width >> 1)) - (b.X + (b.Width >> 1));
			int dy = (a.Y + (a.Height >> 1)) - (b.Y + (b.Height >> 1));
			int width = (a.Width + b.Width) >> 1;
			int height = (a.Height + b.Height) >> 1;
			int crossWidth = width * dy;
			int crossHeight = height * dx;

			if (crossWidth > crossHeight)
			{
				if (crossWidth > -crossHeight)
				{
					return CollisionDirection.UP;
				}
				else
				{
					return CollisionDirection.RIGHT;
				}
			}
			else
			{
				if (crossWidth > -crossHeight)
				{
					return CollisionDirection.LEFT;
				}
				else
				{
					return CollisionDirection.DOWN;
				}
			}
		}

	}
}
