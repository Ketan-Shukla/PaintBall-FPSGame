using UnityEngine;

public class CameraRotation : MonoBehaviour {

    [SerializeField]
    private Transform lookPoint;
    [SerializeField]
    private float rotateSpeed = 12f;

    /// <summary>
    /// Rotates the camera around the arena for the duration of loading screen
    /// </summary>
    void Update() {
        transform.RotateAround(lookPoint.position, Vector3.up, rotateSpeed * Time.deltaTime);
        transform.LookAt(lookPoint, Vector3.up);
    }

}
