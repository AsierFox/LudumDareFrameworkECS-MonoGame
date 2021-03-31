using Core.ECS.Components.Types;
using System.Collections.Generic;

namespace Core.ECS.Components
{
	class MovableActions : Component
	{
		public IMovableActions.Actions ActionToComplete { get; set; } = IMovableActions.Actions.IDLE;

		public IMovableActions.Actions PreviousAction { get; set; } = IMovableActions.Actions.IDLE;
	}
}
