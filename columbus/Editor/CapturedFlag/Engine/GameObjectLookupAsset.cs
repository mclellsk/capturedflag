using UnityEditor;

namespace CapturedFlag.Engine.Scriptables
{
    public class GameObjectLookupAsset
    {
        [MenuItem("Assets/Create/GameObject Lookup")]
        public static void CreateAsset()
        {
            CustomAssetUtility.CreateAsset<GameObjectLookup>();
        }
    }
}
