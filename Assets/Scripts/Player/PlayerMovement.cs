using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
	public float speed = 6f;

	private Vector3 movement;
	private Animator anim;
	private Rigidbody playerRigidbody;
	private int floorMask;
	private float camRayLength = 100f;
	float h;
	float v;
	Quaternion lookAt;

	[SerializeField] Transform cam;

	void Awake()
	{
		floorMask = LayerMask.GetMask("Floor");
		anim = GetComponent<Animator>();
		playerRigidbody = GetComponent<Rigidbody>();
	}


	private void Update()
    {
		if (!IsLocalPlayer)
		{
			return;
		}
		
		ValidateInputServerRpc(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), GetTurnRotation());
		Animating(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
	}



    void FixedUpdate()
	{
		if (IsServer)
		{
			Move(h, v);
			Turning(lookAt.normalized);
			Animating(h, v);
		}
	}

	[ServerRpc]
	void ValidateInputServerRpc(float inputH, float inputV, Quaternion q)
    {
		h = inputH;
		v = inputV;
		lookAt = q;
	}

	void Move(float h, float v)
	{
		movement.Set(h, 0f, v);
		movement = movement.normalized * speed * Time.deltaTime;

		playerRigidbody.MovePosition(transform.position + movement);
	}

	Quaternion GetTurnRotation()
	{
		Camera tempCam = Camera.main;
		if (cam != null)
			tempCam = cam.GetComponent<Camera>();

		Ray camRay = tempCam.ScreenPointToRay(Input.mousePosition);
		RaycastHit floorHit;

		if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
		{
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0f;

			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
			return newRotation;
		}

		return Quaternion.identity;
	}

	void Turning(Quaternion q)
    {
		playerRigidbody.MoveRotation(q);
	}

	void Animating(float h, float v)
	{
		bool walking = h != 0f || v != 0f;
		anim.SetBool("IsWalking", walking);
	}
}
