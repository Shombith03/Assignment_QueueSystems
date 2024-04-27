using System.Collections;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;
    public float transitionDuration = 1f;

    private bool isTransitioning = false;

    void Start()
    {
        // Initially, enable the first camera and disable the second camera
        camera1.enabled = true;
        camera2.enabled = false;
    }

    public void TransitionToCamera2()
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionCamera(camera1, camera2));
        }
    }

    public void TransitionToCamera1()
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionCamera(camera2, camera1));
        }
    }

    private IEnumerator TransitionCamera(Camera fromCamera, Camera toCamera)
    {
        isTransitioning = true;

        float elapsedTime = 0f;
        Vector3 startPos = fromCamera.transform.position;
        Quaternion startRot = fromCamera.transform.rotation;

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            fromCamera.transform.position = Vector3.Lerp(startPos, toCamera.transform.position, t);
            fromCamera.transform.rotation = Quaternion.Lerp(startRot, toCamera.transform.rotation, t);
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        fromCamera.transform.position = toCamera.transform.position;
        fromCamera.transform.rotation = toCamera.transform.rotation;

        // Disable/Enable cameras
        fromCamera.enabled = false;
        toCamera.enabled = true;

        isTransitioning = false;
    }
}
