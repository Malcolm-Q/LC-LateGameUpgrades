## ***LateGameUpgrades***
Source code for my [LateGameUpgrades mod.](https://thunderstore.io/c/lethal-company/p/malco/Lategame_Upgrades/)  for Lethal Company  


Feel free to make a pull request. Please make an effort to have additions be reasonably balanced.

This is MIT, do with it whatever you want.


Join [this modding discord](https://discord.gg/lcmods) and comment [on this post](https://discord.com/channels/1168655651455639582/1178407269994594435)  to discuss the mod.


I was really rushing and learning as I went when I made this so there's quite a bit of stuff that isn't best practice and a lot of housekeeping needs to be done.  
That being said it's all pretty straightforward.  


## **TODO:**
* Cleanup
* Config Implementation
* Rewrite AssetBundle Loading and Spawning to be Concise and Modular

## **Community Suggested Additions:**
If you want to implement one of these please create a branch indicating which feature you are implementing.  
Something like: `<discordNickName>/<feature>`  
Check branches before branching.
* Helmet Cams
    * See [this Solution](https://thunderstore.io/c/lethal-company/p/RickArg/Helmet_Cameras/source/) for example.
* Distraction Item
    * Switch target on chasing enemy.
    * Player can drop it to have agro switched to the item for x seconds.
* Radar Booster Shockwave
    * Pinging a Radar Booster stuns enemies in radius.
    * Could create another more expensive upgrade that allows this for players.
    * see discombobulator code for implementation.
* Improved Hydraulics
    * Door stays closed longer.
* Lightening Rod
    * Occasionally or always redirect lightening to ship.
* BioScanner
    * Type `bioscan` in terminal to get list of living creatures in radius around player.
* Adjustable Zoom / Monitor is Zoomed Out
    * Monitor displays more.
* Scanner Picks up Leaking Pipes
    * Add a ScanNode to the valve you turn to stop steam leak things.
    * Probably just add this as a part of betterscanner
* Player droppable Stun Landmine
    * Drop landmine, if enemy hits it, they're stunned for x seconds.
* RC Car?
    * Use it to find lost teammates or something.
* Rare Scrap With Utility
    * EX. a crucifix that if held prevents the Dress Girl monster from targetting you.

## **Explore:**
* Upgrades applying only to the player that picks them up.
    * Loading savefiles and having player upgrades go to the correct player might be tricky.
