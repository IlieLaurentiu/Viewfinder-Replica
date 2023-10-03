using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class Photograph : MonoBehaviour
{
    [Header("The Photograph GameObject")]
    [SerializeField] private GameObject photograph;

    private Quaternion handheldRotation, focusedRotation;
    private Vector3 handheldPosition, focusedPosition, handheldScale, focusedScale;
    private Vector3 velocity = Vector3.one;
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
                photograph.transform.localRotation = handheldRotation;
                StartCoroutine(AnimatePosition(handheldPosition, 0.2f));
                photograph.transform.localScale = handheldScale;
                break;

            case PhotoState.FOCUSED:
                isFocused = true;
                photograph.transform.localRotation = focusedRotation;
                StartCoroutine(AnimatePosition(focusedPosition, 0.2f));
                photograph.transform.localScale = focusedScale;
                break;
        }
    }

    private enum PhotoState
    {
        NONE,
        HANDHELD,
        FOCUSED
    }

    private IEnumerator AnimatePosition(Vector3 endValue, float duration)
    {
        isAnimating = true;
        float timeElapsed = 0;

        // add 0.7 seconds to duration to fix severe undershoot of smoothdamp
        // rewrite this later
        while (timeElapsed < duration + 0.7f)
        {
            photograph.transform.localPosition = Vector3.SmoothDamp(photograph.transform.localPosition, endValue, ref velocity, duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // fix undershoot of the smoothdamp function
        if(photograph.transform.localPosition != endValue)
        {
            photograph.transform.localPosition = endValue;
        }

        yield return new WaitForSeconds(duration);
        isAnimating = false;
    }
}
