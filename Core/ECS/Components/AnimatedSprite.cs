using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Core.ECS.Components
{
	class AnimatedSprite : Component
	{
		private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

		public interface Animations
		{
			static readonly string IDLE = "idle";
			static readonly string MOVE_UP = "move-up";
			static readonly string MOVE_DOWN = "move-down";
			static readonly string MOVE_RIGHT = "move-right";
			static readonly string MOVE_LEFT = "move-left";
		}

		public Texture2D Texture { get; private set; }

		public int CurrentFrame { get; set; }

		public int FrameWidth { get; private set; }

		public int FrameHeight { get; private set; }

		public Color Opacity { get; set; }

		public float Scale { get; set; }

		private Dictionary<string, SpriteAnimation> _animations;

		private SpriteAnimation _currentAnimation;

		private float _timer;

		private SpriteEffect _spriteEffect;


		public AnimatedSprite(Texture2D texture, int xFrameCount, int yFrameCount)
			: this(texture, xFrameCount, yFrameCount, 1f, 1f)
		{
		}

		public AnimatedSprite(Texture2D texture, int xFrameCount, int yFrameCount, float opacity, float scale)
		{
			_animations = new Dictionary<string, SpriteAnimation>();
			CurrentFrame = 0;
			_timer = 0f;

			Texture = texture;
			FrameWidth = Texture.Width / xFrameCount;
			FrameHeight = Texture.Height / yFrameCount;
			Opacity = Color.White * opacity;
			Scale = scale;
		}

		public void RegisterAnimation(SpriteAnimation animatedSprite)
		{
			if (IsAnimationRegistered(animatedSprite.AnimationName))
			{
				Logger.Debug("The animation '{0}' is already registered!", animatedSprite.AnimationName);
				return;
			}

			_animations.Add(animatedSprite.AnimationName, animatedSprite);
		}

		public void Play(string animationName)
		{
			if (!IsAnimationRegistered(animationName))
			{
				Logger.Error("The animation '{0}' is not registered!!", animationName);
				return;
			}

			CurrentFrame = 0;
			_currentAnimation = _animations[animationName];

			_timer = 0f;
		}

		public void Stop()
		{
			_timer = 0f;

			CurrentFrame = 0;
		}

		public void Update(GameTime gameTime)
		{
			_timer += (float) gameTime.ElapsedGameTime.TotalSeconds;

			if (_timer > _currentAnimation.FrameSpeed)
			{
				_timer = 0f;

				CurrentFrame++;

				if (CurrentFrame >= _currentAnimation.TotalFrames)
				{
					CurrentFrame = 0;
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch, Vector2 position)
		{
			spriteBatch.Draw(Texture,
				new Rectangle(
					(int) position.X, (int) position.Y,
					(int) (FrameWidth * Scale), (int) (FrameHeight * Scale)),
				new Rectangle(
					CurrentFrame * FrameWidth,
					_currentAnimation.ActionRow * FrameHeight,
					FrameWidth, FrameHeight),
				Opacity, 0, Vector2.One,
				_currentAnimation.IsFlip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
		}

		public override void Dispose()
		{
			Texture.Dispose();

			base.Dispose();
		}

		private bool IsAnimationRegistered(string animationName)
		{
			return _animations.ContainsKey(animationName);
		}

	}
}
