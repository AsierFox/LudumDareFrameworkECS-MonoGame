using Microsoft.Xna.Framework;
using System;

namespace Core.ECS.Components
{
	class RigitBody : Component
	{
		private const bool DEFAULT_IS_GRAVITY = false;
		private const float DEFAULT_GRAVITY = 9.81f;
		private const float DEFAULT_MASS = 1f;

		public bool IsGravity { get; set; }
		private float _gravity;
		public float Mass { get; set; }

		public Vector2 NewPosition;
		
		private Vector2 acceleration;
		private Vector2 velocity;
		private Vector2 force;
		private Vector2 friction;


		public RigitBody()
		{
			IsGravity = DEFAULT_IS_GRAVITY;
			_gravity = DEFAULT_GRAVITY;
			Mass = DEFAULT_MASS;
		}

		public RigitBody(bool isGravity, float gravityForce, float mass)
		{
			IsGravity = isGravity;
			_gravity = gravityForce;
			Mass = mass;
		}

		public void Update(GameTime gameTime)
		{
			// Force + Friction = Mass * Acceleration
			acceleration.X = (force.X + friction.X) / Mass;
			acceleration.X = force.X + Mass;

			//if (true)
			//{
			//	UpdateWithGravity(gameTime);
			//}
			//else
			//{
			UpdateWithoutGravity(gameTime);
			//}

			float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

			velocity = Vector2.Multiply(acceleration, delta);
			NewPosition = Vector2.Multiply(velocity, delta);


			// TODO Gravity Testing
			// Must check then if player is is floor to avoid applying gravity
			//Vector2 tempGravity = new Vector2(0, 400);
			//NewPosition = Vector2.Multiply(Vector2.Add(new Vector2(0, 0), force), delta);
		}

		private void UpdateWithGravity(GameTime gameTime)
		{
			// Acceleration * Mass = Gravity * Mass
			acceleration.Y = _gravity + force.Y / Mass;
		}

		private void UpdateWithoutGravity(GameTime gameTime)
		{
			// Force + Friction = Mass * Acceleration
			//acceleration.Y = (force.Y + friction.Y) / Mass;
			acceleration.Y = force.Y + Mass;
		}

		public Vector2 GetForce()
		{
			return force;
		}

		public void SetForce(Vector2 force)
		{
			this.force = force;
		}

		public void SetForceX(float x)
		{
			force.X = x;
		}

		public void SetForceY(float y)
		{
			force.Y = y;
		}

		public void RemoveForce()
		{
			force = Vector2.Zero;
		}

		public void RemoveForceX()
		{
			force.X = 0;
		}

		public void RemoveForceY()
		{
			force.Y = 0;
		}

		public void SetFriction(Vector2 friction)
		{
			this.friction = friction;
		}

		public void RemoveFriction()
		{
			friction = Vector2.Zero;
		}

	}
}
