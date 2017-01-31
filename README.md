# DIY-Smart-LEDs
An arduino + windows project to control addressable led strips with ease. Instructables coming soon

If your intent is to simply download and use, then you want to get the .ino sketch for the arduino and the App.zip folder. 
Install the FastLED library into your arduino IDE and upload the sketch. Make necessary adjustments to the sketch.
Open the .exe file within the App folder and you are ready to go!

For those of you interested in the code, then the Visual Studio folder contains all the resources you need to open the solution on MS visual studio. Feel free to modify the app to your liking.

New features on V1.4:                                                                                                 
-Added the ability to connect to several LED controllers, not just the first one the app finds                                      
-Added a new lighting effect: Color Rain. Random "dropplets" that fading into the backgorund color  								
-You can now assign unique names to each controller in order to easily identify them when using more than one 						
-Fixed an issue where the arduino would soft lock and not recognize any further commands 										 
-Fixed a bug that would cause CPU usage to rise dramatically when selecting "Off" or "Fixed Color"  								
-Added the option to cycle through effects with a pysical button without the need of using a PC 									
-The app is more reliable when sending commands to the controller

Thanks for checking this project out! If you have any ideas for improvements or more features, let me know!
