using System;
using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour
{
    public Teleporter Other;
    public float cooldownTime = 0.5f;

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        float zPos = transform.worldToLocalMatrix.MultiplyPoint3x4(other.transform.position).z;

        if (zPos < 0)
        {
            StartCoroutine(TeleportWithCooldown(other.transform, other));
        }
    }

    //Neutralizing looped teleportation
    private IEnumerator TeleportWithCooldown(Transform obj, Collider playerCollider)
    {
        playerCollider.enabled = false;
        Teleport(obj);
        yield return new WaitForSeconds(cooldownTime);
        playerCollider.enabled = true;
    }

    private void Teleport(Transform obj)
    {
        // Get local position & rotation
        Vector3 localPos = transform.InverseTransformPoint(obj.position);
        Quaternion localRot = Quaternion.Inverse(transform.rotation) * obj.rotation;

        // Mirroring
        localPos = new Vector3(-localPos.x, localPos.y, -localPos.z);

        // Apply on another teleport
        obj.position = Other.transform.TransformPoint(localPos);
        obj.rotation = Other.transform.rotation * localRot;

        // Little offset
        obj.position += Other.transform.forward * 0.1f;
    }
}