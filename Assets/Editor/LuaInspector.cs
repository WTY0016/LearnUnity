using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Editor
{
    [CustomEditor(typeof(DefaultAsset))]
    public class LuaInspector : UnityEditor.Editor {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var path = AssetDatabase.GetAssetPath(target);
            if (!path.EndsWith(".lua")) return;
            EditorGUILayout.BeginVertical();
            var style = new GUIStyle
            {
                border = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(0, 0, 0, 0),
                normal = {textColor = Color.cyan, background = Texture2D.blackTexture}
            };
            EditorGUILayout.TextArea(File.ReadAllText(path), style);
            EditorGUILayout.EndVertical();
            // EditorGUILayout.TextArea(File.ReadAllText(path));
        }
    }


    // [CustomEditor(typeof(PostEffectBase))]
    // public class Test2Inspector : UnityEditor.Editor
    // {
    //     private int selectIndex = 1;
    //     private string[] selections;
    //     private void OnEnable()
    //     {
    //         selections = ((PostEffectBase) target).effects.Select(m => m.Name).ToArray();
    //     }
    //
    //     public override void OnInspectorGUI()
    //     {
    //         base.OnInspectorGUI();
    //         var obj =  target as PostEffectBase;
    //         EditorGUILayout.BeginVertical();
    //         selectIndex = EditorGUILayout.Popup(selectIndex, selections);
    //         obj.curIndex = selectIndex;
    //         EditorGUILayout.EndVertical();
    //     }
    // }
}
