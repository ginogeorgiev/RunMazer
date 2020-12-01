using UnityEngine;
using Survival;

public class PlayerController : MonoBehaviour {
	
	[SerializeField] private PlayerStateMachine.State playerState;

	private Vector2 input;

	[SerializeField] private float walkSpeed = 2;
	[SerializeField] private float runSpeed = 6;
	[SerializeField] private float gravity = -12;
	
	[SerializeField] private float turnSmoothTime = 0.2f;
	float turnSmoothVelocity;

	[SerializeField] private float speedSmoothTime = 0.1f;
	float speedSmoothVelocity;
	float currentSpeed;
	private float velocityY;

	Animator animator;
	private CharacterController controller;
	private static bool isInBase;
	private static readonly int SpeedPercent = Animator.StringToHash("SpeedPercent");

	public static bool IsInBase => isInBase;

	void Start () {
		animator = GetComponent<Animator> ();
		controller = GetComponent<CharacterController>();
	}

	void Update () {

		// Game is not playing, nothing to do
		if (GameStateMachine.GetInstance().GetState() != GameStateMachine.State.Playing) return;
		
		DetermineState();

		Move();

	}

	private void Move()
	{
		input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		Vector2 inputDir = input.normalized;

		if (inputDir != Vector2.zero) {
			float targetRotation = Mathf.Atan2 (inputDir.x, inputDir.y) * Mathf.Rad2Deg;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
		}

		bool running = Input.GetKey (KeyCode.LeftShift) && CoreBars.PlayerCanRun();
		float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
		currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

		velocityY += Time.deltaTime * gravity;
		Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

		controller.Move(velocity * Time.deltaTime);

		if (controller.isGrounded)
		{
			velocityY = 0;
		}

		Animate(running, inputDir);
	}

	private void Animate(bool running, Vector2 inputDir)
	{
		float animationSpeedPercent = ((running) ? 1 : .5f) * inputDir.magnitude;
		animator.SetFloat (SpeedPercent, animationSpeedPercent, speedSmoothTime, Time.deltaTime);
	}
	
	private void DetermineState()
	{
		//player isn't moving
		if (input == Vector2.zero)
		{
			PlayerStateMachine.GetInstance().ChangeState(PlayerStateMachine.State.IsIdle);
			return;
		}

		//player is moving but shift isn't held down
		//or shift is held down but stamina is depleted
		if (!Input.GetKey(KeyCode.LeftShift) || (Input.GetKey(KeyCode.LeftShift) && !CoreBars.PlayerCanRun()))
		{
			PlayerStateMachine.GetInstance().ChangeState(PlayerStateMachine.State.IsWalking);
			return;
		}

		//player is moving, shift is held down and stamina isn't depleted
		PlayerStateMachine.GetInstance().ChangeState(PlayerStateMachine.State.IsRunning);
        
	}
	
	private void PlayerWonGame()
	{
		GameStateMachine.GetInstance().SetState(GameStateMachine.State.GameWon);
	}
	
	private void OnTriggerExit(Collider other)
	{
		switch (other.tag)
		{
			case "Base":
				isInBase = false;
				break;
		}
	}
	
	private void OnTriggerEnter(Collider other)
	{
		switch (other.tag)
		{
			case  "Base":
				isInBase = true;
				break;
			case "Exit":
				if (ScoreManager.Instance.GetFragmentScore() == ScoreManager.Instance.GetMaxFragments())
				{
					this.PlayerWonGame();
					//destroy is temporary
					Destroy(gameObject);
					Debug.Log("you won");
				}
				break;
		}
	}
}