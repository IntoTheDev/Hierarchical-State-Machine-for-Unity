namespace ToolBox.StateMachine
{
	public class IntChanger : IState
	{
		private int _changeValue = 0;
		private SharedData<int> _number = null;

		public IntChanger(int changeValue, SharedData<int> number)
		{
			_changeValue = changeValue;
			_number = number;
		}

		public void OnEnter() { }

		public void Tick(float deltaTime) =>
			_number.Value += _changeValue;

		public void OnExit() { }

		public void OnResume() { }

		public void OnPause() { }
	}
}