

using System.Collections.Generic;
using UnityEngine;

namespace HackedDesign
{
    [CreateAssetMenu(fileName = "CharacterSettings", menuName = "Mouse/Settings/Character")]
    public class CharacterSettings : ScriptableObject
    {
        [SerializeField] public float attackRate = 0.33f;
        [SerializeField] public float walkSpeed = 1.5f;
        [SerializeField] public float runSpeed = 3;
        [SerializeField] public float slideSpeed = 5;
        [SerializeField] public float crouchSpeed = 2;
        [Range(0, .3f)] [SerializeField] public float movementSmoothing = .05f;
        [SerializeField] public float stealthRate = 1f;
        [SerializeField] public float lookAngle = 0;
        [SerializeField] public float maxAngle = 75.0f;
        [SerializeField] public float minAngle = -25.0f;
        [SerializeField] public float shootDistance = 10.0f;
        [SerializeField] public float meleeDistance = 10.0f;
        [SerializeField] public float interactDistance = 1.0f;
        [SerializeField] public LayerMask shootMask = 0;
        [SerializeField] public LayerMask interactMask = 0;
        [SerializeField] public int maxHealth = 100;
        [SerializeField] public int health = 100;
        [SerializeField] public int ammo = 6;
        [SerializeField] public bool infiniteAmmo = false;
        [SerializeField] public bool infiniteHealth = false;
        [SerializeField] public int minBulletDamage = 50;
        [SerializeField] public int maxBulletDamage = 150;
        [SerializeField] public int minMeleeDamage = 25;
        [SerializeField] public int maxMeleeDamage = 100;
        [SerializeField] public List<WeaponSettings> weaponSettings;

    }
}