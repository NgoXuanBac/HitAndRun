using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HitAndRun.Map;
using HitAndRun.Character;

namespace HitAndRun.DistanceProgressBar
{
    public class MbLevelBar : MonoBehaviour
    {
        [SerializeField] private Slider _progressBar;
        [SerializeField] private MbTeam _team;
        [SerializeField] private MbGround _ground;
        [SerializeField] private float _animationSpeed = 2f;
        [SerializeField] private float _offset = 15f;

        [SerializeField] private TMP_Text _current;
        [SerializeField] private TMP_Text _next;

        private void Reset()
        {
            _progressBar ??= GetComponent<Slider>();
            _current ??= transform.Find("Current").GetComponent<TMP_Text>();
            _next ??= transform.Find("Next").GetComponent<TMP_Text>();

            _team ??= FindFirstObjectByType<MbTeam>();
            _ground ??= FindFirstObjectByType<MbGround>();

            _offset = _team.transform.position.z;

            _progressBar.minValue = 0;
            _progressBar.maxValue = 1;
            _progressBar.value = 0;
        }
        private void Start()
        {
            MbGameManager.Instance.OnDataLoaded += UpdateUI;
        }

        private void UpdateUI(SaveData data)
        {
            _current.text = data.Level.ToString();
            _next.text = (data.Level + 1).ToString();
        }

        private void Update()
        {
            if (_progressBar == null || _team == null) return;

            var traveled = _team.transform.position.z - _offset;
            var distance = _ground.Finish.z - _offset * 2;

            var progress = Mathf.Clamp01(traveled / distance);

            _progressBar.value = Mathf.Lerp(_progressBar.value, progress, Time.deltaTime * _animationSpeed);

        }
    }
}

