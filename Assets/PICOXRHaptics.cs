
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class PICOXRHaptics : MonoBehaviour
{
    public float hoverDuration = 1f;
    public float inputDuration = 2f;
    public float vibrationStrength = 0.8f; // Strength from 0 to 1

    private void Start()
    {
        // Find all buttons in the scene and add event listeners dynamically
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            AddHapticEvents(button);
        }
    }

    private void AddHapticEvents(Button button)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();

        // Add hover event
        EventTrigger.Entry hoverEntry = new EventTrigger.Entry();
        hoverEntry.eventID = EventTriggerType.PointerEnter;
        hoverEntry.callback.AddListener((data) => TriggerHaptics(hoverDuration));
        trigger.triggers.Add(hoverEntry);

        // Add click event
        EventTrigger.Entry clickEntry = new EventTrigger.Entry();
        clickEntry.eventID = EventTriggerType.PointerClick;
        clickEntry.callback.AddListener((data) => TriggerHaptics(inputDuration));
        trigger.triggers.Add(clickEntry);
    }

    private void TriggerHaptics(float duration)
    {
        Debug.Log("Triggering haptic feedback");
        StartCoroutine(TriggerHapticFeedback(duration));
    }

    private IEnumerator TriggerHapticFeedback(float duration)
    {
        InputDevice leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        InputDevice rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        float time = 0f;
        while (time < duration)
        {
            if (leftHand.isValid)
                leftHand.SendHapticImpulse(0, vibrationStrength, 0.1f);

            if (rightHand.isValid)
                rightHand.SendHapticImpulse(0, vibrationStrength, 0.1f);

            time += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}