using System;

namespace ToolBox.StateMachine
{
	public class CustomState : IState
	{
		private event Action _action = null;

		public CustomState(Action action) =>
			_action += action;

		public void OnEnter() { }

		public void OnExit() { }

		public void OnPause() { }

		public void OnResume() { }

		public void Tick(float deltaTime) =>
			_action?.Invoke();
	}
}