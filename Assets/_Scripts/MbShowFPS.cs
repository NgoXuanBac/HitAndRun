using UnityEngine;

namespace HitAndRun
{
    public class MbShowFPS : MonoBehaviour
    {
        private float _fps, _ms, _deltaTime = 0f;
        private GUIStyle _style;
        private Rect _rect;
        private int _count = 0;

        private void Start()
        {
            var _font = Resources.Load<Font>("Fonts/LilitaOne-Regular");
            _style = new GUIStyle
            {
                fontSize = (int)(Screen.height * 0.02f),
                fontStyle = FontStyle.Normal,
                alignment = TextAnchor.UpperLeft,
                font = _font,
                normal =
                {
                    textColor = Color.white
                }
            };
            _rect = new Rect(0, 0, Screen.width, Screen.height);
        }

        private void Update()
        {
            _count++;
            _deltaTime += Time.deltaTime;
            if (!(_deltaTime >= 1f)) return;
            _fps = _count / _deltaTime;
            _ms = _deltaTime * 1000 / _count;
            _count = 0;
            _deltaTime = 0f;
        }

        private void OnGUI()
        {
            var info = $"{_fps:0.0}fps，{_ms:0.0}ms";
            GUI.Label(_rect, info, _style);
        }
    }
}