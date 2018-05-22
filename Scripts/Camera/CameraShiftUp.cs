using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class CameraShiftUp : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float upMovementSpeed = 5.0f;

    public Transform cameraUpTarget;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (cameraUpTarget != null)
        {
            StartCoroutine(MoveCameraUp());
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopAllCoroutines();
    }

    private IEnumerator MoveCameraUp()
    {
        while (true)
        {
            cameraUpTarget.position = new Vector3(cameraUpTarget.position.x, cameraUpTarget.position.y + upMovementSpeed * 1, cameraUpTarget.position.z);
            yield return null;
        }
    }
}
