using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float flashDuration = 0.12f;
    [SerializeField] private int flashCount = 2;

    private Color originalColor;
    private Coroutine routine;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        originalColor = spriteRenderer.color;
    }

    public void Flash() // default red
    {
        FlashColor(Color.red);
    }

    public void FlashColor(Color c)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(FlashRoutine(c));
    }

    private IEnumerator FlashRoutine(Color c)
    {
        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = c;
            yield return new WaitForSeconds(flashDuration);

            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }

        routine = null;
    }
}