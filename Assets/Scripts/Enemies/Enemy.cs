using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

namespace HackedDesign
{
    [RequireComponent(typeof(CharController))]
    public class Enemy : MonoBehaviour
    {
        
        [SerializeField] public EnemyType type;
        [SerializeField] public EnemyState currentState;

        [SerializeField] public UnityAction updateBehaviour;
        [SerializeField] public UnityAction hitBehaviour;
        [SerializeField] public UnityAction deathBehaviour;
        [SerializeField] public CharController character;

        private void Awake()
        {

            this.AutoBind<CharController>(ref character);

            character.dieActions.AddListener(Die);
            character.hitActions.AddListener(Hit);
        }
        public void UpdateBehaviour()
        {
            updateBehaviour?.Invoke();
        }

        public void LateUpdateBehaviour()
        {
            character.LateUpdateBehaviour();
        }

        public void Update()
        {
            updateBehaviour?.Invoke();
        }

        public void LateUpdate()
        {
            character.LateUpdateBehaviour();
        }

        public float DistanceToPlayer()
        {
            return (Game.Instance.Player.transform.position - transform.position).magnitude;
        }

        public void TakeDamage(int amount, Vector3 contact)
        {
            character.OperatingSystem.Health -= amount;
        }

        private void Hit()
        {
            Debug.Log("took a hit", this);
        }

        private void Die()
        {
            Debug.Log("died", this);
            //currentState = EnemyState.Dying;
            //character.Die();
        }

        
    }

    public enum EnemyState
    {
        Idle,
        Patrolling,
        Alert,
        Hunting,
        Attack,
        Flanking
    }

    public enum EnemyType
    {
        Ground,
        Air
    }
}
