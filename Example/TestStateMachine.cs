using UnityEngine;

namespace ToolBox.StateMachine.Examples
{
	public class TestStateMachine : HierarchicalStateMachine
	{
		[SerializeField] private SharedData<int> _number = null;
		[SerializeField] private float _waitFor = 0f;

		protected override StateMachine Setup()
		{
			// States
			var intChangerState = new IntChanger(1, _number);
			var emptyState = new Empty();

			// State machine
			var stateMachine = new StateMachine(intChangerState);

			// Condition
			var timeCondition = new WaitFor(_waitFor);

			// Transition
			stateMachine.AddTransition(intChangerState, emptyState, timeCondition, false);

			// Start State
			return stateMachine;
		}
	}
}
