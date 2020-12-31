using System;
using UnityEngine;

namespace ToolBox.StateMachine
{
	[Serializable]
	public class SharedData<T>
	{
		[SerializeField] private T _value = default;

		public T Value
		{
			get => _value;

			set
			{
				_value = value;
				OnValueChanged?.Invoke(_value);
			}
		}

		public event Action<T> OnValueChanged = null;
	}
}
