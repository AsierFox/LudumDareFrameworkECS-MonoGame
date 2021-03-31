namespace Core.ECS.Components
{
	class CharacterAttributes : Component
	{

		public float Speed { get; set; }


		public CharacterAttributes(float speed)
		{
			Speed = speed;
		}

	}
}
