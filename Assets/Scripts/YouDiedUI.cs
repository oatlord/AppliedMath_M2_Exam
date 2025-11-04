using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class YouDiedUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image fadeOverlay;
    [SerializeField] private TMP_Text youDiedText;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private float holdDuration = 2.5f;

    private CanvasGroup group;

    private void Awake()
    {
        if (fadeOverlay != null)
            fadeOverlay.color = new Color(0, 0, 0, 0);
        if (youDiedText != null)
            youDiedText.color = new Color(youDiedText.color.r, youDiedText.color.g, youDiedText.color.b, 0);
    }

    public IEnumerator PlayDeathSequence(System.Action onComplete = null)
    {
        // Fade to black
        yield return FadeOverlay(0, 1, fadeDuration);

        onComplete?.Invoke();

        // Show “You Died” text
        yield return FadeText(0, 1, 0.8f);
        yield return new WaitForSeconds(holdDuration);

        // Fade out both
        yield return FadeText(1, 0, 0.8f);
        yield return FadeOverlay(1, 0, fadeDuration);

    }

    private IEnumerator FadeOverlay(float from, float to, float duration)
    {
        if (fadeOverlay == null) yield break;
        Color c = fadeOverlay.color;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float a = Mathf.Lerp(from, to, t / duration);
            fadeOverlay.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }
        fadeOverlay.color = new Color(c.r, c.g, c.b, to);
    }

    private IEnumerator FadeText(float from, float to, float duration)
    {
        if (youDiedText == null) yield break;
        Color c = youDiedText.color;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float a = Mathf.Lerp(from, to, t / duration);
            youDiedText.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }
        youDiedText.color = new Color(c.r, c.g, c.b, to);
    }
}
