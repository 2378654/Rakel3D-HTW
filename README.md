# Rakel3D
Das Projekt simuliert die Rakel-Technik unter Verwendung einer realen Rakel (Zeicheninstrument) an einer realen Wand. Das Gemalte wird durch einen Projektor auf eine Wand projiziert.

## Anforderungen
+ Unity3D (Verwendete Version: 2022.3.8f1)
+ SteamVR
+ Lighthouse Tracking
    - Vier Lighthouses (Basestations) 
    - Eine Vive Pro bzw. eine Vive Cosmos Elite verwendet --> Wird nicht beim Nutzen des Projekts getragen. Muss lediglich verbunden sein, um das Lighthouse Tracking zu ermöglichen.

+ Der Rakel Controller beinhaltet:
    - Zwei Vive Tracker 3.0
    - PowerBank zur Stromversorgung des eigentlichen Controllers
    - Controller (ESP32 Mikrocontroller mit Eingabeelementen)
    - Rakel als Malwerkzeug modifiziert mit einer weichen Kante, um die Wand nicht zu schädigen

+ Dongle, um die Daten des Controllers an Unity weiterzuleiten: ESP32 per USB an Computer angeschlossen

## Innerhalb des Projekts
+ Anpassen der virtuellen Rakelkante:
    - GameObject "LineRenderer" in der Szene 
    
     ![Offset X und Y sowie Mult X und Y bei Bedarf anpassen](images/LineRendererMultAndOffset.png).
    - Die angepassten Werte müssen nochmal in TrackerPositionX.cs und TrackerPositionY.cs angepasst werden.
+ Anpassen der Entfernung zur Wand:
    - In TrackerStrokeState.cs bei Bedarf den Offset anpassen (Momentan 1.11f)
    - Offset muss bei Änderung ebenfalls bei DistanceToCanvas.cs angepasst werden

