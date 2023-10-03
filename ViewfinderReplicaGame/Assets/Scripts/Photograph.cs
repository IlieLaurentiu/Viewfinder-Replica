using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class Photograph : MonoBehaviour
{
    [Header("The Photograph GameObject")]
    [SerializeField] private GameObject photograph;

    [Header("State Transition Animations")]
    [SerializeField] private float animDuration = 0.5f;

    private Quaternion handheldRotation, focusedRotation;
    private Vector3 handheldPosition, focusedPosition, handheldScale, focusedScale;
    private PhotoState photoState;

    bool isFocused, isAnimating;

    private void Awake()
    {
        photoState = PhotoState.NONE;
    }

    void Start()
    {
        // make the photograph face the player for handheld state
        handheldRotation = Quaternion.Euler(15, -45, 0);

        // store a quaternion with 0 rotation for focused state
        focusedRotation = Quaternion.Euler(0, 0, 0);

        // do the same for the photograph's position and scale
        handheldPosition = new Vector3(-0.7f, -0.3f, 1);
        focusedPosition = new Vector3(0, 0, 1);

        handheldScale = new Vector3(0.3f, 0.3f, 0.3f);
        focusedScale = Vector3.one;
    }

    void Update()
    {
        // On right click switch photograph state
        if(Input.GetKeyDown(KeyCode.Mouse1)) 
        {
            if (isAnimating)
                return;

            if (isFocused) 
            {
                photoState = PhotoState.HANDHELD;
            }
            else
            {
                photoState = PhotoState.FOCUSED; 
            }

            SwitchPhotographState();

            StartCoroutine(Animate(animDuration));
        }
    }

    void SwitchPhotographState()
    {
        switch (photoState)
        {
            case PhotoState.NONE:
                break;

            case PhotoState.HANDHELD:
                isFocused = false;
                break;

            case PhotoState.FOCUSED:
                isFocused = true;
                break;
        }
    }

    private enum PhotoState
    {
        NONE,
        HANDHELD,
        FOCUSED
    }

    private IEnumerator Animate(float duration)
    {
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            if(!isFocused)
            {
                photograph.transform.localPosition = new Vector3(
                    x: Mathf.Lerp(photograph.transform.localPosition.x, handheldPosition.x, timeElapsed / duration),
                    y: Mathf.Lerp(photograph.transform.localPosition.y, handheldPosition.y, timeElapsed / duration),
                    z: Mathf.Lerp(photograph.transform.localPosition.z, handheldPosition.z, timeElapsed / duration)
                    );

                photograph.transform.localScale = new Vector3(
                    x: Mathf.Lerp(photograph.transform.localScale.x, handheldScale.x, timeElapsed / duration),
                    y: Mathf.Lerp(photograph.transform.localScale.y, handheldScale.y, timeElapsed / duration),
                    z: Mathf.Lerp(photograph.transform.localScale.z, handheldScale.z, timeElapsed / duration)
                    );

                photograph.transform.localRotation = Quaternion.Slerp(photograph.transform.localRotation, handheldRotation, timeElapsed / duration);               
            } 
            else
            {
                photograph.transform.localPosition = new Vector3(
                    x: Mathf.Lerp(photograph.transform.localPosition.x, focusedPosition.x, timeElapsed / duration),
                    y: Mathf.Lerp(photograph.transform.localPosition.y, focusedPosition.y, timeElapsed / duration),
                    z: Mathf.Lerp(photograph.transform.localPosition.z, focusedPosition.z, timeElapsed / duration)
                    );

                photograph.transform.localScale = new Vector3(
                    x: Mathf.Lerp(photograph.transform.localScale.x, focusedScale.x, timeElapsed / duration),
                    y: Mathf.Lerp(photograph.transform.localScale.y, focusedScale.y, timeElapsed / duration),
                    z: Mathf.Lerp(photograph.transform.localScale.z, focusedScale.z, timeElapsed / duration)
                    );

                photograph.transform.localRotation = Quaternion.Slerp(photograph.transform.localRotation, focusedRotation, timeElapsed / duration);
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        
    }
}
