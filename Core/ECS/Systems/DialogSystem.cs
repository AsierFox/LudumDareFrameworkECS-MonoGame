using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using System.Collections.Generic;
using System.Linq;
using Core.ECS.Components;

namespace Core.ECS.Systems
{
	class DialogSystem : System
	{
		private float _nextWordTimer;

		private float _textSpeed;

		private BitmapFont _font;

		private IEnumerable<Dialog> _dialogsToDisplay;

		private Dialog _dialogToDisplay;

		private string _displayingTextDialog;

		private int _currentDialogPage;


		public DialogSystem(BitmapFont font)
		{
			_textSpeed = .07f;
			_dialogToDisplay = new Dialog();
			_displayingTextDialog = "";

			_currentDialogPage = 0;
			_nextWordTimer = 0;

			_font = font;
		}

		public void UpdateDialogsToRender(IEnumerable<Dialog> dialogs)
		{
			if (null == dialogs || dialogs.Count() <= 0)
			{
				return;
			}

			if (null != _dialogsToDisplay && _dialogsToDisplay.SequenceEqual(dialogs))
			{
				if (!IsCurrentTextDialogRendered())
				{
					_displayingTextDialog = _dialogToDisplay.Text;
					return;
				}

				_currentDialogPage++;

				if (AreDialogsFinished())
				{
					_dialogsToDisplay = null;
					_dialogToDisplay = null;
					_displayingTextDialog = "";
				}
				else
				{
					_dialogToDisplay = dialogs.ElementAt(_currentDialogPage);
					_displayingTextDialog = "";
				}
			}
			else
			{
				_dialogsToDisplay = dialogs;

				_dialogToDisplay = dialogs.First();
				_currentDialogPage = 0;
				_displayingTextDialog = "";
				
				_nextWordTimer = 0;
			}
		}

		public void Update(GameTime gameTime)
		{
			if (AreDialogsFinished()
				|| IsCurrentTextDialogRendered())
			{
				return;
			}

			_nextWordTimer += (float) gameTime.ElapsedGameTime.TotalSeconds;

			if (_nextWordTimer > _textSpeed)
			{
				_nextWordTimer = 0;

				_displayingTextDialog += _dialogToDisplay.Text[_displayingTextDialog.Length];
			}
		}

		public void Draw(SpriteBatch spriteBatch, OrthographicCamera camera)
		{
			if (AreDialogsFinished())
			{
				return;
			}

			spriteBatch.DrawString(_font, _displayingTextDialog, camera.ScreenToWorld(0, 0), Color.White);
		}

		public bool IsCurrentTextDialogRendered()
		{
			return _displayingTextDialog.Length == _dialogToDisplay.Text.Length;
		}

		public bool AreDialogsFinished()
		{
			return null == _dialogsToDisplay
				|| _currentDialogPage == _dialogsToDisplay.Count();
		}

		public bool IsPlayerInteracting()
		{
			// TODO instead conditions, I can have an array of event types to know
			// what action is currently doing the player
			return null != _dialogsToDisplay;
		}

		public override void Clear()
		{
		}

	}
}
