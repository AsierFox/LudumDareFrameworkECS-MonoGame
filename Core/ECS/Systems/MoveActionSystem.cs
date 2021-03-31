using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Core.ECS.Components;
using Core.ECS.Components.Types;

namespace Core.ECS.Systems
{
	class MoveActionSystem : System
	{
		public void EstablishAction(IMovableActions movableEntity, IMovableActions.Actions actionToPerform)
		{
			movableEntity.GetMovableActions().ActionToComplete = actionToPerform;
		}

		public void Update(GameTime gameTime, IEnumerable<IMovableActions> movableEntities)
		{
			foreach (IMovableActions movableEntity in movableEntities)
			{
				IMovableActions.Actions actionToComplete = movableEntity.GetMovableActions().ActionToComplete;

				if (actionToComplete == IMovableActions.Actions.IDLE)
				{
					continue;
				}

				switch (actionToComplete)
				{
					case IMovableActions.Actions.MOVE_UP:

						movableEntity.GetRigitBody().SetForceY(-movableEntity.GetSpeed());

						if (movableEntity.GetMovableActions().PreviousAction != IMovableActions.Actions.MOVE_UP)
						{
							movableEntity.SetFacingDirection(IMovableActions.FacingDirection.NORTH);
							movableEntity.GetAnimatedSprite().Play(AnimatedSprite.Animations.MOVE_UP);
						}
						break;

					case IMovableActions.Actions.MOVE_DOWN:

						movableEntity.GetRigitBody().SetForceY(movableEntity.GetSpeed());

						if (movableEntity.GetMovableActions().PreviousAction != IMovableActions.Actions.MOVE_DOWN)
						{
							movableEntity.SetFacingDirection(IMovableActions.FacingDirection.SOUTH);
							movableEntity.GetAnimatedSprite().Play(AnimatedSprite.Animations.MOVE_DOWN);
						}
						break;

					case IMovableActions.Actions.MOVE_RIGHT:

						movableEntity.GetRigitBody().SetForceX(movableEntity.GetSpeed());

						if (movableEntity.GetMovableActions().PreviousAction != IMovableActions.Actions.MOVE_RIGHT)
						{
							movableEntity.SetFacingDirection(IMovableActions.FacingDirection.EAST);
							movableEntity.GetAnimatedSprite().Play(AnimatedSprite.Animations.MOVE_RIGHT);
						}
						break;

					case IMovableActions.Actions.MOVE_LEFT:

						movableEntity.GetRigitBody().SetForceX(-movableEntity.GetSpeed());

						if (movableEntity.GetMovableActions().PreviousAction != IMovableActions.Actions.MOVE_LEFT)
						{
							movableEntity.SetFacingDirection(IMovableActions.FacingDirection.WEST);
							movableEntity.GetAnimatedSprite().Play(AnimatedSprite.Animations.MOVE_LEFT);
						}
						break;
				}

				movableEntity.GetMovableActions().PreviousAction = actionToComplete;
				movableEntity.GetMovableActions().ActionToComplete = IMovableActions.Actions.IDLE;
			}
		}

		public override void Clear()
		{
		}

	}
}
