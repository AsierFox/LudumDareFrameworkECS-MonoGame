using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using System.Collections.Generic;
using Core.ECS.Components;
using Core.ECS.Components.Types;
using Core.ECS.Entities;
using Core.Managers;
using Core.ECS.Systems;
using Core.Game;

namespace Core.Scenes
{
	class PlayScene : BaseScene
	{
		private BitmapFont _dialogFont;

		private TiledMap _tiledMapEntity;
		
		private EntityManager _entityManager;
		private TiledMapManager _tiledMapManager;
		private AStarPFManager _aStartPFManager;

		private InputManager _inputManager;
		private TiledMapRenderer _tiledMapRendererSystem;
		private PhysicsSystem _physicsSystem;
		private AnimationRenderSystem _animationRenderSystem;
		private DialogSystem _dialogSystem;
		private MoveActionSystem _moveActionSystem;
		private InteractionSystem _interactionSystem;
		private CombatSystem _combatSystem;

		public PlayScene(IGameCore gameCore)
			: base(gameCore)
		{
			_entityManager = new EntityManager();
		}

		public override void Initialize()
		{
			_inputManager = new InputManager(Keyboard.GetState());
			_animationRenderSystem = new AnimationRenderSystem();
			_moveActionSystem = new MoveActionSystem();
			_combatSystem = new CombatSystem();
			_interactionSystem = new InteractionSystem();
		}

		public override void LoadContent()
		{
			_dialogFont = GameCore.GetContentManager().Load<BitmapFont>("assets/montserrat-32");

			// Player Entity
			Player playerEntity = new Player();

			AnimatedSprite playerAnimations = new AnimatedSprite(
				GameCore.GetContentManager().Load<Texture2D>("assets/player"), 2, 2, 1, 3);
			playerAnimations.RegisterAnimation(new SpriteAnimation(AnimatedSprite.Animations.IDLE, 0, 2));
			playerAnimations.RegisterAnimation(new SpriteAnimation(AnimatedSprite.Animations.MOVE_UP, 0, 2));
			playerAnimations.RegisterAnimation(new SpriteAnimation(AnimatedSprite.Animations.MOVE_DOWN, 0, 2));
			playerAnimations.RegisterAnimation(new SpriteAnimation(AnimatedSprite.Animations.MOVE_RIGHT, 1, 2));
			playerAnimations.RegisterAnimation(new SpriteAnimation(AnimatedSprite.Animations.MOVE_LEFT, 1, 2, true));

			playerAnimations.Play(AnimatedSprite.Animations.IDLE);

			playerEntity.Position = Vector2.Zero;
			playerEntity.FacingDirection = IMovableActions.FacingDirection.SOUTH;
			playerEntity.Attributes = new CharacterAttributes(20000f);

			playerEntity.SetComponent(new RigitBody(false, 2f, 3f));
			playerEntity.SetComponent(playerAnimations);
			playerEntity.SetBoundingBox(new Rectangle(
				(int) playerEntity.Position.X, (int) playerEntity.Position.Y,
				(int) (playerEntity.Prefab.FrameWidth * playerEntity.Prefab.Scale),
				(int) (playerEntity.Prefab.FrameHeight * playerEntity.Prefab.Scale)));

			playerEntity.SetInteractableBoundingBox(new InteractableBoundingBox(
				new Rectangle((int)playerEntity.Position.X, (int)playerEntity.Position.Y,
					(int)(playerEntity.Prefab.FrameWidth * playerEntity.Prefab.Scale),
					(int)(playerEntity.Prefab.FrameWidth * playerEntity.Prefab.Scale)),
				new Vector4(playerEntity.Prefab.FrameHeight, 0, 0, playerEntity.Prefab.FrameHeight)));

			AttackAbility attackAbility = new AttackAbility
			{
				Collider = new Rectangle((int) playerEntity.Position.X, (int) playerEntity.Position.Y,
										playerEntity.GetBodundingBox().Width, playerEntity.GetBodundingBox().Height)
			};

			playerEntity.SetComponent(attackAbility);

			_entityManager.CreateClientPlayer(playerEntity);

			// Tiled Map Entity & MapRenderer System
			_tiledMapEntity = GameCore.GetContentManager().Load<TiledMap>("assets/test-map");
			_tiledMapManager = new TiledMapManager(_tiledMapEntity);
			_tiledMapRendererSystem = new TiledMapRenderer(GameCore.GetGraphicsDevice(), _tiledMapEntity);

			_aStartPFManager = new AStarPFManager(_tiledMapEntity);

			// Generate Map Entities
			_tiledMapManager.GenerateTiledMapEntities(GameCore.GetContentManager(), _entityManager);

			// Setup physics System
			_physicsSystem = new PhysicsSystem(_tiledMapManager.TiledMapRectColliders);

			// Dialog system
			_dialogSystem = new DialogSystem(_dialogFont);
		}

		public override void Update(GameTime gameTime)
		{
			_physicsSystem.RefreshForce(_entityManager.GetMovableEntities());
			
			// Input process

			_inputManager.Start(Keyboard.GetState());

			if (_inputManager.JustKeyPress(Keys.Escape))
			{
				GameCore.SwitchScene(SceneKeys.MENU);
			}

			if (_inputManager.JustKeyPress(Keys.E))
			{
				IEnumerable<Dialog> dialogs = _interactionSystem.CheckNPCInteractions(
					_entityManager.Players.Values, _entityManager.Npcs.Values);

				_dialogSystem.UpdateDialogsToRender(dialogs);
			}

			if (!_dialogSystem.IsPlayerInteracting())
			{
				if (_inputManager.IsKeyDown(Keys.A))
				{
					_moveActionSystem.EstablishAction(_entityManager.GetClientPlayer(), IMovableActions.Actions.MOVE_LEFT);
				}

				if (_inputManager.IsKeyDown(Keys.D))
				{
					_moveActionSystem.EstablishAction(_entityManager.GetClientPlayer(), IMovableActions.Actions.MOVE_RIGHT);
				}

				if (_inputManager.IsKeyDown(Keys.W))
				{
					_moveActionSystem.EstablishAction(_entityManager.GetClientPlayer(), IMovableActions.Actions.MOVE_UP);
				}

				if (_inputManager.IsKeyDown(Keys.S))
				{
					_moveActionSystem.EstablishAction(_entityManager.GetClientPlayer(), IMovableActions.Actions.MOVE_DOWN);
				}

				if (_inputManager.JustKeyPress(Keys.Space))
				{
					_combatSystem.TriggerAttack(_entityManager.GetClientPlayer());
				}
			}

			_inputManager.End(Keyboard.GetState());

			_moveActionSystem.Update(gameTime, _entityManager.Players.Values);
			_physicsSystem.UpdateCollisions(gameTime, _entityManager.GetMovableEntities());
			_combatSystem.Update(gameTime, _entityManager.GetAttackerEntities());
			_interactionSystem.Update(gameTime, _entityManager.GetInteractableEntities());
			_animationRenderSystem.Update(gameTime, _entityManager.GetRenderableEntities());
			_tiledMapRendererSystem.Update(gameTime);
			_dialogSystem.Update(gameTime);

			GameCore.GetOrthographicCamera().LookAt(_entityManager.GetClientPlayer().Position);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			_tiledMapRendererSystem.Draw(
				_tiledMapEntity.GetLayer("Bottom"),
				GameCore.GetOrthographicCamera().GetViewMatrix());

			spriteBatch.Begin(
				samplerState: SamplerState.PointClamp,
				transformMatrix: GameCore.GetOrthographicCamera().GetViewMatrix());

			_physicsSystem.DrawMapColliders(spriteBatch, _entityManager.GetMovableEntities());

			_combatSystem.DrawDebug(spriteBatch, _entityManager.GetAttackerEntities());
			_interactionSystem.DrawDebug(spriteBatch, _entityManager.GetInteractableEntities());

			_animationRenderSystem.Draw(spriteBatch, _entityManager.GetRenderableEntities());
			_dialogSystem.Draw(spriteBatch, GameCore.GetOrthographicCamera());

			spriteBatch.End();

			_tiledMapRendererSystem.Draw(
				_tiledMapEntity.GetLayer("Top"),
				GameCore.GetOrthographicCamera().GetViewMatrix());
		}

		public override void Dispose()
		{
			_tiledMapRendererSystem.Dispose();
			_animationRenderSystem.Clear();
			_interactionSystem.Clear();
			_combatSystem.Clear();
			_physicsSystem.Clear();
			_dialogSystem.Clear();
			_entityManager.Dispose();
		}

		public override string GetSceneKey()
		{
			return SceneKeys.PLAY;
		}

	}
}
