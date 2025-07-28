/*
  Rui Santos & Sara Santos - Random Nerd Tutorials
  Complete project details at https://RandomNerdTutorials.com/esp-now-esp32-arduino-ide/  
  Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files.
  The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
*/

#include <esp_now.h>
#include <WiFi.h>

uint8_t broadcastAddress[] = {0x00,0x4b,0x12,0x3d,0x70,0xf4}; //{0x00,0x4b,0x12,0x3d,0x70,0xf4} //{0x00, 0x4B, 0x12, 0x3e,0x67,0xbc}

// Structure example to receive data
// Must match the sender structure
typedef struct struct_message {
    char a[64];
} struct_message;

// Create a struct_message called myData
struct_message myData;
 
esp_now_peer_info_t peerInfo;

void setup() {
  // Initialize Serial Monitor
  Serial.begin(115200);
  
  // Set device as a Wi-Fi Station
  WiFi.mode(WIFI_STA);

  // Init ESP-NOW
  if (esp_now_init() != ESP_OK) {
    Serial.println("Error initializing ESP-NOW");
    return;
  }

  memcpy(peerInfo.peer_addr, broadcastAddress, 6);
  peerInfo.channel = 0;
  peerInfo.encrypt = false;
  
  // Once ESPNow is successfully Init, we will register for recv CB to
  // get recv packer info
  esp_now_register_recv_cb(esp_now_recv_cb_t(OnDataRecv));

  if (esp_now_add_peer(&peerInfo) != ESP_OK){
    Serial.println("Failed to add peer");
    return;
  }
}
 
void loop() {
  if (Serial.available()) {
    String incoming = Serial.readStringUntil('\n');
    incoming.trim();
    if (incoming.length() > 0) {
      incoming.toCharArray(myData.a, sizeof(myData.a));
      if(incoming == "Reset"){
        SendToReceiver("Reset");
      }
    }
  }
}

// callback function that will be executed when data is received
void OnDataRecv(const uint8_t * mac, const uint8_t *incomingData, int len) {
  memcpy(&myData, incomingData, sizeof(myData));
  Serial.println(myData.a);
}
 
void SendToReceiver(const char* message){
  strncpy(myData.a, message, sizeof(myData.a) - 1);
  myData.a[sizeof(myData.a) - 1] = '\0';
  esp_now_send(broadcastAddress, (uint8_t *) &myData, sizeof(myData));
}

void OnDataSent(const uint8_t *mac_addr, esp_now_send_status_t status) {
  Serial.print("Last Packet Send Status:\t");
  Serial.println(status == ESP_NOW_SEND_SUCCESS ? "Delivery Success" : "Delivery Fail");
}

