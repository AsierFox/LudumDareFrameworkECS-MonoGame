using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Core.Game
{
	interface IGameCore
	{
		void SwitchScene(string toSceneKey);

		GraphicsDevice GetGraphicsDevice();

		ContentManager GetContentManager();

		OrthographicCamera GetOrthographicCamera();
	}
}
