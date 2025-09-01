# Rakel3D

Dieses Projekt simuliert die Rakel-Technik unter Verwendung einer realen Rakel (Zeicheninstrument) an einer realen Wand.  
Das Gemalte wird durch einen Projektor auf eine Wand projiziert.  

Das Projekt nutzt ein **Mehrschicht-Farbmodell**, das aus dem vorherigen Projekt von Brzoska (2023) übernommen wurde.  

---

## Anforderungen

- **Unity3D**  
  - Verwendete Version: `2022.3.8f1`
  - Verwendete Pakete einfügen

- **SteamVR Startanleitung**  
  - Beim Starten von SteamVR warten, bis **alle vier Basisstationen** verbunden sind.  
  - Danach:  
    1. Oberen Tracker (über der Powerbank) einschalten  
    2. Unteren Tracker einschalten  
  - Falls die Geräte in anderer Reihenfolge gekoppelt werden, müssen bei den GameObjects **"Top"** und **"Bottom"** die Device-Nummern ggf. angepasst werden.

- **Lighthouse Tracking**  
  - Vier [Lighthouses (Basestations)](https://www.vive.com/de/accessory/base-station2/)  
  - Eine [Vive Pro](https://www.vive.com/de/product/vive-pro2/overview/) oder eine **Vive Cosmos Elite**  
    - **Hinweis:** Die Brille wird nicht getragen, sondern nur zur Verbindung benötigt, damit das Lighthouse Tracking funktioniert.

- **Kalibrierung**: Zur Kalibrierung des Setups wird die Raumvermessung von SteamVR verwendet.

- **Rakel-Controller** (Eigenbau)  
  - Zwei [Vive Tracker 3.0](https://www.vive.com/de/accessory/tracker3/)  
  - PowerBank zur Stromversorgung  
  - ESP32 Mikrocontroller mit Eingabeelementen  
  - Rakel (Malwerkzeug), modifiziert mit einer weichen Kante zum Schutz der Wand  

- **Dongle**  
  - ESP32 per USB am Computer angeschlossen, um die Daten an Unity weiterzuleiten  


- **Systemübersicht**
(Diagramm einfügen)

---
## Projektsteuerung

### Umschalten zwischen den Controllern
- GameObject **"Interaction"** auswählen  
- Checkbox anklicken zum Aktivieren/Deaktivieren des Wand-Controllers  

### Anpassen der virtuellen Rakelkante
- GameObject **"LineRenderer"** in der Szene auswählen  
- Parameter **Offset X/Y** sowie **Mult X/Y** nach Bedarf anpassen  

![Offset X und Y sowie Mult X und Y bei Bedarf anpassen](images/LineRendererMultAndOffset.png)

### Anpassen der Entfernung zur Wand
- GameObject **"DistanceController"** auswählen  
- **Canvas Offset** anpassen (Standardwert: `1.27`)  

![Abstand zur Leinwand bei Bedarf anpassen](images/DistanceController.png)

- **Fehlerbehebung**
  - Wenn die virtuelle Repräsentation ruckartige Bewegungen macht, könnte das an reflektierenden Oberflächen liegen. Es sollte darauf geachtet werden, alle reflektierenden Oberflächen abzudecken.

- **Bedienung**
  - Wand-Controller
    - Aufklappen aller verfügbaren Funktion über den Button `UI`
  - Rakel-Controller
    - alle verfügbaren Funktionen sind auf dem erstellten Rakel-Controller
    - Leinwandhöhe und Breite bzw. Rakellänge und Farbmenge auf der Rakel über die Slider Potentiometer `Squeegee Length (Width)` und `Amount of Paint (Height)`
    - Bestätigen der eingestellen Leinwandgröße und wiederbefüllen der Rakel über `Refill (Done)`
    - Rückgängig machen des letzten "Striches" über `Undo`
    - Rakel leeren mit `Clear Squeegee`
    - Leinwand leeren mit `Clear Canvas`
    - Speichern eines Bildes mit `Save 1,2,3`
    - Laden eines Bildes mit `Load 1,2,3`
    - Umschalten zwischen leichter und starker Verwischung über `Light Mixing`
---



