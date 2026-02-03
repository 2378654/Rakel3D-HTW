# Rakel3D



This project simulates the squeegee technique using a real squeegee (drawing instrument) on a real wall.  

The painting is projected onto a wall by a projector.  



The project uses a **multi-layer color model** taken from the previous project by Brzoska (2023).  



---



## Requirements



- **Unity3D**  

  - Version used: `2022.3.8f1`



- **SteamVR startup instructions**  

  - When starting SteamVR, wait until **all four base stations** are connected.  

  - Then:  

    1. Turn on the upper tracker (above the power bank)  

    2. Turn on the lower tracker  

  - If the devices are paired in a different order, the device numbers for the **“Top”** and **“Bottom”** GameObjects may need to be adjusted.



- **Lighthouse Tracking**  

  - Four [Lighthouses (base stations)](https://www.vive.com/de/accessory/base-station2/)  

  - One [Vive Pro](https://www.vive.com/de/product/vive-pro2/overview/) or one **Vive Cosmos Elite**  

    - **Note:** The glasses are not worn, but are only needed for connection so that Lighthouse Tracking works.



- **Calibration**: SteamVR's room measurement is used to calibrate the setup.



- **Rakel controller** (homemade)  

  - Two [Vive Trackers 3.0](https://www.vive.com/de/accessory/tracker3/)

- PowerBank for power supply

- ESP32 microcontroller with input elements

- Squeegee (painting tool), modified with a soft edge to protect the wall



- **Dongle**

- ESP32 connected to the computer via USB to forward the data to Unity





- **System overview**

(Insert diagram)



---

## Project control



### Switching between controllers

- Select GameObject **“Interaction”**  

- Click the checkbox to activate/deactivate the wall controller  



### Adjusting the virtual squeegee edge

- Select the **“LineRenderer”** GameObject in the scene  

- Adjust the **Offset X/Y** and **Mult X/Y** parameters as needed  



![Adjust Offset X and Y as well as Mult X and Y as needed](images/LineRendererMultAndOffset.png)



### Adjusting the distance to the wall

- Select the **“DistanceController”** GameObject.

- Adjust the **Canvas Offset**.



![Adjust the distance to the canvas as needed](images/DistanceController.png)



- **Troubleshooting**

  - If the virtual representation makes jerky movements, this could be due to reflective surfaces. Care should be taken to cover all reflective surfaces.



- **Operation**

The operation differs depending on the controller used:

- Squeegee controller (settings are made on the squeegee using the squeegee controller created)

    - Canvas height and width or squeegee length and amount of paint on the squeegee using the slider potentiometers “Squeegee Length (Width)” and “Amount of Paint (Height)”

- Confirm the set canvas size and refill the squeegee using “Refill (Done)”
- 
    - Color selection via rotary potentiometer `Color`

- Undo the last “stroke” via `Undo`

- Empty squeegee with `Clear Squeegee`

- Empty canvas with `Clear Canvas`

- Save an image with `Save 1,2,3`

    - Load an image using `Load 1,2,3`

- Switch between light and heavy blurring using `Light Mixing`

- Pressure is detected by two pressure sensors

- Wall controller (all settings are made on the wall)

    - `Height + / -` and `Width + / -` to adjust the canvas size

- Open all available functions using the `UI` button

- Color selection using 23 different buttons (one button per color)

- Undo the last “stroke” using `Undo`

- Switch between light and heavy blurring using `Light Mixing`

- Empty the squeegee using `Clear Squeegee`

- Empty the canvas using `Clear Canvas`

    - `Squeegee Length` and `Amount of Paint` sliders to adjust the squeegee length and amount of paint

- `Pressure + / -` to adjust the pressure

- Switch between color selection and memory management with `Save and Load / Colors`

- Scroll through the colors with `^` and `v`
