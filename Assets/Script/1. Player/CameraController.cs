using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    private Vector3 shakeOffset = Vector3.zero;

    private Camera cam;
    private float defaultSize;
    private Coroutine zoomRoutine;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (cam != null)
        {
            defaultSize = cam.orthographicSize;
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset + shakeOffset;
        }
    }

    public void ApplyShake(Vector3 shake)
    {
        shakeOffset = shake;
    }

    public void ResetShake()
    {
        shakeOffset = Vector3.zero;
    }

    public void CriticalZoom(float zoomSize = 4.0f, float duration = 0.1f)
    {
        if (zoomRoutine != null)
            StopCoroutine(zoomRoutine);

        zoomRoutine = StartCoroutine(CriticalZoomRoutine(zoomSize, duration));
    }

    private IEnumerator CriticalZoomRoutine(float zoomSize, float duration)
    {
        float timer = 0f;
        float halfDuration = duration / 2f;

        // 카메라 확대
        while (timer < halfDuration)
        {
            cam.orthographicSize = Mathf.Lerp(defaultSize, zoomSize, timer / halfDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        cam.orthographicSize = zoomSize;
        timer = 0f;

        // 카메라 원래대로
        while (timer < halfDuration)
        {
            cam.orthographicSize = Mathf.Lerp(zoomSize, defaultSize, timer / halfDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        cam.orthographicSize = defaultSize;
        zoomRoutine = null;
    }
}