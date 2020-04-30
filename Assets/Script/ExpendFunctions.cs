using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public static class ExpendFunctions
{ 
    public static void Foreach<T>(this IEnumerable<T> array, Action<T> action) 
    {
        foreach (var item in array)
        {
            action(item);
        }
    }

    public static void Log<T>(this IEnumerable<T> array)
    {
        array.Foreach(obj => Debug.Log(obj));
    }

}
