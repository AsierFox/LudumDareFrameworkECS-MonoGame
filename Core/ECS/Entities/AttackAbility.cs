using Microsoft.Xna.Framework;

namespace Core.ECS.Entities
{
	class AttackAbility : Entity
	{

		public bool IsActive;

		public Rectangle Collider { get; set; }

		public AttackAbility()
		{
			IsActive = false;
			Collider = new Rectangle();
		}

	}
}
