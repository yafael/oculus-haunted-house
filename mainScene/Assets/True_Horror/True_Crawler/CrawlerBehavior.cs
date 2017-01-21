using UnityEngine;
using System.Collections;



[RequireComponent(typeof(AudioSource))]
public class CrawlerBehavior : MonoBehaviour {
    public AudioClip[] approach1Sounds;
    public AudioClip[] approach2Sounds;
    public AudioClip[] approach3Sounds;
    public AudioClip killSound;

    public GameObject windowCrawler;

    Transform cameraTransform = null;

    EnemyState state = EnemyState.EnemyStateIdle;

    float enemyTime = 120;

    Vector3 idealVector = new Vector3(-1.0f, 0.0f, 0.0f);

    float tolerance = 25.0f;

    float timeRemainingBeforeStateChange = 0;
    float nextSoundTime = 0;

    public AudioSource audioC;

    // Use this for initialization
    void Start () {
        gameObject.transform.position = new Vector3(0.0f, -10f, 0.0f);

        cameraTransform = GameObject.FindWithTag("MainCamera").transform;
        System.Random rnd1 = new System.Random();

        double extra = 10.0 * 2.0 * (rnd1.NextDouble() - .5);
        enemyTime += (float)extra;

        timeRemainingBeforeStateChange = EnemyTimeForState(state);
    }

    enum EnemyState {
        EnemyStateIdle,
        EnemyStateApproach1,
        EnemyStateApproach2,
        EnemyStateApproach3,
        EnemyStatePreAttack,
        EnemyStateLunging,
        EnemyStateAttacking
    }

    

	// Update is called once per frame
	void Update () {
        
        if (state == EnemyState.EnemyStatePreAttack)
        {
            Vector3 worldDirection = cameraTransform.transform.TransformDirection(Vector3.forward);

            float ang = Vector3.Angle(worldDirection, idealVector);

            if (Mathf.Abs(ang) < tolerance)
            {
                KillPerson();
            }
        }
        if (state == EnemyState.EnemyStateLunging)
        {
            TransformKilled();
        }
        else if (state != EnemyState.EnemyStateAttacking && state != EnemyState.EnemyStatePreAttack)
        {
            Vector3 worldDirection = cameraTransform.transform.TransformDirection(Vector3.forward);

            float ang = Vector3.Angle(worldDirection, idealVector);

            if (Mathf.Abs(ang) < tolerance)
            {
                MoveWindowCrawlerForState(state);
            }

            timeRemainingBeforeStateChange -= Time.deltaTime;
            nextSoundTime -= Time.deltaTime;

            if (nextSoundTime <= 0)
            {
                System.Random rnd2 = new System.Random();
                nextSoundTime = (float)(1.0 + (SoundFrequencyForState(state) * rnd2.NextDouble()));
                PlaySoundForState(state);
            }

            if (timeRemainingBeforeStateChange <= 0)
            {
                state++;
                timeRemainingBeforeStateChange = EnemyTimeForState(state);
                TransformVisualToState(state);
                if (state == EnemyState.EnemyStateApproach3)
                {
                    System.Random rnd = new System.Random();
                    int soundNumber = rnd.Next(0, approach3Sounds.Length - 1);
                    audioC.PlayOneShot(approach3Sounds[soundNumber]);
                }

            }
        }

	}

    bool movedFor2 = false;
    bool movedFor3 = false;

    void MoveWindowCrawlerForState(EnemyState state)
    {
        

        switch (state)
        {
            case EnemyState.EnemyStateIdle:
                break;
            case EnemyState.EnemyStateApproach1:
                break;
            case EnemyState.EnemyStateApproach2:
                if (!movedFor2)
                {
                    iTween.MoveTo(windowCrawler, new Vector3(windowCrawler.transform.position.x, 5.55f, windowCrawler.transform.position.z), 6.0f);
                    movedFor2 = true;
                }
                break;
            case EnemyState.EnemyStateApproach3:
                if (!movedFor3)
                {
                    iTween.MoveTo(windowCrawler, new Vector3(windowCrawler.transform.position.x, 1.55f, windowCrawler.transform.position.z), 4.0f);
                    movedFor3 = true;
                }
                break;
            case EnemyState.EnemyStatePreAttack:
                break;
            case EnemyState.EnemyStateLunging:
                break;
            case EnemyState.EnemyStateAttacking:
                break;
            default:
                break;
        }
     }

    void TransformVisualToState(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.EnemyStateApproach2:
                windowCrawler.transform.position = new Vector3(-8.64f, 1.74f, -11.41f);
                break;
            case EnemyState.EnemyStateApproach3:
                windowCrawler.transform.position = new Vector3(-8.64f, 5.55f, -11.41f);
                windowCrawler.transform.Rotate(0.0f, 180, 0.0f);
                break;
            case EnemyState.EnemyStatePreAttack:
                break;
            case EnemyState.EnemyStateLunging:
                break;
            case EnemyState.EnemyStateAttacking:
                break;
            default:
                break;
        }
    }

    float SoundFrequencyForState(EnemyState state)
    {
        float freq = 10.0f;
        switch (state)
        {
            case EnemyState.EnemyStateApproach2:
                freq = 5.0f;
                break;
            case EnemyState.EnemyStateApproach3:
            case EnemyState.EnemyStatePreAttack:
                freq = 7.0f;
                break;
        }
        return freq;
    }

    void PlaySoundForState(EnemyState state)
    {
        System.Random rnd = new System.Random();

        int soundNumber = 0;

        switch (state)
        {
            case EnemyState.EnemyStateIdle:
                break;
            case EnemyState.EnemyStateApproach1:
                soundNumber = rnd.Next(0,approach1Sounds.Length - 1);
                audioC.PlayOneShot(approach1Sounds[soundNumber]);
                break;
            case EnemyState.EnemyStateApproach2:
                soundNumber = rnd.Next(0, approach2Sounds.Length - 1);
                audioC.PlayOneShot(approach2Sounds[soundNumber]);
                break;
            case EnemyState.EnemyStateApproach3:
            case EnemyState.EnemyStatePreAttack:
                //soundNumber = rnd.Next(0, approach3Sounds.Length - 1);
                //audioC.PlayOneShot(approach3Sounds[soundNumber]);
                break;
            default:
                break;
        }
    }

    float EnemyTimeForState(EnemyState state)
    {
        float time = (enemyTime * (5.0f / 6.0f)) / 3.0f;
        if (state == EnemyState.EnemyStateApproach3)
        {
            time = (enemyTime * (1.0f / 10.0f));
        }

        if (state == EnemyState.EnemyStateIdle)
        {
            time = (enemyTime * (1.0f / 6.0f));
        }
        return time;
    }

    void KillPerson ()
    {
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("toAttack");
        state = EnemyState.EnemyStateLunging;
        audioC.PlayOneShot(killSound);

        Vector3 worldDirection = cameraTransform.transform.TransformDirection(Vector3.forward);

        float scale = .75f;

        Vector3 camPosition = cameraTransform.transform.position;

        gameObject.transform.position = new Vector3(camPosition.x + scale * worldDirection.x, 6.50f + worldDirection.y, camPosition.z + scale * worldDirection.z);

        yDrop = 0.0f;
    }

    float yDrop = 0.0f;

    void TransformKilled ()
    {
        gameObject.transform.Translate(Time.deltaTime * 15.0f * Vector3.down);

        yDrop += Time.deltaTime * 15.0f;

        if (yDrop > 3.86f)
        {
            state = EnemyState.EnemyStateAttacking;
        }
    }

}
