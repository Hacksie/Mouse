using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace HackedDesign
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PhysicsController : MonoBehaviour
    {
        [Header("Game Objects")]
        [SerializeField] private Rigidbody2D body = null;
        [SerializeField] private Collider2D ledgeDetect = null;
        [SerializeField] private Collider2D airDetect = null;
        [SerializeField] private CapsuleCollider2D bodyCollider = null;
        [Header("Settings")]
        [SerializeField] private PhysicsSettings settings = null;
        [SerializeField] private LayerMask environmentMask;

        [Header("Events")]
        [SerializeField] private UnityEvent fallDeathEvent;
        
        //private bool headBlocked;
        private bool onGround;
        private bool onWall;
        private Vector2 contactNormal;

        [Header("Settings")]
        [SerializeField] private Transform ledgeDetectionPoint;
        //[SerializeField] public Vector3 ledgeOffsetStart;
        [SerializeField] public Vector3 ledgeOffsetEnd;

        public Rigidbody2D Body { get { return body; } }
        public bool OnGround { get => onGround; private set => onGround = value; }
        public bool OnWall { get => onWall; private set => onWall = value; }
        public float Friction { get; private set; }
        public Vector2 ContactNormal { get => contactNormal; private set => contactNormal = value; }
        public bool WallJumping { get; private set; }

        public float FallTime { get => lastFallTime; }

        private float lastFallTime = 0;
        private float fallingTime = 0;
        private Vector2 velocity;
        private int jumpPhase;
        private float coyoteCounter, jumpBufferCounter;
        private bool isJumping;
        private float wallDirectionX;
        private float movementSpeed = 0;

        public bool climbingLedge = false;

        public bool knockback = false;

        
        //public Vector3 ledgePosition;

        private bool canGrabLedge = true;

        private void Awake()
        {
            this.AutoBind(ref body);
            this.AutoBind(ref bodyCollider);
        }


        public void Stop() => body.linearVelocity = Vector2.zero;

        public void Freeze()
        {
            Stop();
            body.gravityScale = 0;
        }

        public void Unfreeze() => body.gravityScale = DefaultGravityScale();

        private bool DetectEnvForLedgeGrab() => ledgeDetect.IsTouchingLayers(environmentMask);

        private bool DetectClearAirForLedgeGrab() => airDetect.IsTouchingLayers(environmentMask) == false;

        private float DefaultGravityScale() => settings.fly ? 0 : settings.defaultGravityScale;

        public void Knockback(Vector3 direction, float amount)
        {
            //knockback = true;
            Stop();
            body.AddForce(direction.normalized * amount, ForceMode2D.Impulse);
            
            //StartCoroutine(KnockbackPause());
        }

        private IEnumerator KnockbackPause()
        {
            yield return new WaitForSeconds(Game.Instance.GameSettings.KnockbackTime);
            
            StartCoroutine(KnockbackOver());
        }

        private IEnumerator KnockbackOver()
        {
            yield return new WaitForSeconds(Game.Instance.GameSettings.KnockbackFreezeTime);

            knockback = false;
        }


        public bool LedgeEdgeStart()
        {
            if (climbingLedge) return false;

            //https://www.youtube.com/watch?v=Kh5n63A-YBw
            return DetectEnvForLedgeGrab() && DetectClearAirForLedgeGrab();
        }

        public void LedgeEdgeEnd()
        {
            body.transform.position = transform.position + new Vector3(ledgeOffsetEnd.x * Mathf.Sign(transform.right.x), ledgeOffsetEnd.y, 0);
            body.gravityScale = DefaultGravityScale();
            climbingLedge = false;
            Invoke(nameof(ClearCanGrabLedge), 0.1f); // FIXME:
            
        }

        private void ClearCanGrabLedge()
        {
            canGrabLedge = true;
        }

        

        public void FixedMovement(Vector2 desiredVelocity, float climb, bool jumpFlag, bool jumpHoldFlag)
        {
            velocity = body.linearVelocity;

            var acceleration = OnGround ? settings.maxAcceleration : settings.maxAirAcceleration;
            var maxSpeedChange = acceleration * Time.fixedDeltaTime;

            var contactPerp = -1 * Vector2.Perpendicular(ContactNormal).normalized;

            if(knockback)
            {
                return;
            }

            if (body.transform.position.y < settings.fallingDeathYLimit)
            {
                Debug.Log("Falling death");
                body.linearVelocity = Vector3.zero;
                body.gravityScale = 0;
                fallDeathEvent.Invoke();
                return;
            }



            if (canGrabLedge && LedgeEdgeStart())
            {
                canGrabLedge = false;
                Debug.Log("Ledge Edge Start");
                climbingLedge = true;
                // FIXME: I think this is causing popping inside the collider
                //body.transform.position = ledgeDetectionPoint.position + new Vector3(ledgeOffsetStart.x * Mathf.Sign(transform.right.x), ledgeOffsetStart.y, 0);
                body.linearVelocity = Vector3.zero;
                body.gravityScale = 0;
            }

            if (climbingLedge)
            {

                //Debug.Log("Climbing");
                //Debug.Break();
                //climbingLedge = false;
                return;
            }

            if (OnWall && !OnGround && climb != 0)
            {
                velocity.y = climb * settings.wallClimbSpeed;
                //WallJumping = true;
                //if (velocity.y < -setting.wallSlideMaxSpeed)
                //{
                //    velocity.y = -setting.wallSlideMaxSpeed;
                //}
            }

            //if ((OnWall && velocity.x == 0) || OnGround)
            //{
            //    WallJumping = false;
            //}

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

            if (OnGround) // && body.linearVelocity.y == 0)
            {
                jumpPhase = 0;
                coyoteCounter = settings.coyoteTime;
                if (fallingTime > 0)
                {
                    lastFallTime = Time.time - fallingTime;
                }

                fallingTime = 0;

                isJumping = false;
            }
            else
            {

                coyoteCounter -= Time.fixedDeltaTime;
            }

            if (fallingTime > 0)
            {
                lastFallTime = Time.time - fallingTime;
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

            if (OnWall && settings.wallStick && !jumpHoldFlag && !DetectClearAirForLedgeGrab())
            {
                // FIXME: Wallstick is always false
                velocity = new Vector2(0, -settings.wallSlideMaxSpeed);
                //velocity = new Vector2(0, climb);
                body.gravityScale = 0;
                fallingTime = 0;
            }
            else if (jumpHoldFlag && body.linearVelocity.y > 0)
            {
                body.gravityScale = settings.risingGravityScale;
                fallingTime = 0;
            }
            else if (body.linearVelocity.y == 0)
            {
                body.gravityScale = DefaultGravityScale();
                fallingTime = 0;
            }
            else if (OnGround && body.linearVelocity.y < 0)
            {
                body.gravityScale = DefaultGravityScale();

            }
            else if (!OnWall && (!jumpHoldFlag || body.linearVelocity.y < 0))
            {
                body.gravityScale = settings.fallingGravityScale;
                if (fallingTime == 0)
                {
                    fallingTime = Time.time;
                }

            }

            movementSpeed = Mathf.MoveTowards(movementSpeed, desiredVelocity.x, maxSpeedChange);

            if (OnGround && !OnWall && !jumpFlag)
            {
                velocity.x = contactPerp.x * movementSpeed;
            }
            else
            {
                velocity.x = movementSpeed;
            }


            body.linearVelocity = velocity;
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
                    jumpSpeed += Mathf.Abs(body.linearVelocity.y);
                }
                velocity.y += jumpSpeed;
            }
        }


        private void OnCollisionExit2D(Collision2D collision)
        {
            OnGround = false;
            OnWall = false;
            Friction = 0;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            EvaluateCollision(collision);
            UpdateCurrentFriction(collision);
            if (OnWall && !OnGround && WallJumping)
            {
                body.linearVelocity = Vector2.zero;
            }

            //if (fallingTime > 0 && Timer.time - fallingTime > setting.fallingTimeDeath)
            //{
            //    Debug.Log("Falling death");
            //    body.linearVelocity = Vector3.zero;
            //    body.gravityScale = 0;
            //    fallDeathEvent.Invoke();
            //}
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            EvaluateCollision(collision);
            UpdateCurrentFriction(collision);
        }

        private void EvaluateCollision(Collision2D collision)
        {
            // FIXME: Use a layermask
            if (environmentMask.Contains(collision.gameObject.layer))
            {
                for (int i = 0; i < collision.contactCount; i++)
                {
                    ContactNormal = collision.GetContact(i).normal;
                    if (!settings.fly)
                    {
                        OnGround |= ContactNormal.y >= 0.5f;
                        OnWall |= Mathf.Abs(ContactNormal.x) >= 0.9f;
                    }

                }
            }
        }


        private void UpdateCurrentFriction(Collision2D collision)
        {
            Friction = 0;

            if (collision != null && collision.rigidbody != null && collision.rigidbody.sharedMaterial != null)
            {
                Friction = collision.rigidbody.sharedMaterial.friction;
            }
        }
    }
}