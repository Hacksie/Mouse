
using UnityEngine;

namespace HackedDesign
{
    public interface ICharacterState
    {
        public void Begin();
        public void End();

        public void Animate(CharacterAnimationContext ctx);

        public void ResetAnimationTriggers();

        public void Attack(CharacterAttackContext ctx);

        public float CurrentSpeed(CharacterSpeedContext ctx);

        bool IsAlive {  get; }
    }
}
