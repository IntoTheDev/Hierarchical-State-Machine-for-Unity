using UnityEngine;

namespace ToolBox.StateMachine
{
	[DisallowMultipleComponent]
	public abstract class HierarchicalStateMachine : MonoBehaviour
	{
		private StateMachine _currentStateMachine = null;
		private bool _wasDisabled = false;

		private void Start()
		{
			_currentStateMachine = Setup();
			_currentStateMachine.OnEnter();
		}

		private void OnEnable()
		{
			if (_wasDisabled)
				_currentStateMachine?.OnResume();
		}

		private void OnDisable()
		{
			_currentStateMachine?.OnPause();
			_wasDisabled = true;
		}

		private void OnDestroy() =>
			_currentStateMachine?.OnExit();

		private void Update() =>
			_currentStateMachine.Tick(Time.deltaTime);

		protected abstract StateMachine Setup();
	}
}
