using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PhysicsController : MonoBehaviour
    {
        [Header("Game Objects")]
        [SerializeField] private Rigidbody2D body = null;
        [Header("Settings")]
        [SerializeField] private PhysicsSettings settings = null;

        [Header("Events")]
        [SerializeField] private UnityEvent fallDeathEvent;

        public bool OnGround { get; private set; }
        public bool OnWall { get; private set; }
        public float Friction { get; private set; }
        public Vector2 ContactNormal { get; private set; }
        public bool WallJumping { get; private set; }        

        private float fallingTime = 0;
        private Vector2 velocity;
        private int jumpPhase;
        private float coyoteCounter, jumpBufferCounter;
        private bool isJumping;
        private float wallDirectionX;

        public void Stop() => body.velocity = Vector2.zero;

        public void FixedMovement(Vector2 desiredVelocity, bool jumpFlag, bool jumpHoldFlag)
        {
            velocity = body.velocity;

            var acceleration = OnGround ? settings.maxAcceleration : settings.maxAirAcceleration;
            var maxSpeedChange = acceleration * Time.fixedDeltaTime;
            velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

            if (OnWall)
            {
                if (velocity.y < -settings.wallSlideMaxSpeed)
                {
                    velocity.y = -settings.wallSlideMaxSpeed;
                }
            }

            if ((OnWall && velocity.x == 0) || OnGround)
            {
                WallJumping = false;
            }

            if (jumpFlag && OnWall && !OnGround)
            {
                if (-wallDirectionX == desiredVelocity.x)
                {
                    velocity = new Vector2(settings.wallJumpClimb.x * wallDirectionX, settings.wallJumpClimb.y);
                    WallJumping = true;
                    jumpFlag = false;
                }
                else if (desiredVelocity.x == 0)
                {
                    velocity = new Vector2(settings.wallJumpBounce.x * wallDirectionX, settings.wallJumpBounce.y);
                    WallJumping = true;
                    jumpFlag = false;
                }
                else
                {
                    velocity = new Vector2(settings.wallJumpLeap.x * wallDirectionX, settings.wallJumpLeap.y);
                    WallJumping = true;
                    jumpFlag = false;
                }
            }

            if (OnGround && body.velocity.y == 0)
            {
                jumpPhase = 0;
                coyoteCounter = settings.coyoteTime;
                fallingTime = 0;
                isJumping = false;
            }
            else
            {
                coyoteCounter -= Time.fixedDeltaTime;
            }


            if (jumpFlag)
            {
                jumpBufferCounter = settings.jumpBufferTime;
            }
            else if (!jumpFlag && jumpBufferCounter > 0)
            {
                jumpBufferCounter -= Time.fixedDeltaTime;
            }

            if (jumpBufferCounter > 0)
            {
                CalcJumpVelocity();
            }

            if (OnWall && settings.wallStick && !jumpHoldFlag)
            {
                velocity = new Vector2(0, -settings.wallSlideMaxSpeed);
                body.velocity = Vector2.zero;
                body.gravityScale = 0;
                fallingTime = 0;
            }
            else if (jumpHoldFlag && body.velocity.y > 0)
            {
                body.gravityScale = settings.upwardMovementMultiplier;
                fallingTime = 0;
            }
            else if (body.velocity.y == 0)
            {
                body.gravityScale = settings.defaultGravityScale;
                fallingTime = 0;
            }
            else if (!OnWall && (!jumpHoldFlag || body.velocity.y < 0))
            {
                body.gravityScale = settings.downwardMovementMultiplier;
                if (fallingTime == 0)
                {
                    fallingTime = Time.time;
                }
            }


            body.velocity = velocity;
        }

        private void CalcJumpVelocity()
        {
            if (coyoteCounter > 0 || (jumpPhase < settings.maxAirJumps && isJumping))
            {
                if (isJumping)
                {
                    jumpPhase += 1;
                }

                jumpBufferCounter = 0;
                coyoteCounter = 0;
                var jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * settings.jumpHeight);
                isJumping = true;

                if (velocity.y > 0f)
                {
                    jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
                }
                else if (velocity.y < 0f)
                {
                    jumpSpeed += Mathf.Abs(body.velocity.y);
                }
                velocity.y += jumpSpeed;
            }
        }


        private void OnCollisionExit2D(Collision2D collision)
        {
            //FIXME: Tilemap collision boxes cause this to flash
            OnGround = false;
            OnWall = false;
            Friction = 0;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            EvaluateCollision(collision);
            RetrieveFriction(collision);
            if (OnWall && !OnGround && WallJumping)
            {
                body.velocity = Vector2.zero;
            }

            if (fallingTime > 0 && Time.time - fallingTime > settings.fallingTimeDeath)
            {
                fallDeathEvent.Invoke();
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            EvaluateCollision(collision);
            RetrieveFriction(collision);
        }

        private void EvaluateCollision(Collision2D collision)
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                ContactNormal = collision.GetContact(i).normal;
                OnGround |= ContactNormal.y >= 0.9f;
                OnWall |= Mathf.Abs(ContactNormal.x) >= 0.9f;
            }
        }

        private void RetrieveFriction(Collision2D collision)
        {
            var material = collision?.rigidbody?.sharedMaterial;

            Friction = 0;

            if (material != null)
            {
                Friction = material.friction;
            }
        }
    }
}