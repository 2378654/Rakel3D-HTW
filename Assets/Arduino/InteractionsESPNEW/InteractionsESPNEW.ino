#include <esp_now.h>
#include <WiFi.h>

#define BUTTON_NUM 6

uint8_t broadcastAddress[] = {0x00, 0x4B, 0x12, 0x53, 0x1B, 0x54}; //dev. {0x00, 0x4B, 0x12, 0x53, 0x1B, 0x54} //prod. {0x00, 0x4B, 0x12, 0x3D, 0x70, 0xf4}

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

bool sizeDone = false; //used to check if Size of canvas can be adjusted. Only possible at the Start of the programm

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

  esp_now_register_recv_cb(esp_now_recv_cb_t(OnDataRecv));
}

void loop() {
  SaveAndLoad();
  Pressure();
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
      snprintf(msg, sizeof(msg), (i % 2 == 0 ? "Save%d" : "Load%d"), i/2 + 1);  // 1,3,5 --> Load | 2,4,6 --> Save
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
    if (sizeDone == false){
        sizeDone = true;
    }
    SendToReceiver("Canvas");
    delay(250);
  }
  lastState = state;
}

void Length() {
  unsigned long now = millis();
  if (now - lastSendLength >= interval) {
    int val = analogRead(lengthPin);
    if(sizeDone == false){
      int canvasWidth = map(val, 0, 4095, 1, 10);
      if (canvasWidth != oldCanvasWidth) {
        snprintf(msg, sizeof(msg), "Width%d", canvasWidth);
        SendToReceiver(msg);
        oldCanvasWidth = canvasWidth;
      }
      lastSendLength = now;
    }
    else{
      int length = map(val, 0, 4095, 2, 15);
      if (length != oldLength) {
        snprintf(msg, sizeof(msg), "Length%d", length);
        SendToReceiver(msg);
        oldLength = length;
      }
      
    }
  }
}

void Volume() {
  unsigned long now = millis();
  if (now - lastSendVolume >= interval) {
    int val = analogRead(volumePin);
    if(sizeDone == false){
      int canvasHeight = map(val, 0, 4095, 1, 10);
      if (canvasHeight != oldCanvasHeight) {
        snprintf(msg, sizeof(msg), "Height%d", canvasHeight);
        SendToReceiver(msg);
        oldCanvasHeight = canvasHeight;
      }
      lastSendVolume = now;
    }
    else{
      int volume = map(val, 0, 4095, 60, 256);
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

void OnDataRecv(const uint8_t * mac, const uint8_t *incomingData, int len) {
  memcpy(&data, incomingData, sizeof(data));
  Serial.println(data.a);
  //Check if reset is send
  if (strcmp(data.a, "Reset") == 0){
    sizeDone = false;
  }
}


