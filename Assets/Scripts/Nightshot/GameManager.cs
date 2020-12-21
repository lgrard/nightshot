using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject cameraContainer;
    private CameraMovement cameraMovement;

    private void Start()
    {
        cameraMovement = cameraContainer.GetComponent<CameraMovement>();
    }

    public void RemovePlayer(Transform playerToRemove) => cameraMovement.targets.Remove(playerToRemove);
    public void Screenshake(float magnitude, float duration) => StartCoroutine(cameraMovement.ScreenShake(magnitude,duration));
}
