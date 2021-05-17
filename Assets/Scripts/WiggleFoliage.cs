using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiggleFoliage : MonoBehaviour, IInteractible
{
    [Header("Wiggle Settings")]
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float wiggleDegrees = 10f;
    [SerializeField] private float wiggleSpeed = 10f;
    [SerializeField] private float lerpDuration = 0.4f;
    private bool triggerAnimation = false;
    private float lerpValue = 1;
    private float timer = 0;
    private float timeElapsed = 0;

    private void Start()
    {
        transform.GetComponent<Renderer>().enabled = false;
    }

    private void Update()
    {
        if (triggerAnimation)
        {
            PlayWiggleAnimation();
        }
    }

    private void PlayWiggleAnimation()
    {
        timer += Time.deltaTime;
        float sin = Mathf.Sin(wiggleSpeed * curve.Evaluate(timer));

        // Tune down wiggle
        if (timer > lerpDuration)
        {
            lerpValue = Mathf.Lerp(lerpValue, 0, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
        }

        // Apply wiggle rotation
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, sin * wiggleDegrees * lerpValue));

        // Reset animation
        if (transform.rotation.eulerAngles.z == 0)
        {
            triggerAnimation = false;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            timer = 0;
            lerpValue = 1;
            timeElapsed = 0;
        }
    }

    public void Interact()
    {
        triggerAnimation = true;
    }

    public void SetVisibility(bool isVisible)
    {
        if (isVisible)
        {
            gameObject.transform.GetComponent<Renderer>().enabled = true;
            transform.GetComponent<WiggleFoliage>().enabled = true;
        }
        else
        {
            gameObject.transform.GetComponent<Renderer>().enabled = false;
            transform.GetComponent<WiggleFoliage>().enabled = false;
        }
    }
}
