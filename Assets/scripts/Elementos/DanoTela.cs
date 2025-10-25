using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DanoTela : MonoBehaviour
{
    public Image danoImage;
    public float flashDuration = 0.2f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.4f); // vermelho com transparência

    private Coroutine flashRoutine;

    public void Flash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        danoImage.color = flashColor;

        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(flashColor.a, 0, elapsed / flashDuration);
            danoImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            yield return null;
        }

        danoImage.color = new Color(0, 0, 0, 0);
        flashRoutine = null;
    }
}
