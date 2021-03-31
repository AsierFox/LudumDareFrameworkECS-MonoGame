using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Core.ECS.Components.Types;

namespace Core.ECS.Systems
{
	class AnimationRenderSystem : System
	{
		public void Update(GameTime gameTime, IEnumerable<IAnimationRenderable> entities)
		{
			foreach (IAnimationRenderable animationRenderable in entities)
			{
				animationRenderable.GetAnimatedSprite().Update(gameTime);
			}
		}

		public void Draw(SpriteBatch spriteBatch, IEnumerable<IAnimationRenderable> entities)
		{
			foreach (IAnimationRenderable animationRenderable in entities)
			{
				animationRenderable.GetAnimatedSprite().Draw(spriteBatch,
					animationRenderable.GetPosition());
			}
		}

		public override void Clear()
		{
		}

	}
}
