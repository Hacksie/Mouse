using System.Collections;
using System.Linq;
using System.Net.NetworkInformation;
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

        [SerializeField] private bool onGround;
        [SerializeField] private bool onWall;
        [SerializeField] private Vector2 contactNormal;

        [Header("Settings")]
        [SerializeField] private Transform ledgeDetectionPoint;
        [SerializeField] public Vector3 ledgeOffsetEnd;

        public bool OnGround { get => onGround; private set => onGround = value; }
        public bool OnWall { get => onWall; private set => onWall = value; }
        public float Friction { get; private set; }
        public Vector2 ContactNormal { get => contactNormal; private set => contactNormal = value; }
        public bool WallJumping { get; private set; }
        public float VelocityY { get => body.linearVelocityY; }
        public float LastFallTime { get ; private set; }

        private float fallingTime = 0;
        private float fallingStartY = 0;
        private int jumpPhase;
        private float coyoteCounter, jumpBufferCounter;
        private bool isJumping;
        private float wallDirectionX;
        private float movementSpeed = 0;

        public bool currentlyClimbingLedge = false;

        public bool currentlyKnockback = false;

        private bool canGrabLedge = true;

        private float jumpAnticipationTime = 0.10f;
        private float jumpAnticipationTimer = 0f;
        private bool queuedJump = false;

        private readonly RaycastHit2D[] hitBuffer = new RaycastHit2D[10];
        private PhysicsScene2D physicsScene = Physics2D.defaultPhysicsScene;

        void Awake()
        {
            this.AutoBind(ref body);
            this.AutoBind(ref bodyCollider);
            Physics2D.queriesStartInColliders = true;
        }


        #region Freeze
        public void Stop() => body.linearVelocity = Vector2.zero;

        public void Freeze()
        {
            Stop();
            body.gravityScale = 0;
        }

        public void Unfreeze() => body.gravityScale = DefaultGravityScale();
        #endregion

        #region Knockback
        public void Knockback(Vector3 direction, float amount)
        {
            Stop();
            body.AddForce(direction.normalized * amount, ForceMode2D.Impulse);
        }
        #endregion Knockback

        #region LedgeGrab

        private bool LedgeEdgeStart()
        {
            if (currentlyClimbingLedge) return false;

            //https://www.youtube.com/watch?v=Kh5n63A-YBw
            return DetectEnvForLedgeGrab() && DetectClearAirForLedgeGrab();
        }

        private bool DetectEnvForLedgeGrab() => ledgeDetect != null && ledgeDetect.IsTouchingLayers(environmentMask);

        private bool DetectClearAirForLedgeGrab() => airDetect != null && airDetect.IsTouchingLayers(environmentMask) == false;

        void LedgeEdgeEnd()
        {
            body.transform.position = transform.position + new Vector3(ledgeOffsetEnd.x * Mathf.Sign(transform.right.x), ledgeOffsetEnd.y, 0);
            body.gravityScale = DefaultGravityScale();
            currentlyClimbingLedge = false;
            Invoke(nameof(ClearCanGrabLedge), 0.1f); // FIXME:
        }

        private void ClearCanGrabLedge()
        {
            canGrabLedge = true;
        }

        #endregion LedgeGrab

        #region Movement
        public void FixedMovement(float desiredVelocity, float climbVelocity, bool jumpFlag, bool jumpHoldFlag)
        {
            UpdateContacts(desiredVelocity);
            UpdateFalling();

            // If we're going through a currentlyKnockback, do nothing else
            // If we're climbing a ledge, do nothing else
            if (currentlyKnockback || currentlyClimbingLedge)
            {
                return;
            }

            // Check if we can start grabbing a ledge
            // This must be before the next check
            if (canGrabLedge && LedgeEdgeStart())
            {
                canGrabLedge = false;
                currentlyClimbingLedge = true;
                // FIXME: I think this is causing popping inside the collider
                //body.transform.position = ledgeDetectionPoint.position + new Vector3(ledgeOffsetStart.x * Mathf.Sign(transform.right.x), ledgeOffsetStart.y, 0);
                body.linearVelocity = Vector3.zero;
                body.gravityScale = 0;
                return;
            }

            desiredVelocity = ApplyFriction(desiredVelocity);

            var velocity = body.linearVelocity;

            if (velocity.sqrMagnitude < 1e-2f)
            {
                velocity = Vector2.zero;
            }

            if (OnWall && !OnGround)
            {
                velocity.y = climbVelocity * settings.wallClimbSpeed;
            }

            if ((OnWall && velocity.x == 0) || OnGround)
            {
                WallJumping = false;
            }

            if (jumpFlag && OnWall && !OnGround)
            {

                if (-wallDirectionX == desiredVelocity)
                {
                    velocity = new Vector2(settings.wallJumpClimb.x * wallDirectionX, settings.wallJumpClimb.y);
                    WallJumping = true;
                    jumpFlag = false;
                }
                else if (desiredVelocity == 0)
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

            if (OnGround)
            {
                // Reset jump counters while we're on the ground
                // FIXME: Don't do this every frame
                if (!queuedJump)
                {
                    jumpPhase = 0;
                    coyoteCounter = settings.coyoteTime;
                    isJumping = false;
                }
            }
            else
            {
                coyoteCounter -= Time.fixedDeltaTime;
            }

            if (jumpFlag)
            {
                // The jump buffer prevents the player from jumping too soon after jumping once
                // However, if they let go of the button, then mash it again before it hits 0, we don't want to reset the buffer to the full amount
                jumpBufferCounter = jumpBufferCounter > 0 ? jumpBufferCounter : settings.jumpBufferTime;
            }
            else if (!jumpFlag && jumpBufferCounter > 0)
            {
                jumpBufferCounter -= Time.fixedDeltaTime;
            }

            if (jumpBufferCounter > 0)
            {
                velocity = CalcJumpVelocity(velocity);
            }

            if (OnWall && !jumpHoldFlag && !DetectClearAirForLedgeGrab())
            {
                body.gravityScale = 0;
            }
            else if (jumpHoldFlag && body.linearVelocity.y > 0)
            {
                body.gravityScale = settings.risingGravityScale;
            }
            else if (body.linearVelocity.y == 0)
            {
                body.gravityScale = DefaultGravityScale();
            }
            else if (OnGround && body.linearVelocity.y < 0)
            {
                body.gravityScale = DefaultGravityScale();
            }
            else if (!OnWall && (!jumpHoldFlag || body.linearVelocity.y < Mathf.Epsilon))
            {
                body.gravityScale = settings.fallingGravityScale;
            }

            var acceleration = OnGround ? settings.maxAcceleration : settings.maxAirAcceleration;

            movementSpeed = Mathf.MoveTowards(movementSpeed, desiredVelocity, acceleration * Time.fixedDeltaTime);

            velocity.x = OnGround && !OnWall && !jumpFlag ? CalcContactPerp().x * movementSpeed : movementSpeed;

            body.linearVelocity = velocity;
        }

        private float ApplyFriction(float desiredVelocity)
        {
            return desiredVelocity * Mathf.Clamp01(1.0f - Friction);
        }

        private Vector2 CalcContactPerp()
        {
            return -1 * Vector2.Perpendicular(ContactNormal).normalized;
        }

        private float DefaultGravityScale() => settings.fly ? 0 : settings.defaultGravityScale;

        private void UpdateContacts(float desiredVelocity)
        {
            Friction = 0;

            Vector2 hitDirection = new Vector2(desiredVelocity, 0).normalized;
            if (hitDirection == Vector2.zero)
            {
                hitDirection = Vector2.down;
            }

            var hits = physicsScene.CapsuleCast(
                origin: (Vector2)transform.position + new Vector2(0, 1),
                size: new Vector2(0.76f, 2.1f),
                capsuleDirection: CapsuleDirection2D.Vertical,
                angle: 0f,
                direction: hitDirection,
                contactFilter: new ContactFilter2D
                {
                    useLayerMask = true,
                    layerMask = environmentMask,
                    useTriggers = false
                },
                results: this.hitBuffer,
                distance: 0.1f
             );

            Vector2 summedNormal = Vector2.zero;
            int validHitCount = 0;


            for (int i = 0; i < hits; i++)
            {
                var hit = hitBuffer[i];
                if (hit.collider != null)
                {
                    summedNormal += hit.normal;
                    validHitCount++;

                    if (hit.rigidbody != null && hit.rigidbody.sharedMaterial != null)
                    {
                        Friction = Mathf.Max(Friction, hit.rigidbody.sharedMaterial.friction);
                    }
                }
            }

            ContactNormal = validHitCount > 0 ? (summedNormal / validHitCount).normalized : Vector2.zero;
            OnGround = ContactNormal.y >= 0.5f;
            OnWall = Mathf.Abs(ContactNormal.x) >= 0.75f;
        }
        #endregion Movement

        #region Falling
        private void UpdateFalling()
        {
            if (!OnGround && !OnWall) // Then we're falling
            {
                if (fallingTime == 0)
                {
                    fallingTime = Time.time;
                    fallingStartY = transform.position.y;
                }
            }
            else if (fallingTime > 0)
            {
                LastFallTime = Time.time - fallingTime;
                fallingTime = 0;

                //Debug.Log($"Fall {fallingStartY} {transform.position.y} {fallingStartY - transform.position.y}");

                if (fallingStartY - transform.position.y >= Game.Instance.GameSettings.FallDeathHeight)
                {
                    Debug.Log("SPLAT");
                    Stop();
                    fallDeathEvent.Invoke();
                }
            }
        }
        #endregion Falling

        #region Jump
        private Vector2 CalcJumpVelocity(Vector2 currentVelocity)
        {
            if ((coyoteCounter > 0 || (jumpPhase < settings.maxAirJumps && isJumping)) && !queuedJump)
            {
                jumpAnticipationTimer = jumpAnticipationTime;
                queuedJump = true;
                return currentVelocity; // wait a few frames before jumping
            }

            if (!queuedJump)
            {
                return currentVelocity;
            }

            if (jumpAnticipationTimer > 0f)
            {
                jumpAnticipationTimer -= Time.fixedDeltaTime;
                if (jumpAnticipationTimer > 0f) return currentVelocity; // still anticipating
            }

            queuedJump = false;
            if (isJumping)
            {
                jumpPhase += 1;
            }

            jumpBufferCounter = 0;
            coyoteCounter = 0;
            var jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * settings.jumpHeight);
            isJumping = true;

            if (currentVelocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - currentVelocity.y, 0f);
            }
            else if (currentVelocity.y < 0f)
            {
                jumpSpeed += Mathf.Abs(body.linearVelocity.y);
            }
            currentVelocity.y += jumpSpeed;
            jumpAnticipationTimer = 0f;

            return currentVelocity;

        }

        #endregion Jump
    }
}