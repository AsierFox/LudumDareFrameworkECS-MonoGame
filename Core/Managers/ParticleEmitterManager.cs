using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Core.ECS.Components;
using Core.ECS.Entities;
using Core.Game;

namespace Core.Managers
{
	class ParticleEmitterManager
	{

		public float GenerationSpeed = .005f;

		public float GlobalVelocitySpeed = 1f;
		
		public int MaxParticles = 100;

		private float _generateTimer;

		private float _swayTimer;

		private Random _rand;

		private Particle _particlePrefab;

		private List<Particle> _particles;


		public ParticleEmitterManager(Particle particle)
		{
			_particlePrefab = particle;

			_rand = new Random();
			_generateTimer = 0f;
			_swayTimer = 0f;
			_particles = new List<Particle>();
		}

		public void Update(GameTime gameTime)
		{
			float delta = (float) gameTime.ElapsedGameTime.TotalSeconds;

			_generateTimer += delta;
			_swayTimer += delta;

			if (_generateTimer > GenerationSpeed)
			{
				_generateTimer = 0f;

				if (_particles.Count < MaxParticles)
				{
					_particles.Add(GenerateNewParticle());
				}
			}

			if (_swayTimer > GlobalVelocitySpeed)
			{
				_swayTimer = 0f;

				float xSway = _rand.Next(-2, 2);

				foreach (Particle particle in _particles)
				{
					// Multiply by Scale to make faster when it's bigger,
					// and divide just simply to slow down a bit
					Vector2 newVelocity = particle.Velocity;
					newVelocity.X = (xSway * particle.Prefab.Scale) / 50;
					particle.Velocity = newVelocity;
				}
			}

			foreach (Particle particle in _particles)
			{
				particle.Position += particle.Velocity;
			}

			for (int i = 0; i < _particles.Count; i++)
			{
				if (_particles[i].IsRemoved)
				{
					_particles.RemoveAt(i);
					i--;
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			foreach (Particle particle in _particles)
			{
				spriteBatch.Draw(particle.Prefab.Texture, particle.Position, null,
					Color.White * particle.Prefab.Opacity, particle.Prefab.Rotation,
					particle.Prefab.Origin, particle.Prefab.Scale, SpriteEffects.None, 0f);
			}
		}

		private Particle GenerateNewParticle()
		{
			int x = _rand.Next(0, GameCore.Screen.VIRTUAL_WIDTH);
			float ySpeed = _rand.Next(0, 100) / 100f;

			Sprite newParticlePrefab = _particlePrefab.Prefab.Clone() as Sprite;
			newParticlePrefab.Opacity = (float) _rand.NextDouble();
			newParticlePrefab.Rotation = MathHelper.ToRadians(_rand.Next(0, 360));
			newParticlePrefab.Scale = (float) _rand.NextDouble() + _rand.Next(0, 3);

			Particle newParticle = new Particle(newParticlePrefab);
			newParticle.Position = new Vector2(x, -newParticlePrefab.Texture.Height);
			newParticle.Velocity = new Vector2(0, ySpeed);

			return newParticle;
		}

	}
}
