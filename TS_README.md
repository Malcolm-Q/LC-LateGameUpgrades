# LateGameUpgrades
***INSTALL THE DEPENDENCIES***  
***ALL CLIENTS NEED THIS MOD INSTALLED***  
to install just put the MoreShipUpgrades folder in your BepInEx plugins folder.

### *Everything about this mod can be changed via the extremely extensive config "com.malco.lethalcompany.moreshipupgrades.config"*  
The config is automatically synced from host to clients excluding changes to equipment (peeper, night vision goggles, portable teles)

Type `lategame` in the terminal to see mod info!  
Type `lategame store` or `lgu` to view current upgrade status and cost.  

This adds:
* 30+ Upgrades.
* 5 complex team oriented missions (Contracts).
* Mission items, NPC, rewards and behaviours.
* A couple store items.
   
The goal of this mod is to allow you to make playthroughs last longer and to remedy the stale lategame of not having anything to spend your money on and only going to one moon.

If using V40 - downgrade to V2.1.0  
If using v45 - downgrade to v2.8.6  
If using v50 - downgrade to v3.8.1  
If using v64 - downgrade to v3.10.4  

## Community

### Please post bugs, assets, or ideas [on this post](https://discord.com/channels/1168655651455639582/1178407269994594435) after joining [this modding discord.](https://discord.gg/hzEcKFSSDX)

Feel free to [create a pull request](https://github.com/Malcolm-Q/LC-LateGameUpgrades) and help with the mod.

Anyone who contributes in any way is greatly appreciated.

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

## Upgrade Store Interface
``lgu`` prompts an interactive user interface in which you can select the upgrade you wish to upgrade or to simply check what it does through its displayed information.
You can check the key binds used for More Ship Upgrades to understand how to navigate through the interface (and customize to your liking)

## Upgrades & Items
***Everything is tweakable via the config***

* __Stimpack - $600__
    * Increase health by 20 per level.

* __Hunter - $700__
    * Allows you to collect and sell samples from killed monsters
    * **Lvl 1**: Hoarding Bugs & Snare Fleas
    * **Lvl 2**: Spiders, Baboon Hawks, Tulip Snake & Kidnapper Fox
    * **Lvl 3**: Bracken, Thumper, Eyeless Dog, Maneater & Manticoil
    * **Lvl 4**: Forest Giant

* __Medkit - $300__
    * Allows you to heal (default 20 points).
    * Limited uses (default 3)

* __Protein Powder - $500__
    * Increase damage dealt with shovels and signs.

* __Lightning Rod - $1000__
    * Redirects lightning to the ship.
    * The closer you (and your metal object) are to the ship, the more likely the ship will attract the lightning.
    * Alternative modes such as redirect all item lightning bolts, random lightning bolts and both.

* __Fast Encryption - $300__
    * Upgrades the signal transmitter.
    * Must have signal transmitter purchased.
    * Speeds up the transmission speed of the characters and removes the limit of characters in the transmission message.

* __Interns - $1000__
    * Replaces your dead friend with a fresh intern (revives your friend).
    * Teleports to a random location in the facility.

* __Walkie GPS - $450__
    * Upgrades the walkie talkie to show your position and time.
    * Must be holding it.
    * Useful for fog or finding home.

* __Locksmith - $640__
    * Makes noise when picking, makes a lot of noise when failing.
    * Just run into/interact with a locked door to start the minigame.
    * Strike the pins in the order they flash to unlock the door.

* __Beekeeper - $450__
    * Circuit bees & Butler Hornets do significantly less damage to you.

* __Bigger Lungs - $600__
    * Increased sprint duration.
    * Can increase the stamina regeneration speed through configuration
    * Can decrease the stamina cost when jumping through configuration

* __Running Shoes - $650__
    * Increased movement speed.
    * At max level, reduces the noise made by your footsteps.

* __Strong Legs - $300__
    * Jump higher.

* __Malware Broadcaster - $550__
    * Instead of disabling turrets and landmines; Destroy them.
    * Can enable alternate behaviours - Exploding hazards (default), destroying, or disabling for a longer period.

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
    * Alternative modes where either speed or stamina consumption debuffs due to weight are removed.

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
    * Alternative mode where you can revert back time spent on a moon through 'quantum' command

* __Helmet - $750__
    * Blocks incoming damage through amount of hits.
    * Once reached the limit of hits, it will be destroyed, having to purchase a new one if needed.
    * Alternative mode where the helmet gives partial damage reduction and only breaks when the next hit would kill the player.

* __Drop Pod Thrusters - $300__
    * Speeds up the shop's drop pod to deliver items faster.
    * The drop pod leaves after picking up the items when the upgrade's active.

* __Sick Beats - $500__
    * Boomboxes when playing music apply effects on nearby players such as movement speed, damage boost and defense.
    * An icon is displayed when the effects are being applied to your player.

* __Shutter Batteries - $300__
    * The ship's doors can last longer while being closed.
    * Though let's be honest, they will get in anyways.

* __Aluminium Coils - $750__
    * Provides several buffs to the zap gun such as:
	* Increased stun range
	* Increased stun timer after stopping zapping
	* Decreased difficulty in its minigame
	* Decreased cooldown time after failing the minigame (stopping zapping the enemy)

* __Landing Thrusters - $300__
    * Speeds up the ship whenever it's landing/taking off.
	* Can configure what moments for the upgrade to take effect (only landing/departing or both)

* __Deeper Pockets - $500__
    * Increases the amount of two-handed items you can carry in your inventory.
    * Can configure if the wheelbarrow can be pocketed when this upgrade's active.

* __Reinforced Boots - $250__
    * Reduces incoming fall damage.

* __Scavenger Instincts - $800__
    * Increases the average amount of scrap it can spawn in a given level.

* __Mechanical Arms - $300__
    * Increases the player's interaction range with items and interactables.

* __Clay Glasses - $200__
    * Increases the maximum distance from which the player can spot a "Clay Surgeon" entity. 
    
* __Weed Genetic Manipulation - $100__
    * Increases the effectiveness of the Weed Killer item in eradicating plants. 

* __Fedora Suit - $750__
    * Makes Butler enemies not immediately attack you when alone with them.
    * If attacked, they will still retaliate.

* __Traction Boots - $100__
    * Increases the player's traction force to the ground to allow easier movement direction change.

* __Oxygen Canisters - $100__
    * Decreases the oxygen consumption rate of the player.
    
* __Rubber Boots - $50__
    * Decreases the movement hinderance when walking on water surfaces.
    * This also includes the quick sand patches during the Rainy weather.
    * If the percentage decrease reaches or exceeds 100%, you will not be slowed down when walking through it.
    * This reduces how much movement hindurance you get from walking on water surfaces, not how much you're hindered by which is always by half multiplied by outside hinder multipliers such as this one and cobwebs.
    
* __Life Insurance - $200__
    * Decreases the credit loss due to player deaths.

* __Carbon Kneejoints - $100__
    * Decreases the movement speed loss when crouching.
    * At 100%, crouching speed will be the same as walking.

* __Midas Touch - $1000__
    * Increases the value of the scrap found in moon trips.

* __Quick Hands - $100__
    * Increases the interaction speed of the player on objects.
    
* __Jet Fuel - $400__
    * Increases the acceleration rate of the jetpack while in flight.
    
* __Jetpack Thrusters - $300__
    * Increases the maximum speed of the jetpack while in flight. 

* __Silver Bullets - $500__
    * Allows the shotgun to kill ghost girls.
    
* __Hollow Point - $750__
    * Increases the damage output of the shotgun.
   
* __Long Barrel - $500__
    * Increases the range of the damage zones and overall range of the shotgun. 

* __Fusion Matter - $500__
    *  Allows players to teleport with some items in their inventory.

* __Particle Infuser - $650__
    *  Increases the speed of the teleporters during teleportation phase.

* __Scrap Keeper - $1000__
    * In case of full team wipe, each scrap has a chance of staying in the ship.

* __Medical Nanobots - $300__
    * Increases the health regeneration cap of the vanilla health regeneration mechanic.
    
* __Effective Bandaids - $250__
    * Increases the amount of health regenerated from the vanilla health regeneration mechanic. 
    
## Item Progression Mode

This mod offers an alternative mode for obtaining upgrades through selling items to The Company. The following modes are available for playthroughs:
* Apparatice
    * Items that are considered an "apparatus" (configurable due to custom interiors having custom items) will provide a randomly picked upgrade to level up when sold to The Company
* Chance per Scrap
    * When any kind of scrap item is sold to The Company, a roll will be made and check if it meets the chance criteria (value is configurable).
    * If successfull, an upgrade will be picked to level up according to the selected chance mode:
        * Random: an upgrade will be picked at random without any outside factors influencing the choice.
        * Cheapest: the upgrade that has the lowest cost to purchase will be picked
        * Lowest Level: the upgrade that has the lowest level will be picked

* Nearest Value
    * On each quota met event, the value accquired to meet the quota will be used to pick an upgrade that has at least the same cost
    for the quota value.
    * If no upgrades were found that had a cost lower than the meeting quota, no upgrade is picked.

* Unique Scrap
    * Upon save boot, each scrap item will be associated to an upgrade at random.
    When the item is sold, percentage of that amount (configurable) is used to contribute to the costs of the respective upgrade.
    Once the amount contributed to the item has been reached, the upgrade will rank up to the next level (if allowed).

* Custom Scrap
    * Similar logic with "Unique Scrap" but the items assigned to each upgrade are the ones assigned in each upgrade's configuration.
    * The names inserted in this configuration can be either the names displayed in their scan nodes or their internal names (in this case, the value of "itemProperties.itemName)
        * The names are case insensitive

## Randomize Upgrades Mode

This mod offers a "Randomize Upgrade" mode where a set amount of upgrades are picked from the total enabled and then shown in the upgrade store.
* You can configure the amount of upgrades that can be picked randomly to appear in the store
* You can configure this mode to allow showing already purchased upgrades.
* You can configure when should the randomization of the upgrades be executed (per moon landing, per moon routing or per quota).

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
* ``contracts``
    * Brings out an interactive UI where you can select to obtain a contract, see current contract status and information related to each contract type.

## API
* Hunter
    * You can register your own items to the Hunter upgrade for them to spawn on enemy kills! Just follow the wiki page associated with ``Hunter API``.
* Discombobulator
    * You can manipulate the discombobulator feature to your liking such as altering the cooldown or attempt to trigger it.
* Quantum Disruptor
    * You can attempt to trigger the Quantum Disruptor's Revert Time feature when configurated for it.
* Upgrades
    * You can contribute towards an upgrade for some amount of value which is affected by the configured item progression multiplier
    * You can trigger a rank up of an upgrade.

## Items/Commands moved to their own Mods
- [Peeper](https://thunderstore.io/c/lethal-company/p/WhiteSpike/Peeper/)
- [Portable Teleporters](https://thunderstore.io/c/lethal-company/p/WhiteSpike/Portable_Teleporters/)
- [Wheelbarrow](https://thunderstore.io/c/lethal-company/p/WhiteSpike/Wheelbarrow/)
- [Shopping Cart](https://thunderstore.io/c/lethal-company/p/WhiteSpike/Shopping_Cart/)
- [Diving Kit](https://thunderstore.io/c/lethal-company/p/WhiteSpike/Diving_Kit/)
- [Scrap Insurance](https://thunderstore.io/c/lethal-company/p/WhiteSpike/Scrap_Insurance/)
- [Extend Deadline](https://thunderstore.io/c/lethal-company/p/WhiteSpike/Extend_Deadline/)
- [Weather Probe](https://thunderstore.io/c/lethal-company/p/WhiteSpike/Weather_Probe/)
- [Helmet](https://thunderstore.io/c/lethal-company/p/WhiteSpike/Helmet/)