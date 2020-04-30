using System.Collections.Generic;
using System.IO;
using Script.AlogLearn;
using UnityEngine;
using XLua;

public class LuaEntrance
{
    public static LuaEntrance Instance { get; set; }
    private string[] luaFilePath = {
        "Assets\\Script\\Lua\\"
    };

    private LuaEnv env;
    public LuaEntrance()
    {
        Instance = this;
        env = new LuaEnv();
        env.AddLoader(CustomLoader);
    }
    
    public void Start()
    {
        var objs = new List<GameObject>{new GameObject("1"), new GameObject("2"), new GameObject("3"),};
        env.Global.Set("equip", objs);
        env.DoString("require 'main'");
    }
    private byte[] CustomLoader(ref string filePath)
    {
        for (int i = 0; i < luaFilePath.Length; i++)
        {
            var path = Path.Combine(luaFilePath[i], filePath + ".lua");
            Debug.LogFormat("search path {0}", path);
            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
        }
        return null;
    }
}
