using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class CameraShiftDown : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float downMovementSpeed = 5.0f;

    public Transform cameraDownTarget;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (cameraDownTarget != null)
        {
            StartCoroutine(MoveCameraDown());
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopAllCoroutines();
    }

    private IEnumerator MoveCameraDown()
    {
        while (true)
        {
            cameraDownTarget.position = new Vector3(cameraDownTarget.position.x, cameraDownTarget.position.y + downMovementSpeed * -1, cameraDownTarget.position.z);
            yield return null;
        }
    }
}
