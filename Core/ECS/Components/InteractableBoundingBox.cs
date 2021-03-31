using Microsoft.Xna.Framework;

namespace Core.ECS.Components
{
	class InteractableBoundingBox
	{

		public Rectangle BoundingBox { get; set; }

		public Vector4 Offset { get; set; }

		public InteractableBoundingBox(Rectangle boundingBox)
		{
			BoundingBox = boundingBox;
			Offset = Vector4.Zero;
		}

		public InteractableBoundingBox(Rectangle boundingBox, Vector4 offset)
		{
			BoundingBox = boundingBox;
			Offset = offset;
		}

	}
}
