//ESP-NOW
#include <esp_now.h>
#include <WiFi.h>


#define BUTTON_NUM 6

//Receiver
uint8_t broadcastAddress[] = {0x00, 0x4B, 0x12, 0x53, 0x1B, 0x54};

//Struct to send
typedef struct struct_message {
  char a[32];
} struct_message;

//Send message
char msg[32];


struct_message data;

esp_now_peer_info_t peerInfo;


void OnDataSent(const uint8_t *mac_addr, esp_now_send_status_t status) {
  Serial.print("\r\nLast Packet Send Status:\t");
  Serial.println(status == ESP_NOW_SEND_SUCCESS ? "Delivery Success" : "Delivery Fail");
}

//SaveNLoad
//Save
const int SAVE1_PIN = 21;
const int SAVE2_PIN = 19;
const int SAVE3_PIN = 18;
//Load
const int LOAD1_PIN = 23;
const int LOAD2_PIN = 17;
const int LOAD3_PIN = 16;
const int saveNloadPins[BUTTON_NUM] = {SAVE1_PIN, LOAD1_PIN, SAVE2_PIN, LOAD2_PIN, SAVE3_PIN, LOAD3_PIN};
bool lastButtonStates[BUTTON_NUM] = {HIGH,HIGH,HIGH,HIGH,HIGH,HIGH};

//FSR
int oldPressure = 0;
const int fsrPin = 33; //26
unsigned long lastPressureSent = 0;
const unsigned int pressureTimeout = 200;

//Color
int oldValue = 0;
const int colorpin = A0;//34; //33
int oldColor;

//CSB
const int CSB_PIN = 22;  
const int LED_PIN =  4; 
int buttonStateCSB = 0; 
int on = 1;

//ReapplyColor
const int reapplyPin = 13; 
int buttonStateReapply = 0;

//ClearRakel
const int clearPin = 32; 
int buttonStateClear = 0;

//Rakel Length
int oldLength = 0;
const int lengthPin = 34;

//Paint Volume
const int volumePin = 35;
int oldVolume = 0;

void setup() {
  pinMode(reapplyPin, INPUT_PULLUP);
  pinMode(clearPin, INPUT_PULLUP);
  pinMode(LED_PIN, OUTPUT);
  pinMode(CSB_PIN, INPUT_PULLUP);
  for (int i = 0; i < BUTTON_NUM; i++) {
    pinMode(saveNloadPins[i], INPUT_PULLUP); 
  }
  WiFi.mode(WIFI_STA);
  Serial.begin(115200);
  while(!Serial){}

  if (esp_now_init() != ESP_OK) {
    Serial.println("Error initializing ESP-NOW");
    return;
  }

  esp_now_register_send_cb(OnDataSent);

  //Register peer
  memcpy(peerInfo.peer_addr, broadcastAddress, 6);
  peerInfo.channel = 0;
  peerInfo.encrypt = false;

  //Add peer
  if (esp_now_add_peer(&peerInfo) != ESP_OK){
    Serial.println("Failed to add peer");
    return;
  }
  
}

void loop() {
  Pressure();
  Color();
  CSB();
  ReapplyColor();
  SaveAndLoad();
  Length();
  Volume();
  ClearRakel();
}

void SendToReceiver(const char* message){
  strncpy(data.a, message,sizeof(data.a) - 1);
  data.a[sizeof(data.a) - 1] = '\0';

  esp_err_t result = esp_now_send(broadcastAddress, (uint8_t *) &data, sizeof(data));

  if (result == ESP_OK) {
  Serial.println("Sent with success");
  }
  else {
    Serial.println("Error sending the data");
  }
  delay(100);
}

void SaveAndLoad(){
  
  for (byte i = 0; i < BUTTON_NUM; i++) {
    bool currentState = digitalRead(saveNloadPins[i]);

    if (lastButtonStates[i] == HIGH && currentState == LOW) {
      if (i % 2 == 1) { // 1, 3, 5 → Load
        snprintf(msg, sizeof(msg), "Load%d", i/2 + 1);
        SendToReceiver(msg);
      } else { // 0, 2, 4 → Save
        snprintf(msg, sizeof(msg), "Save%d", i/2 + 1);
        SendToReceiver(msg);
      }
    }
    lastButtonStates[i] = currentState;
  }
  delay(100);
}

void Pressure(){
  int raw = analogRead(fsrPin);
  int value = map(raw,0,4095,0,1000);
  if(value != oldPressure){
    snprintf(msg, sizeof(msg), "Pressure%d", value);
    SendToReceiver(msg);
  }
  delay(100);
}

void CSB(){
  buttonStateCSB = digitalRead(CSB_PIN);
  if(buttonStateCSB == LOW){         
    if (on == 0){
        on = 1;
        delay(250);
      }
      else{
        on = 0;
        delay(250);
      }
      if (on == 0){
        digitalWrite(LED_PIN, HIGH);
        snprintf(msg, sizeof(msg), "CSB1");
        SendToReceiver(msg);
      }
      else{
        digitalWrite(LED_PIN, LOW);
        snprintf(msg, sizeof(msg), "CSB0");
        SendToReceiver(msg);
      }
  }
  delay(100);
}

void ReapplyColor(){
  buttonStateReapply = digitalRead(reapplyPin);

  if (buttonStateReapply == LOW) {
    snprintf(msg, sizeof(msg), "Reapply");
    SendToReceiver(msg);
  }
  delay(100);
}

void ClearRakel(){
  buttonStateClear = digitalRead(clearPin);

  if (buttonStateClear == LOW) {
    snprintf(msg, sizeof(msg), "Clear");
    SendToReceiver(msg);
  }
  delay(100);
}

void Length(){
  int lengthValue = analogRead(lengthPin);
  int length = map(lengthValue,0,4095,2,15);
  if(length != oldLength){
    snprintf(msg, sizeof(msg), "Length%d",length);
    SendToReceiver(msg);
    oldLength = length;
  }
  delay(100);
}

void Volume(){
  int volumeValue = analogRead(volumePin);
  int volume = map(volumeValue,0,4095,60,600);
  if(volume < oldVolume - 5 || volume > oldVolume + 5){
    snprintf(msg, sizeof(msg), "Volume%d", volume);
    SendToReceiver(msg);
    oldVolume = volume;
  }
  delay(100);
}


void Color(){
  int colorValue = analogRead(colorpin);
  int color = map(colorValue,0,4095,0,22);
  if(color != oldColor){
    snprintf(msg, sizeof(msg), "Color%d", color);
    SendToReceiver(msg);
    oldColor = color;
  }
  delay(100);
}