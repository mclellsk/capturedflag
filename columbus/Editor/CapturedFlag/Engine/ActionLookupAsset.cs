using UnityEditor;

namespace CapturedFlag.Engine.Scriptables
{
    public class ActionLookupAssets
    {
        [MenuItem("Assets/Create/Action Lookup")]
        public static void CreateAsset()
        {
            CustomAssetUtility.CreateAsset<ActionLookup>();
        }
    }
}
