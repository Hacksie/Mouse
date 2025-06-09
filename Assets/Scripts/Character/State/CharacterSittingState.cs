using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HackedDesign
{
    public class CharacterSittingState : ICharacterState
    {
        private readonly Animator animator;

        public bool IsAlive => true;

        public CharacterSittingState(Animator animator)
        {
            this.animator = animator;
        }

        public void Animate(CharacterAnimationContext ctx)
        {
            if (this.animator == null)
            {
                return;
            }
            this.animator.SetBool(AnimatorParams.Sit, true);
            this.animator.SetBool(AnimatorParams.Grounded, true);
            this.animator.SetBool(AnimatorParams.Dead, false);
        }

        public void ResetAnimationTriggers()
        {
            if (this.animator == null)
            {
                return;
            }
            this.animator.ResetTrigger(AnimatorParams.Interact);
            this.animator.ResetTrigger(AnimatorParams.Roll);
            this.animator.ResetTrigger(AnimatorParams.Melee);
            this.animator.ResetTrigger(AnimatorParams.BasicAttack);
            this.animator.ResetTrigger(AnimatorParams.StrongAttack);
            this.animator.ResetTrigger(AnimatorParams.Jump);
        }

        public void Begin()
        {
        }

       public void End()
        {
        }

        public void Attack(CharacterAttackContext ctx)
        {

        }
        public float CurrentSpeed(CharacterSpeedContext ctx) => 0;
    }
}
