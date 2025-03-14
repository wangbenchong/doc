# 图集查询与管理



```c#
/////////////////////////////////////////////////////////////////////////////////
//
//  author: wangbenchong
//  date:   2022.5.25
//  desc:   ngui 界面图集使用分析
//							
//
/////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using Object = UnityEngine.Object;


public class UIResourceCheckEditor : EditorWindow
{
    private static UIResourceCheckEditor editor = null;
    [MenuItem("GameObject/【Z_WodCN】/Check ui art resource", false, 8)]
	static void ShowWindow()
	{
        if(editor != null)
        {
            editor.Init();
            return;
        }
		editor = GetWindow<UIResourceCheckEditor>();
        editor.titleContent = new GUIContent("UIResource Check");
        editor.Init();
    }
    public class Data
    {
        public string atlasName;
        public List<UISprite> sprites = new List<UISprite>();
        public bool foldOut = true;
    }
	GameObject selectGo;
    List<Data> datas = new List<Data>();
    private Vector2 m_vScrollVec;
    List<UITexture> textures = new List<UITexture>();
    private Vector2 m_vScrollVec2;
    List<GameObject> fxlist = new List<GameObject>();
    private Vector2 m_vScrollVec3;
    int modeIndex = 0;
    string[] m_tabNames = { "Sprite Atlas", "Texture", "fx_ui particle" };


    public void Init()
    {
        if (Selection.gameObjects == null || Selection.gameObjects.Length < 1)
        {
            return;
        }
        selectGo = Selection.gameObjects[0];
        if (selectGo == null)
        {
            return;
        }
        Refresh();
    }
    private void Refresh()
    {
        datas.Clear();
        textures.Clear();
        fxlist.Clear();
        if (selectGo == null)
        {
            return;
        }
        //--------------fill datas--------
        Dictionary<string, int> nameIndexDic = new Dictionary<string, int>();
        var arr = selectGo.GetComponentsInChildren<UISprite>(true);
        for (int i = 0; arr != null && i < arr.Length; i++)
        {
            var spr = arr[i];
            string atlasName = spr.atlasName;
            if (string.IsNullOrEmpty(atlasName))
            {
                continue;
            }
            if (nameIndexDic.ContainsKey(atlasName))
            {
                var data = datas[nameIndexDic[atlasName]];
                data.sprites.Add(spr);
            }
            else
            {
                var data = new Data();
                data.sprites.Add(spr);
                data.atlasName = atlasName;
                datas.Add(data);
                nameIndexDic.Add(atlasName, datas.Count - 1);
            }
        }
        for(int i=0;i<datas.Count;i++)
        {
            datas[i].sprites.Sort(_SortSprite);
        }
        //--------------fill textures--------
        var arr2 = selectGo.GetComponentsInChildren<UITexture>(true);
        for (int i = 0; arr2 != null && i < arr2.Length; i++)
        {
            var tex = arr2[i];
            if(tex.mainTexture != null)
            {
                textures.Add(tex);
            }
        }
        textures.Sort(_SortTexture);
        //--------------fill fxlist--------
        var arr3 = selectGo.GetComponentsInChildren<UIParticleWidget>(true);
        HashSet<GameObject> hash = new HashSet<GameObject>();
        for (int i = 0; arr3 != null && i < arr3.Length; i++)
        {
            var particle = arr3[i];
            Transform target = particle.transform.parent;
            if (target != null)
            {
                if (!target.name.StartsWith("fx_ui"))
                {
                    target = target.parent;
                }
                if (target != null && target.name.StartsWith("fx_ui"))
                {
                    if (target.parent != null && target.parent.name.StartsWith("fx_ui"))
                    {
                        target = target.parent;
                    }
                    hash.Add(target.gameObject);
                }
            }
        }
        fxlist.AddRange(hash);
        fxlist.Sort(_SortGameObject);
    }
    private int _SortSprite(UISprite a, UISprite b)
    {
        if(a == null || b == null)
        {
            return 0;
        }
        return a.spriteName.CompareTo(b.spriteName);
    }
    private int _SortTexture(UITexture a, UITexture b)
    {
        int n =  GetMaxSize(b) - GetMaxSize(a);
        if(n == 0)
        {
            return string.Compare(a.mainTexture.name, b.mainTexture.name);
        }
        return n;
    }
    private int _SortGameObject(GameObject a, GameObject b)
    {
        return string.Compare(a.name, b.name);
    }
    private int GetMaxSize(UITexture t)
    {
        if(t.mainTexture == null)
        {
            return 0;
        }
        return Mathf.Max(t.mainTexture.width, t.mainTexture.height);
    }

	void OnGUI()
	{
        selectGo = EditorGUILayout.ObjectField("Root GameObject", selectGo, typeof(GameObject), true) as GameObject;
        if (selectGo == null)
        {
            return;
        }
        if(GUILayout.Button("Refresh") || GUI.changed)
        {
            Refresh();
            return;
        }
        modeIndex = GUILayout.SelectionGrid(modeIndex, m_tabNames, m_tabNames.Length);
        if (modeIndex == 0)
        {
            ShowFirstTab();
        }
        else if(modeIndex == 1)
        {
            ShowSecondTab();
        }
        else if (modeIndex == 2)
        {
            ShowThirdTab();
        }
    }
    private void ShowFirstTab()
    {
        m_vScrollVec = EditorGUILayout.BeginScrollView(m_vScrollVec, false, false);
        for (int i = 0; i < datas.Count; i++)
        {
            EditorGUIUtility.labelWidth = 200;
            var data = datas[i];
            data.foldOut = EditorGUILayout.Foldout(data.foldOut, data.atlasName);
            if (data.foldOut)
            {
                EditorGUIUtility.labelWidth = 30;
                for (int j = 0; j < data.sprites.Count; j++)
                {
                    var spr = data.sprites[j];
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.ObjectField((j + 1).ToString(), spr, typeof(UISprite), true);
                    EditorGUILayout.LabelField(spr == null ? " " : spr.spriteName);
                    EditorGUILayout.EndHorizontal();
                }
            }
            GUILayout.Space(3);
        }
        EditorGUILayout.EndScrollView();
    }
    private void ShowSecondTab()
    {
        m_vScrollVec2 = EditorGUILayout.BeginScrollView(m_vScrollVec2, false, false);
        EditorGUIUtility.labelWidth = 30;
        for (int i = 0; i < textures.Count; i++)
        {
            var tex = textures[i];
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.ObjectField((i + 1).ToString(), tex, typeof(UITexture), true);
            if(tex != null && tex.mainTexture != null)
            {
                EditorGUILayout.LabelField(GetMaxSize(tex).ToString(), GUILayout.Width(50));
                EditorGUILayout.LabelField(tex.mainTexture.name);
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
    } 
    private void ShowThirdTab()
    {
        m_vScrollVec3 = EditorGUILayout.BeginScrollView(m_vScrollVec3, false, false);
        EditorGUIUtility.labelWidth = 30;
        for (int i = 0; i < fxlist.Count; i++)
        {
            var go = fxlist[i];
            EditorGUILayout.ObjectField((i + 1).ToString(), go, typeof(GameObject), true);
        }
        EditorGUILayout.EndScrollView();
    }
}
```

# 图集反解

```c#
#region NGUI图集反解
[MenuItem("Assets/Z_WodCN/NGUI图集反解为原图")]
private static void Unpack()
{
    if (!Selection.activeGameObject)
    {
        return;
    }
    var atlas = Selection.activeGameObject.GetComponent<UIAtlas>();
    if(atlas == null)
    {
        return;
    }
    CutTextures(atlas);
}


/// <summary>;
/// 将图集中的所有图片拆分并保存
/// </summary>;
/// <param name="_atlas">;图集</param>;
/// <param name="_refresh">;是否立即更新</param>;
private static void CutTextures(UIAtlas _atlas)
{
    if (_atlas == null) return;
    if (_atlas.GetListOfSprites() == null) return;
    if (_atlas.GetListOfSprites().buffer == null) return;
    if (_atlas.GetListOfSprites().buffer.Length <= 0) return;
    string save_Atlas_path = Application.dataPath + "/Work2D";
    // 创建路径
    var path = save_Atlas_path;
    if (!System.IO.Directory.Exists(path))
    {
        Directory.CreateDirectory(path);
    }
    path = save_Atlas_path + "/" + _atlas.name;
    if (!System.IO.Directory.Exists(path))
    {
        Directory.CreateDirectory(path);
    }
    // 开始
    var sprites = _atlas.GetListOfSprites();
    float totalCount = sprites.size;
    int counter = 0;
    foreach (string spriteName in sprites)
    {
        path = save_Atlas_path + "/" + _atlas.name + "/" + spriteName + ".png";
        if (EditorUtility.DisplayCancelableProgressBar("Is puting png files into Work2D Folder", path, counter / totalCount))
        {
            break;
        }
        SaveSpriteAsTexture(_atlas, spriteName, path);
        counter ++;
    }
    EditorUtility.ClearProgressBar();
    if (EditorUtility.DisplayDialog("Done", "Created file count(save to Work2D):" + counter, "ok"))
    {
        AssetDatabase.Refresh();
    }
}
/// <summary>;
/// 保存图集中的图片
/// </summary>;
/// <param name="_atlas">;图片所在的图集</param>;
/// <param name="_sprite">;图集中要保存的图片名字</param>;
/// <param name="_path">;要保存的路径</param>;
private static void SaveSpriteAsTexture(UIAtlas _atlas, string _sprite, string _path)
{
    UIAtlasMaker.SpriteEntry se = UIAtlasMaker.ExtractSprite(_atlas, _sprite);
    if (se != null)
    {
        byte[] bytes = se.tex.EncodeToPNG();
        System.IO.File.WriteAllBytes(_path, bytes);
        AssetDatabase.ImportAsset(_path);
        if (se.temporaryTexture)
            UnityEngine.Object.DestroyImmediate(se.tex);
    }
}
#endregion
```



# 源码改良

```c#
//TODO仅选中物体显示白框
```

