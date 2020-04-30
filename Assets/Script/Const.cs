using System.IO;
using UnityEngine;

public class Const
{
    public static readonly string AssetBundlePath = Path.Combine(Application.dataPath, "AssetBundles/");

    public static readonly string[] NeedBundledAssetPath = 
    {
        "Assets/Images/",
        "Assets/Prefabs/",
        "Assets/Materials/",
    };

    public const string SublimePath = "D:/Sublime TEXT 3/subl.exe";
}
