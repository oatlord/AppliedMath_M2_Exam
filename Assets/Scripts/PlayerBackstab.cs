using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class PlayerBackstab : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform enemy;
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private Animator swordAnimator;
    [SerializeField] private PlayerControls playerControls;

    [Header("Backstab Settings")]
    [SerializeField] private float detectionRange = 2.5f;
    [SerializeField] private float angleThreshold = 60f;
    [SerializeField] private float messageResetTime = 2f;

    [Header("Scene Management")]
    public GameObject sceneManagerObject;
    public String sceneToLoad;

    private bool canBackstab;
    private bool hasBackstabbed;
    private bool messageLocked;
    private Coroutine textAnimRoutine;
    private Coroutine messageRoutine;

    private const string DefaultText = "Press V or Left Click to Perform \na Backstab behind the Enemy!";

    public void SetPromptActive(bool active)
    {
        if (promptText != null)
            promptText.enabled = active;
    }

    private void Start()
    {
        if (promptText) promptText.text = DefaultText;
    }

    private void Update()
    {
        if (!enemy || hasBackstabbed) return;

        // Check if player is behind and close enough
        Vector3 dirToPlayer = (transform.position - enemy.position).normalized;
        float angle = Vector3.Angle(enemy.forward, dirToPlayer);
        float distance = Vector3.Distance(transform.position, enemy.position);
        canBackstab = angle < angleThreshold && distance <= detectionRange;

        if (promptText && !messageLocked)
        {
            Color color = canBackstab ? Color.yellow : Color.white;
            AnimatePrompt(DefaultText, color);
        }

        if ((Input.GetKeyDown(KeyCode.V) || Input.GetMouseButtonDown(0)) && !hasBackstabbed)
        {
            swordAnimator.SetTrigger("hasAttacked");
            if (canBackstab)
            {
                hasBackstabbed = true;
                ShowMessage("Backstab Successful!", true);
                Debug.Log("Backstab successful!");
            }
            else
            {
                ShowMessage("Attack Failed!", false);
                Debug.Log("Not behind the enemy — cannot backstab!");
            }
        }
    }

    private void ShowMessage(string message, bool successful)
    {
        if (messageRoutine != null) StopCoroutine(messageRoutine);
        messageLocked = true;

        if (promptText)
            AnimatePrompt(message, successful ? Color.green : Color.red);

        if (successful)
        {
            if (playerControls != null)
                playerControls.enabled = false;
            if (swordAnimator != null)
                swordAnimator.enabled = false;
            messageRoutine = StartCoroutine(ChangeSceneAfterDelay(2f));
        }
        else
        {
            messageRoutine = StartCoroutine(ResetMessage());
        }
    }

    private IEnumerator ChangeSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (sceneManagerObject != null)
        {
            SceneManager sceneManager = sceneManagerObject.GetComponent<SceneManager>();
            if (sceneManager != null)
            {
                sceneManager.GoToScene(sceneToLoad);
            }
        }
    }

    private IEnumerator ResetMessage()
    {
        yield return new WaitForSeconds(messageResetTime);
        if (promptText)
        {
            yield return FadeTextAlpha(1f, 0f, 0.3f);
            promptText.text = DefaultText;
            yield return FadeTextAlpha(0f, 1f, 0.35f);
        }
        messageLocked = false;
    }

    private void AnimatePrompt(string newText, Color color)
    {
        if (!promptText) return;
        if (textAnimRoutine != null) StopCoroutine(textAnimRoutine);
        textAnimRoutine = StartCoroutine(FadeAndPulseText(newText, color));
    }

    private IEnumerator FadeAndPulseText(string newText, Color color)
    {
        float duration = 0.15f;
        // Fade out
        yield return FadeTextAlpha(1f, 0f, duration, 0.95f);
        promptText.text = newText;
        promptText.color = new Color(color.r, color.g, color.b, 0f);
        // Fade in and pulse
        yield return FadeTextAlpha(0f, 1f, duration, 1.1f, color);
        promptText.color = new Color(color.r, color.g, color.b, 1f);
        promptText.transform.localScale = Vector3.one;
    }

    private IEnumerator FadeTextAlpha(float from, float to, float duration, float pulse = 1f, Color? color = null)
    {
        Color baseColor = color ?? promptText.color;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, t / duration);
            float scale = 1f + Mathf.Sin((t / duration) * Mathf.PI) * (pulse - 1f);
            promptText.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
            promptText.transform.localScale = Vector3.one * scale;
            yield return null;
        }
    }
}
