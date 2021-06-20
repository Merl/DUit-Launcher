# DUit-Launcher

>I heard you like launcher, so I put a launcher into your launcher

## Why?
Recently we got some allowed companiontools to run side by side with DU. So we have to start not one but at least two or three programs when playing DU.
The most common and simple answer if one does want to start the companiontools together with DU is of course a batch file.
The downside of using a batch file is that often console windows are left open and you have to close the programs after closing DU.

DUit-Launcher addresses those two caveats by giving the option to hide companion windows and itself and by keeping track of the running DU process and started companiontools. Once DU is closed all companiontools will be closed too.

The idea is for DUit-Launcher to act as a "shortcut to dual-launcher.exe replacement". Instead of the normal DU Icon you just start DUit-Launcher. This will start the normal DU Launcher together with your configured companiontools.

## Usage
On first start DUit-Launcher will ask you to select the path to dual-launcher.exe. 
After selecting the correct exe you will see the DUit-Launcher Window.

* **Main Programs** keeps track of dual-launcher and dual.exe.
* **Additional Programs** lists your companiontools and keeps track of them.
* **Add** will open a file selection dialog, here you can select your companiontools executable.
* **Edit** will open a file selection dialog for the element you select in one of the lists. ***do this only if programs are not running***
* **Delete** will delete the selected item. This is only valid for the Additional Programs list.
* **Minimize to tray** if checked DUit-Launcher will minimize to tray instead of taskbar.
* **Minimize on start** if checked DUit-Launcher will minimize and start DU immediately instead of opening its own window.
* **Quit on DU Exit** if checked DUit-Launcher will quit itself after DU has exited.
* **Save** ***do not forget to save or your changes will be lost on exiting DUit-Launcher.***

The intended use is for all 3 checkboxes to be checked, but you can choose which to use. :)

*Double click* or *right click &gt; Configure* to open the configuration window if DUit-Launcher is minimised to tray.

## Advanced Usage
* **Hide Window** if checked the companiontools application window will be hidden.
* **Comandline** In addition to the Edit Button you can enter the path directly
* **Arguments** At the time of writing I am not aware of any companiontools that use arguments but some tools are written in e.g. Java. In that case you would select java.exe as your companiontool and then add "-jar path:\to\your\companiontool.jar" as Argument.

## Planed features
* Advanced Add: Add companiontools from GitHub and auto update on new releases
* Manage Autoconfigurations
* Make an installer? Maybe?

## Licensing
Varying from the repo License the background picture and DU logo are taken from the [Dual Universe press kit](https://www.dualuniverse.game/contact) and property of [Novaquark](https://www.dualuniverse.game).

DUit-Launcher except from the above mentioned is BSD-2-Clause.

