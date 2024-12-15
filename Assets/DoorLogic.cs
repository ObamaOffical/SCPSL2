using System.Collections;
using UnityEngine;

public class Door_Script : MonoBehaviour
{
    public Transform PlayerPos;
    public Transform Door;

    private bool IsOpen = false;
    [SerializeField]
    public float Speed = 1f;
    [SerializeField]
    private float SlideAmount = 1.9f;

    private bool done = true;
    
    [SerializeField]
    private Vector3 StartPosition;
    [SerializeField]
    private Vector3 SlideDirection = Vector3.right;

    private Coroutine AnimationCoroutine;

    private void OnMouseDown()
    {
        if (!IsOpen && done)
        {
            Open(PlayerPos.position);
        }
        else if (done)
        {
            Close();
        }
    }

    private void Awake()
    {
        StartPosition = Door.transform.position;
    }

    public void Open(Vector3 UserPosition)
    {
        if (!IsOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            AnimationCoroutine = StartCoroutine(DoSlidingOpen());
        }
    }

    private IEnumerator DoSlidingOpen()
    {
        done = false;
        Vector3 startPosition = Door.transform.position;
        Vector3 endPosition = StartPosition + SlideAmount * SlideDirection;

        float time = 0;
        IsOpen = true;
        while (time < 1)
        {
            Door.transform.position = Vector3.Lerp(startPosition, endPosition, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }
        done = true;
    }

    public void Close()
    {
        if (IsOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            AnimationCoroutine = StartCoroutine(DoSlidingClose());
        }
    }

    private IEnumerator DoSlidingClose()
    {
        done = false;
        Vector3 endPosition = StartPosition;
        Vector3 startPosition = Door.transform.position;
        float time = 0;

        IsOpen = false;

        while (time < 1)
        {
            Door.transform.position = Vector3.Lerp(startPosition, endPosition, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }
        done = true;
    }
}