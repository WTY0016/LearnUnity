using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace Editor
{
    public static class ExpendTools
    {
        [OnOpenAsset(0)]
        public static bool OpenShaderWithSublime(int instance, int line)
        {
            var asset = AssetDatabase.GetAssetPath(instance);
            if (!asset.EndsWith("shader")) return false;
            if (!File.Exists(Const.SublimePath)) return false;
            var process = new Process
            {
                StartInfo = {FileName = Const.SublimePath, Arguments = $"{asset}:{line}:0", CreateNoWindow = false}
            };
            process.Start();
            return true;
        }

        [OnOpenAsset(1)]
        public static bool RelocationLog(int instance, int line)
        {
            
            return false;
        }
        
        [DrawGizmo(GizmoType.NonSelected)]
        private static void ShowNonSelectObjectShaderName(Transform transform, GizmoType gizmoType)
        {
            if (!SceneManager.GetActiveScene().name.Contains("Shader")) return;
            var render = transform.GetComponent<MeshRenderer>();
            if (render == null) return;
            if (render.sharedMaterial == null) return;
            var shaderName = render.sharedMaterial.shader.name;
            var curPos = transform.position;
            var camera = SceneView.currentDrawingSceneView.camera;
            var transform1 = camera.transform;
            var dir = curPos - transform1.position;
            var forward = transform1.forward;
            if (Vector3.Dot(dir, forward) > 0)
            {
                Handles.Label(curPos, Path.GetFileName(shaderName), new GUIStyle(){normal = new GUIStyleState(){textColor = (GizmoType.Selected & gizmoType) != 0 ? Color.red : Color.yellow}, fontSize = 20});
            }

            // var light = GameObject.FindWithTag("Light");
            // if (light == null) return;
            // var target = Vector3.Normalize(light.transform.eulerAngles) * 10;
            // Handles.DrawLine(curPos, curPos + target );
        }

        [DrawGizmo(GizmoType.Selected)]
        private static void ShowSelectObjectShaderName(Transform transform, GizmoType gizmoType)
        {
            ShowNonSelectObjectShaderName(transform, gizmoType);
        }
    }
}