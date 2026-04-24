// Scripts/Player/CameraFollow.cs
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;            // Player 드래그
    public Vector3 offset = new Vector3(0, 18, -1);
    public float smoothSpeed = 8f;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = target.position + offset;
        transform.position = Vector3.Lerp(
            transform.position,
            desired,
            smoothSpeed * Time.deltaTime
        );
    }
}