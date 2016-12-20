#include <FastLED.h>
#include <EEPROM.h>

#define NUM_LEDS 24     //Replace this value with the numbers of leds on your strip. Note, some strips count 3 leds as a single segment, normally the ones operation at 12v
const String Device_Name = "Case Lights"; //**New in 1.4** now you can also assign a unique name to your device in oder to use multiple controllers at once. 
#define DATA_PIN  2     //You can also chage the data pin for your led strip if you so desire

String command = "Off", prevcomd = "Off";
int hue, val = 1, state, pix = 0, rate = 5, back_R, back_G, back_B, fore_R, fore_G, fore_B;
unsigned long prevTime = 0, wait = 100, prevHueTime;
CRGBArray <NUM_LEDS> leds;
String commandList[] = {"Off", "Fixed Color", "Color Fade", "Color Breathe", "Color Chase", "Color Chase Bounce", "Color Cycle", 
                        "Color Switching", "Color Switching Bounce", "Rainbow", "Rainbow Breathe", "Rainbow Chase", 
                        "Rainbow Chase Bounce", "Custom Pattern"};


void setup() { 
  FastLED.addLeds <WS2812, DATA_PIN> (leds, NUM_LEDS); 
  FastLED.setBrightness(70);
  Serial.begin(19200);
  Serial.setTimeout(50);
  //////////////////////////////////////////////////////
  int memValue = EEPROM.read(0);
  command = commandList[memValue];
  prevcomd = command;
  rate = EEPROM.read(1);
  back_R = EEPROM.read(2);
  back_G = EEPROM.read(3);
  back_B = EEPROM.read(4);
  fore_R = EEPROM.read(5);
  fore_G = EEPROM.read(6);
  fore_B = EEPROM.read(7);
  //Serial.println("Cmd:" + command + " RGB1:" + fore_R + "," +  fore_G + "," + fore_B  + " RGB2:" + back_R + "," + back_G + "," + back_B + "," + " Rate:" + rate );
}


void loop(){  
  
  if(Serial.available() > 1){
    char a = ',';
    command = Serial.readStringUntil(a);
    ////////////////////////////////////////////////////////////
    if(command == "id"){
      Serial.print("1.4,addressable," + Device_Name + ",");   //Order:Version, Addressable/Simple, ControllerName, LedNumber
      Serial.print(NUM_LEDS);
      Serial.println("\n");
      command = prevcomd;
    }else{
      if(command == "Color1"){
        SerialConfirm();
        fore_R = Serial.parseInt();
        fore_G = Serial.parseInt();
        fore_B = Serial.parseInt();
        fore_R = constrain(fore_R,0,255);
        fore_G = constrain(fore_G,0,255);
        fore_B = constrain(fore_B,0,255);
        command = prevcomd;        
      }else{
        if(command == "Color2"){
          back_R = Serial.parseInt();
          back_G = Serial.parseInt();
          back_B = Serial.parseInt();
          back_R = constrain(back_R,0,255);
          back_G = constrain(back_G,0,255);
          back_B = constrain(back_B,0,255);
          command = prevcomd;
          SerialConfirm();
        }else{
          if(command == "Rate"){
          rate = Serial.parseInt();
          rate = constrain(rate,1,20);
          SerialConfirm();
          command = prevcomd;      
          }
        }
      }
    }  
    
    if(command == "Pixel"){
      pix = Serial.parseInt();
      pix = constrain(pix, 0, (NUM_LEDS - 1));
      fore_R = Serial.parseInt();
      fore_G = Serial.parseInt();
      fore_B = Serial.parseInt();
      fore_R = constrain(fore_R,0,255);
      fore_G = constrain(fore_G,0,255);
      fore_B = constrain(fore_B,0,255);
      Serial.flush();
      Serial.println("OK\n"); 
    }
    if(command == "Save Default"){
      SerialConfirm();
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
      EEPROM.update(2, back_R);
      EEPROM.update(3, back_G);
      EEPROM.update(4, back_B);
      EEPROM.update(5, fore_R);
      EEPROM.update(6, fore_G);
      EEPROM.update(7, fore_B);
      command = prevcomd;
    }
    
  }
  //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
  unsigned long hueTime =  millis();
    if(hueTime - prevHueTime >= (rate*5)){
      prevHueTime = hueTime;
      hue++;
    }
  
  if(command == "GetData"){
    Serial.println(prevcomd + "," + fore_R + "," +  fore_G + "," + fore_B  + "," + back_R + "," + back_G + "," + back_B + ","
    + rate );
    command = prevcomd;
  }
  
  if(command == "Off"){
    wait = 5;
    unsigned long currentTime =  millis();
    if(currentTime - prevTime >= wait){
      prevTime = currentTime;
      Off();
    }
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
  Serial.println("OK\n");  
}

void Off(){
  leds.fadeToBlackBy(2);
  FastLED.show();
  prevcomd = "Off";
}

void FixedColor(){
  setAllColor1();
  prevcomd = "Fixed Color";
}

void ColorBreathe(){
  int red, green, blue;
  if (val >= 255 || val <= 0){
    state++;
    if (state == 4)
    {
      state = 0;
    }
  }
  switch (state){
  case 0:
    red = map(fore_R, 0, 255, 0, val);
    green = map(fore_G, 0, 255, 0, val);
    blue = map(fore_B, 0, 255, 0, val);
    fill_solid(leds, NUM_LEDS, CRGB(green, red, blue));
    val++;
    break;
  case 1:
    red = map(fore_R, 0, 255, 0, val);
    green = map(fore_G, 0, 255, 0, val);
    blue = map(fore_B, 0, 255, 0, val);
    fill_solid(leds, NUM_LEDS, CRGB(green, red, blue));
    val--;
    break;
  case 2:
    red = map(back_R, 0, 255, 0, val);
    green = map(back_G, 0, 255, 0, val);
    blue = map(back_B, 0, 255, 0, val);
    fill_solid(leds, NUM_LEDS, CRGB(green, red, blue));
    val++;
    break;
  case 3:
    red = map(back_R, 0, 255, 0, val);
    green = map(back_G, 0, 255, 0, val);
    blue = map(back_B, 0, 255, 0, val);
    fill_solid(leds, NUM_LEDS, CRGB(green, red, blue));
    val--;
    break;
  }
  FastLED.show();
  prevcomd = "Color Breathe";
}

void ColorChase(){
  for(int i = 0; i < NUM_LEDS; i++){
    leds[i] = blend(leds[i],CRGB(back_G,back_R,back_B),65);
  }
  if(pix >= NUM_LEDS)
  {
    pix = 0;
  }
  leds[pix] = CRGB(fore_G,fore_R,fore_B);
  FastLED.show();
  pix++;
  prevcomd = "Color Chase";
}


void ColorSwitchBounce(){
  if(state > 3){
    state = 0;
  }  
  switch (state){
    case 0:
      leds[pix] = CRGB(fore_G,fore_R,fore_B);
      pix++;
      break;
    case 1:
      leds[pix] = CRGB(back_G,back_R,back_B);
      pix++;
      break;
    case 2:
      leds[pix] = CRGB(fore_G,fore_R,fore_B);
      pix--;
      break;
    case 3:
      leds[pix] = CRGB(back_G,back_R,back_B);
      pix--;
      break;  
  }
  if(pix <= 0)
  {
    pix = NUM_LEDS;
    state++;
  }
  if(pix >= NUM_LEDS)
  {
    pix = 0;
    state++;
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
      leds[pix] = CRGB(fore_G,fore_R,fore_B);
      pix++;
      break;
    case 1:
      leds[pix] = CRGB(back_G,back_R,back_B);
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
    leds[i] = blend(leds[i],CRGB(back_G,back_R,back_B),65);
  }
  if (pix >= NUM_LEDS){
    state = 1;
    pix = NUM_LEDS -1;
  }
  if (pix <= 0){
    state = 0;
    pix = 0;
  }
  if(state == 0){
    pix++;
  }else{
    pix--;
  }
  leds[pix] = CRGB(fore_G,fore_R,fore_B);
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
    if (state == 4)
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
  case 2:
    fill_solid(leds, NUM_LEDS, CHSV(hue,255,val));
    val++;
    break;
  case 3:
    fill_solid(leds, NUM_LEDS, CHSV(hue,255,val));
    val--;
    break;
  }
  FastLED.show();
  prevcomd = "Rainbow Breathe";
}

void RainbowChase(){
  for(int i = 0; i < NUM_LEDS; i++){
    leds[i] = blend(leds[i],CRGB(back_G,back_R,back_B),65);
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
    leds[i] = blend(leds[i],CRGB(back_G,back_R,back_B),65);
  }
  if (pix >= NUM_LEDS){
    state = 1;
    pix = NUM_LEDS -1;
  }
  if (pix <= 0){
    state = 0;
    pix = 0;
  }
  if(state == 0){
    pix++;
  }else{
    pix--;
  }
  leds[pix] = CHSV(hue, 255, 255);
  FastLED.show();  
  prevcomd = "Rainbow Chase Bounce";
}

void ColorCycle(){
  fill_solid(leds, NUM_LEDS, CHSV(hue,255,200));
  FastLED.show();
  prevcomd = "Color Cycle";
}

void ColorFade(){
  if (val >= 300 || val <= 0){
    state++;
    if (state >= 2)
    {
      state = 0;
    }
  }
  switch (state){
  case 0:
    for(int i = 0; i < NUM_LEDS; i++){
      //Serial.println("Before" + " RGB1:" + fore_R + "," +  fore_G + "," + fore_B  + " Pix::" + leds[i].red + "," + leds[i].green + "," + leds[i].blue );
      if(fore_R > leds[i].green) leds[i].green++;
      if(fore_R < leds[i].green) leds[i].green--;
      ////////////////////////////////////////////////////////////
      if(fore_G > leds[i].red) leds[i].red++;
      if(fore_G < leds[i].red) leds[i].red--;
      ////////////////////////////////////////////////////////////
      if(fore_B > leds[i].blue) leds[i].blue++;
      if(fore_B < leds[i].blue) leds[i].blue--;
      //Serial.println("after" + " RGB1:" + fore_R + "," +  fore_G + "," + fore_B  + " Pix::" + leds[i].red + "," + leds[i].green + "," + leds[i].blue );
    }
    val++;
    break;
  case 1:
    for(int i = 0; i < NUM_LEDS; i++){
      if(back_R > leds[i].green) leds[i].green++;
      if(back_R < leds[i].green) leds[i].green--;
      ////////////////////////////////////////////////////////////
      if(back_G > leds[i].red) leds[i].red++;
      if(back_G < leds[i].red) leds[i].red--;
      ////////////////////////////////////////////////////////////
      if(back_B > leds[i].blue) leds[i].blue++;
      if(back_B < leds[i].blue) leds[i].blue--;
    }
    val--;
    break;
  }
  FastLED.show();
  prevcomd = "Color Fade";
}

void setAllColor1(){
  for(int i = 0; i < NUM_LEDS; i++){
    leds[i].setRGB(fore_G, fore_R, fore_B);
  }  
  FastLED.show();
}

void setAllColor2(){
  for (int i = 0; i < NUM_LEDS; i++){
    leds[i].setRGB(back_G, back_R, back_B);
  }
  FastLED.show();
}

void SinglePixel(){
  leds[pix].setRGB(fore_G, fore_R, fore_B);
  FastLED.show();
  prevcomd = "";
}

