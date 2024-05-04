using System.Collections.Generic;
using System.Collections; 
using UnityEngine;
using UnityEngine.VFX;
using Unity.VisualScripting;


#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using Yarn.Unity.Example;
using Yarn.Unity;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 * TODO: Fix/Add Crouch
 * TODO: Add Transition Animations
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    
    
    public class ThirdPersonController : MonoBehaviour
    {
        // Enum for character state machine
        public enum CharacterState
        {
            Fish,
            Leg,
            Wing,
            Rocket
        }
        public CharacterState currentState = CharacterState.Fish;

        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public float Acceleration=1f;
        public float Deceleration=1f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        public float FlyPower=1f;
        [Tooltip("Boost at the beginning of flying")]
        public float FlyBoost=5f;

        [Tooltip("Maximum vertical speed when flying up")]
        public float maxFlyPower=20f; 

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;
        [Tooltip("Maximum allowed time player can press jump before landing and it will still be read.")]
        public float JumpMaxTime=0.2f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;
        [HideInInspector] public bool flipped = false;
        [HideInInspector] public bool isSkidding = false;
        [HideInInspector] public float playerSpeed = 0.0f;
        [HideInInspector] public float playerFallSpeed = 0.0f;



        [Header("State Values")]
        public float LegMoveSpeed=15f;
        public float LegAcceleration=1.5f;
        public float LegDeceleration=1.5f;

        public float FishFlopHeight=0.2f;

        public float WingsJumpHeight=1.2f;


        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;



        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private Vector2 _horizontalVelocity;
        private float _terminalVelocity = 53.0f;
        private bool _jumpLock = false;
        private bool _moveLock = false;
        private bool _crouch = false;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        //true if _input.jump was true in the last frame
        private bool prevJumped=false;
        //true if _input.fly was true in the last frame
        private bool isJumping=false;
        private bool isFlying=false;
        private bool prevFly=false;
        //time since jump input pressed, used to store jump if pressed too early before landing
        private float timeSinceJump=0f;
        
        //tresholds for switching animation when walking
        public float[] walkSpeedTresholds={0f,0.5f,1f};

        private SpriteRenderer sprite;
        private DialogueRunner dialogueRunner;
        public float interactionRadius = 3f;

        public bool inDialogue=false;
        public bool inTransform = false;
        public NPC npcTalkingTo;

        private GameManager gameManager;

        public bool canMoveInDialogue=false;
        public bool limitedFuel=false;

        public float fuelLossRate=0.2f;
        public float fuelReplenishRate=0.2f;
        public float fuel=1f;

        public float maxHeight=-1f;

        private bool respawning=false;
        private Transform respawnLocation;

        private float respawnTimer=0f;
        public float respawnTime=1f;
        private Vector3 preRespawnPosition;

        public SpriteRenderer[] shadows;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }


        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM 
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

            sprite=GetComponentInChildren<SpriteRenderer>();
            dialogueRunner=FindObjectOfType<DialogueRunner>();
            gameManager=FindObjectOfType<GameManager>();
            
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);

            if(!respawning){

                JumpAndGravity();
                GroundedCheck();
                Move();
                
                //Change State when M(orph) is pressed
                if (Input.GetKeyDown(KeyCode.M))
                {
                    ChangeState();
                }

                if (Input.GetKeyUp(KeyCode.E) && !dialogueRunner.IsDialogueRunning && !gameManager.inComplimentGame){
                    CheckForNearbyNPC();
                }

                _moveLock=false;

                // Call state-specific logic
                HandleStateBehaviour();

                Crouch();

                if((dialogueRunner.IsDialogueRunning && !canMoveInDialogue) || gameManager.inComplimentGame){
                    _moveLock=true;
                }
                if (inTransform)
                {
                    _moveLock = true;
                }

                if(_hasAnimator){
                    Animate();
                }

            }else{
                respawnTimer+=Time.deltaTime;
                float k=Mathf.Sin(Mathf.Min(respawnTimer/respawnTime,1f)*Mathf.PI/2);
                transform.position=Vector3.Lerp(preRespawnPosition,respawnLocation.position,k);
                if(Vector3.Distance(transform.position,respawnLocation.position)<0.1f){
                    respawning=false;
                    GetComponent<Collider>().enabled=true;
                    _controller.Move(Vector3.zero);
                }

            }

        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        // Switch Through States
        void ChangeState()
        {
            SetState((CharacterState)(((int)currentState + 1) % 4)); // Cycle through states
            // if(currentState==CharacterState.Fish){
            //     currentState=CharacterState.Rocket;
            // }else{
            //     currentState=CharacterState.Fish;
            // }
        }

        public void SetState(CharacterState targetState)
        {
            currentState = targetState;
            GameObject.FindObjectOfType<InMemoryVariableStorage>().SetValue("$mcState",targetState.ToString());
            gameManager.PlayerChangedState();
        }

        // State Machine
        void HandleStateBehaviour()
        {
            switch (currentState)
            {
                case CharacterState.Fish:
                    HandleFishState();
                    break;
                case CharacterState.Leg:
                    HandleLegState();
                    break;
                case CharacterState.Wing:
                    HandleWingState();
                    break;
                case CharacterState.Rocket:
                    HandleRocketState();
                    break;
            }
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                //_animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        void HandleFishState()
        {
            if (Grounded)
            {
                MoveSpeed=0;
            }
            else
            {
                MoveSpeed = 4;
            }
            _jumpLock = false;
            _crouch = true;
            //_moveLock = true;
            JumpHeight = FishFlopHeight;
            _animator.SetInteger("metamorphosis",1);
        }

        void HandleLegState()
        {
            // if pressed fish button goes back to what fish mode does
            // if not can walk around but no jump
            if (_input.fish && Grounded)
            {
                //_moveLock = true;
                _jumpLock = false;
                _crouch = true;
            }
            else
            {
                _moveLock = false;
                _jumpLock = true;
                if (Grounded)
                {
                    _crouch = false;
                }
            }
            MoveSpeed = LegMoveSpeed;
            Acceleration=LegAcceleration;
            Deceleration=LegDeceleration;
            // Enable walk and run with slippery effect
            _animator.SetInteger("metamorphosis",2);
        }

        void HandleWingState()
        {
            // if pressed fish button goes back to what fish mode does
            // if not can walk around and do normal jump
            if (_input.fish && Grounded)
            {
                _moveLock = true;
                JumpHeight = FishFlopHeight;
                _crouch = true;
            }
            else
            {
                _moveLock = false;
                JumpHeight = WingsJumpHeight;
                if (Grounded)
                {
                    _crouch = false;
                }
            }
            MoveSpeed = LegMoveSpeed;
            Acceleration=LegAcceleration;
            Deceleration=LegDeceleration;
            _jumpLock = false;
            //Debug.Log("Wing");
            // Similar to LegState but allow jumping
            _animator.SetInteger("metamorphosis",3);
        }

        void HandleRocketState()
        {
            // if pressed fish button goes back to what fish mode does
            // if not can walk around and do super jump (placeholder)
            if (_input.fish && Grounded)
            {
                //_moveLock = true;
                JumpHeight = 0.2f;
                _crouch = true;
            }
            else
            {
                //_moveLock = false;
                JumpHeight = WingsJumpHeight; // Placeholder
                if (Grounded)
                {
                    _crouch = false;
                }
            }
            MoveSpeed = LegMoveSpeed;
            Acceleration=LegAcceleration;
            Deceleration=LegDeceleration;
            _jumpLock = false;
            //Debug.Log("Fly");
            // Fly
            _animator.SetInteger("metamorphosis",4);
        }

        // make the movement slippery after sprint is pressed

        private void Move()
        {
            // Disable movement when movelock is on and grounded
            if (_moveLock)
            {
                MoveSpeed = 0; // Disable movement
            }

            // set target speed based on move speed, sprint speed and if sprint is pressed
            //float targetSpeed = (_input.sprint && !_moveLock) ? SprintSpeed : MoveSpeed;
            float targetSpeed=MoveSpeed;

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            if((!_moveLock || (currentState==CharacterState.Fish && !dialogueRunner.IsDialogueRunning)) && _input.move.x<0){
                sprite.flipX=true;
                flipped = true;
                shadows[0].enabled=false;
                shadows[1].enabled=true;
            }
            else if((!_moveLock || (currentState==CharacterState.Fish && !dialogueRunner.IsDialogueRunning)) && _input.move.x>0){
                sprite.flipX=false;
                flipped = false;
                shadows[0].enabled=true;
                shadows[1].enabled=false;
            }

            if(currentState==CharacterState.Fish || _crouch){

                if(Grounded){
                    MoveSpeed=0;
                }

                // a reference to the players current horizontal velocity
                float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

                float speedOffset = 0.1f;
                float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

                // accelerate or decelerate to target speed
                if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                    currentHorizontalSpeed > targetSpeed + speedOffset)
                {
                    // creates curved result rather than a linear one giving a more organic speed change
                    // note T in Lerp is clamped, so we don't need to clamp our speed
                    _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                        Time.deltaTime * SpeedChangeRate);

                    // round speed to 3 decimal places
                    _speed = Mathf.Round(_speed * 1000f) / 1000f;
                }
                else
                {
                    _speed = targetSpeed;
                }


                // normalise input direction
                Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

                // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
                // if there is a move input rotate player when the player is moving
                if (_input.move != Vector2.zero)
                {
                    _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                    _mainCamera.transform.eulerAngles.y;
                    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                        RotationSmoothTime);

                    // rotate to face input direction relative to camera position
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                    //Debug.Log("rotate player");
                }


                Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

                // move the player
                _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                                new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime); 

            }else{
                    // normalise input direction
                Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

                // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
                // if there is a move input rotate player when the player is moving
                if (_input.move != Vector2.zero)
                {
                    _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                    _mainCamera.transform.eulerAngles.y;
                    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                        RotationSmoothTime);

                    // rotate to face input direction relative to camera position
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                }

                Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

                Vector3 targetVelocity=targetDirection*targetSpeed;

                if(_moveLock){
                    targetVelocity=Vector3.zero;
                }

                Vector3 currentVelocity=new Vector3(_controller.velocity.x,0f,_controller.velocity.z);

                Vector3 playerVelocity=currentVelocity;
                //playerVelocity=Vector3.Lerp(currentVelocity,targetVelocity,Time.deltaTime*Acceleration);

                if(currentVelocity.magnitude>0f){
                    playerVelocity=currentVelocity-currentVelocity*Time.deltaTime*Deceleration;
                }

                if(targetVelocity.magnitude>0f){
                    playerVelocity=playerVelocity+targetVelocity*Time.deltaTime*Acceleration;
                }else if(currentVelocity.magnitude>0f){
                    playerVelocity=currentVelocity-currentVelocity*Time.deltaTime*Deceleration;
                    if(playerVelocity.magnitude>0f && (playerVelocity.magnitude<1.5f || _moveLock)){
                        playerVelocity=playerVelocity-currentVelocity*1.5f*Time.deltaTime*Deceleration;
                    }
                    if(playerVelocity.magnitude>0f && _moveLock){
                        playerVelocity=playerVelocity-currentVelocity*1.5f*Time.deltaTime*Deceleration;
                    }
                }

                _controller.Move(playerVelocity * Time.deltaTime +
                                new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime); 

            }

            /*
            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }

            */
        }

        private void Crouch()
        {   
            if (_crouch)
            {
                if(currentState!=CharacterState.Fish) HandleFishState();
                transform.localScale = new Vector3(1.0f, 0.5f, 1.0f);
            }
            else
            {
                if (Grounded)
                {
                    transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                }
            }
        }

        private void JumpAndGravity()
        {
            
            if (Grounded)
            {
                isJumping=false;
                
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    //_animator.SetBool(_animIDJump, false);
                    //_animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (!_moveLock && _input.jump && (prevJumped==false || timeSinceJump<JumpMaxTime) && !_jumpLock && _jumpTimeoutDelta <= 0.0f )
                {
                    Debug.Log("jumped");
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        //_animator.SetBool(_animIDJump, true);
                    }
                    isJumping=true;
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        //_animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                //_input.jump = false;
            }

            //Handling rocketting/flying 
            isFlying=false;
            if(!_moveLock && currentState==CharacterState.Rocket && _input.fly){
                if((!limitedFuel || fuel>0) && (transform.position.y<maxHeight || maxHeight==-1)){
                    if(Grounded && _verticalVelocity<0){
                        _verticalVelocity=0f;
                    }
                    _verticalVelocity+=FlyPower*Time.deltaTime;
                    if(!prevFly){
                        _verticalVelocity+=FlyBoost;
                    }
                    _verticalVelocity=Mathf.Min(_verticalVelocity,maxFlyPower);
                    isFlying=true;
                    if(limitedFuel){
                        fuel-=fuelLossRate*Time.deltaTime;
                    }
                }
            }else if(!_input.fly){
                fuel+=fuelReplenishRate*Time.deltaTime;
            }

            fuel=Mathf.Clamp(fuel,0f,1f);

            //gravity modifier
            float gModifier=1f;

            if(_input.jump && (currentState==CharacterState.Fish || _crouch)){
                gModifier=0.5f;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * gModifier * Time.deltaTime;
            }

            if(_input.jump && !prevJumped){
                timeSinceJump=0f;
            }else{
                timeSinceJump+=Time.deltaTime;
            }

            prevJumped=_input.jump;
            prevFly=_input.fly;
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }

        void Animate(){
            if(_animator.GetBool("landing") && (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime>=1 || !Grounded)){
                _animator.SetBool("landing",false);
                Debug.Log("landing false");
            }
            if(Grounded && !_animator.GetBool("grounded")){
                _animator.SetBool("landing",true);
            }
            
            _animator.SetBool("grounded",Grounded);
            bool walking=!_moveLock && _input.move!=Vector2.zero && Grounded && !(currentState==CharacterState.Fish);
            _animator.SetBool("walking",walking);
            float velocity=new Vector3(_controller.velocity.x,0f,_controller.velocity.z).magnitude;
            int i=0;
            _animator.SetBool("skidding",false);
            isSkidding = false;
            playerSpeed = velocity;
            playerFallSpeed = _verticalVelocity;
            if(walking){
                //Debug.Log(velocity);
                foreach(float s in walkSpeedTresholds){
                    if(velocity>=s){
                        i++;
                    }else{
                        break;
                    }
                }
            }else if(Grounded && Mathf.Abs(velocity)>0.3f){
                isSkidding = true;
                _animator.SetBool("skidding",true);
            }
            _animator.SetInteger("walkSpeed",i);
            if(isJumping && !_animator.GetBool("jumping")){
                _animator.SetTrigger("jump");
            }
            _animator.SetBool("jumping",isJumping);
            if(isFlying && !_animator.GetBool("flying")){
                _animator.SetTrigger("fly");
            }
            _animator.SetBool("flying",isFlying);

            if(_verticalVelocity<0f){
                _animator.SetBool("falling",true);
            }else{
                _animator.SetBool("falling",false);
            }

            if(Grounded){
                float a;
                a=shadows[0].color.a;
                shadows[0].color=new Color(1f,1f,1f,Mathf.Lerp(a,0.72f,0.99f));
                a=shadows[1].color.a;
                shadows[1].color=new Color(1f,1f,1f,Mathf.Lerp(a,0.72f,0.99f));
            }
            else{
                float a;
                a=shadows[0].color.a;
                shadows[0].color=new Color(1f,1f,1f,Mathf.Lerp(a,0f,0.99f));
                a=shadows[1].color.a;
                shadows[1].color=new Color(1f,1f,1f,Mathf.Lerp(a,0f,0.99f));
            }
        }

        public void CheckForNearbyNPC()
        {
            List<NPC> allParticipants = new List<NPC>(FindObjectsOfType<NPC>());
            var target = allParticipants.Find(delegate (NPC p)
            {
                return string.IsNullOrEmpty(p.talkToNode) == false && // has a conversation node?
                (p.transform.position - this.transform.position)// is in range?
                .magnitude <= interactionRadius;
            });
            if (target != null)
            {
                // Kick off the dialogue at this node.
                FindObjectOfType<DialogueRunner>().StartDialogue(target.talkToNode);
                npcTalkingTo=target;
                // reenabling the input on the dialogue
                //dialogueInput.enabled = true;
            }
        }

        void OnControllerColliderHit(ControllerColliderHit hit){
            Rigidbody rb=hit.collider.attachedRigidbody;
            if(rb!=null && !rb.isKinematic){
                //rb.velocity=_controller.velocity;
                rb.AddForce(hit.moveDirection * _controller.velocity.magnitude, ForceMode.Impulse);
            }
            if(hit.gameObject.tag=="Package"){
                hit.gameObject.GetComponent<Package>().PlayerCollide();
            }else if(hit.gameObject.tag=="Missile"){
                hit.gameObject.GetComponent<Missile>().CollidePlayer();
            }
            RespawnCollider rc=hit.gameObject.GetComponent<RespawnCollider>();
            if(rc!=null){
                gameManager.Respawn();
            }
        }

        public bool IsCrouching(){
            return _crouch;
        }

        public bool IsFlying(){
            return isFlying;
        }

        public void Metamorphosis(CharacterState cs, float time1=2f, float time2=0.5f){
            StartCoroutine(PlayerTransformCoroutine(cs,time1,time2));
        }

        private IEnumerator PlayerTransformCoroutine(CharacterState cs, float time1=2f, float time2=0.5f){
            inTransform=true;
            // wait
            yield return new WaitForSeconds(time1);
            // play smoke
            FindObjectOfType<PlayerVFX>().PlayMagicSmoke();
            // wait until the smoke happen
            yield return new WaitForSeconds(time2);
            // change state
            SetState(cs);
            // allow movement
            inTransform = false;
        }

        [YarnCommand]
        public void Metamorphosis(string cState){
            CharacterState cs=CharacterState.Rocket;
            switch(cState.ToLower()){
                case "fish":
                    cs=CharacterState.Fish;
                    break;
                case "leg":
                    cs=CharacterState.Leg;
                    break;
                case "wing":
                    cs=CharacterState.Wing;
                    break;
                case "rocket":
                    cs=CharacterState.Rocket;
                    break;
            }
            StartCoroutine(PlayerTransformCoroutine(cs,0f,0.5f));
        }

        public void Respawn(Transform t){
            if(currentState!=CharacterState.Rocket){
                respawnLocation=t;
                respawning=true;
                GetComponent<Collider>().enabled=false;
                respawnTimer=0f;
                preRespawnPosition=transform.position;
            }
        }
    }
}