using UnityEngine;

namespace HackedDesign
{
    public interface IAttackController
    {
        public bool IsAnimatingAttack { get; }
        public bool CanAttack { get; }
        public bool HasGun {  get; }
        public bool CanShoot {  get; }
        public void Shoot(Vector3 target);
        public void Melee();
    }
}
