using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Collections.Generic;
using Core.ECS.Components.Types;
using Core.ECS.Entities;

namespace Core.ECS.Systems
{
	class CombatSystem : System
	{

		// TODO Pass ability to trigger
		public void TriggerAttack(IAttacker attacker)
		{
			attacker.GetAttackAbility().IsActive = true;
		}

		public void Update(GameTime gameTime, IEnumerable<IAttacker> attackers)
		{
			foreach (IAttacker attacker in attackers)
			{
				if (!attacker.GetAttackAbility().IsActive)
				{
					continue;
				}

				Rectangle newCollider = new Rectangle
				{
					Width = attacker.GetAttackAbility().Collider.Width,
					Height = attacker.GetAttackAbility().Collider.Height
				};

				switch (attacker.GetFacingDirection())
				{
					case IMovableActions.FacingDirection.NORTH:

						newCollider.X = (int) attacker.GetPosition().X;
						newCollider.Y = (int) attacker.GetPosition().Y - newCollider.Height;
						break;
					
					case IMovableActions.FacingDirection.SOUTH:

						newCollider.X = (int) attacker.GetPosition().X;
						newCollider.Y = (int) attacker.GetPosition().Y + attacker.GetBodundingBox().Height;
						break;
					
					case IMovableActions.FacingDirection.WEST:

						newCollider.X = (int) attacker.GetPosition().X - newCollider.Width;
						newCollider.Y = (int) attacker.GetPosition().Y;
						break;
					
					case IMovableActions.FacingDirection.EAST:

						newCollider.X = (int) attacker.GetPosition().X + attacker.GetBodundingBox().Width;
						newCollider.Y = (int) attacker.GetPosition().Y;
						break;
				}

				attacker.GetAttackAbility().Collider = newCollider;
			}
		}

		public void DrawDebug(SpriteBatch spriteBatch, IEnumerable<IAttacker> attackers)
		{
			foreach (IAttacker attacker in attackers)
			{
				if (attacker.GetAttackAbility().IsActive)
				{
					spriteBatch.DrawRectangle(attacker.GetAttackAbility().Collider, Color.Red);
					attacker.GetAttackAbility().IsActive = false;
				}
			}
		}

		public void ResolveCombatAttack(CombatCollision combatCollision)
		{
		}

		public override void Clear()
		{
		}

	}
}
