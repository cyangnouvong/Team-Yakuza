using UnityEngine;
using System.Collections;

// Include the namespace required to use Unity UI and Input System
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{

	// Create public variables for player speed, and for the Text UI game objects
	public float speed;

	private float movementX;
	private float movementY;

	private bool groundContact = true;

	private Rigidbody rb;

	public TextMeshProUGUI NPCText;
    public float timeToAppear = 3f;
    private float timeWhenDisappear;
    public GameObject buttonPressStandingSpot;

    // At the start of the game..
    void Start()
	{
		// Assign the Rigidbody component to our private rb variable
		rb = GetComponent<Rigidbody>();
	}

	void Update()
    {
        if (NPCText.enabled && (Time.time >= timeWhenDisappear))
        {
            NPCText.enabled = false;
        }
        if (Input.GetKeyUp("space") && groundContact)
		{
			Debug.Log("space pressed");
			rb.AddForce(new Vector3(0f, 5f, 0f), ForceMode.Impulse);
		}
        // NPC interaction code
        bool doButtonPress = false;

        float buttonDistance = float.MaxValue;

        if (buttonPressStandingSpot != null)
        {
            buttonDistance = Vector3.Distance(transform.position, buttonPressStandingSpot.transform.position);
        }

        if (Input.GetKeyUp("left ctrl"))
        {
            Debug.Log("distance " + buttonDistance);
            Debug.Log("Action pressed");

            if (buttonDistance <= 2f)
            {
                Debug.Log("Button press initiated");
                doButtonPress = true;
            }
        }
        if (doButtonPress)
        {
            // display the text
            EnableText();
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

    public void EnableText()
    {
        NPCText.enabled = true;
        timeWhenDisappear = Time.time + timeToAppear;
    }
}
