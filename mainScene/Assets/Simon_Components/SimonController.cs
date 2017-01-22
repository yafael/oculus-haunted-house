using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class SimonController : MonoBehaviour {

    public Text hudText;
    public GameObject chestLid;
    public Light halo;

	private const int RED_BUTTON_INDEX = 0;
	private const int GREEN_BUTTON_INDEX = 1;
	private const int YELLOW_BUTTON_INDEX = 2;
	private const int BLUE_BUTTON_INDEX = 3;

	[SerializeField] private List<SimonButton> availableButtons;
	[SerializeField] private int puzzleLength = 4;

	private bool analogWasReset = true;
	private List<string> puzzleKey;
	private int currentPuzzleIndex;
	private Vector2 input;
    private bool puzzleActive = false;
    private float timeToShowCodeText = 2.0f;

    private float flashTimeRemaining = 0;
    private float defaultFlashTime = 1.0f;

    private FirstPersonController firstPersonController;

	// Use this for initialization
	void Start () {
		GeneratePuzzle();
	}

	void GeneratePuzzle()
	{
		// generate puzzle from list
		puzzleKey = new List<string>();
		currentPuzzleIndex = 0;
		Random rnd = new Random();
		for (int i = 0; i < puzzleLength; i++)
		{
			puzzleKey.Add(availableButtons[Random.Range(0, availableButtons.Count)].name);
		}
	}

    // Update is called once per frame
    void Update()
    {
        if (puzzleActive)
        {
            Vector2 i = new Vector2(0, 0);

            if (CrossPlatformInputManager.GetButtonDown("Jump"))
            {
                i.y = 1;
            }
            if (CrossPlatformInputManager.GetButtonDown("Fire1"))
            {
                i.y = -1;
            }
            if (CrossPlatformInputManager.GetButtonDown("Fire2"))
            {
                i.x = 1;
            }
            if (CrossPlatformInputManager.GetButtonDown("Fire3"))
            {
                i.x = -1;
            }

            input = i;


            flashTimeRemaining -= Time.deltaTime;

            float halfDefault = defaultFlashTime / 2.0f;

            float intensityScale = 2.5f;

            if (flashTimeRemaining > 0 && flashTimeRemaining < halfDefault)
            {
                halo.intensity = intensityScale * (flashTimeRemaining / halfDefault);
            }
            else if (flashTimeRemaining > halfDefault)
            {
                halo.intensity = intensityScale * (halfDefault / flashTimeRemaining);
            }
            else 
            {
                halo.intensity = 0;
            }

            
            timeToShowCodeText -= Time.deltaTime;
            if (timeToShowCodeText <= 0)
            {
                hudText.text = "";
            }

            // we don't want to register the command if analog stick isn't tilted enough
            if (input.magnitude < 0.5f)
            {
                // have we recently entered a command? If so, make us able to send another command now.
                if (!analogWasReset)
                {
                    analogWasReset = true;
                }
            }
            else if (analogWasReset)
            {
                

                // find what direction the tilt most represents
                float upProduct = Vector2.Dot(Vector2.up, input);
                float downProduct = Vector2.Dot(Vector2.down, input);
                float leftProduct = Vector2.Dot(Vector2.left, input);
                float rightProduct = Vector2.Dot(Vector2.right, input);

                int selectedIndex = -1;

                // compare up and down
                float maxHorizontal = Mathf.Max(leftProduct, rightProduct);
                if (upProduct >= downProduct)
                {
                    if (upProduct > maxHorizontal)
                    {
                        // up is the direction. Hardcoded as red
                        selectedIndex = RED_BUTTON_INDEX;
                        FlashLight(Color.red);
                    }
                    else
                    {
                        if (rightProduct > leftProduct)
                        {
                            // right is the direction
                            selectedIndex = GREEN_BUTTON_INDEX;
                            FlashLight(Color.green);

                        }
                        else
                        {
                            // left is the direction
                            selectedIndex = BLUE_BUTTON_INDEX;
                            FlashLight(Color.blue);
                        }
                    }
                }
                else
                {
                    // down is the direction
                    selectedIndex = YELLOW_BUTTON_INDEX;
                    FlashLight(Color.yellow);
                }

                bool isCorrectButton = puzzleKey[currentPuzzleIndex] == availableButtons[selectedIndex].name;
                availableButtons[selectedIndex].Press(isCorrectButton);

                if (isCorrectButton)
                {
                    currentPuzzleIndex++;
                    if (currentPuzzleIndex >= puzzleKey.Count)
                    {
                        puzzleActive = false;
                        firstPersonController.m_WalkSpeed = 2;
                        DoorCollisionDetection.hasCompletedChallenge = true;
                        // do other animation and stuff here
                        Animator anim = chestLid.GetComponent<Animator>();
                        anim.SetTrigger("openChest");

                        FlashLight(Color.cyan);
                    }
                }
                else
                {
                    currentPuzzleIndex = 0;
                    FlashLight(Color.magenta);
                }

                analogWasReset = false;
            }
        }
    }

    void FlashLight(Color color)
    {
        halo.color = color;
        flashTimeRemaining = defaultFlashTime;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            firstPersonController = other.GetComponent<FirstPersonController>();
            firstPersonController.m_WalkSpeed = 0;
            puzzleActive = true;
            GetComponent<BoxCollider>().enabled = false;
            hudText.text = "Crack the code.";
        }
    }
}
