using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace HitAndRun.Setting
{
    public class Menu : MonoBehaviour
    {
        // Phương thức để load Scene theo tên
        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        // Load Scene không bị giật lag
        IEnumerator LoadSceneAsync(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }


    }
}
