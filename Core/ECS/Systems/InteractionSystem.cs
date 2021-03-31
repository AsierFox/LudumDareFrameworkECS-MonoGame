using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Collections.Generic;
using Core.ECS.Components;
using Core.ECS.Components.Types;
using Core.ECS.Entities;
using Core
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	.Managers;

namespace Core.ECS.Systems
{
	class InteractionSystem : System
	{

		private CollisionManager _collisionManager;

		public InteractionSystem()
		{
			_collisionManager = new CollisionManager();
		}

		public IEnumerable<Dialog> CheckNPCInteractions(IEnumerable<Player> players, IEnumerable<Character> npcs)
		{
			foreach (Player player in players)
			{
				foreach (Character npc in npcs)
				{
					if (_collisionManager.CheckCollision(player.GetInteractableBoundingBox().BoundingBox, npc.BoundingBox))
					{
						return npc.Dialogs;
					}
				}
			}

			return null;
		}

		internal void Update(GameTime gameTime, IEnumerable<IInteractable> interactables)
		{
			foreach (IInteractable interactale in interactables)
			{
				Rectangle newCollider = new Rectangle();

				newCollider.Width = interactale.GetInteractableBoundingBox().BoundingBox.Width;
				newCollider.Height = interactale.GetInteractableBoundingBox().BoundingBox.Height;

				Vector4 offset = interactale.GetInteractableBoundingBox().Offset;

				switch (interactale.GetFacingDirection())
				{
					case IMovableActions.FacingDirection.NORTH:

						newCollider.X = (int)offset.Y + (int)interactale.GetPosition().X;
						newCollider.Y = (int)interactale.GetPosition().Y - newCollider.Height;
						break;

					case IMovableActions.FacingDirection.SOUTH:

						newCollider.X = (int)offset.Z + (int)interactale.GetPosition().X;
						newCollider.Y = (int)interactale.GetPosition().Y + interactale.GetBodundingBox().Height;
						break;

					case IMovableActions.FacingDirection.WEST:

						newCollider.X = (int)interactale.GetPosition().X - newCollider.Width;
						newCollider.Y = (int)offset.X + (int)interactale.GetPosition().Y;
						break;

					case IMovableActions.FacingDirection.EAST:

						newCollider.X = (int)interactale.GetPosition().X + interactale.GetBodundingBox().Width;
						newCollider.Y = (int)offset.W + (int)interactale.GetPosition().Y;
						break;
				}
				
				interactale.SetInteractableBoundingBox(
					new InteractableBoundingBox(newCollider, interactale.GetInteractableBoundingBox().Offset));
			}
		}

		internal void DrawDebug(SpriteBatch spriteBatch, IEnumerable<IInteractable> interactables)
		{
			foreach (IInteractable interactable in interactables)
			{
				spriteBatch.DrawRectangle(interactable.GetInteractableBoundingBox().BoundingBox, Color.Green);
			}
		}

		public override void Clear()
		{
		}

	}
}
