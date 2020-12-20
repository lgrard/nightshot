using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject cameraContainer;

    public void RemovePlayer(Transform playerToRemove)
    {
        cameraContainer.GetComponent<CameraMovement>().targets.Remove(playerToRemove);
    }
}
