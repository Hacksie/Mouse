

using UnityEngine;

namespace HackedDesign
{
    [RequireComponent(typeof(Enemy))]
    public class DarkMec : MonoBehaviour
    {
        [SerializeField] private Enemy enemy;
        [SerializeField] private Animator animator;

        private void Awake()
        {
            if(enemy == null)
            {
                enemy = GetComponent<Enemy>();
            }

            animator = GetComponent<Animator>();

            enemy.currentState = EnemyState.Idle;
            enemy.updateBehaviour += UpdateBehaviour;
            
        }

        private void UpdateBehaviour()
        {
            /*
            switch(enemy.currentState)
            {
                case EnemyState.Dying:
                    animator.SetTrigger("die");
                    // FIXME: Check animator state to see if the animation has fully played out
                    enemy.currentState = EnemyState.Dead;
                    break;
                case EnemyState.Dead:
                    break;
                case EnemyState.Idle:
                    animator.Play("idle");
                    if(enemy.DistanceToPlayer() < 20)
                    {
                        enemy.currentState = EnemyState.Patrolling;
                    }
                    
                    break;
                case EnemyState.Patrolling:
                    animator.Play("walk");
                    if (enemy.DistanceToPlayer() < 7)
                    {
                        enemy.currentState = EnemyState.Hunting;
                    }
                    break;
                case EnemyState.Hunting:
                    animator.Play("attack low");
                    break;
            }*/

        }


    }
}
