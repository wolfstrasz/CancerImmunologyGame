using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ImmunotherapyGame.Core;

namespace ImmunotherapyGame
{
    public class PacParticle : CellParticle
    {
        [Header ("Attributes")]
        [SerializeField]
        private float eatSpeed = 1f;

        [Header("Debug")]
        [ReadOnly]
        private Cell hitCell = null;
        
        
        // Animator callback?
        void OnFinishedEating()
		{
            if (hitCell != null)
                hitCell.ApplyHealthAmount(-Mathf.Abs(effectToHealth));

            Destroy(gameObject);
        }

		protected override void OnCollisionWithTarget(Cell cell)
		{
            hitCell = cell;
            coll.enabled = false;
            speed = eatSpeed;


            direction = cell.transform.position - transform.position;
            transform.right = direction;
            render.flipY = direction.x <= 0;
            animator.SetTrigger("Eat");
        }
	}
}
