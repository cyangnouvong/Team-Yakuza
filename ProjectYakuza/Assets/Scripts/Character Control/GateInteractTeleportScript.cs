using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GateInteractTeleportScript : MonoBehaviour
{
    public Rigidbody rbody;
    public GameObject townGate;
    public GameObject interactTextBox;
    public GameObject interactText;
    public GameObject camera;
    public GameObject sceneSwitchText;

    void FixedUpdate()
    {
        float townGateDistance = -1.0f;
        if (townGate != null)
        {
            townGateDistance = Vector3.Distance(rbody.transform.position, townGate.transform.position);
        }

        if (townGateDistance != -1 && townGateDistance < 3.0f)
        {
            interactText.SetActive(true);
            interactTextBox.SetActive(true);
        }
        else
        {
            interactText.SetActive(false);
            interactTextBox.SetActive(false);
        }

        if (interactText.activeSelf && Input.GetKey(KeyCode.E))
        {
            FadeBlackScript.fade_out = true;
            ThirdPersonCamera.followPlayer = false;

            StartCoroutine(teleport());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        FadeBlackScript.fade_out = true;
        StartCoroutine(switchScene());
    }

    IEnumerator teleport()
    {
        yield return new WaitForSeconds(0.96f);
        rbody.transform.position = new Vector3(-24f, 0.04999983f, -19f);
        rbody.transform.rotation = Quaternion.Euler(0, 85, 0);
        camera.transform.position = new Vector3(-29.27984f, 2.549999f, -19.46193f);
        camera.transform.rotation = Quaternion.Euler(6, -275f, 0);
        ThirdPersonCamera.followPlayer = true;
        FadeBlackScript.fade_in = true;
    }

    IEnumerator switchScene()
    {
        yield return new WaitForSeconds(1f);
        sceneSwitchText.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("ChurchScene");
    }
}
