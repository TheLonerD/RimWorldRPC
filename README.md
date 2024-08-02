
<p align="center">
<img src="https://i.imgur.com/0bQFhhY.png">
</p>

# RWRichPresence
RWRichPresence updates your Discord status with some information about your game. Now updated for the latest version of the game - 1.5!

## Feautures

* Display various game information 
* Customizable text 
* Plenty of settings, so you can enable and disable certain aspects!

*Currently shows:*
* Colony name
* Amount of days colony has lasted
* Hour of the current day
* Quadrum
* Amount of time your game has been running for
* Year
* Event ( new add )

## Installation ( For 1.5 )
*Skip to step 3 if you have the mod from Steam Workshop*

1. Download the latest release [(link)](https://github.com/MasterPNJ/RimWorldRPC/releases)
2. Extract the folder '3291415439' from the zip to your RimWorld Mod folder as per usual. DON'T RENAME THE ZIP AND FOLDER!
3. Run Rimworld and enable the mod, save and restart the game.
4. If it's first time you enable the mod, you need to restart the game one more time.

Have fun !

## Installation ( For 1.4 and before )
*Skip to step 3 if you have the mod from Steam Workshop*

1. Download the latest release :
  * For 1.4 ( d1rknwh1te3 ) [(link 1.4)](https://github.com/d1rknwh1te3/RimWorldRPC/releases)
  * For 1.3 & before ( original author ) [(link 1.3 & before)](https://github.com/Weilbyte/RWRichPresence/releases)
3. Extract the `RimRPC` folder from the zip to your RimWorld Mod folder as per usual.
4. Go to Discord's `discord-rpc` release page. [(link)](https://github.com/discordapp/discord-rpc/releases)
5. Download `discord-rpc-win.zip` for windows or `discord-rpc-linux.zip` if you are using linux.
6. Open the zip and follow instructions below depending on your PC architecture and platform:  
  * *(Windows 32 bit)*  Copy  `discord-rpc\win32-dynamic\bin\discord-rpc.dll` into your RimWorld folder - `RimWorld\MonoBleedingEdge\EmbedRuntime`  
  * *(Windows 64 bit)* Copy `discord-rpc\win64-dynamic\bin\discord-rpc.dll` into your RimWorld folder - `RimWorld\MonoBleedingEdge\EmbedRuntime` or for 1.1 `Rimworld\RimWorldWin64_Data\Plugins`
  * *(Linux 64 bit)* Copy `discord-rpc\linux-dynamic\lib\discord-rpc.so` into your RimWorld folder - `RimWorld/MonoBleedingEdge/EmbedRuntime`
6. Rename `discord-rpc.dll` to `0discord-rpc.dll`.
  * On linux you will need to rename`discord-rpc.so` to `lib0discord-rpc.so` or `0discord-rpc`

Thats pretty much it, youre set. 

##  Usage
Can be added and removed (spits out a single error that you can ignore) mid-game.  
Once in-game, presence will update twice every **in-game** hour which equals to once every *20.5* **real-time** seconds.

There is a mod settings menu which you can use to tweak what is displayed. From there you can also set custom text (as shown in the image) to replace either the top or bottom line.


Make sure RimWorld is added as a game in Discord's settings.

## Credits
Jdalt40

d1rknwh1te3 ( for 1.4 update ) : https://github.com/d1rknwh1te3/RimWorldRPC

Weilbyte ( Original author ) : https://github.com/Weilbyte/RWRichPresence

## Links

[Steam Original Author 1.3 & before](https://steamcommunity.com/sharedfiles/filedetails/?id=1463057070)

[Steam 1.4/1.5 version](https://steamcommunity.com/sharedfiles/filedetails/?id=3291415439)
