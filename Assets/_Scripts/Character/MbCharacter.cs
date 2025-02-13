using System.Threading;
using Cysharp.Threading.Tasks;
using HitAndRun.Bullet;
using UnityEditor;
using UnityEngine;

namespace HitAndRun.Character
{
#if UNITY_EDITOR
    [CustomEditor(typeof(MbCharacter))]
    public class ECharacterInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            var character = (MbCharacter)target;
            GUI.enabled = Application.isPlaying;
            var shooting = character.ShootingPattern;
            var text = shooting is SpreadShot ? "SingleShot" : "SpreadShot";
            if (GUILayout.Button($"Shooting Pattern: {text}"))
            {
                character.SetShootingPattern(shooting is SpreadShot ? new SingleShot() : new SpreadShot());
            }
            GUI.enabled = true;
            EditorGUILayout.Space();
            DrawDefaultInspector();
        }
    }
#endif
    [RequireComponent(typeof(MbCharacterBody))]
    public class MbCharacter : MonoBehaviour
    {
        [SerializeField] private MbCharacterBody _body;
        [SerializeField, Range(0, 1)] private float _fireRate = 0.2f;
        [SerializeField, Range(1, 100)] private float _bulletSpeed = 50f;
        public float FireRate => _fireRate;
        private IShootingPattern _shootingPattern = new SingleShot();
        public IShootingPattern ShootingPattern => _shootingPattern;
        public MbCharacterBody Body => _body;
        [SerializeField] private Transform _shooter;
        private CancellationTokenSource _cts = new();

        public void Reset()
        {
            _body ??= GetComponent<MbCharacterBody>();
            _shooter = transform.Find("Shooter");
            _body.Level = 2;
        }
        private void OnEnable()
        {
            _cts = new CancellationTokenSource();
            AutoShoot();
        }

        private async void AutoShoot()
        {
            while (!_cts.IsCancellationRequested)
            {
                _shootingPattern.Shoot(_bulletSpeed, _shooter);
                await UniTask.Delay((int)(_fireRate * 1000));
            }
        }

        public void SetShootingPattern(IShootingPattern pattern)
        {
            _shootingPattern = pattern;
        }
        private void OnDisable()
        {
            _cts.Cancel();
        }

    }
}

