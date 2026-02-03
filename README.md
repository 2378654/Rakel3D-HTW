# Rakel3D

This project simulates the **squeegee technique** using a real squeegee (drawing instrument) on a real wall.

The painting is projected onto the wall using a **projector**.

The project is based on a **multi-layer color model** taken from a previous project by [Brzoska(2023)](https://github.com/donatbrzoska/Layers).

---

## Requirements

### Software
- **Unity3D**
  - Version used: `2022.3.8f1`

### SteamVR Startup Instructions
1. Start SteamVR and wait until **all four base stations** are connected.
2. Turn on the trackers in the following order:
   1. Upper tracker (above the power bank)
   2. Lower tracker

> ⚠️ If devices are paired in a different order, the device numbers of the **“Top”** and **“Bottom”** GameObjects may need to be adjusted.

### Lighthouse Tracking
- Four [Lighthouses (Base Stations 2.0)](https://www.vive.com/de/accessory/base-station2/)
- One of the following headsets:
  - [Vive Pro](https://www.vive.com/de/product/vive-pro2/overview/)
  - **Vive Cosmos Elite**

> **Note:**  
> The headset is **not worn**, but required to establish the Lighthouse Tracking connection.

### Calibration
- SteamVR room setup is used for system calibration.

### Rakel (Squeegee) Controller
- Homemade controller consisting of:
  - Two [Vive Trackers 3.0](https://www.vive.com/de/accessory/tracker3/)
  - Power bank for power supply
  - ESP32 microcontroller with input elements
  - Modified squeegee with a soft edge to protect the wall

### Connectivity
- **Dongle**
- ESP32 connected via **USB** to forward data to Unity

### System Overview
*(Insert diagram here)*

---

## Project Control

### Switching Between Controllers
- Select the **“Interaction”** GameObject
- Toggle the checkbox to activate or deactivate the wall controller

---

### Adjusting the Virtual Squeegee Edge
- Select the **“LineRenderer”** GameObject
- Adjust the following parameters:
  - `Offset X / Y`
  - `Mult X / Y`

![Adjust Offset X and Y as well as Mult X and Y as needed](images/LineRendererMultAndOffset.png)

---

### Adjusting the Distance to the Wall
- Select the **“DistanceController”** GameObject
- Adjust the **Canvas Offset**

![Adjust the distance to the canvas as needed](images/DistanceController.png)

---

## Operation

The operation differs depending on the controller being used.

---

### Squeegee Controller
(All settings are made directly on the squeegee controller)

- Adjust canvas size or squeegee parameters using sliders:
  - **Squeegee Length (Width)**
  - **Amount of Paint (Height)**
- Confirm canvas size and refill squeegee using:
  - `Refill (Done)`
- Color selection via rotary potentiometer:
  - `Color`
- Undo last stroke:
  - `Undo`
- Empty squeegee:
  - `Clear Squeegee`
- Clear canvas:
  - `Clear Canvas`
- Save images:
  - `Save 1`, `Save 2`, `Save 3`
- Load images:
  - `Load 1`, `Load 2`, `Load 3`
- Toggle blur mode:
  - `Light Mixing` (light / heavy)
- Pressure is detected via **two pressure sensors**

---

### Wall Controller
(All settings are made on the wall UI)

- Adjust canvas size:
  - `Height + / -`
  - `Width + / -`
- Open full UI:
  - `UI`
- Color selection:
  - 23 individual color buttons
- Undo last stroke:
  - `Undo`
- Toggle blur mode:
  - `Light Mixing`
- Empty squeegee:
  - `Clear Squeegee`
- Clear canvas:
  - `Clear Canvas`
- Adjust squeegee parameters:
  - `Squeegee Length`
  - `Amount of Paint`
- Adjust pressure:
  - `Pressure + / -`
- Switch modes:
  - `Save and Load / Colors`
- Scroll through colors:
  - `^` and `v`

---

## Troubleshooting

- If the virtual representation shows **jerky movements**, reflective surfaces may interfere with tracking.
- Make sure **all reflective surfaces are covered**.
