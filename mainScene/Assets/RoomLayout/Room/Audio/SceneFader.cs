using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneFader : MonoBehaviour {

    public float fadeSpeed = 1.5f;

    [SerializeField]  private GUITexture gTexture;

    // Use this for initialization
    void Start () {
        gTexture.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);
    }

    // Update is called once per frame
    void Update () {
        FadeToBlack();
    }

    public void FadeToBlack()

    {
        // Lerp the colour of the texture between itself and black.

        gTexture.color = Color.Lerp(gTexture.color, Color.black, fadeSpeed * Time.deltaTime);

    }
}
