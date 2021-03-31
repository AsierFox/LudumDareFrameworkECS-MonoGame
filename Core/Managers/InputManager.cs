using Microsoft.Xna.Framework.Input;
using System.Runtime.CompilerServices;

namespace Core.Managers
{
	class InputManager
	{
		private KeyboardState _state;
		private KeyboardState _previousState;


		public InputManager(KeyboardState state)
		{
			_previousState = state;
		}

		public void Start(KeyboardState state)
		{
			_state = state;
		}

		public void End(KeyboardState state)
		{
			_previousState = state;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsKeyDown(Keys key)
		{
			return _state.IsKeyDown(key);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool JustKeyPress(Keys key)
		{
			return _state.IsKeyDown(key)
				&& !_previousState.IsKeyDown(key);
		}

	}
}
