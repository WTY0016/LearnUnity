using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.U2D;
using Image = UnityEngine.UI.Image;

public class UIEntrance : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		AssetBundle.LoadFromFile("Assets/AssetBundles/images");
		var assetbundle = AssetBundle.LoadFromFile("Assets/AssetBundles/atlas");
		assetbundle.GetAllAssetNames().Log();
		var atlas = assetbundle.LoadAsset<SpriteAtlas>("spriteatlas");
		var sps = new Sprite[atlas.spriteCount];
		atlas.GetSprites(sps);
		// assetbundle.Unload(false);
		var plane = transform.GetChild(0);
		for(var i = 0; i < sps.Length; i++)
		{
			var image = new GameObject("image" + i);
			var imageCom = image.AddComponent<Image>();
			image.hideFlags = HideFlags.HideInHierarchy;
			image.transform.SetParent(plane);
			imageCom.sprite = sps[i];	
		}
		assetbundle.Unload(false);
	}

}
