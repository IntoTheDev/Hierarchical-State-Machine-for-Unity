using UnityEngine;

namespace ToolBox.StateMachine.Examples
{
	public class TestStateMachine : HierarchicalStateMachine
	{
		protected override StateMachine Setup()
		{
			var patrolState = new PatrolState(transform, speed: 5f, randomizeStartSpeed: true);
			var emptyState = new Empty();

			var stateMachine = new CustomStateMachine()
				.Configure(patrolState)
				.AddTransition(patrolState, emptyState, new WaitFor(waitFor: 1f), reversed: false)
				.AddTransition(emptyState, patrolState, new WaitFor(waitFor: 3f), reversed: false);

			return stateMachine;
		}
	}

	public class PatrolState : IState
	{
		private Transform _transform = null;
		private float _speed = 5f;
		private float _currentSpeed = 0f;

		public PatrolState(Transform transform, float speed, bool randomizeStartSpeed)
		{
			_transform = transform;
			_speed = speed;

			if (randomizeStartSpeed)
			{
				var possibleValues = new float[] { -_speed, _speed };
				_currentSpeed = possibleValues[Random.Range(0, possibleValues.Length)];
			}
		}

		public void OnEnter() =>
			_currentSpeed = _currentSpeed == _speed ? -_speed : _speed;

		public void OnExit() { }

		public void OnPause() { }

		public void OnResume() { }

		public void Tick(float deltaTime)
		{
			var move = Vector3.right * (_currentSpeed * deltaTime);
			_transform.position += move;
		}
	}
}
