

using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "CharacterSettings", menuName = "Mouse/Settings/Character")]
    public class CharacterSettings : ScriptableObject
    {
        [SerializeField] private float attackRate = 0.33f;
        [SerializeField] private float walkSpeed = 4f;
        [SerializeField] private float runSpeed = 9;
        [SerializeField] private float crouchSpeed = 2;
        [Range(0, .3f)] [SerializeField] public float movementSmoothing = .05f;
        [SerializeField] private float shootDistance = 10.0f;
        [SerializeField] private float meleeDistance = 10.0f;
        [SerializeField] private float interactDistance = 1.0f;
        [SerializeField] private LayerMask attackMask = 0;
        [SerializeField] public LayerMask interactMask = 0;
        //[SerializeField] public int maxHealth = 100;
        [SerializeField] public int startingHealth = 100;
        [SerializeField] public int startingAmmo = 6;
        [SerializeField] public bool infiniteAmmo = false;
        [SerializeField] public bool infiniteHealth = false;
        [SerializeField] public int minBulletDamage = 50;
        [SerializeField] public int maxBulletDamage = 150;
        [SerializeField] public int minMeleeDamage = 25;
        [SerializeField] public int maxMeleeDamage = 100;
        [SerializeField] public List<WeaponSettings> weaponSettings;
        [SerializeField] public float momentumAirLoss = 0.5f;
        [SerializeField] public float baseMomentumFactor = 0.1f;
        [SerializeField] public float attackMomentumLoss = 0.5f;
        [SerializeField] public float minMomentum = 0f;
        [SerializeField] public float maxMomentum = 5f;

        public float ShootDistance { get => shootDistance; private set => shootDistance = value; }
        public float AttackRate { get => attackRate; private set => attackRate = value; }
        public float WalkSpeed { get => walkSpeed; private set => walkSpeed = value; }
        public float RunSpeed { get => runSpeed; private set => runSpeed = value; }
        public float CrouchSpeed { get => crouchSpeed; private set => crouchSpeed = value; }
        public float InteractDistance { get => interactDistance; private set => interactDistance = value; }
        public float MeleeDistance { get => meleeDistance; private set => meleeDistance = value; }
        public LayerMask AttackMask { get => attackMask; private set => attackMask = value; }
    }
}