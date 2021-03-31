using Microsoft.Xna.Framework;
using Core.ECS.Components;
using Core.ECS.Components.Types;

namespace Core.ECS.Entities
{
	class Enemy : Entity, IAnimationRenderable, IPhysicsBody, IAttacker
	{
		public Vector2 Position;

		public Rectangle BoundingBox { get; set; }

		public AnimatedSprite Prefab { get; set; }

		public RigitBody RigitBody { get; set; }

		public CharacterAttributes Attributes { get; set; }


		public Enemy()
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

		public CharacterAttributes GetCharacterAttributes()
		{
			throw new System.NotImplementedException();
		}

		public float GetSpeed()
		{
			throw new System.NotImplementedException();
		}

		public MovableActions GetMovableActions()
		{
			throw new System.NotImplementedException();
		}

		public IMovableActions.FacingDirection GetFacingDirection()
		{
			throw new System.NotImplementedException();
		}

		public void SetFacingDirection(IMovableActions.FacingDirection facingDirection)
		{
			throw new System.NotImplementedException();
		}

		AttackAbility IAttacker.GetAttackAbility()
		{
			throw new System.NotImplementedException();
		}

		void IAttacker.SetAttackAbility(AttackAbility newCollider)
		{
			throw new System.NotImplementedException();
		}

	}
}
