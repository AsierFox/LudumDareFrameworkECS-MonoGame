namespace Core.ECS.Entities
{
	abstract class Entity
	{
		public int ID { get; set; }

		public bool IsHide { get; set; }

		public bool IsRemoved { get; set; }


		public Entity()
		{
			IsHide = false;
			IsRemoved = false;
		}

		public virtual void Dispose()
		{
		}

	}
}
