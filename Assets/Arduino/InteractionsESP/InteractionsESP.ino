#include <esp_now.h>
#include <WiFi.h>

#define BUTTON_NUM 6

uint8_t broadcastAddress[] = {0x00, 0x4B, 0x12, 0x3D, 0x70, 0xf4}; //00 4b 12 53 1b 54

typedef struct struct_message {
  char a[64];  
} struct_message;

struct_message data;
esp_now_peer_info_t peerInfo;

char msg[64];

// Pins
const int SAVE1_PIN = 21, SAVE2_PIN = 19, SAVE3_PIN = 18;
const int LOAD1_PIN = 23, LOAD2_PIN = 17, LOAD3_PIN = 16;
const int saveNloadPins[BUTTON_NUM] = {SAVE1_PIN, LOAD1_PIN, SAVE2_PIN, LOAD2_PIN, SAVE3_PIN, LOAD3_PIN};
bool lastButtonStates[BUTTON_NUM] = {HIGH,HIGH,HIGH,HIGH,HIGH,HIGH};

const int fsrPin = 33;
const int colorpin = A0;  // 36
const int CSB_PIN = 22, LED_PIN = 15;
const int reapplyPin = 13, clearPin = 4;
const int lengthPin = 34, volumePin = 35;
const int undoPin =5;
const int clearCanvasPin = 27;
//const int sizePin = 39;
const int fsrPin2 = 32;
int oldPressure, oldPressure2, oldColor, oldLength, oldVolume, oldSize, oldZone, oldCanvasWidth, oldCanvasHeight = -1;
int on = 1;

unsigned long lastSendPressure = 0;
unsigned long lastSendColor = 0;
unsigned long lastSendVolume = 0;
unsigned long lastSendLength = 0;
unsigned long lastSendSize = 0;
unsigned long lastSendFormatA = 0;
unsigned long lastSendFormatB = 0;
const unsigned long interval = 200;

int sizeDone =0; //used to check if Size of canvas can be adjusted. Only possible at the Start of the programm

void setup() {
  Serial.begin(115200);
  WiFi.mode(WIFI_STA);

  pinMode(reapplyPin, INPUT_PULLUP);
  pinMode(clearPin, INPUT_PULLUP);
  pinMode(undoPin, INPUT_PULLUP);
  pinMode(clearCanvasPin, INPUT_PULLUP);
  pinMode(LED_PIN, OUTPUT);
  pinMode(CSB_PIN, INPUT_PULLUP);
  for (int i = 0; i < BUTTON_NUM; i++) pinMode(saveNloadPins[i], INPUT_PULLUP);

  if (esp_now_init() != ESP_OK) {
    Serial.println("Error initializing ESP-NOW");
    return;
  }

  esp_now_register_send_cb(OnDataSent);

  memcpy(peerInfo.peer_addr, broadcastAddress, 6);
  peerInfo.channel = 0;
  peerInfo.encrypt = false;

  if (esp_now_add_peer(&peerInfo) != ESP_OK){
    Serial.println("Failed to add peer");
    return;
  }
}

void loop() {
  SaveAndLoad();
  //Pressure();
  Color();
  CSB();
  ReapplyColor();
  ClearRakel();
  Length();
  Volume();
  Undo();
  ClearCanvas();
  //Size();
  //FormatA();
  //FormatB();
}

void SendToReceiver(const char* message){
  strncpy(data.a, message, sizeof(data.a) - 1);
  data.a[sizeof(data.a) - 1] = '\0';
  esp_now_send(broadcastAddress, (uint8_t *) &data, sizeof(data));
}

void SaveAndLoad() {
  for (byte i = 0; i < BUTTON_NUM; i++) {
    bool currentState = digitalRead(saveNloadPins[i]);
    if (lastButtonStates[i] == HIGH && currentState == LOW) {
      snprintf(msg, sizeof(msg), (i % 2 == 0 ? "Save%d" : "Load%d"), i/2 + 1);
      SendToReceiver(msg);
    }
    lastButtonStates[i] = currentState;
  }
}

void Pressure() {
  unsigned long now = millis();
  if (now - lastSendPressure >= interval) {
    int raw = analogRead(fsrPin);
    int raw2 = analogRead(fsrPin2);
    if (abs(raw - oldPressure) > 20 || abs(raw2- oldPressure2) > 20) {
      int rawAvg = (raw + raw2)/2;
      snprintf(msg, sizeof(msg), "Pressure%d", rawAvg);
      SendToReceiver(msg);
      oldPressure = raw;
      oldPressure2 = raw2;
    }
    lastSendPressure = now;
  }
}

const int BOUND = 80;
int zoneSize = 4095 / 22;

void Color() {
  unsigned long now = millis();
  if (now - lastSendColor >= interval) {
    int value = analogRead(colorpin);
    //int color = map(value, 0, 4095, 0, 22);
    int currentZone = value / zoneSize;
    if (currentZone != oldZone && abs(value - (oldZone * zoneSize)) > BOUND) {
      snprintf(msg, sizeof(msg), "Color%d", currentZone);
      SendToReceiver(msg);
      oldZone = currentZone;
    }
    lastSendColor = now;
  }
}

void CSB() {
  static bool lastState = HIGH;
  int state = digitalRead(CSB_PIN);
  if (state == LOW && lastState == HIGH) {
    on = !on;
    digitalWrite(LED_PIN, on ? LOW : HIGH);
    snprintf(msg, sizeof(msg), on ? "CSB0" : "CSB1");
    SendToReceiver(msg);
    delay(250);  
  }
  lastState = state;
}

void ReapplyColor() {
  static bool lastState = HIGH;
  bool state = digitalRead(reapplyPin);
  if (state == HIGH && lastState == LOW) {
    SendToReceiver("Reapply");
    delay(250);
  }
  lastState = state;
}

void ClearRakel() {
  static bool lastState = HIGH;
  bool state = digitalRead(clearPin);
  if (state == LOW && lastState == HIGH) {
    SendToReceiver("Clear");
    delay(250);
  }
  lastState = state;
}

void Undo(){
  static bool lastState = HIGH;
  bool state = digitalRead(undoPin);
  if (state == LOW && lastState == HIGH) {
    SendToReceiver("Undo");
    delay(250);
  }
  lastState = state;
}

void ClearCanvas(){
  static bool lastState = HIGH;
  bool state = digitalRead(clearCanvasPin);
  if (state == LOW && lastState == HIGH) {
    if (sizeDone == 0){
        sizeDone = 1;
    }
    else{
    SendToReceiver("Canvas");
    delay(250);
    }
  }
  lastState = state;
}

void Length() {
  unsigned long now = millis();
  if (now - lastSendLength >= interval) {
    int val = analogRead(lengthPin);
    if(sizeDone == 0){
      int canvasWidth = map(val, 0, 4095, 1, 8);
      if (canvasWidth != oldCanvasWidth) {
        snprintf(msg, sizeof(msg), "Width%d", canvasWidth);
        SendToReceiver(msg);
        oldCanvasWidth = canvasWidth;
      }
    }
    else{
      int length = map(val, 0, 4095, 2, 15);
      if (length != oldLength) {
        snprintf(msg, sizeof(msg), "Length%d", length);
        SendToReceiver(msg);
        oldLength = length;
      }
      lastSendLength = now;
    }
  }
}

void Volume() {
  unsigned long now = millis();
  if (now - lastSendVolume >= interval) {
    int val = analogRead(volumePin);
    if(sizeDone == 0){
      int canvasHeight = map(val, 0, 4095, 1, 8);
      if (canvasHeight != oldCanvasHeight) {
        snprintf(msg, sizeof(msg), "Height%d", canvasHeight);
        SendToReceiver(msg);
        oldCanvasHeight = canvasHeight;
      }
    }
    else{
      int volume = map(val, 0, 4095, 60, 600);
      if (abs(volume - oldVolume) > 5) {
        snprintf(msg, sizeof(msg), "Volume%d", volume);
        SendToReceiver(msg);
        oldVolume = volume;
      }
      lastSendVolume = now;
    }
  }
}

void OnDataSent(const uint8_t *mac_addr, esp_now_send_status_t status) {
  Serial.print("Last Packet Send Status:\t");
  Serial.println(status == ESP_NOW_SEND_SUCCESS ? "Delivery Success" : "Delivery Fail");
}

/*void Size(){
  unsigned long now = millis();
  if (now - lastSendSize >= interval) {
    int val = analogRead(sizePin);
    int size = map(val, 0, 4095, 2, 8);
    if (size != oldSize) {
      snprintf(msg, sizeof(msg), "Size%d", size);
      SendToReceiver(msg);
      oldSize = size;
    }
    lastSendSize = now;
  }
}
*/
/*
void FormatA(){
  unsigned long now = millis();
  if (now - lastSendFormatA >= interval) {
    int val = analogRead(FormatAPin);
    int formatA = map(val, 0, 4095, 2, 8);
    if (formatA != oldFormatA) {
      snprintf(msg, sizeof(msg), "FormatA%d", formatA);
      SendToReceiver(msg);
      oldFormatA = formatA;
    }
    lastSendFormatA = now;
  }
}

void FormatB(){
  unsigned long now = millis();
  if (now - lastSendFormatB >= interval) {
    int val = analogRead(FormatBPin);
    int formatB = map(val, 0, 4095, 2, 8);
    if (formatB != oldFormatB) {
      snprintf(msg, sizeof(msg), "FormatB%d", formatB);
      SendToReceiver(msg);
      oldFormatB = formatB;
    }
    lastSendFormatB = now;
  }
}
*/

