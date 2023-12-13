![LUTransparent](https://github.com/Malcolm-Q/LC-LateGameUpgrades/assets/118214091/a39a7b59-651b-4fa2-8224-cdd9327c02ab)


Source code for my [LateGameUpgrades mod.](https://thunderstore.io/c/lethal-company/p/malco/Lategame_Upgrades/)  for Lethal Company  

If reporting a bug. Please include your logoutput.log file if possible, it's in your bepinex folder.

The releases contains a nightly build for V2.6.0.

Join [this modding discord](https://discord.gg/hzEcKFSSDX) and comment [on this post](https://discord.com/channels/1168655651455639582/1178407269994594435)  to discuss the mod.


## **Frequently Asked Questions(FAQ)**

* **the mod isn't working!**
   * Make sure you have all dependencies installed. The current dependencies for v2.1.0 are [Bepinex](https://thunderstore.io/c/lethal-company/p/BepInEx/BepInExPack/), [LethalLib](https://thunderstore.io/c/lethal-company/p/Evaisa/LethalLib/), and [HookGenPatcher.](https://thunderstore.io/c/lethal-company/p/Evaisa/HookGenPatcher/)  
   * Follow the instructions on Thunderstore or in the installation.txt file. If you're having trouble manually installing mods, use a modmanager like r2modman.
* **I have the latest version and all the dependencies, but I cannot see the items besides the teleporters what do I do?**
   * The store for the upgrades are no longer in the main terminal store. To access the new store you can type either lategame store, or lgu in the terminal.
* **I don't like this feature, how do i turn it off/modify it?**
   * There is a config which makes most of this mod entirely customizable to the user, configs for r2 modman are easy to access. Click Config editor once your profile is selected, and then choose the file called BepInEx\config\com.malco.lethalcompany.moreshipupgrades.cfg and hit edit.
   * For manual installations the config would be in your Lethal Company\BepInEx\config folder, following the same naming convention the r2modman one is.
* **Is this mod clientside or serverside??**
   * This mod is not clientside, ***everyone*** needs to have it installed with the same configuration settings.
* **Can I suggest an addition?**
   * Absolutely, I cannot promise the mod will incorporate your suggestion but we suggestions are greatly appreciated.
   * Simply place your suggestion in the discord channel for the mod, and be sure to ping either myself (@dilly_the_dillster) or keith (@_kieth)
* **When will x feature be implemented?**
   * As the case for all development goes, we can never give you an exact date, however we can say whether it will be soon, if its actively being worked on,  and anything along those lines. The usual answer will probably be "soon".  
   * It may not always be up to date but you can check the TODO section of this README for what is currently being worked on.
* **Can I contribute to the mod?**
   * Absolutely! We welcome anyone who desires to help. Feel free to submit your new features or additions with a pull request [here.](https://github.com/Malcolm-Q/LC-LateGameUpgrades)
   * If you want to contribute art / models please reach out to @dilly_the_dillster or @_kieth in the discord.
   * Please make an effort to have additions be reasonably balanced and customizable via the config.
* **Why are my credits desynced from other players when purchasing an upgrade?**
   * This is an issue with MoreCompany/BiggerLobby. Effort has been made to provide stronger compatibility but this issue can still be present.
* **Why can I not buy the upgrades in lategame store?**
    * Enter the full name of the upgrade and the full name only (case insensitive).
    * Ex: `beekeeper`
    * If you enter `buy beekeeper` or `purchase beekeeper` it will not work.

## **Contributing:**
You will need to set up Evaisa's [Netcode Patcher](https://github.com/EvaisaDev/UnityNetcodeWeaver) if you want to add more custom netcode. If you are making simple additions that don't need to be tested online you can still build an unpatched dll and test it.

If adding custom objects you need to set up a unity environment for making asset bundles. I recommend using [Evaisa's Template](https://github.com/EvaisaDev/LethalCompanyUnityTemplate). Prefabs for the assets are in the [UnityFiles folder.](/UnityFiles/)

This project uses [LethalLib](https://github.com/EvaisaDev/LethalLib) by, you guessed it, [Evaisa](https://ko-fi.com/evaisa) to add items to the shop, register network prefabs, and in the future probably more so you can read about it there.

The most recent (good chance it's unstable) dll and asset bundle can be found in the [output folder.](/output/)


## **TODO:**
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
