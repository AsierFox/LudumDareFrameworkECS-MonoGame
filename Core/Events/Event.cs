namespace Core.Events
{
	public abstract class Event
	{

		private string _topic;

		public Event(string topic)
		{
			_topic = topic;
		}

		public string GetTopic()
		{
			return _topic;
		}

	}
}
