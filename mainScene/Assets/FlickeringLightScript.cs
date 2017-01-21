using UnityEngine;
using System.Collections;

public class FlickeringLightScript : MonoBehaviour
{
    private Light myLight;
    float minFlickerSpeed = 10.0f;
    float maxFlickerSpeed = 50.0f;

    LightState state = LightState.LightStateOff;

    float timeUntilNextFlicker = 0;
    float timeUntilFlickerEnds = 0;
    float flickerSet = 0;

    float flickerFrequency = 0.08f;

    enum LightState
    {
        LightStateOn,
        LightStateOff,
        LightStateFlickering
    }

    void Start()
    {
        myLight = GetComponent<Light>();
       // StartCoroutine(flicker());

        timeUntilNextFlicker = Random.Range(15.0f, 30.0f);
    }

    IEnumerator flicker()
    {
        print("Entered");
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 4; i++)
        {
            myLight.enabled = true;
            yield return new WaitForSeconds(Random.Range(minFlickerSpeed, maxFlickerSpeed));
            print("HI");
            myLight.enabled = false;
            yield return new WaitForSeconds(Random.Range(minFlickerSpeed + 1, maxFlickerSpeed));
        }
        yield return new WaitForSeconds(10f);
    }
    void Update()
    {
        if (this.state == LightState.LightStateFlickering)
        {
            timeUntilFlickerEnds -= Time.deltaTime;

            //Flicker
            flickerSet -= Time.deltaTime;

            if (flickerSet <= 0)
            {
                myLight.enabled = !myLight.enabled;
                flickerSet = flickerFrequency;
            }


            if (timeUntilFlickerEnds <= 0)
            {
                System.Random rnd = new System.Random();

                bool on = rnd.Next(0, 4) != 1;

                state = on ? LightState.LightStateOn : LightState.LightStateOff;
                myLight.enabled = on;
                timeUntilNextFlicker = Random.Range(15.0f, 30.0f);
            }
        }
        else
        {
            timeUntilNextFlicker -= Time.deltaTime;

            if (timeUntilNextFlicker <= 0)
            {
                state = LightState.LightStateFlickering;
                timeUntilFlickerEnds = (float)Random.Range(.5f, 2.0f);
                flickerSet = flickerFrequency;
            }
        }  
    }
}
