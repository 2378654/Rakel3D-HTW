# Rakel3D


* TO-DOs 10.09.2024

    + checken wegen wand Tracker AUsrichtung (Andere Ausrichtung Probieren)
        - Rotieren auf X-Achse ohne Änderungen möglich
        - X-Achse relevant weil Tracker an Wand montiert werden soll
    + festhalten welcher Abstand benötigt wird wenn Tracker an Wand festgeklebt oder Wand vor Tracker steht
    + checken wegen Rotations Offset, wenn Rakel horizontal
        - Quaternions umrechnen
    + prüfe ob Bewegung immernoch präzise + wenn nicht anpassen
        - Passt alles noch, vermutlich weil Basestations weiterhin an den selben Positionen im selben Winkel stehen
    + checken Device List\
    + innerhalb von Unity Bild für Wall Tracker
    + Prüfen wann der Rakel Tracker zu weit weg ist und von was?
        - Zu weit weg von der Origin (Zentrum der Szene)
        - https://discussions.unity.com/t/assertion-failed-invalid-worldaabb-object-is-too-large-or-too-far-away-from-the-origin/672935

* Auswertung 10.09.2023
    + Projekt wurde mit Personen getestet
        - wurde als Positiv und unterhaltsam angenommen
    + Feedback
        - 2D-Postion auf der Leinwand anzeigen, besser zum Debuggen und nutzerfreundlicher
        - Mesh der virtuellen Rakel ausschalten = vermeidet Verwirrung
        - Umrechnung der Quaternions für die Rotation und Tilt der Rakel
        - Vive Cosmos Elite bzw. Vive Pro 2 wurden verwendet, da diese kompatibel mit dem Valve Lighthouse 2 Tracking sind.
            - https://www.vive.com/eu/accessory/base-station2/
            - 2 Basestations = 5 * 5m 
            - 4 Basestations = 10 * 10m
            - Tracking SetUp nicht auf Tracking an der Wand ausgerichtet sondern eher im Raum, dadurch Tracking Verlust ab und zu möglich
            - Vive Tracker muss fixiert an der Rakel angebracht sein

* TO-DOs 18.09.2024
    + Bilder und Video für Demonstration
        - 2 Videos von Bildschirm und von Wand
    + Z-Offset festlegen
        - Done
    + Rotation und Positionierung anpassen

* 02.10.2024:
    + Rotation Reichweite prüfen (-180 bis 180)
        - auch mit -180 nicht vollständig rotierbar
    + Vielleicht nochmal vollständiges Bild malen?
    + Überprüfen ob nicht nur canvasposition - 0.07 sondern auch rakelposition + 0.07 möglich?
        - möglich
    + Code fertigstellen --> sicherheitshalber Fotos vom Code machen? oder über die Cloud speichern
        - done
    + Bild von Rakel mit korrekter Ausrichtung des Trackers
        - Done


* 04.03.2025:
    + Rotation prüfen --> sollte jetzt passen --> Rotation done
    + InputManager Z Position der Rakel auf und in Configuration auskommentieren und Auto Z Position ausprobieren 
        - Pressure Parameter nutzen --> Done --> Pressure Controller eingebaut
    + transform.rotation.z vs transform.eulerangles.z --> done --> eulerangles.z

* 05.03.2025:
    + Finetune Boxcollider für Buttons -->Done
        - Colors --> 0.05
        - Save & Load --> 0.07
        - Pressure --> 0.001
        - ColorsSelection --> 0.25
        - Scroll --> 0.15
    + Pressure Controller begrenzen 
     -  nicht unter 0 --> Done
     - nicht über 1 --> Done
    + Position der Rakel und des Indikators genauer machen --> Done
    + Anzeige von Entfernung zur Wand anpassen --> Done + Rakel Indikator aus wenn weiter als eine LE weg
    + Überprüfen der neu erstellten Wall --> Done
    + Überblick über gespeicherte Bilder --> Done
    + Videos vom Speichern und der Farbwauswahl --> Done
    + Farbauswahl nach unten schieben oder weniger Farben auf einmal anzeigen. Scroll UP Button zu hoch --> Done nur noch vier Farben auf einmal

* 06.03.2025
    + Probanden für AttrakDiff finden -> Done
    + Bild von Pressure Buttons und gegenüberstellung verschiedener Pressure Werte (3 Bilder vielleicht)

* 18.03.2025
    + Bild von Pressure Buttons und gegenüberstellung verschiedener Pressure Werte (3 Bilder vielleicht)
    + check tilt Value = GameObject.Find("RenderedRakel").transform.eulerAngles.y;
    + check collider für canvas und indikator
    + Anchor Problem angucken, bilder machen



---------------------------------------
## Master

* 23.04.2025
    + Fragen nach anderen Widerständen (bsp 5kOhm)
    + ESP32 S3 Mini und wie verbindet man den?
    + On/Off Schalter
    + Ausprobieren des neuen UIs an der Wand
    + Ausprobieren die Raumvermessung so zu verwenden das kein Offset für X benötigt wird
    + Wie funktioniert das mit zb LEDs? Werden die auf die Platine direkt gelötet?