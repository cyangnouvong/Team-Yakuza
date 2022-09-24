using UnityEngine;
using System.Collections;

// Include the namespace required to use Unity UI and Input System
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{

	// Create public variables for player speed, and for the Text UI game objects
	public float speed;
	public GameObject winTextObject;

	private float movementX;
	private float movementY;

	private bool groundContact = true;

	private Rigidbody rb;
	private int count;

	// At the start of the game..
	void Start()
	{
		// Assign the Rigidbody component to our private rb variable
		rb = GetComponent<Rigidbody>();

		// Set the count to zero 
		count = 0;

		// Set the text property of the Win Text UI to an empty string, making the 'You Win' (game over message) blank
		winTextObject.SetActive(false);
	}

	void Update()
    {
		if (Input.GetKeyUp("space") && groundContact)
		{
			Debug.Log("space pressed");
			rb.AddForce(new Vector3(0f, 5f, 0f), ForceMode.Impulse);
		}
	}

	void FixedUpdate()
	{
		// Create a Vector3 variable, and assign X and Z to feature the horizontal and vertical float variables above
		Vector3 movement = new Vector3(movementX, 0.0f, movementY);

		rb.AddForce(movement * speed);
	}

	void OnTriggerEnter(Collider other)
	{
		// ..and if the GameObject you intersect has the tag 'Pick Up' assigned to it..
		if (other.gameObject.CompareTag("PickUp"))
		{
			other.gameObject.SetActive(false);

			// Add one to the score variable 'count'
			count = count + 1;
		}

		if (count == 3)
        {
			winTextObject.SetActive(true);
        }
	}

	void OnMove(InputValue value)
	{
		Vector2 v = value.Get<Vector2>();

		movementX = v.x;
		movementY = v.y;
	}

	void OnCollisionEnter(Collision collision)
	{

		if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Box")
		{
			groundContact = true;
		}

	}

	void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Box")
		{
			groundContact = false;

		}
	}
}
