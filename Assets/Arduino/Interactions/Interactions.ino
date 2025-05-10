//SaveAndLoad
#include <ezButton.h>
#define BUTTON_NUM 6

#define SAVE_BUTTON1 6  
#define LOAD_BUTTON1 7  
#define SAVE_BUTTON2 8  
#define LOAD_BUTTON2 9  
#define SAVE_BUTTON3 10  
#define LOAD_BUTTON3 11

ezButton buttonArray[] = {
  ezButton(SAVE_BUTTON1),
  ezButton(LOAD_BUTTON1),
  ezButton(SAVE_BUTTON2),
  ezButton(LOAD_BUTTON2),
  ezButton(SAVE_BUTTON3),
  ezButton(LOAD_BUTTON3)
};

//FSR
int oldRaw = 0;
const int fsrPin = A3;
const float minVal = 0;  
const float maxVal = 550; 

//Color
int oldValue = 0;
const int colorpin = A0;
const int PIN_RED   = 3;
const int PIN_GREEN = 4;
const int PIN_BLUE  = 5;
int oldsegment;

//CSB
const int CSB_PIN = 12;  
const int LED_PIN =  13;   
int buttonStateCSB = 0; 
int on = 1;

//ReapplyColor
const int reapplyPin = 2; 
int buttonStateReapply = 0;

//ClearRakel
const int clearPin = A4; 
int buttonStateClear = 0;

//Rakel Length
int oldLength = 0;
const int lengthPin = A1;

//Paint Volume
const int volumePin = A2;
int oldVolume = 0;

void setup() {
  pinMode(reapplyPin, INPUT_PULLUP);
  pinMode(LED_PIN, OUTPUT);
  pinMode(CSB_PIN, INPUT_PULLUP);
  for (byte i = 0; i < BUTTON_NUM; i++) {
    buttonArray[i].setDebounceTime(100);  // set debounce time to 100 milliseconds
  }
  Serial.begin(115200);
  while(!Serial){}

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

void SaveAndLoad(){
  for (byte i = 0; i < BUTTON_NUM; i++) {
    buttonArray[i].loop();
    int button_state = buttonArray[i].getState(); 


    if (buttonArray[i].isPressed()) {
      if (i == 1 || i == 3 || i == 5){
        Serial.print("Load");
        Serial.println(i/2 + 1);
      }
      else if (i == 0 || i == 2 || i == 4){
        Serial.print("Save");
        Serial.println(i/2 + 1);
      }
    }
  }
}

void Pressure(){
  int raw = analogRead(fsrPin);
  
  raw = constrain(raw, minVal, maxVal);

  if(raw != oldRaw){
    Serial.print("Pressure");
    Serial.println(raw);
    delay(100);
    oldRaw = raw;
  } 
  
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
        Serial.print("CSB");
        Serial.println(1);
      }
      else{
        digitalWrite(LED_PIN, LOW);
        Serial.print("CSB");
        Serial.println(0);
      }
    delay(200);
  }
}

void ReapplyColor(){
  buttonStateReapply = digitalRead(reapplyPin);

  if (buttonStateReapply == HIGH) {
    Serial.println("Reapply");
    delay(200);
  }
}

void ClearRakel(){
  buttonStateClear = digitalRead(clearPin);

  if (buttonStateClear == HIGH) {
    Serial.println("Clear");
    delay(200);
  }
}

void Length(){
  int lengthValue = analogRead(lengthPin);
  int Length = lengthValue / 31;
  
  if(lengthValue > oldLength+5 || lengthValue < oldLength -5){
    Serial.print("Length");
    Serial.println(Length);
    oldLength = lengthValue;
  }
}

void Volume(){
  int volumeValue = analogRead(volumePin);
  int Volume = volumeValue / 2;
  
  if(volumeValue > oldVolume+5 || volumeValue < oldVolume -5){
    Serial.print("Volume");
    Serial.println(Volume);
    oldVolume = volumeValue;
  }
}


void Color(){
  int analogValue = analogRead(colorpin);
  int segment = analogValue / 44.48;
  // println out the value you read:
  if(analogValue > oldValue+5 || analogValue < oldValue -5){
    //Serial.write(analogValue);
    Serial.print("Color");
    Serial.println(segment);
    oldValue = analogValue;
  } 
  /*if(segment != oldsegment){
    switch(segment) {
      case 0:
        //Serial.println("CadmiumGreen\n");
        analogWrite(PIN_RED,   0);
        analogWrite(PIN_GREEN, 107);
        analogWrite(PIN_BLUE,  60);
        break;
      case 1:
        //Serial.println("Green\n");
        analogWrite(PIN_RED,   5);
        analogWrite(PIN_GREEN, 145);
        analogWrite(PIN_BLUE,  10);
        break;
      case 2:
        //Serial.println("CadmiumLightGreen\n");
        analogWrite(PIN_RED,   128);
        analogWrite(PIN_GREEN, 181);
        analogWrite(PIN_BLUE,  46);
        break;
      case 3:
        //Serial.println("LemonYellow\n");
        analogWrite(PIN_RED,   254);
        analogWrite(PIN_GREEN, 242);
        analogWrite(PIN_BLUE,  80);
        break;
      case 4:
        //Serial.println("CadmiumYellow\n");
        analogWrite(PIN_RED,   255);
        analogWrite(PIN_GREEN, 246);
        analogWrite(PIN_BLUE,  0);
        break;
      case 5:
        //Serial.println("CadmiumOrange\n");
        analogWrite(PIN_RED,   237);
        analogWrite(PIN_GREEN, 135);
        analogWrite(PIN_BLUE,  45);
        break;
      case 6:
        //Serial.println("DarkOrange\n");
        analogWrite(PIN_RED,   255);
        analogWrite(PIN_GREEN, 71);
        analogWrite(PIN_BLUE,  26);
        break;
      case 7:
        //Serial.println("CadmiumRed\n");
        analogWrite(PIN_RED,   227);
        analogWrite(PIN_GREEN, 0);
        analogWrite(PIN_BLUE,  34);
        break;
      case 8:
        //Serial.println("DarkRed\n");
        analogWrite(PIN_RED,   148);
        analogWrite(PIN_GREEN, 15);
        analogWrite(PIN_BLUE,  0);
        break;
      case 9:
        //Serial.println("Chocolate\n");
        analogWrite(PIN_RED,   123);
        analogWrite(PIN_GREEN, 63);
        analogWrite(PIN_BLUE,  0);
        break;
      case 10:
        //Serial.println("Bordeaux\n");
        analogWrite(PIN_RED,   134);
        analogWrite(PIN_GREEN, 45);
        analogWrite(PIN_BLUE,  89);
        break;
      case 11:
        //Serial.println("Rose\n");
        analogWrite(PIN_RED,   246);
        analogWrite(PIN_GREEN, 152);
        analogWrite(PIN_BLUE,  215);
        break;
    }
    oldsegment = segment;
    
    
  }*/
  delay(100);
}