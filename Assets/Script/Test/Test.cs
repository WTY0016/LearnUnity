using System;
using System.Linq;
using System.Reflection;
using Script.AlogLearn;
using UnityEngine;

public partial class Test : MonoBehaviour
{
	private void Start()
	{
		var assembly = Assembly.GetExecutingAssembly();
		var types = assembly.GetTypes();
		foreach (var type in types)
		{
			if (type.GetCustomAttribute<AutoRunAttribute>(true) != null)
			{
				if (type.IsSubclassOf(typeof(MonoBehaviour)))
				{
					gameObject.AddComponent(type);
					continue;
				}
			}
			var methods = type.GetMethods();
			foreach (var methodInfo in methods)
			{
				if (methodInfo.GetCustomAttribute<AutoRunAttribute>(true) != null)
				{
					var obj = Activator.CreateInstance(type);
					methodInfo.Invoke(obj, null);
				}
			}
		}
	}
	
}	

