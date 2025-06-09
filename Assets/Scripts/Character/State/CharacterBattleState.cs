
using UnityEngine;

namespace HackedDesign
{
    public class CharacterBattleState : ICharacterState
    {
        private const float ShootShakeIntensity = 0.7f;
        private const float ShootShakeTime = 0.1f;
        private readonly ICharacterExecute characterExecute;
        private readonly Animator animator;
        private float nextAttackTimer = int.MinValue;

        public bool IsAlive => true;

        public CharacterBattleState(ICharacterExecute charExecute, Animator animator)
        {
            this.animator = animator;
            this.characterExecute = charExecute;
        }

        public void Animate(CharacterAnimationContext ctx)
        {
            if (this.animator == null)
            {
                return;
            }
            this.animator.SetBool(AnimatorParams.Sit, false);
            this.animator.SetBool(AnimatorParams.Dead, false);
            this.animator.SetBool(AnimatorParams.Crouched, ctx.crouched);
            this.animator.SetBool(AnimatorParams.Grounded, ctx.onGround);
            this.animator.SetBool(AnimatorParams.Hang, ctx.onWall);
            this.animator.SetFloat(AnimatorParams.VelocityY, ctx.velocityY);
            this.animator.SetFloat(AnimatorParams.MovementMagnitude, ctx.movementMagnitude);
            this.animator.SetBool(AnimatorParams.RollOnLand, ctx.rollOnLand);
            this.animator.SetFloat(AnimatorParams.Aiming, ctx.aiming ? 1 : 0);
            this.animator.SetBool(AnimatorParams.LedgeStart, ctx.isClimbingLedge);
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
            if (Time.time >= nextAttackTimer)
            {
                if (ctx.operatingSystem.HasAmmo && ctx.aiming && ctx.operatingSystem.HasPistol)
                {
                    ctx.operatingSystem.DecreaseAmmo();
                    //operatingSystem.Momentum -= settings.attackMomentumLoss;
                    Shoot(ctx);
                    this.animator.SetTrigger(AnimatorParams.BasicAttack);

                }
                else
                {
                    //operatingSystem.Momentum -= settings.attackMomentumLoss;
                    Melee(ctx);
                    switch (Random.Range(0, 2 + (ctx.operatingSystem.HasAmmo && ctx.operatingSystem.HasPistol ? 1 : 0)))
                    {
                        case 0:
                            this.animator.SetTrigger(AnimatorParams.Punch);
                            break;
                        case 1:
                            this.animator.SetTrigger(AnimatorParams.Kick);
                            break;
                        case 2:
                            this.animator.SetTrigger(AnimatorParams.Melee);
                            break;
                    }
                }

                nextAttackTimer = Time.time + ctx.settings.AttackRate;
            }
        }

        private void Shoot(CharacterAttackContext ctx)
        {
            if (ctx.isPlayer)
            {
                CameraShake.Instance.Shake(ShootShakeIntensity, ShootShakeTime);
            }

            if (ctx.target != null)
            {
                var result = Physics2D.Raycast(ctx.pivot, ctx.target - ctx.pivot, ctx.settings.ShootDistance, ctx.settings.AttackMask);

                Debug.DrawRay(ctx.pivot, ctx.target - ctx.pivot, Color.red, 0.3f);

                if (result)
                {

                    if (result.transform.TryGetComponent<BreakGlass>(out var glass))
                    {
                        glass.Break(ctx.pivot);
                    }
                    if (result.transform.TryGetComponent<CharController>(out var targetChar))
                    {

                        var weapon = ctx.operatingSystem.CurrentWeapon;
                        var damage = Random.Range(weapon.minShootDamage, weapon.maxShootDamage + 1);
                        Debug.Log("Shoot " + targetChar.name + " " + damage);
                        targetChar.TakeDamage(damage, result.point);
                    }
                    else
                    {
                        FXPool.Instance.Spawn(FXType.EnvHit, result.point, ctx.pivot - (Vector3)result.point);
                    }
                }
            }
        }

        private void Melee(CharacterAttackContext ctx)
        {
            var results = Physics2D.OverlapCircleAll(ctx.pivot, ctx.settings.MeleeDistance, ctx.settings.AttackMask);

            if (ctx.isPlayer && results != null && results.Length > 0)
            {
                CameraShake.Instance.Shake(0.5f, 0.3f);
            }

            //var result = Physics2D.Raycast(head.position, target - head.position, operatingSystem.settings.meleeDistance, attackMask);
            foreach (var result in results)
            {
                if (result.transform.TryGetComponent<BreakGlass>(out var glass))
                {
                    glass.Break(ctx.pivot);
                }

                if (result.transform.TryGetComponent<CharController>(out var targetChar))
                {
                    var weapon = ctx.operatingSystem.CurrentWeapon;
                    var damage = Random.Range(weapon.minMeleeDamage, weapon.maxMeleeDamage);
                    targetChar.TakeDamage(damage, result.ClosestPoint(ctx.pivot));
                }
            }
        }

        public float CurrentSpeed(CharacterSpeedContext ctx) => ctx.crouched ? ctx.crouchSpeed : (ctx.walk ? ctx.walkSpeed : ctx.runSpeed);
    }
}
