using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed;
	private Vector3 movement;
	private Animator anim;
	private Rigidbody playerRigidbody;
	private int floorMask;
	private float camRayLength = 100f;

	void Awake()
	{
		speed = 6f;
		floorMask = LayerMask.GetMask("Floor");
		anim = GetComponent<Animator>();
		playerRigidbody = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");
        
		Move(h, v);
		Turning();
		Animating(h, v);
	}

	void Move(float h, float v)
	{
		if (h != 0 || v != 0)
		{
			anim.SetBool("Running", true);
		}
		else
		{
			anim.SetBool("Running", false);
		}
		
		movement.Set (h, 0f, v);
		movement = movement.normalized * speed * Time.deltaTime;
        
        
		playerRigidbody.MovePosition (transform.position + movement);
		
		
	}

	void Turning()
	{
		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit floorHit;

		if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
		{
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0f;

			Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
			playerRigidbody.MoveRotation(newRotation);
		}
	}

	void Animating(float h, float v)
	{
		bool walking = h != 0f || v != 0f;
		anim.SetBool("Running", walking);
	}
}
