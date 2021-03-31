using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Events
{
	interface ISubscriber
	{

		void OnEventTriggered(Event task);

	}
}
