using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HitAndRun
{
    public class InputHelper
    {
        private static TouchCreator lastTouch;
        public static List<Touch> GetTouches()
        {
            var touches = new List<Touch>();
            touches.AddRange(Input.touches);
#if UNITY_EDITOR
            lastTouch ??= new TouchCreator();
            if (Input.GetMouseButtonDown(0))
            {
                lastTouch.phase = TouchPhase.Began;
                lastTouch.deltaPosition = new Vector2(0, 0);
                lastTouch.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                lastTouch.fingerId = 0;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                lastTouch.phase = TouchPhase.Ended;
                var newPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                lastTouch.deltaPosition = newPosition - lastTouch.position;
                lastTouch.position = newPosition;
                lastTouch.fingerId = 0;
            }
            else if (Input.GetMouseButton(0))
            {
                lastTouch.phase = TouchPhase.Moved;
                var newPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                lastTouch.deltaPosition = newPosition - lastTouch.position;
                lastTouch.position = newPosition;
                lastTouch.fingerId = 0;
            }
            else
            {
                lastTouch = null;
            }
            if (lastTouch != null) touches.Add(lastTouch.Create());
#endif
            return touches;
        }

        private class TouchCreator
        {
            static readonly BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
            static readonly Dictionary<string, FieldInfo> fields;
            readonly object touch;
            public float deltaTime { get { return ((Touch)touch).deltaTime; } set { fields["m_TimeDelta"].SetValue(touch, value); } }
            public int tapCount { get { return ((Touch)touch).tapCount; } set { fields["m_TapCount"].SetValue(touch, value); } }
            public TouchPhase phase { get { return ((Touch)touch).phase; } set { fields["m_Phase"].SetValue(touch, value); } }
            public Vector2 deltaPosition { get { return ((Touch)touch).deltaPosition; } set { fields["m_PositionDelta"].SetValue(touch, value); } }
            public int fingerId { get { return ((Touch)touch).fingerId; } set { fields["m_FingerId"].SetValue(touch, value); } }
            public Vector2 position { get { return ((Touch)touch).position; } set { fields["m_Position"].SetValue(touch, value); } }
            public Vector2 rawPosition { get { return ((Touch)touch).rawPosition; } set { fields["m_RawPosition"].SetValue(touch, value); } }

            public Touch Create()
            {
                return (Touch)touch;
            }

            public TouchCreator()
            {
                touch = new Touch();
            }

            static TouchCreator()
            {
                fields = new Dictionary<string, FieldInfo>();
                foreach (var f in typeof(Touch).GetFields(flag))
                {
                    fields.Add(f.Name, f);
                }
            }
        }
    }



}
