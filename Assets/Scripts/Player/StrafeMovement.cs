using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class StrafeMovement : MonoBehaviour
{
    [SerializeField]
    private float accel = 200f;         // How fast the player accelerates on the ground
    [SerializeField]
    private float airAccel = 200f;      // How fast the player accelerates in the air
    [SerializeField]
    private float maxSpeed = 6.4f;      // Maximum player speed on the ground
    [SerializeField]
    private float maxAirSpeed = 0.6f;   // "Maximum" player speed in the air
    [SerializeField]
    private float friction = 8f;        // How fast the player decelerates on the ground
    [SerializeField]
    private float jumpForce = 5f;       // How high the player jumps

    [SerializeField]
    public GameObject camObj;

    public float lastJumpPress = -1f;
    private float jumpPressDuration = 0.1f;
    private bool onGround = false;

    private Rigidbody rb;
    public Transform orientation;

    //Crouch & Slide
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;
    public float slideForce = 400;
    public float slideCounterMovement = 0.2f;
    public bool crouching;

    private float _h = 0f, _v = 0f;
    private GroundChecker groundChecker;

    private bool isTutorial;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        groundChecker = GetComponent<GroundChecker>();
        isTutorial = LevelManager.levelManager.checkpoints[0].isTutorial;
    }

    public void SetValues(float horizontal, float vertical)
    {
        _h = horizontal;
        _v = vertical;
    }

    private void FixedUpdate()
    {
        if (!isTutorial)
        {
            if(ScoreManager.scoreManager.speedPanel.activeInHierarchy)
            {
                ScoreManager.scoreManager.SetSpeed(new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude);
            }
        }

        if (crouching)
        {
            //Kadangi žaidėjas juda į priekį, slidimo atvėju yra taikoma jėga prieš jo judėjimo kryptį,
            //kad žaidėjas sustotų
            rb.AddForce(0.001f * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
        }
        else
        {
            Vector2 input = new Vector2(_h, _v);
            // Get player velocity
            Vector3 playerVelocity = rb.velocity;
            // Slow down if on ground
            playerVelocity = CalculateFriction(playerVelocity);
            // Add player input
            playerVelocity += CalculateMovement(input, playerVelocity);
            // Assign new velocity to player objeceif
            rb.velocity = playerVelocity;
        }
    }

    public void StartCrouch()
    {
        playerScale = transform.localScale;
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        if (rb.velocity.magnitude > 0.5f)
        {
            if (groundChecker.CheckGround())
            {
                //Kai žaidėjas slysta yra taikoma galia pagal tai kur žaidėjas dabar žiūri
                rb.AddForce(orientation.transform.forward * slideForce);
            }
        }
    }

    public void StopCrouch()
    {
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    /// <summary>
    /// Slows down the player if on ground
    /// </summary>
    /// <param name="currentVelocity">Velocity of the player</param>
    /// <returns>Modified velocity of the player</returns>
	private Vector3 CalculateFriction(Vector3 currentVelocity)
	{
        onGround = groundChecker.CheckGround();
		float speed = currentVelocity.magnitude;

        if (!onGround || Keyboard.current.spaceKey.isPressed || speed == 0f)
            return currentVelocity;

        //Skaičiuojama kaip greitai žaidėjas nusileis ant žemės
        float drop = speed * friction * Time.deltaTime;
        return currentVelocity * (Mathf.Max(speed - drop, 0f) / speed);
    }
    
    /// <summary>
    /// Moves the player according to the input. (THIS IS WHERE THE STRAFING MECHANIC HAPPENS)
    /// </summary>
    /// <param name="input">Horizontal and vertical axis of the user input</param>
    /// <param name="velocity">Current velocity of the player</param>
    /// <returns>Additional velocity of the player</returns>
	private Vector3 CalculateMovement(Vector2 input, Vector3 velocity)
	{
        onGround = groundChecker.CheckGround();

        //Different acceleration values for ground and air
        float curAccel = accel;
        if (!onGround)
            curAccel = airAccel;

        //Ground speed
        float curMaxSpeed = maxSpeed;

        //Air speed
        if (!onGround)
            curMaxSpeed = maxAirSpeed;
        
        //Get rotation input and make it a vector
        Vector3 camRotation = new Vector3(0f, camObj.transform.rotation.eulerAngles.y, 0f);
        Vector3 inputVelocity = Quaternion.Euler(camRotation) *
                                new Vector3(input.x * curAccel, 0f, input.y * curAccel);

        //Ignore vertical component of rotated input
        Vector3 alignedInputVelocity = new Vector3(inputVelocity.x, 0f, inputVelocity.z) * Time.deltaTime;

        //Get current velocity
        Vector3 currentVelocity = new Vector3(velocity.x, 0f, velocity.z);

        //How close the current speed to max velocity is (1 = not moving, 0 = at/over max speed)

        //Skaičiuojama kaip arti dabartinis greitis yra didžiausios galimo greičio (1 - nejuda, 0  - virš didžiausio greičio)
        float max = Mathf.Max(0f, 1 - (currentVelocity.magnitude / curMaxSpeed));

        //How perpendicular the input to the current velocity is (0 = 90°)
        float velocityDot = Vector3.Dot(currentVelocity, alignedInputVelocity);

        //Scale the input to the max speed
        Vector3 modifiedVelocity = alignedInputVelocity * max;

        //The more perpendicular the input is, the more the input velocity will be applied
        Vector3 correctVelocity = Vector3.Lerp(alignedInputVelocity, modifiedVelocity, velocityDot);

        //Apply jump
        correctVelocity += GetJumpVelocity(velocity.y);

        //Return
        return correctVelocity;
    }

    /// <summary>
    /// Calculates the velocity with which the player is accelerated up when jumping
    /// </summary>
    /// <param name="yVelocity">Current "up" velocity of the player (velocity.y)</param>
    /// <returns>Additional jump velocity for the player</returns>
	private Vector3 GetJumpVelocity(float yVelocity)
	{
		Vector3 jumpVelocity = Vector3.zero;

		if(Time.time < lastJumpPress + jumpPressDuration && yVelocity < jumpForce && groundChecker.CheckGround())
		{
			lastJumpPress = -1f;
			jumpVelocity = new Vector3(0f, jumpForce - yVelocity, 0f);
		}
		return jumpVelocity;
	}
}
