using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Gui;
using MonoGame.Extended.Gui.Controls;
using MonoGame.Extended.ViewportAdapters;
using Core.Game;

namespace Core.Scenes
{
	class MenuScene : BaseScene
	{

		private GuiSystem _guiSystem;

		public MenuScene(IGameCore gameCore)
			: base(gameCore)
		{
		}
		
		public override void Initialize()
		{
		}

		public override void LoadContent()
		{
			Skin.CreateDefault(GameCore.GetContentManager().Load<BitmapFont>("assets/montserrat-32"));

			ViewportAdapter viewportAdapter = new DefaultViewportAdapter(GameCore.GetGraphicsDevice());

			GuiSpriteBatchRenderer guiRenderer =
				new GuiSpriteBatchRenderer(GameCore.GetGraphicsDevice(), () => Matrix.Identity);

			Button playButton = new Button
			{
				Content = "Play!!",
				HorizontalAlignment = HorizontalAlignment.Centre,
				VerticalAlignment = VerticalAlignment.Top
			};

			playButton.Clicked += (sender, args) => GameCore.SwitchScene(SceneKeys.PLAY);

			Screen menuScreen = new Screen
			{
				Content = new StackPanel
				{
					BackgroundColor = Color.SaddleBrown,
					Items =
					{
						new Label
						{
							Content = "ExSide",
							HorizontalAlignment = HorizontalAlignment.Centre
						},
						playButton
					}
				}
			};

			_guiSystem = new GuiSystem(viewportAdapter, guiRenderer)
			{
				ActiveScreen = menuScreen
			};
		}

		public override void Update(GameTime gameTime)
		{
			_guiSystem.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Begin(
				samplerState: SamplerState.PointClamp,
				transformMatrix: GameCore.GetOrthographicCamera().GetViewMatrix());

			_guiSystem.Draw(gameTime);

			spriteBatch.End();
		}

		public override void Dispose()
		{
		}

		public override string GetSceneKey()
		{
			return SceneKeys.MENU;
		}

	}
}
