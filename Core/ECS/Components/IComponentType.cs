using Microsoft.Xna.Framework;
using Core.ECS.Entities;

namespace Core.ECS.Components.Types
{
	// TODO Add SetComponents methods to interfaces (?)

	interface IComponentType
	{
		Vector2 GetPosition();

		void SetPosition(Vector2 newPosition);
	}

	interface IAnimationRenderable : IComponentType
	{
		AnimatedSprite GetAnimatedSprite();
	}

	interface IPhysicsBody : IComponentType
	{
		RigitBody GetRigitBody();

		Rectangle GetBodundingBox();

		void SetBoundingBox(Rectangle newBoundingBox);
	}
	interface IMovableActions : IAnimationRenderable, IPhysicsBody
	{
		public enum FacingDirection
		{
			NORTH,
			SOUTH,
			WEST,
			EAST
		}
		public enum Actions
		{
			IDLE,
			MOVE_UP,
			MOVE_DOWN,
			MOVE_RIGHT,
			MOVE_LEFT
		}

		float GetSpeed();

		MovableActions GetMovableActions();

		FacingDirection GetFacingDirection();

		void SetFacingDirection(FacingDirection facingDirection);
	}

	interface IAttacker : IMovableActions
	{
		AttackAbility GetAttackAbility();

		void SetAttackAbility(AttackAbility newCollider);

		CharacterAttributes GetCharacterAttributes();
	}

	interface IInteractable : IMovableActions
	{
		InteractableBoundingBox GetInteractableBoundingBox();

		void SetInteractableBoundingBox(InteractableBoundingBox newCollider);
	}

}
