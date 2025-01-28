using UnityEngine;

public class Clock : MonoBehaviour
{
    public Transform hourHand;    // Points down by default
    public Transform minuteHand;  // Points up by default
    public Transform secondHand;  // Points up by default

    [Header("Predefined Start Time")]
    [Range(0, 23)] public int startHour = 12;
    [Range(0, 59)] public int startMinute = 0;
    [Range(0, 59)] public int startSecond = 0;

    private float elapsedTime;
    bool wincon = false;
    public Wincon winning;
    public int winconSeconds = 0;
    float survivedSeconds = 0;
    float originalTaskInterval;

    void Start()
    {
        // Calculate the total elapsed seconds since midnight for the predefined time
        elapsedTime = startHour * 3600f + startMinute * 60f + startSecond;
        originalTaskInterval = TaskManager.Instance.randomTaskInterval;
    }

    void Update()
    {
        // Update elapsed time in seconds
        elapsedTime += Time.deltaTime;
        survivedSeconds += Time.deltaTime;

        // Calculate the current hour, minute, and second
        float currentHour = (elapsedTime / 3600f) % 24f;
        float currentMinute = (elapsedTime / 60f) % 60f;
        float currentSecond = elapsedTime % 60f;

        // Calculate the rotation angles
        float secondsAngle = currentSecond * 6f; // 360 degrees / 60 seconds
        float minutesAngle = ((currentMinute-2) * 6f) + (currentSecond / 10f); // Minute + partial
        float hoursAngle = (currentHour % 12 * 30f) + (currentMinute / 2f); // Hour + partial

        // Adjust for initial orientations
        const float hourHandOffset = 180f; // Down points to 180°
        const float otherHandsOffset = 0f; // Up points to 0°

        // Rotate the hands
        if (secondHand != null)
            secondHand.localRotation = Quaternion.Euler(0f, 0f, -secondsAngle + otherHandsOffset);
        if (minuteHand != null)
            minuteHand.localRotation = Quaternion.Euler(0f, 0f, -minutesAngle + otherHandsOffset);
        if (hourHand != null)
            hourHand.localRotation = Quaternion.Euler(0f, 0f, -hoursAngle + hourHandOffset);

        if ((survivedSeconds > winconSeconds) && !wincon)
        {
            wincon = true;
            winning.Winnered();
        }

        AdjustRandomTaskInterval(survivedSeconds, winconSeconds);
    }


    public void AdjustRandomTaskInterval(float clockElapsedTime, float winningTime)
    {
        float percentElapsed = (clockElapsedTime / winningTime) * 100f;
        float newInterval = originalTaskInterval;

        if (percentElapsed >= 100f)
        {
            newInterval = 1000f;
        }
        else if (percentElapsed >= 95f)
        {
            newInterval = newInterval * 0.1f;
        }
        else if (percentElapsed >= 90f)
        {
            newInterval = newInterval * 0.2f;
        }
        else if (percentElapsed >= 80f)
        {
            newInterval = newInterval * 0.6f;
        }
        else if (percentElapsed >= 70f)
        {
            newInterval = newInterval * 0.3f;
        }
        else if (percentElapsed >= 60f)
        {
            newInterval = newInterval * 0.9f;
        }
        else if (percentElapsed >= 50f)
        {
            newInterval = newInterval * 0.5f;
        }
        else if (percentElapsed >= 40f)
        {
            newInterval = newInterval * 0.8f;
        }
        else if (percentElapsed >= 30f)
        {
            newInterval = newInterval * 0.5f;
        }
        else if (percentElapsed >= 20f)
        {
            newInterval = newInterval * 0.9f;
        }
        else if (percentElapsed >= 10f)
        {
            newInterval = newInterval * 0.9f;
        }

        TaskManager.Instance.randomTaskInterval = newInterval;
    }

}
