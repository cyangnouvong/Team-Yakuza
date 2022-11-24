using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    // Added tutorial and dialogue UI canvas https://www.youtube.com/watch?v=8oTYabhj248
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    public static int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        CharacterControlScript.freezeCharacter = true;
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (index == 1)
        {
            ThirdPersonCamera.followPlayer = true;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            } else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        // type each character 1 by 1
        foreach(char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        } else
        {
            index++;
            gameObject.SetActive(false);
            CharacterControlScript.freezeCharacter = false;
        }
        Debug.Log(index);
    }
}
