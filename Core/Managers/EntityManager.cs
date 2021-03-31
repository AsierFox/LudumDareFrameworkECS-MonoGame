using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using Core.ECS.Components.Types;
using Core.ECS.Entities;

namespace Core.Managers
{
	class EntityManager
	{
		public IDictionary<int, Player> Players { get; private set; }
		public IDictionary<int, Character> Npcs { get; private set; }
		public IDictionary<int, Enemy> Enemies { get; private set; }
		public IDictionary<int, GameObject> Objects { get; private set; }

		private int clientPlayerId;
		private int _lastEntityId;
		
		public EntityManager()
		{
			Players = new Dictionary<int, Player>();
			Npcs = new Dictionary<int, Character>();
			Enemies = new Dictionary<int, Enemy>();
			Objects = new Dictionary<int, GameObject>();
			clientPlayerId = 0;
			_lastEntityId = 0;
		}

		public int CreateEntity(Entity entity)
		{
			_lastEntityId++;

			entity.ID = _lastEntityId;

			if (entity is Player)
			{
				Players.Add(entity.ID, entity as Player);
			}
			else if (entity is Character)
			{
				Npcs.Add(entity.ID, entity as Character);
			}
			else if (entity is Enemy)
			{
				Enemies.Add(entity.ID, entity as Enemy);
			}
			else if (entity is GameObject)
			{
				Objects.Add(entity.ID, entity as GameObject);
			}
			else
			{
				throw new Exception("Entity could not be created!");
			}

			return entity.ID;
		}

		public void CreateClientPlayer(Player clientPlayer)
		{
			if (clientPlayerId != 0)
			{
				throw new Exception("Client Player already created!");
			}
			
			clientPlayerId = CreateEntity(clientPlayer);
		}

		public void RemoveEntity(Entity entityToRemove)
		{
			bool wasRemoved = false;

			if (entityToRemove is Player)
			{
				wasRemoved = Players.Remove(entityToRemove.ID);

				if (wasRemoved)
				{
					clientPlayerId = 0;
				}
			}
			else if (entityToRemove is Character)
			{
				wasRemoved = Npcs.Remove(entityToRemove.ID);
			}
			else if (entityToRemove is Enemy)
			{
				wasRemoved = Enemies.Remove(entityToRemove.ID);
			}
			else if (entityToRemove is GameObject)
			{
				wasRemoved = Objects.Remove(entityToRemove.ID);
			}

			if (!wasRemoved)
			{
				throw new Exception("The entity could not be founded");
			}
		}

		public Player GetClientPlayer()
		{
			if (0 == clientPlayerId || Players.Count <= 0)
			{
				throw new Exception("Player is not created");
			}

			return Players[clientPlayerId];
		}

		// TODO Change To IMovableAction when Enemies and Characters implement it
		public IEnumerable<IPhysicsBody> GetMovableEntities()
		{
			return Players.Values.Cast<IPhysicsBody>()
				.Concat(Npcs.Values)
				.Concat(Enemies.Values)
				.ToList();
		}

		public IEnumerable<IAnimationRenderable> GetRenderableEntities()
		{
			return Players.Values.Cast<IAnimationRenderable>()
				.Concat(Npcs.Values)
				.Concat(Enemies.Values)
				.Concat(Objects.Values)
				.ToList();
		}

		public IEnumerable<IInteractable> GetInteractableEntities()
		{
			return Players.Values.Cast<IInteractable>()
				.ToList();
		}

		public IEnumerable<IAttacker> GetAttackerEntities()
		{
			return Players.Values.Cast<IAttacker>()
				.Concat(Enemies.Values)
				.ToList();
		}

		public IEnumerable<Entity> GetAllEntities()
		{
			return Players.Values.Cast<Entity>()
				.Concat(Npcs.Values)
				.Concat(Enemies.Values)
				.Concat(Objects.Values)
				.ToList();
		}

		public void Dispose()
		{
			foreach (Entity entity in GetAllEntities())
			{
				entity.Dispose();
			}

			_lastEntityId = 0;
		}

	}
}
