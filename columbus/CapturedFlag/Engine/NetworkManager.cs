using System.Collections;
using UnityEngine;

namespace CapturedFlag.Engine
{
    public static class NetworkManager
    {
        public static IEnumerator TestConnection(System.Action<bool> resultHandler)
        {
            WWW conn = new WWW("http://www.google.com");
            yield return conn;
            resultHandler(conn.error == null);
        }
    }
}
