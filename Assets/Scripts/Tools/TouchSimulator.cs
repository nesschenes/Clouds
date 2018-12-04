using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Clouds.Extensions
{
    public class InputHelper : MonoBehaviour
    {
#if UNITY_EDITOR
        static TouchCreator mLastFakeTouch;
#endif

        /// <summary>Include fake touches</summary>
        public static Touch[] GetTouches()
        {
            List<Touch> touches = new List<Touch>();
            touches.AddRange(Input.touches);

#if UNITY_EDITOR
            if (mLastFakeTouch == null)
                mLastFakeTouch = new TouchCreator();

            if (Input.GetMouseButtonDown(0))
            {
                mLastFakeTouch.phase = TouchPhase.Began;
                mLastFakeTouch.deltaPosition = new Vector2(0, 0);
                mLastFakeTouch.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                mLastFakeTouch.fingerId = 0;
            }
            else if (Input.GetMouseButton(0))
            {
                Vector2 newPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                mLastFakeTouch.deltaPosition = newPosition - mLastFakeTouch.position;
                mLastFakeTouch.phase = mLastFakeTouch.deltaPosition == Vector2.zero ? TouchPhase.Stationary : TouchPhase.Moved;
                mLastFakeTouch.position = newPosition;
                mLastFakeTouch.fingerId = 0;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Vector2 newPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                mLastFakeTouch.deltaPosition = newPosition - mLastFakeTouch.position;
                mLastFakeTouch.phase = TouchPhase.Ended;
                mLastFakeTouch.position = newPosition;
                mLastFakeTouch.fingerId = 0;
            }
            else
            {
                mLastFakeTouch = null;
            }

            if (mLastFakeTouch != null)
                touches.Add(mLastFakeTouch.Create());
#endif

            return touches.ToArray();
        }
    }

    public class TouchCreator
    {
        static BindingFlags flag = BindingFlags.Instance | BindingFlags.NonPublic;
        static Dictionary<string, FieldInfo> fields;

        object touch;

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
            foreach (var f in typeof(Touch).GetFields(BindingFlags.Instance | BindingFlags.NonPublic))
                fields.Add(f.Name, f);
        }
    }
}