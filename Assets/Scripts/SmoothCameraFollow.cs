using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [SerializeField]
    private GameObject m_FollowTarget;
    [SerializeField]
    private float _lerpCoefficient;
    [SerializeField]
    private Vector3 _distanceOffset;

    private void FixedUpdate()
    {
        Vector3 desiredPosition = m_FollowTarget.transform.position + _distanceOffset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _lerpCoefficient);

        transform.position = smoothedPosition;
    }

}
