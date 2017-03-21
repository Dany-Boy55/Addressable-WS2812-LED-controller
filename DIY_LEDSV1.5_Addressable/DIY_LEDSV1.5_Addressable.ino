#include <FastLED.h>
#include <EEPROM.h>

#define NUM_LEDS 80     //Replace this value with the numbers of leds on your strip. Note, some strips count 3 leds as a single segment, normally the ones operation at 12v
const String Device_Name = "Case Lights"; //**New in 1.4** now you can also assign a unique name to your device in oder to use multiple controllers at once. 
#define DATA_PIN  2     //You can also chage the data pin for your led strip if you so desire

#define BUTTON_PIN 3    //Choose which pin you want to use to connect a button to manually change the effect of the LEDS

String command = "Off", prevcomd = "Off";
CRGB foreColor, backColor, calcColor;
int hue, val = 1, state, pix = 0, rate = 5;
unsigned long prevTime = 0, wait = 100, prevHueTime, prevButTime;
CRGBArray <NUM_LEDS> leds;
const String commandList[] = {"Off", "Fixed Color", "Color Fade", "Color Breathe", "Color Chase", "Color Chase Bounce", "Color Cycle", 
                        "Color Switching", "Color Switching Bounce", "Rainbow", "Rainbow Breathe", "Rainbow Chase", "Color Rain", 
                        "Rainbow Chase Bounce", "Color Gradient", "Custom Pattern", "Pixel"};


void setup() { 
  FastLED.addLeds <WS2812, DATA_PIN> (leds, NUM_LEDS); 
  FastLED.setBrightness(100);
  Serial.begin(19200);
  Serial.setTimeout(50);
  pinMode(BUTTON_PIN, INPUT_PULLUP);
  //////////////////////////////////////////////////////
  int commandIndex = EEPROM.read(0);
  command = commandList[commandIndex];
  prevcomd = command;
  int rate = EEPROM.read(1);
  foreColor.red = EEPROM.read(2);
  foreColor.green = EEPROM.read(3);
  foreColor.blue = EEPROM.read(4);
  backColor.red = EEPROM.read(5);
  backColor.green = EEPROM.read(6);
  backColor.blue = EEPROM.read(7);  
}


void loop(){  
  
  if(Serial.available() > 0){
    char a = ',';
    String input = Serial.readStringUntil(a);
    
    for(int i = 0; i < sizeof(commandList); i++){
      if(input == commandList[i]){
        command = input;
        SerialConfirm();
      }
    }
    ////////////////////////////////////////////////////////////
    if(input == "id"){
      Serial.print("1.4,addressable," + Device_Name + ",");   //Order:Version, Addressable/Simple, ControllerName, LedNumber
      Serial.print(NUM_LEDS);
      Serial.println("\n");
      command = prevcomd;
    }else{
      if(input == "Color1"){
        SerialConfirm();
        foreColor.red = Serial.parseInt();
        foreColor.green = Serial.parseInt();
        foreColor.blue = Serial.parseInt();        
      }else{
        if(input == "Color2"){
          SerialConfirm();
          backColor.red = Serial.parseInt();
          backColor.green = Serial.parseInt();
          backColor.blue = Serial.parseInt();
        }else{
          if(input == "Rate"){
            SerialConfirm();
            rate = Serial.parseInt();
            rate = constrain(rate,1,20);             
          }
        }
      }
    }
    
    if(input == "Read"){
      Serial.println(prevcomd + "," + foreColor.red + "," +  foreColor.green + "," + foreColor.blue  + "," + backColor.red + "," + backColor.green + "," + backColor.blue + ","
      + rate + "\n");
      command = prevcomd;
    }
    
    if(command == "Pixel"){
      pix = Serial.parseInt();
      pix = constrain(pix, 0, NUM_LEDS);
      int r = Serial.parseInt();
      int g = Serial.parseInt();
      int b = Serial.parseInt();
      r = constrain(r,0,255);
      g = constrain(g,0,255);
      b = constrain(b,0,255);
      Serial.flush();
      Serial.println("OK\n"); 
    }
    
    if(input == "Save Default"){
      SerialConfirm();
      command = prevcomd;
      int memVal;
      for(int i = 0; i < sizeof(commandList); i++){
        if(prevcomd == commandList[i]){
          memVal = i;
          break;
        }else{
          memVal = 0;
        }
      }
      EEPROM.update(0, memVal);
      EEPROM.update(1, rate);
      EEPROM.update(2, foreColor.red);
      EEPROM.update(3, foreColor.green);
      EEPROM.update(4, foreColor.blue);
      EEPROM.update(5, backColor.red);
      EEPROM.update(6, backColor.green);
      EEPROM.update(7, backColor.blue);
    }
    Serial.flush();
  }
  //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  unsigned long hueTime =  millis();
    if(hueTime - prevHueTime >= (rate*20)){
      prevHueTime = hueTime;
      hue++;
    }

  unsigned long buttonTime =  millis();
    if(buttonTime - prevButTime >= 200){
      prevButTime = buttonTime;
      if(!digitalRead(BUTTON_PIN)){
        for(int i = 0; i < sizeof(commandList); i++){
          if(command == commandList[i]){
            command = commandList[i+1];
            break;
          }          
        }
        if(command == "Custom Pattern"){
          command = "Off";
        }
      }
    }
  
  if(command == "Off"){
    wait = 5;
    unsigned long currentTime =  millis();
    if(currentTime - prevTime >= wait){
      prevTime = currentTime;
      Off();
    }
  }

  if(command == "Custom Pattern"){
    fill_solid(leds, NUM_LEDS, CRGB(0,0,0));
  }
  
  if(command == "Fixed Color"){
    FixedColor();
  }

  if(command == "Color Breathe"){
    wait = rate;
    unsigned long currentTime =  millis();
    if(currentTime - prevTime >= wait){
      prevTime = currentTime;
      ColorBreathe();
    }
  }

  if(command == "Color Chase"){
    wait = rate*15;
    unsigned long currentTime =  millis();
    if(currentTime - prevTime >= wait){
      prevTime = currentTime;
      ColorChase();
    }
  }

  if(command == "Color Rain"){
    wait = rate*6;
    unsigned long currentTime =  millis();
    if(currentTime - prevTime >= wait){
      prevTime = currentTime;
      ColorRain();
    }
  }

  if(command == "Color Fade"){
    wait = rate*3;
    unsigned long currentTime =  millis();
    if(currentTime - prevTime >= wait){
      prevTime = currentTime;
      ColorFade();
    }
  }

  if(command == "Color Chase Bounce"){
    wait = rate*15;
    unsigned long currentTime =  millis();
    if(currentTime - prevTime >= wait){
      prevTime = currentTime;
      ColorChaseBounce();
    }    
  }
  
  if(command == "Color Switching"){
    wait = rate*15;
    unsigned long currentTime =  millis();
    if(currentTime - prevTime >= wait){
      prevTime = currentTime;
      ColorSwitch();
    }
  }

  if(command == "Color Switching Bounce"){
    wait = rate*15;
    unsigned long currentTime =  millis();
    if(currentTime - prevTime >= wait){
      prevTime = currentTime;
      ColorSwitchBounce();
    }
  }

  if(command == "Color Gradient"){
    ColorGradient();
  }
  
  if(command == "Rainbow"){
    wait = rate*10;
    unsigned long currentTime =  millis();
    if(currentTime - prevTime >= wait){
      prevTime = currentTime;
      Rainbow();
    }
  }

  if(command == "Rainbow Breathe"){
    wait = rate;
    unsigned long currentTime =  millis();
    if(currentTime - prevTime >= wait){
      prevTime = currentTime;
      RainbowBreathe();
    }
  }
  
  if(command == "Rainbow Chase"){
    wait = rate*15;
    unsigned long currentTime =  millis();
    if(currentTime - prevTime >= wait){
      prevTime = currentTime;
      RainbowChase();
    }
  }
  
  if(command == "Rainbow Chase Bounce"){
    wait = rate*15;
    unsigned long currentTime =  millis();
    if(currentTime - prevTime >= wait){
      prevTime = currentTime;
      RainbowChaseBounce();
    }
  }
  
  if(command == "Color Cycle"){
    wait = rate*10;
    unsigned long currentTime =  millis();
    if(currentTime - prevTime >= wait){
      prevTime = currentTime;
      ColorCycle();
    }
  }

  if(command == "Pixel"){
    SinglePixel();
  }
}    

////////////////////////////////////////////////////////////////////////////////////////////////////////

void SerialConfirm(){
  state = 0;
  pix = 0;
  Serial.flush(); 
  Serial.print("OK\n");  
}

void ColorGradient(){
  for(int i = 0; i < NUM_LEDS; i++){
    leds[i] = blend(backColor, foreColor, i*10);
  }
  FastLED.show();
  prevcomd = "Color Gradient";
}

void Off(){
  leds.fadeToBlackBy(2);
  FastLED.show();
  prevcomd = "Off";
}

void FixedColor(){
  fill_solid(leds, NUM_LEDS, foreColor);
  FastLED.show();
  prevcomd = "Fixed Color";
}

void ColorBreathe(){
  if (val >= 100){
    state++;
    val = 0;
    if (state == 4)
    {
      state = 0;
    }
  }
  
  switch (state){
    case 0:
      for(int i = 0; i < NUM_LEDS; i++){
        leds[i] = blend(leds[i],foreColor,5);
      }
      break;
    case 1:
      for(int i = 0; i < NUM_LEDS; i++){
        leds[i].fadeToBlackBy(5);
      }
      break;
    case 2:
      for(int i = 0; i < NUM_LEDS; i++){
        leds[i] = blend(leds[i],backColor,5);
      }
      break;
    case 3:
      for(int i = 0; i < NUM_LEDS; i++){
        leds[i].fadeToBlackBy(5);
      }
      break;
  }
  val++;
  FastLED.show();
  prevcomd = "Color Breathe";
}

void ColorChase(){
  for(int i = 0; i < NUM_LEDS; i++){
    leds[i] = blend(leds[i],backColor,65);
  }
  if(pix >= NUM_LEDS)
  {
    pix = 0;
  }
  leds[pix] = foreColor;
  FastLED.show();
  pix++;
  prevcomd = "Color Chase";
}


void ColorSwitchBounce(){
  if(state > 3){
    state = 0;
  }
  if(pix <= 0)
  {
    pix = NUM_LEDS - 1;
    state++;
  }
  if(pix >= NUM_LEDS)
  {
    pix = 0;
    state++;
  }
  switch (state){
    case 0:
      leds[pix] = foreColor;
      pix++;
      break;
    case 1:
      leds[pix] = backColor;
      pix++;
      break;
    case 2:
      leds[pix] = foreColor;
      pix--;
      break;
    case 3:
      leds[pix] = backColor;
      pix--;
      break;  
  }  
  FastLED.show();
  prevcomd = "Color Switching Bounce";
}

void ColorSwitch(){
  if(state > 1){
    state = 0;
  }  
  switch (state){
    case 0:
      leds[pix] = foreColor;
      pix++;
      break;
    case 1:
      leds[pix] = backColor;
      pix++;
      break;   
  }
  if(pix >= NUM_LEDS)
  {
    pix = 0;
    state++;
  }
  FastLED.show();
  prevcomd = "Color Switching";
}

void ColorChaseBounce(){
  for(int i = 0; i < NUM_LEDS; i++){
    leds[i] = blend(leds[i],backColor,65);
  }
  switch(state){
    case 0:
      pix++;
      break;
    case 1:
      pix--;
      break;
  }
  if (pix >= NUM_LEDS){
    state = 1;
    pix = NUM_LEDS -1;
  }
  if (pix <= 0){
    state = 0;
    pix = 0;
  }  
  leds[pix] = foreColor;
  FastLED.show();  
  prevcomd = "Color Chase Bounce";
}

void Rainbow(){
  fill_rainbow(leds, NUM_LEDS, hue, 5);
  FastLED.show();
  hue++;
  prevcomd = "Rainbow";
}

void RainbowBreathe(){
  if (val >= 255 || val <= 0){
    state++;
    if (state == 2)
    {
      state = 0;
    }
  }
  switch (state){
  case 0:
    fill_solid(leds, NUM_LEDS, CHSV(hue,255,val));
    val++;
    break;
  case 1:
    fill_solid(leds, NUM_LEDS, CHSV(hue,255,val));
    val--;
    break;
  }
  FastLED.show();
  prevcomd = "Rainbow Breathe";
}

void RainbowChase(){
  for(int i = 0; i < NUM_LEDS; i++){
    leds[i] = blend(leds[i],backColor,65);
  }
  if(pix >= NUM_LEDS)
  {
    pix = 0;
  }
  leds[pix] = CHSV(hue, 255, 255);
  FastLED.show();
  pix++;
  prevcomd = "Rainbow Chase";
}

void RainbowChaseBounce(){
  for(int i = 0; i < NUM_LEDS; i++){
    leds[i] = blend(leds[i],backColor,65);
  }
  switch(state){
    case 0:
      pix++;
      break;
    case 1:
      pix--;
      break;
  }
  if (pix >= NUM_LEDS){
    state = 1;
    pix = NUM_LEDS -1;
  }
  if (pix <= 0){
    state = 0;
    pix = 0;
  }
  leds[pix] = CHSV(hue, 255, 255);
  FastLED.show();  
  prevcomd = "Rainbow Chase Bounce";
}

void ColorRain(){
  for(int i = 0; i < NUM_LEDS; i++){
    leds[i] = blend(leds[i],backColor,20);
  }
  pix = random(0, NUM_LEDS);
  leds[pix] = foreColor;
  FastLED.show();
  prevcomd = "Color Rain";
}

void ColorCycle(){
  fill_solid(leds, NUM_LEDS, CHSV(hue,255,200));
  FastLED.show();
  prevcomd = "Color Cycle";
}

void ColorFade(){
  if (val >= 256){
    state++;
    val = 0;
    if (state >= 2)
    {
      state = 0;
    }
  }
  switch(state){
    case 0:
      for(int i = 0; i < NUM_LEDS; i++){
        leds[i] = blend(leds[i],foreColor,5);
      }
      break;
    case 1:
      for(int i = 0; i < NUM_LEDS; i++){
        leds[i] = blend(leds[i],backColor,5);
      }
    break;
  }
  val++;
  FastLED.show();
  prevcomd = "Color Fade";
}

void SinglePixel(){
  leds[pix] = foreColor;
  FastLED.show();
  prevcomd = "";
}

