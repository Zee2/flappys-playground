using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Birb : MonoBehaviour
{
    public Perch TargetPerch;

    public Animator BirbAnimator;

    public float Speed;

    public float TurningSpeed;

    public List<AudioClip> birdTweets = new List<AudioClip>();

    private bool isFlying = true;

    private Vector3 goalPosition;

    private enum State
    {
        Wandering,
        Seeking,
        Landed
    }

    private State state = State.Wandering;

    private bool turningLeft; // During wander

    private Vector3 wanderPoint;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(ChangeMind());
        StartCoroutine(Tweet());
    }

    private IEnumerator ChangeMind()
    {
        while (true)
        {
            wanderPoint = new Vector3(Random.Range(-0.5f,0.5f),
                                      Random.Range(-0.4f,0.0f),
                                      Random.Range(-0.5f,0.5f));
            wanderPoint += Camera.main.transform.position + Vector3.Scale(new Vector3(1,0.1f,1), Camera.main.transform.forward * 1.0f);
            
            yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
        }
    }

    private IEnumerator Tweet()
    {
        while (true)
        {
            audioSource.PlayOneShot(birdTweets[Random.Range(0, birdTweets.Count)]);
            
            yield return new WaitForSeconds(Random.Range(2.0f, 5.0f));
        }
    }

    void Update()
    {
        switch (state)
        {
            case State.Wandering:
                isFlying = true;
                FlyTowards(wanderPoint);
                
                if (TargetPerch.Valid)
                {
                    state = State.Seeking;
                }
                break;
            case State.Seeking:
                isFlying = true;
                FlyTowards(TargetPerch.transform.position);

                bool closeEnough = Vector3.Distance(transform.position, TargetPerch.transform.position) < 0.025f;

                if (!TargetPerch.Valid)
                {
                    state = State.Wandering;
                }
                else if (TargetPerch.Valid && closeEnough)
                {
                    state = State.Landed;
                }
                break;
            case State.Landed:
                isFlying = false;
                transform.position = Vector3.Lerp(transform.position, TargetPerch.transform.position, 15 * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, TargetPerch.transform.rotation, 15 * Time.deltaTime);

                if (!TargetPerch.Valid)
                {
                    state = State.Wandering;
                }
                break;
        }
        
        BirbAnimator.SetBool("IsLanded", !isFlying);
    }

    void FlyTowards(Vector3 pos)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(pos - transform.position), TurningSpeed * Time.deltaTime);
        transform.position = transform.position + transform.forward * Speed * Time.deltaTime;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(goalPosition, 0.05f);
    }
}
