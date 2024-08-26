using UnityEngine;

public class TrackerRakelTilt : FloatValueSource
{
    public override void Update()
    {

        Value = (GameObject.Find("RenderedRakel").transform.rotation.y) * (-90f); 
    }
    
    /*private FrameStopwatch FrameStopwatch;
    private float TILT_STEP_PER_SECOND = 50;

    public TrackerRakelTilt()
    {
        FrameStopwatch = new FrameStopwatch();
    }

    public override void Update()
    {
        FrameStopwatch.Update();

        if (Input.GetKey(KeyCode.A))
        {
            Value += FrameStopwatch.SecondsSinceLastFrame * TILT_STEP_PER_SECOND;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Value -= FrameStopwatch.SecondsSinceLastFrame * TILT_STEP_PER_SECOND;
        }

        Value = Rakel.ClampTilt(Value);
    }*/
    
}