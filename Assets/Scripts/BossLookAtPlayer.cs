using UnityEngine;

public class BossLookAtPlayer : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform player;

    [Header("Flip Settings")]
    [Tooltip("Wenn true: Boss schaut bei +ScaleX nach rechts. Wenn false, invertiert.")]
    [SerializeField] private bool scaleXFacesRight = true;

    [Tooltip("Wie groﬂ soll der Boss sein? (wird die absolute ScaleX/ScaleY setzen)")]
    [SerializeField] private float baseScaleX = 1f;
    [SerializeField] private float baseScaleY = 1f;

    private void Awake()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        // Debug einmalig
        Debug.Log($"BossLookAtPlayer Awake. Player={(player ? player.name : "NULL")}");
    }

    private void LateUpdate()
    {
        if (player == null) return;

        float dx = player.position.x - transform.position.x;
        if (Mathf.Abs(dx) < 0.001f) return;

        bool wantFaceRight = dx > 0f; // player rechts => face right

        // Wenn ScaleXFacesRight=false, invertieren wir die Logik
        if (!scaleXFacesRight) wantFaceRight = !wantFaceRight;

        float xSign = wantFaceRight ? 1f : -1f;

        // Setze Scale konsistent (verhindert Drift)
        transform.localScale = new Vector3(baseScaleX * xSign, baseScaleY, 1f);
    }
}