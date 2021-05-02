using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame
{
    public class PacParticle : MonoBehaviour
    {
        [SerializeField]
        private Collider2D coll = null;
        [SerializeField]
        private Animator animator = null;

        [Header ("Attributes")]
        [SerializeField]
        private float damage = 2f;
        [SerializeField]
        private float speed = 4f;
        [SerializeField]
        private float eatSpeed = 1f;

        [Header("Debug")]
        [ReadOnly]
        private float distance = 0f;
        [ReadOnly]
        private Vector3 direction = Vector3.zero;
        [ReadOnly]
        private MatrixCell hitMatrix = null;

		private void FixedUpdate()
		{
            Vector3 moveVector = direction * speed * Time.fixedDeltaTime;
            distance -= moveVector.magnitude;
            transform.position += moveVector;

            if (distance <= 0f && hitMatrix == null)
			{
                coll.enabled = false;
                Destroy(gameObject);
			}
		}

		public void SetData(Vector3 direction, float distance)
		{
            this.direction = direction;
            this.distance = distance;
            hitMatrix = null;
            transform.right = direction;
		}

		public void OnTriggerEnter2D(Collider2D collision)
		{
            Debug.Log(collision.gameObject);
            MatrixCell matrix = collision.gameObject.GetComponent<MatrixCell>();
            if (matrix != null)
			{
                speed = eatSpeed;
                hitMatrix = matrix;
                coll.enabled = false;
                direction = matrix.transform.position - transform.position;
                transform.right = direction;
                animator.SetTrigger("Eat");
			}
		}

        void OnFinishedEating()
		{
            if (hitMatrix != null)
                hitMatrix.HitCell(damage);

            Destroy(gameObject);
        }
    }
}
