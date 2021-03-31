namespace Core.ECS.Components
{
	class SpriteAnimation : Component
	{
		public int ActionRow { get; private set; }

		public int TotalFrames { get; private set; }

		public float FrameSpeed { get; set; }
		
		public bool IsLooping { get; set; }

		public bool IsFlip { get; set; }

		public string AnimationName { get; private set; }


		public SpriteAnimation(string animationName, int actionRow, int totalFrames)
			: this(animationName, actionRow, totalFrames, false)
		{
		}

		public SpriteAnimation(string animationName, int actionRow, int totalFrames, bool isFlip)
		{
			AnimationName = animationName;
			ActionRow = actionRow;
			TotalFrames = totalFrames;
			IsFlip = isFlip;
			FrameSpeed = 0.2f;
			IsLooping = true;
		}

	}
}
