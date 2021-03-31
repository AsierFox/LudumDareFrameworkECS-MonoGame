using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Core.ECS.Components;
using Core.ECS.Components.Types;
using Core.ECS.Systems;

namespace Core.ECS.Entities
{
	class Character : Entity, IAnimationRenderable, IPhysicsBody
	{
		public Vector2 Position;

		public Rectangle BoundingBox { get; set; }

		public AnimatedSprite Prefab { get; set; }

		public RigitBody RigitBody { get; set; }

		public CharacterAttributes Attributes { get; set; }

		public IList<Dialog> Dialogs { get; set; }


		public Character()
		{
		}

		public override void Dispose()
		{
			Prefab.Dispose();

			base.Dispose();
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

		public RigitBody GetRigitBody()
		{
			return RigitBody;
		}

		public Rectangle GetBodundingBox()
		{
			return BoundingBox;
		}

		public void SetBoundingBox(Rectangle newBoundingBox)
		{
			BoundingBox = newBoundingBox;
		}

	}
}
