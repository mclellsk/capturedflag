using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections.Generic;
using CapturedFlag.Engine.Scriptables;

namespace CapturedFlag.Engine
{
    public static class CustomAssetUtility
    {
        /// <summary>
        /// Create an asset in the selected folder from the Unity Editor.
        /// </summary>
        /// <remarks>
        /// Source: http://www.jacobpennock.com/Blog/customassetutility-source/
        /// </remarks>
        /// <typeparam name="T">Asset type to create.</typeparam>
        public static void CreateAsset<T>() where T : ScriptableObject
        {
            var asset = ScriptableObject.CreateInstance<T>();

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString() + ".asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            //EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }

        /// <summary>
        /// Create an asset of the given type in the specified folder with the specified filename.
        /// If the folder doesn't exist, one is created, but the call must be made again to actually create
        /// the asset.
        /// </summary>
        /// <typeparam name="T">Type of asset to create.</typeparam>
        /// <param name="folder">Folder name.</param>
        /// <param name="filename">File name.</param>
        /// <returns>Asset created.</returns>
        public static T CreateAsset<T>(string folder, string filename) where T : ScriptableObject
        {
            var path = "Assets/" + folder;

            var fullPath = AssetDatabase.GenerateUniqueAssetPath(path + "/" + typeof(T).Name + "_" + filename + ".asset");

            var asset = AssetDatabase.LoadAssetAtPath(fullPath, typeof(T)) as T;
            if (asset == null)
            {
                if (!AssetDatabase.IsValidFolder(path))
                {
                    Debug.Log("Path does not exist: \"" + path + "\" Creating...");
                    AssetDatabase.CreateFolder("Assets", folder);
                }
                else
                {
                    asset = ScriptableObject.CreateInstance<T>();
                    string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(fullPath);
                    AssetDatabase.CreateAsset(asset, assetPathAndName);
                    AssetDatabase.SaveAssets();
                }
            }

            return asset;
        }

        public delegate void HandleFiles<T>(StreamWriter w, List<UnityEngine.Object> files);

        /// <summary>
        /// Search path is the folder in the Asset folder that should be examined for type T assets.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="searchPath"></param>
        /// <param name="suffix"></param>
        public static void GenerateGlobalVariables<T>(string searchPath, HandleFiles<T> handle, string suffix = "") where T : ScriptableObject
        {
            var fileName = "Global" + typeof(T).Name + suffix;
            var fullPath = "Assets/" + fileName + ".cs";
            var files = GetAssetsAtPath<T>("Resources/" + searchPath);
            using (StreamWriter w = new StreamWriter(fullPath, false))
            {
                w.WriteLine("using UnityEngine;");
                w.WriteLine("using System;");
                w.WriteLine("using System.Collections;");
                w.WriteLine("using System.Collections.Generic;");
                w.WriteLine("using CapturedFlag.Engine.Scriptables;");
                w.WriteLine("");
                w.WriteLine("public class " + fileName + " : MonoBehaviour");
                w.WriteLine("{");
                w.WriteLine("");
                w.WriteLine("\t/* GLOBAL VARIABLES */");
                w.WriteLine("");
                handle(w, files);
                w.WriteLine("");
                w.WriteLine("}");
            }
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Writes all assets of type T, where T is a child of BaseID, into a write stream meant for the body 
        /// of a global class used for referencing. This is to be used exclusively for the generation of global variables for type T.
        /// </summary>
        /// <typeparam name="T">Asset type.</typeparam>
        /// <param name="w">Write stream.</param>
        /// <param name="files">Files to iterate through.</param>
        public static void AddVariables<T>(StreamWriter w, List<UnityEngine.Object> files) where T : ScriptableObject
        {
            List<string> variableNames = new List<string>();
            List<string> assignments = new List<string>();

            foreach (UnityEngine.Object file in files)
            {
                var filename = file.name;
                var t = filename.Split('_');
                if (t.Length > 1)
                    filename = t[1];
                for (int i = 0; i < filename.Length; i++)
                {
                    if (Char.IsUpper(filename[i]) && filename[i] != '_')
                    {
                        filename = filename.Insert(i, "_");
                        i++;
                    }
                }

                var tFile = file as T;
                variableNames.Add(filename.ToUpper());
                w.WriteLine("\t" + "public static " + typeof(T).Name + " " + filename.ToUpper() + ";");
                assignments.Add("\t" + filename.ToUpper() + " = (" + typeof(T).Name + ")Resources.Load(\"" + AssetDatabase.GetAssetPath(file).Replace("Assets/Resources/", "").Replace(".asset", "") + "\");");
            }

            w.WriteLine("");
            w.WriteLine("\t" + "public static List<" + typeof(T).Name + "> idList = new List<" + typeof(T).Name + ">();");
            w.WriteLine("");
            w.WriteLine("\t" + "void Awake()");
            w.WriteLine("\t" + "{");
            w.WriteLine("\t" + "\t" + "DontDestroyOnLoad(this.gameObject);");
            w.WriteLine("\t" + "}");
            w.WriteLine("");
            w.WriteLine("\t" + "void Start()");
            w.WriteLine("\t" + "{");
            foreach (string assignment in assignments)
            {
                w.WriteLine("\t" + assignment);
            }
            w.WriteLine("");
            foreach (string variableName in variableNames)
            {
                w.WriteLine("\t\t idList.Add(" + variableName +");");
            }
            w.WriteLine("\t" + "}");
        }

        /// <summary>
        /// http://forum.unity3d.com/threads/how-to-get-list-of-assets-at-asset-path.18898/
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">Folder in Assets to search.</param>
        /// <returns></returns>
        public static List<UnityEngine.Object> GetAssetsAtPath<T>(string path)
        {
            List<UnityEngine.Object> assets = new List<UnityEngine.Object>();
            string[] files = Directory.GetFiles(Application.dataPath + "/" + path);
            foreach(string file in files)
            {
                int index = file.IndexOf("Assets");
                string localPath = file.Substring(index);

                var asset = AssetDatabase.LoadAssetAtPath(localPath, typeof(T));
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }
    }
}

