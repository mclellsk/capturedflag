using UnityEngine;
using UnityEngine.SceneManagement;

namespace CapturedFlag.Engine
{
    public class LoadingScreen : MonoBehaviour
    {
        public static LoadingScreen instance;

        public AsyncOperation operation;

        public GameObject display;

        void Awake()
        {
            if (instance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }

        void LateUpdate()
        {
            if (operation != null)
            {
                if (operation.progress < 1f)
                {
                    var position = GameObject.FindGameObjectWithTag("UI").transform.position;

                    if (display != null && !display.activeSelf)
                    {
                        if (position != null)
                            display.transform.position = new Vector3(position.x, position.y, position.z + 1);

                        display.SetActive(true);
                    }
                }
                else
                {
                    display.SetActive(false);
                    operation = null;
                }
            }
        }

        public void LoadLevel(string scene)
        {
            operation = SceneManager.LoadSceneAsync(scene);
        }
    }
}
