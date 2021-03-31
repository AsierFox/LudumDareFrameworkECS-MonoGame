using Core.ECS.Components.Types;

namespace Core.ECS.Entities
{
	class CombatCollision : Entity
	{

		public IAttacker Attacker { get; set; }

		public IAttacker Defender { get; set; }

		public AttackAbility AttackAbility { get; set; }

	}
}
