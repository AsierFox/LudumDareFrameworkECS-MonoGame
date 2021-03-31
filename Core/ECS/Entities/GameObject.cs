using Microsoft.Xna.Framework;
using Core.ECS.Components;
using Core.ECS.Components.Types;

namespace Core.ECS.Entities
{
	class GameObject : Entity, IAnimationRenderable
	{

		public Vector2 Position { get; set; }

		public Rectangle BoundingBox { get; set; }

		public AnimatedSprite Prefab { get; set; }

		
		public GameObject()
		{
		}

		public Vector2 GetPosition()
		{
			return Position;
		}

		public void SetPosition(Vector2 newPosition)
		{
			Position = newPosition;
		}

		public AnimatedSprite GetAnimatedSprite()
		{
			return Prefab;
		}

		public override void Dispose()
		{
			Prefab.Dispose();

			base.Dispose();
		}

	}
}
