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
            