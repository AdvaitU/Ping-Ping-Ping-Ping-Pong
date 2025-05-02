using UnityEngine;

/// <summary>
/// Basic wall behavior for bouncing the puck. Assign a Physics Material 2D to control bounciness.
/// Attach this to wall GameObjects with a BoxCollider2D.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class Wall : MonoBehaviour
{
    [Header("Wall Bounce Properties")]

    [Tooltip("Physics Material 2D to control bounce behavior.")]
    public PhysicsMaterial2D wallMaterial;

    private void Awake()
    {
        var col = GetComponent<BoxCollider2D>();
        if (wallMaterial != null)
        {
            col.sharedMaterial = wallMaterial;
        }
    }
}

