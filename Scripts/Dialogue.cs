using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;  // Add this namespace for scene management

public class Dialogue : MonoBehaviour
{
    public UIManager sceneManager;
    public TextMeshProUGUI textComponent;
    public ScreenFade ScreenFade;
    public UIManager sceneTransitionManager;
    public GameObject dialogueBox;   // The text UI component for displaying dialogue

    // Two sets of dialogue lines (you can add more if needed)
    public string[] generalLines;           // General dialogue lines
    public string[] shopkeeperLines;        // Shopkeeper dialogue lines

    private string[] currentLines;          // Current set of dialogue lines
    public float textSpeed = 0.05f;         // Speed at which characters appear
    private int index = 0;                  // Current line index
    private bool isTyping = false;          // Flag to indicate if the line is still being typed
    private bool isDialogueFinished = false; // Flag to indicate if the dialogue has finished
    private bool isShopDialogue = false;    // Flag to indicate if the current dialogue is shop-related

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;  // Clear the text at the beginning
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // Check for mouse click (left-click)
        {
            if (isTyping)
            {
                // If the text is still typing, stop typing and show the complete line
                StopAllCoroutines();
                textComponent.text = currentLines[index];
                isTyping = false;
            }
            else
            {
                // If the line is fully shown, move to the next line
                NextLine();
            }
        }
    }

    // Start the dialogue by setting the index to the first line of the current dialogue set
    public void StartDialogue(string[] lines, bool isShop)
    {
        dialogueBox.SetActive(true);
        currentLines = lines;  // Set the current lines to the provided dialogue set
        index = 0;
        isDialogueFinished = false;
        isShopDialogue = isShop;  // Set the shop dialogue flag
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    // Coroutine to type each character of the current line
    IEnumerator TypeLine()
    {
        isTyping = true;
        textComponent.text = string.Empty;

        foreach (char c in currentLines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;  // Finished typing the current line
    }

    // Proceed to the next line in the dialogue
    void NextLine()
    {
        if (index < currentLines.Length - 1)
        {
            // If there are more lines, increment the index and start typing the next line
            index++;
            StartCoroutine(TypeLine());
        }
        else
        {
            // If all lines have been shown, mark the dialogue as finished
            isDialogueFinished = true;
            gameObject.SetActive(false);  // Optionally hide the dialogue UI after it's done

            // Check if this was the shopkeeper dialogue before loading the shop scene
            if (isShopDialogue)
            {

                UIManager.Instance.FadeAndLoadScene("Shop");
                //Invoke("LoadShopScene", 1.1f);  // Load the shop scene if it was shop dialogue

            }
        }
    }
    void LoadShopScene()
    {
        sceneManager.OpenShop();
    }

    // Public method to check if the dialogue has finished
    public bool IsDialogueFinished()
    {
        return isDialogueFinished;
    }

    // New method to start shopkeeper-specific dialogue
    public void StartShopDialogue()
    {
        StartDialogue(shopkeeperLines, true);  // Use the shopkeeper's dialogue lines and set the flag to true
    }
}
