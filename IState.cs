namespace ToolBox.StateMachine
{
	public interface IState
	{
		void Tick(float deltaTime);

		void OnEnter();

		void OnExit();

		void OnResume();

		void OnPause();
	}
}