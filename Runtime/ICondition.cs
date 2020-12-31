namespace ToolBox.StateMachine
{
	public interface ICondition
	{
		void OnEnter();

		void OnResume();

		bool Check(float deltaTime);

		void OnPause();

		void OnExit();
	}
}
