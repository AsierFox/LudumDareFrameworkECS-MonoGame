using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;
using System.Linq;
using Core.ECS.Components;
using Core.ECS.Entities;

namespace Core.Managers
{
	class TiledMapManager
	{
		public interface ITiledMapProperties
		{
			const string SPAWN_PROPERTY = "spawn"; // TODO Change by SPAWN
			const string TYPE_PROPERTY = "type";

			public interface IPlayers
			{
				const string SPAWN_VALUE = "player";
			}

			public interface IObjects
			{
				const string SPAWN_VALUE = "object";

				const string HEALTH_POTION = "health-potion";
			}

			public interface ICharacters
			{
				const string SPAWN_VALUE = "npc";
				const string DIALOG_NAME = "dialog";
			}

			public interface IDialogs
			{
				const string BREAK_LINE = "\n";
				const string SPLIT_DIALOGS = "\n\n";
			}

			public interface IEnemies
			{
				const string SPAWN_VALUE = "enemy";
			}

			public interface IMap
			{
				const string SPAWN_VALUE = "interaction";

				const string SWITCH_MAP = "switch-mapName";
			}

		}

		private IEnumerable<TiledMapObject> _tiledMapObjects;

		// IEnumerable is better to query collections, and we only have to
		// add/remove colliders when a new map is loaded.
		public IEnumerable<TiledMapRectCollider> TiledMapRectColliders { get; private set; }


		public TiledMapManager(TiledMap tiledMap)
		{
			GetTiledMapObjects(tiledMap);

			BuildMapColliders();
		}

		private void GetTiledMapObjects(TiledMap tiledMap)
		{
			_tiledMapObjects = tiledMap.ObjectLayers
				.SelectMany(objectLayer => objectLayer.Objects)
				.Where(tiledMapObject => tiledMapObject is TiledMapRectangleObject);
		}

		public void BuildMapColliders()
		{
			IList<TiledMapRectCollider> tiledMapRectColliders = new List<TiledMapRectCollider>();

			foreach (TiledMapObject tiledMapObject in _tiledMapObjects)
			{
				TiledMapRectCollider newRectCollider = new TiledMapRectCollider
				{
					ColliderF = new RectangleF(
						tiledMapObject.Position.X, tiledMapObject.Position.Y,
						tiledMapObject.Size.Width, tiledMapObject.Size.Height)
				};

				IDictionary<string, string> colliderProperties = tiledMapObject.Properties;

				if (!colliderProperties.ContainsKey(ITiledMapProperties.SPAWN_PROPERTY))
				{
					tiledMapRectColliders.Add(newRectCollider);
				}
			}

			TiledMapRectColliders = tiledMapRectColliders;
		}

		public void GenerateTiledMapEntities(ContentManager content, EntityManager entityManager)
		{
			foreach (TiledMapObject tiledMapObject in _tiledMapObjects)
			{
				IDictionary<string, string> colliderProperties = tiledMapObject.Properties;

				if (colliderProperties.ContainsKey(ITiledMapProperties.SPAWN_PROPERTY))
				{
					switch (colliderProperties[ITiledMapProperties.SPAWN_PROPERTY])
					{
						case ITiledMapProperties.ICharacters.SPAWN_VALUE:

							//Character npcEntity = new Character();

							//AnimatedSprite catAnimations = new AnimatedSprite(
							//	content.Load<Texture2D>("assets/textures/cat"), 1, 1, 1, 3);
							//catAnimations.RegisterAnimation(new SpriteAnimation("idle", 0, 1));
							//catAnimations.Play("idle");

							//npcEntity.Position = new Vector2(tiledMapObject.Position.X, tiledMapObject.Position.Y);
							//npcEntity.Prefab = catAnimations;
							//npcEntity.RigitBody = new RigitBody();
							//npcEntity.BoundingBox = new Rectangle(
							//	(int) npcEntity.Position.X, (int) npcEntity.Position.Y,
							//	(int) (npcEntity.Prefab.FrameWidth * npcEntity.Prefab.Scale),
							//	(int) (npcEntity.Prefab.FrameHeight * npcEntity.Prefab.Scale));

							//if (colliderProperties.ContainsKey(ITiledMapProperties.ICharacters.DIALOG_NAME))
							//{
							//	if ( null == colliderProperties[ITiledMapProperties.ICharacters.DIALOG_NAME])
							//	{
							//		throw new Exception("The Dialog cannot be null");
							//	}

							//	IList<Dialog> dialogs = new List<Dialog>();

							//	string[] dialogTexts =
							//		colliderProperties[ITiledMapProperties.ICharacters.DIALOG_NAME]
							//		.Split(ITiledMapProperties.IDialogs.SPLIT_DIALOGS);

							//	foreach (string dialogText in dialogTexts)
							//	{
							//		Dialog newDialog = new Dialog();
							//		newDialog.Text = dialogText;

							//		dialogs.Add(newDialog);
							//	}

							//	npcEntity.Dialogs = dialogs;
							//}

							//entityManager.CreateEntity(npcEntity);
							
							break;

						case ITiledMapProperties.IObjects.SPAWN_VALUE:

							//Character npcEntity2 = new Character();

							//AnimatedSprite catAnimations2 = new AnimatedSprite(
							//	content.Load<Texture2D>("assets/textures/cat"), 1, 1, 1, 3);
							//catAnimations2.RegisterAnimation(new SpriteAnimation("idle", 0, 1));
							//catAnimations2.Play("idle");

							//npcEntity2.Position = new Vector2(tiledMapObject.Position.X, tiledMapObject.Position.Y);
							//npcEntity2.Prefab = catAnimations2;
							//npcEntity2.RigitBody = new RigitBody();
							//npcEntity2.BoundingBox = new Rectangle(
							//	(int)npcEntity2.Position.X, (int)npcEntity2.Position.Y,
							//	(int)(npcEntity2.Prefab.FrameWidth * npcEntity2.Prefab.Scale),
							//	(int)(npcEntity2.Prefab.FrameHeight * npcEntity2.Prefab.Scale));

							//entityManager.CreateEntity(npcEntity2);

							break;

						default:

							throw new Exception("Tiled Map Invocation error, property '"
								+ colliderProperties[ITiledMapProperties.SPAWN_PROPERTY] + "' not found!");
					}
				}
			}
		}

	}
}
