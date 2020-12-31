Hierarchical State Machine for Unity. Most likely i will rewrite it multiple times in future but for now it suits my needs.

### TODO
- [ ] Detailed guide
- [x] Git Package

## Simple Example:
```csharp
public class TestStateMachine : HierarchicalStateMachine
{
	[SerializeField] private float _timeInPatrol = 1f;
	[SerializeField] private float _timeInIdle = 3f;

	protected override StateMachine Setup()
	{
		var patrolState = new PatrolState(transform, speed: 5f, randomizeStartSpeed: true);
		var emptyState = new Empty();

		var stateMachine = new StateMachine(startState: patrolState)
			.Configure()
			.AddTransition(patrolState, emptyState, new WaitFor(waitFor: 1f), reversed: false)
			.AddTransition(emptyState, patrolState, new WaitFor(waitFor: 3f), reversed: false);

		return stateMachine;
	}
}
```
