# LED controller
An arduino + windows project to control led strips with ease. Instructables coming soon

## Important notice! Repo will be archived soon! Successor here [OpenRGB](http://www.github.com/Dany_Boy55/OpenRGB.git)
#### This repo will be archived very soon as I will re-start this project from scratch. I have not really mantained or updgraded de code in a very long while, and looking back, it is full of bad practices and all sorts of fundamental setbacks. Additionally, versioning/documenting was a total mess due to me not really knowing what I was doing. If you still want to use and build upon this repo, you are more than welcome  to do so but be aware it is not really ideal. 

If your intent is to simply download and use, then you want to get the .ino sketch for the arduino and the App.zip folder. 
Install the FastLED library into your arduino IDE and upload the sketch. Make necessary adjustments to the sketch.
Open the .exe file within the App folder and you are ready to go!

For those of you interested in the code, then the Visual Studio folder contains all the resources you need to open the solution on MS visual studio. Feel free to modify the app to your liking. You can find a breif youtube demo here: https://www.youtube.com/watch?v=55OrRJvSOvo

Improvements from 1.0 to 1.3 were not documented, so uhm yeah. 

New features on V1.4:                                                                                                 
* Added the ability to connect to several LED controllers, not just the first one the app finds
* Added a new lighting effect: Color Rain. Random "dropplets" that fade into the backgorund color
* You can now assign unique names to each controller in order to easily identify them when using more than one*
* Fixed an issue where the arduino would soft lock and not recognize any further commands
* Fixed a bug that would cause CPU usage to rise dramatically when selecting "Off" or "Fixed Color"
* Added the option to cycle through effects with a pysical button without the need of using a PC
* The app is more reliable when sending commands to the controller

New features on V1.5:
* Added "Color Gradient" and "Color Fade" effects
* Improved stability for the arduino sketch and fixed some overflow errors
* Extended the rate from 0-20 to 0-50 to allow for faster/slower effects
* Improved the color calculations in the arduino for smoother transitions

New features on V1.7:
* Added functionality to run the app in the backgorund with the help of a system tray icon (for hardwa monitoring)
* Added hardware monitoring options for CPU and GPU (temperature and utilization)
* Improved connectivity with the controller (should result in faster and more effective identification and communication)
* Added a menu bar, the app can now run on top at all times
* User setttings are now saved from the app at exit, not only when requested by the user
* Improved functionality when scanning for LED controllers, the app will now scan 5 times in succession, hopefully resulting in better controller recognitions

Goals going forward:
* Add sound reactiveness; make the LEDs a real time audio indicator (no significant progress yet)
* Redesign and simplify UI (no date yet)
* Redesign the back-end for both the app and the arduino to provide better responsiveness (end of April)

Thanks for checking this project out! If you have any ideas for improvements or more features, let me know!

<a rel="license" href="http://creativecommons.org/licenses/by-sa/4.0/"><img alt="Creative Commons License" style="border-width:0" src="https://i.creativecommons.org/l/by-sa/4.0/88x31.png" /></a><br />This work is licensed under a <a rel="license" href="http://creativecommons.org/licenses/by-sa/4.0/">Creative Commons Attribution-ShareAlike 4.0 International License</a>.
