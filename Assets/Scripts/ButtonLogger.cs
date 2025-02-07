using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonLogger : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject clickSource = eventData.pointerPress;

        if (clickSource != null)
        {
            Debug.Log("Button clicked by: " + clickSource.name);
        }

        // Get the input device
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevices(devices);

        foreach (var device in devices)
        {
            if (device.isValid)
            {
                Debug.Log("Device name: " + device.name + " | Role: " + device.characteristics);
            }
        }
    }

}

