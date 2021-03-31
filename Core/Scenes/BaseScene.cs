using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Core.Game;

namespace Core.Scenes
{
	abstract class BaseScene
	{
		protected IGameCore GameCore { get; private set; }


		public BaseScene(IGameCore gameCore)
		{
			GameCore = gameCore;
		}

		public abstract void Initialize();

		public abstract void LoadContent();

		public abstract void Update(GameTime gameTime);

		public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

		public abstract void Dispose();

		public abstract string GetSceneKey();

		protected void SwitchScene(string toSceneKey)
		{
			GameCore.SwitchScene(toSceneKey);
		}

	}
}
