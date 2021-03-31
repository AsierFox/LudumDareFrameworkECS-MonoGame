using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Collections.Generic;
using Core.ECS.Components;
using Core.ECS.Components.Types;
using Core.ECS.Entities;
using Core.Managers;

namespace Core.ECS.Systems
{
	class PhysicsSystem : System
	{
		private IEnumerable<TiledMapRectCollider> _tiledMapColliders;

		private CollisionManager _collisionManager;


		public PhysicsSystem(IEnumerable<TiledMapRectCollider> tiledMapColliders)
		{
			_collisionManager = new CollisionManager();

			_tiledMapColliders = tiledMapColliders;
		}

		public void RefreshForce(IEnumerable<IPhysicsBody> movableEntities)
		{
			foreach (IPhysicsBody physicsBody in movableEntities)
			{
				physicsBody.GetRigitBody().RemoveForce();
			}
		}

		public void UpdateCollisions(GameTime gameTime, IEnumerable<IPhysicsBody> movableEntities)
		{
			foreach (IPhysicsBody physicsBody in movableEntities)
			{
				physicsBody.GetRigitBody().Update(gameTime);

				Vector2 entityPosition = physicsBody.GetPosition();
				Rectangle entityBoundingBox = physicsBody.GetBodundingBox();

				float lastSafePositionX = entityPosition.X;
				entityPosition.X += physicsBody.GetRigitBody().NewPosition.X;
				entityBoundingBox.X = (int) entityPosition.X;

				// TODO Only check rects that are near

				foreach (TiledMapRectCollider mapCollider in _tiledMapColliders)
				{
					if (_collisionManager.CheckCollision(entityBoundingBox, mapCollider.Collider))
					{
						entityPosition.X = lastSafePositionX;
						break;
					}
				}

				float lastSafePositionY = entityPosition.Y;
				entityPosition.Y += physicsBody.GetRigitBody().NewPosition.Y;
				entityBoundingBox.Y = (int) entityPosition.Y;

				foreach (TiledMapRectCollider mapCollider in _tiledMapColliders)
				{
					if (_collisionManager.CheckCollision(entityBoundingBox, mapCollider.Collider))
					{
						entityPosition.Y = lastSafePositionY;
						break;
					}
				}

				physicsBody.SetPosition(entityPosition);
				physicsBody.SetBoundingBox(entityBoundingBox);
			}
		}

		public void DrawMapColliders(SpriteBatch spriteBatch, IEnumerable<IPhysicsBody> movableEntities)
		{
			foreach (IPhysicsBody physicsBody in movableEntities)
			{
				Color rectColor = Color.Red;

				if (physicsBody is Player)
				{
					rectColor = Color.Cyan;
				}
				else if (physicsBody is Character)
				{
					rectColor = Color.LightGreen;
				}
				else if (physicsBody is Enemy)
				{
					rectColor = Color.Violet;
				}
				else if (physicsBody is GameObject)
				{
					rectColor = Color.Fuchsia;
				}

				spriteBatch.DrawRectangle(physicsBody.GetBodundingBox(), rectColor);
			}

			foreach (TiledMapRectCollider mapCollider in _tiledMapColliders)
			{
				spriteBatch.DrawRectangle(mapCollider.Collider, Color.Red);
			}
		}

		public override void Clear()
		{
		}

	}
}
