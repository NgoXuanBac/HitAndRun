using System;
using UnityEngine;

namespace Thai
{
    public class MoveForward : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 10f;

        private void Update()
        {
            transform.position += Vector3.forward * (moveSpeed * Time.deltaTime);
        }
    }
}