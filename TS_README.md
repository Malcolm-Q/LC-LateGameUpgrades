# LateGameUpgrades
***INSTALL THE DEPENDENCIES***  
***ALL CLIENTS NEED THIS MOD INSTALLED***  
to install just put the MoreShipUpgrades folder in your BepInEx plugins folder.

### *Everything about this mod can be changed via the extremely extensive config "com.malco.lethalcompany.moreshipupgrades.config"*  
The config is automatically synced from host to clients excluding changes to equipment (peeper, night vision goggles, portable teles)


Type `lategame` in the terminal to see mod info!  
Type `lategame store` or `lgu` to view current upgrade status and cost.  
Type `info <upgrade>` for dynamic info.

This adds:
* 20+ Upgrades.
* 5 complex team oriented missions (Contracts).
* Mission items, NPC, rewards and behaviours.
* 8 new Store items.
* 1 rare scrap (Shopping cart).
   
The goal of this mod is to allow you to make playthroughs last longer and to remedy the stale lategame of not having anything to spend your money on and only going to one moon.

If using V40 - downgrade to V2.1.0  
If using v45 - downgrade to v2.8.6

## Compatibility

### This mod changes and patches a lot so problems may arise.  
- Bigger Lobby and More Company are more or less compatible but credit desync can arise  
- Use common sense, EX: if you download a mod that changes the movement speed of the player every frame and the movement speed upgrade from my mod doesn't work that's why.

## Community

### Please post bugs, assets, or ideas [on this post](https://discord.com/channels/1168655651455639582/1178407269994594435) after joining [this modding discord.](https://discord.gg/hzEcKFSSDX)

Feel free to [create a pull request](https://github.com/Malcolm-Q/LC-LateGameUpgrades) and help with the mod.

Anyone who contributes in any way is greatly appreciated. People willing to contribute 3D models are needed.

## Credit
- GitHub Contributors
    - Dilly_The_Dillster
    - WhiteSpike - [git](https://github.com/WhiteSpike)
    - Croquetilla
- Graphics / Art / Models
    - Sad Amazon
    - Bobilka
    - Pixelated Engie - [twitter](https://twitter.com/PixelatedEngie)
- Beta Testers
    - Lann
    - Kapt
    - Rootbeer
    - Glitched
    - a glitched npc - [twitch](https://www.twitch.tv/a_glitched_npc)

## Upgrades & Items
***Everything is tweakable via the config***

* __Diving Kit - $650__
    * Breathe underwater.
    * Heavy, two handed, low visibility.

* __Stimpack - $600__
    * Increase health by 20 per level.

* __Hunter - $700__
    * Allows you to collect and sell samples from killed monsters
    * **Lvl 1**: Hoarding Bugs & Snare Fleas
    * **Lvl 2**: Spiders & Baboon Hawks
    * **Lvl 3**: Bracken, Thumper, & Eyeless Dog

* __Medkit - $300__
    * Allows you to heal (default 20 points).
    * Limited uses (default 3)

* __Protein Powder - $500__
    * Increase damage dealt with shovels and signs.

* __Lightning Rod - $1000__
    * Redirects lightning to the ship.
    * The closer you (and your metal object) are to the ship, the more likely the ship will attract the lightning.

* __Fast Encryption - $300__
    * Upgrades the signal transmitter.
    * Must have signal transmitter purchased.
    * Instantly sends an unrestricted message to all clients chat when using transmit.

* __Interns - $1000__
    * Replaces your dead friend with a fresh intern (revives your friend).
    * Teleports to a random location in the facility.
    * $1k per use

* __Walkie GPS - $450__
    * Upgrades the walkie talkie to show your position and time.
    * Must be holding it.
    * Useful for fog or finding home.

* __Peeper - $500__
    * Looks at coil heads for you.

* __Locksmith - $640__
    * Makes noise when picking, makes a lot of noise when failing.
    * Just run into a locked door to start the minigame.
    * Strike the pins in the order they flash to unlock the door.

* __Portable Teleporter - $300__
    * An item that when used teleports you back to the ship.
    * Keeps items.
    * 90% chance to get destroyed on use.

* __Advanced Portable Teleporter - $1750__
    * Same as above.
    * 20% chance to get destroyed on use.

* __Beekeeper - $450__
    * Circuit bees do significantly less damage to you.

* __Bigger Lungs - $600__
    * Increased sprint duration.

* __Running Shoes - $650__
    * Increased movement speed.

* __Strong Legs - $300__
    * Jump higher.

* __Malware Broadcaster - $550__
    * Instead of disabling turrets and landmines; Destroy them.
    * Can enable alternate behaviours - Exploding hazards (default), destroying, or disabling for a longer period.

* __Light Footed - $350__
    * Enemies have to be closer to hear your footsteps.
    * Applies to both walking and running.

* __Night Vision - $380__
    * Press Left Alt to toggle night vision.
    * Has self regenerating batery.
    * Pick up and lmb to equip.
    * Lose on death (by default).

* __NV Headset Batteries - 300__
    * increases regen speed and decreases depletion speed of NV
    * Can also increase range and intensity of night vision light for alternate behaviour (if enabled in the config).

* __Discombobulator - $450__
    * Enter `initattack` into the terminal to stun enemies around the ship.
    * Enter `cooldown` to view cooldown (120 seconds).
    * Attracts enemies in a larger radius than loud horn.

* __Better Scanner - $650__
    * Level 1
        * Increase distance of Ship and Entrance pings drastically.
        * Increase distance of all other pings.
    * Level 2
        * Unlocks 5 new scan commands - scan player, scan enemies, scan scrap, scan hives, scan doors.
        * Type `info better scanner` for information on each.
    * Level 3
        * Allows you to scan scrap through walls.
        * Change the config if you also want to scan enemies through walls.

* __Back Muscles - $715__
    * Carryweight is drastically reduced.
    * % reduced increases each upgrade.

* __Lethal Deals - $300__
    * Guarantees at least one item will be on sale in the store.

* __Bargain Connections - $200__
    * Increases the maximum amount of items that can be on sale in the store.
    * Each level increase further increases the amount.

* __Market Influence - $250__
    * Guarantees a minimum sale percentage applied on the items when on sale in the store.
    * Each level increase further increases the minimum sale percentage.

* __Quantum Disruptor - $1000__
    * Increases the amount of time you can stay during a moon landing (time it takes to reach final hour in the moon)
    * Each level increase further increases the amount of time.

* __Helmet - $750__
    * Blocks incoming damage through amount of hits.
    * Once reached the limit of hits, it will be destroyed, having to purchase a new one if needed.

* __Wheelbarrow - $400__
    * Allows depositing items inside and carry all of them at once.
    * Can be restricted to amount of items or weight allowed in the wheelbarrow.
    * Applies a weight reduction multiplier on the deposited items when carried through the wheelbarrow.
    * Makes noise when being carried so be careful around monsters when using it!

* __Drop Pod Thrusters - $300__
    * Speeds up the shop's drop pod to deliver items faster.

* __Sick Beats - $500_
    * Boomboxes when playing music apply effects on nearby players such as movement speed, damage boost and defense.
    * An icon is displayed when the effects are being applied to your player.

* __Shutter Batteries - $300__
    * The ship's doors can last longer while being closed.
    * Though let's be honest, they will get in anyways.

## Contracts
* __Defusal Contract__
    * Look around the facility for a ticking bomb. Defuse it before the timer runs out or anyone near it might meet a gruesome fate.
    * A serial is shown on it. You will need this for the command ``lookup <serial>`` in the terminal which it will show the right sequence of wires you need to cut to defuse it.

* __Data Contract__
    * An old laptop has been found inside the facility with valuable data. Find it and retrieve the disk contained within.
    * Upon first interaction, an IP is displayed that needs to be inputted in the command ``bruteforce <IP>`` in the terminal.
    * This will show the user credentials used to log in into the laptop.
    * Upon logging in, a command line screen is displayed. Players will need to navigate through the screen through the commands:
        *  ``ls`` (list all files in current directory/folder)
        *  ``cd <folder>`` (move into selected folder)
        *  ``cd ..`` (move back to parent/previous folder)
        * ``mv survey.db`` (retrieve the requested file when present in current directory)

* __Exterminator Contract__
    * A nest of hoarding bugs has been detected on this facility. Clear them out and destroy the nest.
    * It's this simple: look for the nest while avoiding the monsters within and destroy it. A loot object will spawn after destroying it.

* __Exorcism Contract__
    * A ritual site has been detected in the facility. Look for the site and stop the ritual by using the correct items scattered around.
    * A pentagram will spawn around the facility that you will need to find. It makes a sound to tell that it's nearby.
    * Upon being found, you can look at the altar and find which demon the altar belongs to. You will need this information for the terminal command ``demon <demonName>``.
    * It will show the list of required items you need to put into the altar to stop the ritual. Any wrong item put in the altar will attract unwanted visitors.
    * Upon inserting the right items, a loot item will spawn to which you can carry to the ship.

* __Extraction Contract__
    * A fellow employee has been reported missing in the facility. You will need to find him and escort them back to the ship for safety (and disciplinary action by The Company)
    * They will be laying on the floor pleading for help. You can tell when they are nearby through their shouts.
    * You will also find several medkits found in the facility. You will need one as the scavenger appears to have hurt themselves and cannot get back up on their feet.
    * After helping the employee, you will have to carry them out of the facility. They are really heavy and will start making alot of noise when sensing any nearby danger.
    * After being escorted back to the ship, they will thank you for your help non-stop til you arrive at The Company (where you escort them for disciplinary action).

## Commands
* ``extend deadline <days>``
    * Extends the current deadline of the profit quota by ``<days>``.
    * Each day added requires $800 (configurable).
* ``probe <moonName> [weatherType]``
    * Sends out a weather probe to the selected moon to change its current weather.
    * If provided a ``weatherType``, it will change the moon's weather to the selected one for $500 (configurable)
    * Otherwise, a random weather will be selected and will cost $300 (configurable)
    * You can configure the random weather command to only select cleared weathers (off by default)
