using UnityEngine;
public class TrackerRakelPositionX : FloatValueSource
{
    
    public override void Update()
    {
        //Value = (GameObject.Find("RenderedRakel").transform.position.x + 0.41f) * 15f; //Testaufbau
        Value = (GameObject.Find("RenderedRakel").transform.position.x - 0.02f) * 4.9f; //vor Ort
    }
}