using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using Script.AlogLearn;
using UnityEngine;
using UnityEngine.Profiling;


public class AssetBundleLearn:MonoBehaviour
{
    private AssetBundle _manifestAssetBundle;
    private AssetBundleManifest _manifest;
    private void Start()
    {
        _manifestAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Const.AssetBundlePath, "AssetBundles"));
        _manifest = _manifestAssetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        // yield return StartCoroutine(LoadAssetBundle("models.ty", OnLoadABAsync));
        // LoadAssetBundleAsync("models.ty", OnLoadABAsync);
        Debug.Log("start");
    }

    private async Task LoadAssetBundleAsync(string abName, Action<AssetBundle> cb = null)
    {
        Debug.Log("11111");
        var bundle = AssetBundle.LoadFromFileAsync(Path.Combine(Const.AssetBundlePath, abName));
        await bundle;
        Debug.Log("await bundle ing....");
        foreach (var dependency in _manifest.GetAllDependencies(abName))
        {
            Debug.Log(dependency);
            await AssetBundle.LoadFromFileAsync(Path.Combine(Const.AssetBundlePath, dependency));
        }
        cb?.Invoke(bundle.assetBundle);
        Debug.Log("finish");
    }

    private IEnumerator LoadAssetBundle(string abName, Action<AssetBundle> cb = null)
    {
        var bundle = AssetBundle.LoadFromFileAsync(Path.Combine(Const.AssetBundlePath, abName));
        yield return bundle;
        foreach (var dependency in _manifest.GetAllDependencies(abName))
        {
            Debug.Log(dependency);
            yield return AssetBundle.LoadFromFileAsync(Path.Combine(Const.AssetBundlePath, dependency));
        }
        cb?.Invoke(bundle.assetBundle);
    }
    

    private void OnLoadABAsync(AssetBundle ab)
    {
        Profiler.BeginSample("CreateCube");
        var cube = ab.LoadAsset<GameObject>("Cube");
        Instantiate(cube);
        // AssetBundle.UnloadAllAssetBundles(false);
        Instantiate(cube);
        Profiler.EndSample();
    }
}
