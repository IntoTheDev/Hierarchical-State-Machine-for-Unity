using System;
using System.Collections.Generic;

namespace ToolBox.StateMachine
{
	[Serializable]
	public class StateMachine : IState
	{
		private IState _currentState;
		private IState _startState;
		private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>();
		private List<Transition> _currentTransitions = new List<Transition>();
		private List<Transition> _anyTransitions = new List<Transition>();
		private List<IState> _concurrentStates = new List<IState>();

		private static List<Transition> _emptyTransitions = new List<Transition>(0);

		public IState CurrentState => _currentState;

		public StateMachine(IState startState) =>
			_startState = startState;

		public void OnResume()
		{
			_currentState?.OnResume();

			var count = _concurrentStates.Count;
			for (int i = 0; i < count; i++)
				_concurrentStates[i].OnResume();

			OnTransitionsResume(_currentTransitions);
			OnTransitionsResume(_anyTransitions);
		}

		public void OnPause()
		{
			_currentState?.OnPause();

			var count = _concurrentStates.Count;
			for (int i = 0; i < count; i++)
				_concurrentStates[i].OnPause();

			OnTransitionsPause(_currentTransitions);
			OnTransitionsPause(_anyTransitions);
		}

		public void OnEnter()
		{
			SetState(_startState, true);

			var count = _concurrentStates.Count;
			for (int i = 0; i < count; i++)
				_concurrentStates[i].OnEnter();

			OnTransitionsEnter(_anyTransitions);
		}

		public void Tick(float deltaTime)
		{
			var transition = GetTransition(deltaTime);
			if (transition != null)
				SetState(transition.To, false);

			_currentState?.Tick(deltaTime);

			var count = _concurrentStates.Count;
			for (int i = 0; i < count; i++)
				_concurrentStates[i].Tick(deltaTime);
		}

		public void OnExit()
		{
			_currentState?.OnExit();

			var count = _concurrentStates.Count;
			for (int i = 0; i < count; i++)
				_concurrentStates[i].OnExit();

			OnTransitionsExit(_currentTransitions);
			OnTransitionsExit(_anyTransitions);
		}

		public void SetState(IState state, bool force)
		{
			if (state == _currentState && !force)
				return;

			if (_currentState != null)
			{
				_currentState.OnExit();
				OnTransitionsExit(_currentTransitions);
			}

			_currentState = state;

			_transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
			if (_currentTransitions == null)
				_currentTransitions = _emptyTransitions;

			_currentState.OnEnter();
			OnTransitionsEnter(_currentTransitions);
		}

		public StateMachine AddTransition(IState from, IState to, ICondition predicate, bool reversed)
		{
			if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
			{
				transitions = new List<Transition>();
				_transitions[from.GetType()] = transitions;
			}

			transitions.Add(new Transition(to, predicate, reversed));

			return this;
		}

		public StateMachine AddAnyTransition(IState state, ICondition predicate, bool reversed)
		{
			_anyTransitions.Add(new Transition(state, predicate, reversed));

			return this;
		}

		public StateMachine AddConcurrentState(IState state)
		{
			if (!_concurrentStates.Contains(state))
				_concurrentStates.Add(state);

			return this;
		}

		public StateMachine Configure()
		{
			_transitions = new Dictionary<Type, List<Transition>>();
			_currentTransitions = new List<Transition>();
			_anyTransitions = new List<Transition>();
			_concurrentStates = new List<IState>();

			Preconfigure();

			return this;
		}

		public StateMachine Configure(StateMachine startState)
		{
			_startState = startState;

			Configure();

			return this;
		}

		protected virtual void Preconfigure() { }

		private Transition GetTransition(float deltaTime)
		{
			int count = _currentTransitions.Count;

			for (int i = 0; i < count; i++)
			{
				Transition transition = _currentTransitions[i];

				var conditionResult = transition.Condition.Check(deltaTime);
				var isReversed = transition.Reversed;

				var canTransition = (conditionResult && !isReversed) || (!conditionResult && isReversed);

				if (canTransition)
					return transition;
			}

			count = _anyTransitions.Count;

			for (int i = 0; i < count; i++)
			{
				Transition transition = _anyTransitions[i];

				var conditionResult = transition.Condition.Check(deltaTime);
				var isReversed = transition.Reversed;

				var canTransition = (conditionResult && !isReversed) || (!conditionResult && isReversed);

				if (canTransition)
					return transition;
			}

			return null;
		}

		private void OnTransitionsEnter(List<Transition> transitions)
		{
			int count = transitions.Count;

			for (int i = 0; i < count; i++)
				transitions[i].Condition.OnEnter();
		}

		private void OnTransitionsResume(List<Transition> transitions)
		{
			int count = transitions.Count;

			for (int i = 0; i < count; i++)
				transitions[i].Condition.OnResume();
		}

		private void OnTransitionsExit(List<Transition> transitions)
		{
			int count = transitions.Count;

			for (int i = 0; i < count; i++)
				transitions[i].Condition.OnExit();
		}

		private void OnTransitionsPause(List<Transition> transitions)
		{
			int count = transitions.Count;

			for (int i = 0; i < count; i++)
				transitions[i].Condition.OnPause();
		}

		private class Transition
		{
			public ICondition Condition { get; } = null;
			public IState To { get; } = null;
			public bool Reversed { get; } = false;

			public Transition(IState to, ICondition condition, bool reversed)
			{
				To = to;
				Condition = condition;
				Reversed = reversed;
			}
		}
	}
}