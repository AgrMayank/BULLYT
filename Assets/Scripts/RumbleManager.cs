
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Haptics;
using UnityEngine.InputSystem.XInput;

public class RumbleManager : MonoBehaviour
{
    public static RumbleManager Instance;
    private Gamepad pad;

    private Coroutine stopRumbleAfterTimeCoroutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void RumblePulse(float lowFrequency = 0.75f, float highFrequency = 0.25f, float duration = 0.5f)
    {
        // Get reference to our Gamepad
        pad = Gamepad.current;

        // Set LightBar color to PS Controllers if present
        // if (DualSenseGamepadHID.current != null)
        //     DualSenseGamepadHID.current.SetLightBarColor(Color.green);

        // if we have a current Gamepad
        if (pad != null)
        {
            pad.SetMotorSpeeds(lowFrequency, highFrequency);

            Debug.Log("RUMBLE");

            // stop the rumble after a certain amount of time
            stopRumbleAfterTimeCoroutine = StartCoroutine(StopRumble(duration, pad));
        }
    }

    private IEnumerator StopRumble(float duration, Gamepad pad)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;

            //once our duration is over
            pad.SetMotorSpeeds(0f, 0f);
        }
    }
}