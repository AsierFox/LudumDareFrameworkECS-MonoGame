using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;
using System.Collections.Generic;
using Core.Scenes;

namespace Core.Game
{
	public class GameCore : Microsoft.Xna.Framework.Game, IGameCore
	{
		public class Screen
		{
			public const int WIDTH = 1024;
			public const int HEIGHT = 768;
			public const int VIRTUAL_WIDTH = 800;
			public const int VIRTUAL_HEIGHT = 480;

			public Vector2 center;
		}

		private readonly GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		private Screen _screen;

		private OrthographicCamera _camera;

		private BaseScene _currentScene;

		private IDictionary<string, BaseScene> _scenes;

		public GameCore()
		{
			_screen = new Screen();
			
			_graphics = new GraphicsDeviceManager(this)
			{
				PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
				PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height,
				IsFullScreen = false,
				PreferredDepthStencilFormat = DepthFormat.Depth16
			};

			_scenes = new Dictionary<string, BaseScene>();
			_scenes.Add(SceneKeys.PLAY, new PlayScene(this));

			_currentScene = _scenes[SceneKeys.PLAY];

			Window.IsBorderless = false;
			Window.AllowUserResizing = true;
			
			IsMouseVisible = true;

			Content.RootDirectory = "Content";
		}

		protected override void Initialize()
		{
			// TODO Test screen center, maybe use VIRTUAL_WIDTH (?)
			_screen.center = new Vector2(
				Screen.WIDTH / 2.0f,
				Screen.HEIGHT / 2.0f);

			_camera = new OrthographicCamera(
				new BoxingViewportAdapter(Window, GraphicsDevice,
					Screen.VIRTUAL_WIDTH, Screen.VIRTUAL_HEIGHT));

			_currentScene.Initialize();
			
			base.Initialize();
		}

		protected override void LoadContent()
		{
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			_currentScene.LoadContent();
		}

		protected override void Update(GameTime gameTime)
		{
			_currentScene.Update(gameTime);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			// Setting the sampler state to 'SamplerState.PointClamp' is recommended
			// to remove gaps between the tiles when rendering
			
			_currentScene.Draw(gameTime, _spriteBatch);

			base.Draw(gameTime);
		}

		protected override void Dispose(bool disposing)
		{
			foreach (BaseScene scene in _scenes.Values)
			{
				scene.Dispose();
			}

			_spriteBatch.Dispose();
			_graphics.Dispose();

			base.Dispose(disposing);
		}

		public void SwitchScene(string toSceneKey)
		{
			if (!_scenes.ContainsKey(toSceneKey))
			{
				throw new Exception("The SceneKey does not exist.");
			}

			if (_currentScene.GetSceneKey().Equals(toSceneKey))
			{
				// Changing to the same scene ?, log this
			}

			_currentScene.Dispose();

			_currentScene = _scenes[toSceneKey];
			_currentScene.Initialize();
			_currentScene.LoadContent();
		}

		public GraphicsDevice GetGraphicsDevice()
		{
			return GraphicsDevice;
		}

		public ContentManager GetContentManager()
		{
			return Content;
		}

		public OrthographicCamera GetOrthographicCamera()
		{
			return _camera;
		}

	}
}
