## ***LateGameUpgrades***
Source code for my [LateGameUpgrades mod.](https://thunderstore.io/c/lethal-company/p/malco/Lategame_Upgrades/)  for Lethal Company  


Feel free to make a pull request. Please make an effort to have additions be reasonably balanced.

If reporting a bug. Please include your logoutput.log file if possible, it's in your bepinex folder.

Join [this modding discord](https://discord.gg/hzEcKFSSDX) and comment [on this post](https://discord.com/channels/1168655651455639582/1178407269994594435)  to discuss the mod.

## **Contributing:**
You will need to set up Evaisa's [Netcode Patcher](https://github.com/EvaisaDev/UnityNetcodeWeaver) if you want to add more custom netcode. If you are making simple additions that don't need to be tested online you can still build an unpatched dll and test it.

If adding custom objects you need to set up a unity environment for making asset bundles. I recommend using [Evaisa's Template](https://github.com/EvaisaDev/LethalCompanyUnityTemplate). Prefabs for the assets are in the [UnityFiles folder.](/UnityFiles/)

This project uses [LethalLib](https://github.com/EvaisaDev/LethalLib) by, you guessed it, [Evaisa](https://ko-fi.com/evaisa) to add items to the shop, register network prefabs, and in the future probably more so you can read about it there.

The most recent (good chance it's unstable) dll and asset bundle can be found in the [output folder.](/output/)


## **TODO:**
* Individiaul upgrades and saving (steamid dictionaries).
* Defibrilator item.
* Upgraded walkie talkie (Navigation).
* More meaninful tiered upgrades.

## **Community Suggested Additions:**
If you want to implement one of these please create a branch indicating which feature you are implementing.  
Something like: `<discordNickName>/<feature>`  
* Distraction Item
    * Switch target on chasing enemy.
    * Player can drop it to have agro switched to the item for x seconds.
* Radar Booster Shockwave
    * Pinging a Radar Booster stuns enemies in radius.
    * Could create another more expensive upgrade that allows this for players.
    * see discombobulator code for implementation.
* Lightening Rod
    * Occasionally or always redirect lightening to ship.
* BioScanner
    * Type `bioscan` in terminal to get list of living creatures in radius around player.
* Scanner Picks up Leaking Pipes
    * Add a ScanNode to the valve you turn to stop steam leak things.
    * Probably just add this as a part of betterscanner
* Player droppable Stun Landmine
    * Drop landmine, if enemy hits it, they're stunned for x seconds.
* RC Car?
    * Use it to find lost teammates or something.
* Rare Scrap With Utility
    * EX. a crucifix that if held prevents the Dress Girl monster from targetting you.


This is MIT, do with it whatever you want.