using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Particles;
using MonoGame.Extended.Particles.Modifiers;
using MonoGame.Extended.Particles.Modifiers.Containers;
using MonoGame.Extended.Particles.Modifiers.Interpolators;
using MonoGame.Extended.Particles.Profiles;
using MonoGame.Extended.TextureAtlases;
using System;
using System.Collections.Generic;

namespace Core.Managers
{
	class MonogameParticleEmitterManager
	{

		private Texture2D _particleTexture;
		private ParticleEffect _particleEffect;


		public MonogameParticleEmitterManager(GraphicsDevice graphicsDevice)
		{
			_particleTexture = new Texture2D(graphicsDevice, 1, 1);
			_particleTexture.SetData(new[] { Color.White });

			ParticleInit(new TextureRegion2D(_particleTexture));
		}

		public void Update(GameTime gameTime)
		{
			float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

			_particleEffect.Update(delta);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_particleEffect);
		}

		private void ParticleInit(TextureRegion2D textureRegion)
		{
			_particleEffect = new ParticleEffect(autoTrigger: false)
			{
				Position = new Vector2(400, 240),
				Emitters = new List<ParticleEmitter>
				{
					new ParticleEmitter(textureRegion, 500, TimeSpan.FromSeconds(2.5),
						Profile.Ring(150f, Profile.CircleRadiation.In))
					{
						Parameters = new ParticleReleaseParameters
						{
							Speed = new Range<float>(0f, 50f),
							Quantity = 3,
							Rotation = new Range<float>(-1f, 1f),
							Scale = new Range<float>(3.0f, 4.0f)
						},
						Modifiers =
						{
							new AgeModifier
							{
								Interpolators =
								{
									new ColorInterpolator
									{
										StartValue = new HslColor(0.33f, 0.5f, 0.5f),
										EndValue = new HslColor(0.5f, 0.9f, 1.0f)
									}
								}
							},
							new RotationModifier {RotationRate = -2.1f},
							new RectangleContainerModifier {Width = 800, Height = 480},
							new LinearGravityModifier {Direction = -Vector2.UnitY, Strength = 30f}
						}
					}
				}
			};
		}

		public void Dispose()
		{
			_particleTexture.Dispose();
			_particleEffect.Dispose();
		}

	}
}
