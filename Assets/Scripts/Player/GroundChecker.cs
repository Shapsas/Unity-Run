using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    private CapsuleCollider capsuleCollider;
    [SerializeField]
    private LayerMask groundLayers;

    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponentInChildren<CapsuleCollider>();
    }

    /// <summary>
    /// Checks if the player is touching the ground. This is a quick hack to make it work, don't actually do it like this.
    /// </summary>
    /// <returns>True if the player touches the ground, false if not</returns>
    public bool CheckGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        bool result = Physics.Raycast(ray, capsuleCollider.bounds.extents.y + 0.1f, groundLayers);
        return result;
    }
}
