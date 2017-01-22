using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class SimonController : MonoBehaviour {

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
	void Update () {
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
				}
				else
				{
					if (rightProduct > leftProduct)
					{
						// right is the direction
						selectedIndex = GREEN_BUTTON_INDEX;
					}
					else
					{
						// left is the direction
						selectedIndex = BLUE_BUTTON_INDEX;
					}
				}
			}
			else
			{
				// down is the direction
				selectedIndex = YELLOW_BUTTON_INDEX;
			}

			bool isCorrectButton = puzzleKey[currentPuzzleIndex] == availableButtons[selectedIndex].name;
			availableButtons[selectedIndex].Press(isCorrectButton);

			if (isCorrectButton)
			{
				currentPuzzleIndex++;
				if (currentPuzzleIndex >= puzzleKey.Count)
				{
					Debug.Log("Puzzle Complete! Starting over.");
					Start();
				}
			}
			else
			{
				currentPuzzleIndex = 0;
			}

			analogWasReset = false;
		}
	}

	void FixedUpdate () {
		float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
		float vertical = CrossPlatformInputManager.GetAxis("Vertical");

		input = new Vector2 (horizontal, vertical);
	}

}
