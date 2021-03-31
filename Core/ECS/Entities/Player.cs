using Microsoft.Xna.Framework;
using Core.ECS.Components;
using Core.ECS.Components.Types;

namespace Core.ECS.Entities
{
	class Player : Entity,
		IAnimationRenderable,
		IPhysicsBody,
		IMovableActions,
		IAttacker,
		IInteractable
	{
		public Vector2 Origin { get; set; }

		public Vector2 Position { get; set; }
		
		public Rectangle BoundingBox { get; set; }
		
		public InteractableBoundingBox InteractableBoundingBox { get; set; }

		public AttackAbility AttackAbility { get; set; }

		public AnimatedSprite Prefab { get; set; }

		public RigitBody RigitBody { get; set; }

		public MovableActions MovableActions { get; private set; }

		public IMovableActions.FacingDirection FacingDirection { get; set; }

		public CharacterAttributes Attributes { get; set; }


		public Player()
		{
			MovableActions = new MovableActions();
		}

		public void SetComponent(AnimatedSprite spriteAnimations)
		{
			Prefab = spriteAnimations;
		}

		public void SetComponent(RigitBody rigitBody)
		{
			RigitBody = rigitBody;
		}

		public void SetComponent(AttackAbility attackAbility)
		{
			AttackAbility = attackAbility;
		}

		public override void Dispose()
		{
			Prefab.Dispose();

			base.Dispose();
		}

		public AnimatedSprite GetAnimatedSprite()
		{
			return Prefab;
		}

		public Vector2 GetPosition()
		{
			return Position;
		}

		public void SetPosition(Vector2 newPosition)
		{
			Position = newPosition;
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

		public InteractableBoundingBox GetInteractableBoundingBox()
		{
			return InteractableBoundingBox;
		}

		public void SetInteractableBoundingBox(InteractableBoundingBox interactableBoundingBox)
		{
			InteractableBoundingBox = interactableBoundingBox;
		}

		public float GetSpeed()
		{
			return Attributes.Speed;
		}

		public MovableActions GetMovableActions()
		{
			return MovableActions;
		}

		public IMovableActions.FacingDirection GetFacingDirection()
		{
			return FacingDirection;
		}

		public void SetFacingDirection(IMovableActions.FacingDirection facingDirection)
		{
			FacingDirection = facingDirection;
		}

		public AttackAbility GetAttackAbility()
		{
			return AttackAbility;
		}

		public void SetAttackAbility(AttackAbility attackAbility)
		{
			AttackAbility = attackAbility;
		}

		public CharacterAttributes GetCharacterAttributes()
		{
			return Attributes;
		}

	}
}
