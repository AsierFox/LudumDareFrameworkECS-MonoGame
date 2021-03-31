using System.Collections.Generic;

namespace Core.Events
{
	class EventSystem
	{
		private static EventSystem _instance;

		private IDictionary<string, IList<ISubscriber>> subscribers;

		private EventSystem()
		{
			subscribers = new Dictionary<string, IList<ISubscriber>>();
		}

		public static EventSystem GetInstance()
		{
			if (null == _instance)
			{
				_instance = new EventSystem();
			}

			return _instance;
		}

		public void RegisterSubscriber(string topic, ISubscriber subscriber)
		{
			if (null == subscribers[topic])
			{
				subscribers.Add(topic, new List<ISubscriber>());
			}

			subscribers[topic].Add(subscriber);
		}

		public void EmitEvent(string topic, Event task)
		{
			foreach (ISubscriber subscriber in subscribers[topic])
			{
				subscriber.OnEventTriggered(task);
			}
		}

	}

}
