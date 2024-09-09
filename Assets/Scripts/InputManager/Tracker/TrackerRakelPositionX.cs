using UnityEngine;
public class TrackerRakelPositionX : FloatValueSource
{
    
    public override void Update()
    {
        //Value = (GameObject.Find("RenderedRakel").transform.position.x);
        //Value = (GameObject.Find("RenderedRakel").transform.position.x + 0.06f) * 15f; //Offset und Mult wenn Tracker an oberer Ecke
        Value = (GameObject.Find("RenderedRakel").transform.position.x - 0.04f) * 15f; //Neue Berechnung
        //Value = (GameObject.Find("RenderedRakel").transform.position.x + 0.18f) * 15f; //Neue Berechnung 2.0
    }
}