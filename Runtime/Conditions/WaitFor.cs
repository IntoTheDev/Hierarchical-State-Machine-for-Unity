﻿using UnityEngine;

namespace ToolBox.StateMachine
{
	[System.Serializable]
	public class WaitFor : ICondition
	{
		[SerializeField, Min(0f)] private float _waitFor = 0f;
		private float _timeSpend = 0f;

		public WaitFor(float waitFor) =>
			_waitFor = waitFor;

		public void OnEnter() =>
			_timeSpend = 0f;

		public bool Check(float deltaTime)
		{
			_timeSpend += deltaTime;
			return _timeSpend >= _waitFor;
		}

		public void OnExit() { }

		public void OnResume() { }

		public void OnPause() { }
	}
}