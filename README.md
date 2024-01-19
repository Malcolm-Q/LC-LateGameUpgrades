![LUTransparent](https://github.com/Malcolm-Q/LC-LateGameUpgrades/assets/118214091/a39a7b59-651b-4fa2-8224-cdd9327c02ab)

Source code for my [LateGameUpgrades mod.](https://thunderstore.io/c/lethal-company/p/malco/Lategame_Upgrades/)  for Lethal Company  
#### [Frequently Asked Questions(FAQ)](https://github.com/Malcolm-Q/LC-LateGameUpgrades/issues/60#issue-2051585712)

#### [Read This Before Creating An Issue](https://github.com/Malcolm-Q/LC-LateGameUpgrades/issues/56)

#### [Post and Discuss Suggestions Here!](https://github.com/Malcolm-Q/LC-LateGameUpgrades/discussions)

Join [this modding discord](https://discord.gg/hzEcKFSSDX) and comment [on this post](https://discord.com/channels/1168655651455639582/1178407269994594435)  to discuss the mod.


## **Contributing:**
- You will need to set up Evaisa's [Netcode Patcher](https://github.com/EvaisaDev/UnityNetcodeWeaver) if you want to add more custom netcode. If you are making simple additions that don't need to be tested online you can still build an unpatched dll and test it.  
- If adding custom objects you need to set up a unity environment for making asset bundles. I recommend using [Evaisa's Template](https://github.com/EvaisaDev/LethalCompanyUnityTemplate). Prefabs for the assets are in the [UnityFiles folder.](/UnityFiles/)  
- IMPORTANT: ***Do not include your new assets in the assets.zip file.*** Create a new file called IncomingAssets<name>.zip that contains only the new assets you've made.  
- This project uses [LethalLib](https://github.com/EvaisaDev/LethalLib) by, you guessed it, [Evaisa](https://ko-fi.com/evaisa) to add items to the shop, register network prefabs, and in the future probably more so you can read about it there.  
- The most recent (good chance it's unstable) dll and asset bundle can be found in the [output folder.](/output/)  

*Included is a projectSetup.exe and projectSetup.py, you can run one or the other to quickly add all (or most of if some have been recently added) of the dependents to your .csproj

## **Community Suggested Additions:**
If you want to implement one of these please create a branch indicating which feature you are implementing.  
Something like: `<discordNickName>/<feature>` 
* Planet Scanner
    * When in orbit type something like scan <moon> to get info about what your visit to that moon would be like.
    * This would involve generating random seeds for each moon in orbit and ensuring they use them for generation.
    * Information about enemies, scrap, and maphazards could be retrieved with this info.


*This is MIT, do with it whatever you want.*
