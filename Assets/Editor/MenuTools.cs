
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTools
{
	[MenuItem("Tools/build asset bundle")]
	public static void BuildPipeline()
	{
		if (Directory.Exists(Const.AssetBundlePath)) Directory.Delete(Const.AssetBundlePath, true);
		Directory.CreateDirectory(Const.AssetBundlePath);
		UnityEditor.BuildPipeline.BuildAssetBundles(Const.AssetBundlePath, BuildAssetBundleOptions.None,
			BuildTarget.StandaloneWindows64);
		EditorUtility.DisplayDialog("build result", "success", "ok");
	}
	
	[MenuItem("Tools/set assetbundle name")]
	public static void SetAssetBundleName()
	{
		var assets = AssetDatabase.GetAllAssetPaths().Where(str=> Const.NeedBundledAssetPath.Any(str.Contains));
		foreach (var asset in assets)
		{
			var assetImporter = AssetImporter.GetAtPath(asset);
			var fileInfo = new FileInfo(asset);
			var dir = fileInfo.Directory.Name;
			assetImporter.assetBundleName = Path.Combine(dir, Path.GetFileNameWithoutExtension(asset));
			assetImporter.assetBundleVariant = "cn";
			assetImporter.SaveAndReimport();
		}
		EditorUtility.DisplayDialog("Set Assets Bundle Name", "Set Success!", "OK");
	}

	[MenuItem("Test/Test Texture Importer")]
	public static void TestTextureImporter()
	{
		TextureImporter textureImporter = AssetImporter.GetAtPath("Assets/Images/1.png") as TextureImporter;
		textureImporter.textureType = TextureImporterType.Sprite;
		textureImporter.SaveAndReimport();
	}

	[MenuItem("Tools/Bake")]
	public static void BakeScene()
	{
		Lightmapping.Bake();
		Debug.Log("Bake finish ");
	}

	[MenuItem("Tools/Print subclass of xx")]
	public static void Test()
	{
		var type = typeof(EditorWindow);
		var filter = "Console";
		var assembly = Assembly.GetAssembly(type);
		assembly.GetTypes().Where(t=> t.IsSubclassOf(type)).Where(t=>t.Name.Contains(filter)).Foreach(Debug.Log);
	}

	[MenuItem("Tools/Swich Scene")]
	public static void SwitchScene()
	{
		EditorSceneManager.sceneOpened += (scene, mode) =>
		{
			Debug.Log("Opened");
			Debug.Log(scene);
			Debug.Log(mode);
		};
		EditorSceneManager.sceneOpening += (path, mode) =>
		{
			Debug.Log("Opening");
			Debug.Log(path);
			Debug.Log(mode);
		};
		EditorSceneManager.OpenScene("Assets/Scenes/ShaderScene.unity");
	}

	[MenuItem("TestFunc/1")]
	public static void TestFunc1()
	{
		var assembly = Assembly.GetAssembly(typeof(EditorWindow));
		var windowType = assembly.GetType("UnityEditor.ConsoleWindow");
		var consoleField = windowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
		var consoleWindow = consoleField.GetValue(null) as EditorWindow;
		var activeFiled = windowType.GetField("m_ActiveText", BindingFlags.Instance | BindingFlags.NonPublic);
		var str = activeFiled.GetValue(consoleWindow) as string;
		Debug.Log(str);
	}

	[MenuItem("TestFunc/log receive")]
	public static void TestLogReceive()
	{
		Application.logMessageReceived += (condition, trace, type) =>
		{
			Debug.Log($"condition:{condition} trace:{trace} type:{type} \n end");
		};
		Debug.Log("test log");
	}
	
	[MenuItem("TestClick/控制台测试")]
	public static void TestFunc()
	{
		var assembly = typeof(EditorWindow).Assembly;
		var windowType = assembly.GetType("UnityEditor.ConsoleWindow");
		var logEntryType = assembly.GetType("UnityEditor.LogEntry");
		Debug.Log("====================================");
		// var consoleFiled = windowType.GetField("ms_ConsoleWindow", BindingFlags.NonPublic | BindingFlags.Static);
		windowType.GetMethods(BindingFlags.NonPublic | BindingFlags.Static).Log();
		Debug.Log("***************************");
		windowType.GetFields(BindingFlags.Static | BindingFlags.NonPublic).Log();
		// var consoleWindow = consoleFiled?.GetValue(null);
		var doubleClickedEvent = windowType.GetEvent("entryWithManagedCallbackDoubleClicked", BindingFlags.NonPublic | BindingFlags.Static);
		var addEventMethod = windowType.GetMethod("add_entryWithManagedCallbackDoubleClicked",
			BindingFlags.NonPublic | BindingFlags.Static);
		var conditionFieldInfo = logEntryType.GetField("condition");
		var fileFieldInfo = logEntryType.GetField("file");
		var lineFieldInfo = logEntryType.GetField("line");
		Action<object> callback = (obj) =>
		{
			Debug.Log("***********************");
			Debug.Log(conditionFieldInfo.GetValue(obj));
			Debug.Log(fileFieldInfo.GetValue(obj));
			Debug.Log(lineFieldInfo.GetValue(obj));
			Debug.Log("************************");
		};

		var handlerType = doubleClickedEvent?.EventHandlerType;
		var methodInfo = handlerType?.GetMethod("Invoke");
		var paramTypes = methodInfo?.GetParameters().Select(p => p.ParameterType).ToArray();
		var aName = new AssemblyName {Name = "DynamicTypes"};
		var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(aName, AssemblyBuilderAccess.Run);
		var modelBuilder = assemblyBuilder.DefineDynamicModule(aName.Name);
		var typeBuilder = modelBuilder.DefineType("DynamicType", TypeAttributes.Class | TypeAttributes.Public);
		var methodBuilder = typeBuilder.DefineMethod("DynamicOnLogEntryDoubleClick",
			MethodAttributes.Public | MethodAttributes.Static, methodInfo?.ReturnType, paramTypes);
		var ilGenerator = methodBuilder.GetILGenerator();
		ilGenerator.EmitCall(OpCodes.Call, callback.Method, new []{logEntryType});
		var finishedType = typeBuilder.CreateType();
		var finalMethodInfo = finishedType.GetMethod("DynamicOnLogEntryDoubleClick", BindingFlags.Public | BindingFlags.Static);
		Debug.Log(finalMethodInfo);
		var @delegate = Delegate.CreateDelegate(handlerType, finalMethodInfo);
		Debug.Log(doubleClickedEvent?.GetType());
		// doubleClickedEvent?.AddEventHandler(consoleWindow, @delegate);
		addEventMethod.Invoke(null, new object[] {@delegate});
		addEventMethod.GetParameters().Log();
		// doubleClickedEvent.GetType().GetMethod("").IsVir.tual
		Debug.Log("success");
	}


	[MenuItem("TestClick/TestClick2")]
	public static void TestFunc2()
	{
		var assembly = typeof(EditorWindow).Assembly;
		var logEntriesType = assembly.GetType("UnityEditor.LogEntries");
		var logEntryType = assembly.GetType("UnityEditor.LogEntry");
		var getEntryInternalMethod =
			logEntriesType.GetMethod("GetEntryInternal", BindingFlags.Static | BindingFlags.Public);
		var entry = Activator.CreateInstance(logEntryType);
		getEntryInternalMethod.Invoke(null, new object[] {1, entry});
		logEntryType.GetFields().Foreach(info =>
			{
				Debug.Log($"propName : {info.Name}  value:{info.GetValue(entry)}");	
			});
	}

	[MenuItem("TestClick/查看所有unity特性")]
	public static void TestFunc3()
	{
		AppDomain.CurrentDomain.GetAssemblies().Where(assembly => assembly.FullName.Contains("Unity")).SelectMany(assembly => assembly.GetTypes().Where(t=>t.IsSubclassOf(typeof(Attribute)))).Log();
		// var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
		// assembly.GetTypes().Where(t=>t.IsSubclassOf(typeof(Attribute))).Log();
	}

	[MenuItem("TestClick/查找Inspector")]
	public static void Test4()
	{
		typeof(DefaultAsset).Assembly.GetTypes().Where(t=>t.Name.Contains("Inspector")).Log();
		Debug.Log("find finished");
		
	}

	[MenuItem("Assets/CopyAssetPath", priority = 1)]
	public static void Copy()
	{
		var obj = Selection.objects[0];
		var path = AssetDatabase.GetAssetPath(obj);
		GUIUtility.systemCopyBuffer = path;
	}
}
