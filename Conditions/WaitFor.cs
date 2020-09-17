using UnityEngine;

namespace ToolBox.StateMachine
{
	public class WaitFor : ICondition
	{
		private float _waitFor = 0f;
		private float _value = 0f;

		public WaitFor(float waitFor) =>
			_waitFor = waitFor;

		public void OnEnter() =>
			_value = 0f;

		public bool Check()
		{
			_value += Time.deltaTime;
			return _value >= _waitFor;
		}

		public void OnExit() { }

		public void OnResume() { }

		public void OnPause() { }
	}
}