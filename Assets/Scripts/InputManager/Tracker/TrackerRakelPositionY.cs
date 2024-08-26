using UnityEngine;
public class TrackerRakelPositionY : FloatValueSource
{
    public override void Update()
    {
        //Value = (GameObject.Find("RenderedRakel").transform.position.y);
        //Value = (GameObject.Find("RenderedRakel").transform.position.y - 1.62f) * 17f; //Offset und Mult wenn Tracker an oberer Ecke
        Value = (GameObject.Find("RenderedRakel").transform.position.y - 1.52f) * 16f; //Offset und Mult wenn Tracker in der Mitter des Rakel
        //Value = (GameObject.Find("RenderedRakel").transform.position.y - 1.76f) * 17f; //Neue Berechnung
    }
}
