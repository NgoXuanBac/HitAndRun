using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HitAndRun.Gui
{
    public class MbLoading : MonoBehaviour
    {
        [SerializeField] private Transform _loading;
        [SerializeField] private float _minimumLoadingTime = 3f;
        [SerializeField] private float _speed = 2;
        [SerializeField] private string _sceneName;
        private void Reset()
        {
            _loading = transform.Find("Loading");
        }

        private void Start()
        {
            _loading.DOLocalRotate(Vector3.forward * 360, _speed, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);

            StartCoroutine(LoadSceneAsync(_sceneName));
        }

        private void OnDestroy()
        {
            _loading.DOKill();
        }

        IEnumerator LoadSceneAsync(string sceneName)
        {
            var startTime = Time.time;

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;

            while (operation.progress < 0.9f)
            {
                yield return null;
            }

            var elapsedTime = Time.time - startTime;
            if (elapsedTime < _minimumLoadingTime)
            {
                yield return new WaitForSeconds(_minimumLoadingTime - elapsedTime);
            }

            operation.allowSceneActivation = true;
        }
    }

}
