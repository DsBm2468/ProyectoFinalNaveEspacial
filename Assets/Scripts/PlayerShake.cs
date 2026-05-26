using UnityEngine;
using System.Collections;

public class PlayerShake : MonoBehaviour
{
    public Transform cameraTransform;

    public float shakeDuration = 0.15f;

    public float shakeAmount = 0.15f;

    Vector3 originalPos;

    void Start()
    {
        originalPos =
        cameraTransform.localPosition;
    }

    public void HitShake()
    {
        StopAllCoroutines();

        StartCoroutine(
        ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine()
    {
        float elapsed = 0;

        while (
        elapsed <
        shakeDuration)
        {
            Vector3 offset =
            Random.insideUnitSphere *
            shakeAmount;

            cameraTransform.localPosition =
            originalPos +
            offset;

            elapsed +=
            Time.deltaTime;

            yield return null;
        }

        cameraTransform.localPosition =
        originalPos;
    }
}