namespace ToolBox.StateMachine
{
	public interface ICondition
	{
		void OnEnter();

		void OnResume();

		bool Check();

		void OnPause();

		void OnExit();
	}
}
