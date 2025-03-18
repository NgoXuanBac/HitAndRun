using System;
using System.Collections.Generic;
using HitAndRun.Bullet;
using UnityEditor;
using UnityEngine;

namespace HitAndRun.Character
{
    public class MbTeam : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] private float _gap = 0.4f;
        [SerializeField] private Transform _follow;
        [SerializeField] private MbTeamMovement _movement;
        public MbTeamMovement Movement => _movement;
        private MbCharacter _head;
        private MbCharacter _tail;

        private void Reset()
        {
            _follow = transform.Find("Follow");
            _movement = GetComponent<MbTeamMovement>();
            _movement.enabled = false;
        }

        private void Start()
        {
            _movement.enabled = false;
            _movement.OnFinish += Attack;
        }

        public void SetUp()
        {
            _movement.enabled = false;
            transform.localPosition = Vector3.forward * 10f;
            _follow.localPosition = new Vector3(0, _follow.localPosition.y, _follow.localPosition.z);
            _movement?.Reset();

            _head = _tail = null;
            var character = MbCharacterSpawner.Instance.Spawn(transform.position, transform);
            character.OnGrab += Collect;
            character.OnDead += Leave;
            _head = _tail = character;
        }

        public void AddCharacter()
        {
            // for (var i = _head; i != null; i = i.Right) i.IsActive = true;

            var character = MbCharacterSpawner.Instance.Spawn(transform.position + new Vector3(0, 0, 8f), null);

            var characters = new List<MbCharacter>();
            for (var i = _head; i != null; i = i.Right)
            {
                characters.Add(i);
            }
            Collect(
                characters[UnityEngine.Random.Range(0, characters.Count)],
                character,
                UnityEngine.Random.Range(0, 2) == 0
            );
        }

        public void ActiveCharacters()
        {
            for (var i = _head; i != null; i = i.Right)
            {
                i.IsActive = true;
            }
            _movement.enabled = true;
            InputHelper.GetTouches();
        }

        private void Attack(Vector3 position)
        {
            _movement.enabled = false;
            var sum = 0f;
            for (var i = _head; i != null; i = i.Right)
            {
                sum += i.Body.Width + _gap;
            }

            var startX = -sum * 0.5f + _head.Body.Width * 0.5f;
            for (var i = _head; i != null; i = i.Right)
            {
                i.IsAttack = true;
                i.transform.SetParent(null, true);
                i.Body.MoveToTarget(new Vector3(startX, 0, position.z));
                startX += i.Body.Width + _gap;
                i.ShootingPattern = new SingleShot();
            }
            _follow.position = new Vector3(0, _follow.position.y, _follow.position.z);
        }

        private void Update()
        {
            if (!_head || !_movement.enabled)
            {
                _movement.enabled = false;
                return;
            }

            var isMoving = false;
            for (var i = _head; i != null; i = i.Right)
            {
                isMoving |= i.Body.IsMoving;
            }

            if (!isMoving && _head)
            {
                for (var i = _head; i != null; i = i.Right)
                {
                    if (i.Right && i.Body.Level == i.Right.Body.Level)
                    {
                        if (i.IsMerging || i.Right.IsMerging) continue;

                        var d1 = Mathf.Abs(i.Body.Target.x - _follow.localPosition.x);
                        var d2 = Mathf.Abs(i.Right.Body.Target.x - _follow.localPosition.x);

                        var (merged, removed) = d1 <= d2 ? (i, Remove(i.Right)) : (i.Right, Remove(i));
                        removed.OnGrab -= Collect;
                        removed.OnDead -= Leave;
                        removed.Body.Target = merged.Body.Target;

                        merged.IsMerging = true;

                        removed.Body.WhenMoveCompleted(() =>
                        {
                            merged.Body.Level *= 2;
                            merged.IsMerging = false;
                            MbCharacterSpawner.Instance.Despawn(removed);
                            MbCharacterTracker.Instance.RemoveCharacter(removed);
                        });
                    }
                }

                Rearrange();
            }
        }

        private void Rearrange()
        {
            var center = _head;
            var min = Mathf.Abs(_head.transform.localPosition.x - _follow.localPosition.x);
            for (var i = _head.Right; i != null; i = i.Right)
            {
                var distance = Mathf.Abs(i.transform.localPosition.x - _follow.localPosition.x);
                if (distance < min)
                {
                    min = distance;
                    center = i;
                }
            }
            for (var i = center.Right; i != null; i = i.Right)
            {
                i.Body.Target = i.Left.Body.Target + (_gap + i.Left.Body.Width) * Vector3.right;
            }
            for (var i = center.Left; i != null; i = i.Left)
            {
                i.Body.Target = i.Right.Body.Target - (_gap + i.Right.Body.Width) * Vector3.right;
            }
            Follow();
        }

        private void Leave(MbCharacter remove)
        {
            remove.OnDead -= Leave;
            remove.OnGrab -= Collect;
            Remove(remove);
        }

        private void Collect(MbCharacter current, MbCharacter insert, bool isRight)
        {
            MbCharacterTracker.Instance.AddCharacter(insert);

            insert.IsActive = true;
            insert.transform.parent = transform;
            insert.OnDead += Leave;
            insert.OnGrab += Collect;

            var isEdge = isRight ? current.Right == null : current.Left == null;

            if (isRight) AddRightOf(current, insert);
            else AddLeftOf(current, insert);

            if (!isEdge)
            {
                var offset = _gap / 2 + insert.Body.Width / 2;
                for (var i = _head; i && i != insert; i = i.Right)
                    i.Body.Target -= offset * Vector3.right;

                for (var i = _tail; i && i != insert; i = i.Left)
                    i.Body.Target += offset * Vector3.right;
            }

            var direction = isRight ? 1 : -1;
            insert.Body.Target = current.Body.Target
                + (current.Body.Width / 2 + _gap + insert.Body.Width / 2) * direction * Vector3.right;
        }

        private void Follow()
        {
            var sum = 0f;
            var count = 0;
            for (var i = _head; i != null; i = i.Right)
            {
                sum += i.Body.Target.x;
                count++;
            }
            if (count == 0) return;
            _follow.localPosition = new Vector3(sum / count, _follow.localPosition.y, _follow.localPosition.z);
        }

        private MbCharacter Remove(MbCharacter current)
        {
            if (current == null) return current;

            if (current.Left != null)
                current.Left.Right = current.Right;

            if (current.Right != null)
                current.Right.Left = current.Left;

            if (_head == current) _head = current.Right;
            if (_tail == current) _tail = current.Left;

            return current;
        }

        private void AddRightOf(MbCharacter current, MbCharacter insert)
        {
            if (current == null || insert == null || current == insert) return;
            Remove(insert);

            if (current.Right == null)
            {
                current.Right = insert;
                insert.Left = current;
                _tail = insert;
            }
            else
            {
                insert.Right = current.Right;
                insert.Left = current;
                current.Right.Left = insert;
                current.Right = insert;
            }
        }

        private void AddLeftOf(MbCharacter current, MbCharacter insert)
        {
            if (current == null || insert == null || current == insert) return;
            Remove(insert);

            if (current.Left == null)
            {
                insert.Right = current;
                current.Left = insert;
                _head = insert;
            }
            else
            {
                insert.Left = current.Left;
                insert.Right = current;
                current.Left.Right = insert;
                current.Left = insert;
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(MbTeam))]
    public class ETeamInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            var team = (MbTeam)target;
            GUI.enabled = Application.isPlaying;
            if (GUILayout.Button("Add"))
            {
                team.AddCharacter();
            }
            GUI.enabled = true;
            EditorGUILayout.Space();
            DrawDefaultInspector();
        }
    }
#endif
}

