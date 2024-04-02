## V 3.4.4 - 2024-03-31
- Another attempt at fixing the upgrades not loading when reconnecting.
    - If it still persists, report the issue in the github repository with logs attached from both host and clients.
    - If there's an issue already reported in the repository, post on that one instead.
- Fixed some upgrade infos showing incorrect values.
- Changed Bigger Lungs info to include stamina regeneration and stamina cost reduction on jumps.
- Fixed ``weather <moonName>`` leading to negative values due to lack of price check.

## V 3.4.3 - 2024-03-23
- Fixed clients disconnecting throwing errors, leading to upgrades on next reconnect to not load.
- Fixed error throw when quiting the game (harmless but an error)

## V 3.4.2 - 2024-03-21
- Fixed configuration synchronization due to serialization issues with colours
    - As a consequence, reverted the synchronization between host and client for colours
- Fixed upgrades not being loaded when you have sales saved and enabled one of the upgrades after the sales.
- Fixed upgrades being able to be purchased in the cases of being free through configuration.
- Added configuration for Bigger Lungs' additional effects (Stamina regeneration increase and stamina cost reduction on jumps)

## V 3.4.1 - 2024-03-20
- Fixed weather syncing between host and client breaking vital initializations.
- Fixed NV UI appearing for clients even with NV upgrade off
- Made NV colours be synced between host and client.
    - This is a temporary solution for when I understand better why the configuration entries for these are being broken, leading to clients not being able to use Night Vision.

## V 3.4.0 - 2024-03-19
### Additions
- Added "Sigurd" upgrade which might provide a boost on The Company's buying rate on certain amount of days.
    - Configurable enabled only on last day of meeting the profit quota.
    - Configurable enabled on any day of meeting the profit quota.
    - Configurable purchase price.
    - Configurable probability chance (%) that the upgrade will provide a boost on The Company's buying rate on any day of meeting the profit quota.
    - Configurable probability chance (%) that the upgrade will provide a boost on The Company's buying rate on the last day of meeting the profit quota.
    - Configurable amount of boost granted to The Company's buying rate when rolled successfully on any day of meeting the profit quota.
    - Configurable amount of boost granted to The Company's buying rate when rolled successfully on the last day of meeting the profit quota.
- Added "Climbing Gloves" upgrade which increases the speed at which you climb any sort of ladders (or similar)
    - Configurable enabled upgrade.
    - Configurable shared or individual upgrade.
    - Configurable price of the first purchase of the upgrade.
    - Configurable list of prices past the first purchase of the upgrade.
    - Configurable initial speed boost on the climbing speed of the upgrade.
    - Configurable incremental speed boost on the climbing speed of the upgrade.
- Added "Efficient Engines" upgrade which applies a discount on moon routing prices.
    - Configurable enabled upgrade.
    - Configurable price of the first purchase of the upgrade.
    - Configurable list of prices past the first purchase of the upgrade.
    - Configurable initial discount applied on Moon Routing of the upgrade.
    - Configurable incremental discount applied on Moon Routing of the upgrade.
- Added "probe" command which a weather probe is sent to selected moon and change its current weather.
    - Usage: ``probe <moonName> [weatherType]``
        - moonName = Name of the moon you wish to change the weather of.
        - weatherType = Type of weather you wish to change the moon's weather to.
    - A confirm prompt is required when specifying the weather on a given moon.
    - If weather type is absent, a random weather allowed on the moon will be choosen.
    - The probe cannot change a moon's weather to some other weather that is not allowed in it.
        - E.g Titan's to Flooded or Dine's to Rainy.
        - However if using mods that allow changing allowed weathers on moons, it *should* use the selected ones.
    - Configuration:
        - Configurable enabled command.
        - Configurable price when executing a weather probe with random weather.
        - Configurable price when executing a weather probe with specified weather.
        - Configurable toggle for randomized weather probe to always make the moon's weather cleared.
- Added configuration for the LGU's Store Sales to apply only on the first ever purchase.
    - E.g Protein Powder's on sale, you buy it once, the next level purchase will not have the sale anymore.
- Added configuration to customize the Night Vision's UI colours (text and image)
- Added configuration for message popups to appear on chat or not when upgrades are loaded.
### Changes
- When a given upgrade's configuration allows them to be immediately loaded (essentialy being free), they are automatically loaded.
    - For tier upgrades, only the initial value configuration will apply.
- Added wire interactions on all sides of the bomb (from defusal contract) and a serial preview (shown when looking at the middle of the item) for such cases of being spawned inside a wall
### Fixes
- Fixed Better Scanner sometimes preventing enemies being scanned while in full sight.
- Fixed Night Vision being togglable while manipulating the terminal.
- Fixed some info descriptions exibiting "ConfigEntry" text instead of the respective value.
- Fixed Quantum Disruptor's info displaying the wrong value, leading to confusion.
- Fixed Boombox crash error that would lead to mods that also manipulate the boombox to not work as intended.
- Fixed LGU's Store sales not being saved properly, leading to being lost on reboot.
- Possibly fixed host losing night vision when a client dies with "Lose Night Vision on Death" enabled.
### Code Changes
- Made Hunter implementation more robust to configuration changes (credits to [achohbee](https://github.com/achohbee))
- Configuration synchronization is now handled by [CSync](https://thunderstore.io/c/lethal-company/p/Sigurd/CSync/)
    - When meaningful updates are released to [Owen3H's](https://github.com/Owen3H) [CSync](https://thunderstore.io/c/lethal-company/p/Owen3H/CSync/), we will change to that one.
- Changed implementation of acquiring old saves to not clutter the current save file format.

## V 3.3.1 - 2024-03-10
### Fixes
- Fixed Stimpack not being applied past the first day of the upgrade being applied.
- Fixed Portable Teleporter not being triggered when using different control bindings other than mouse
- Fixed Peeper throwing errors when spawning/deleting. (Harmless bug but error nonetheless.)
- Fixed Sick Beats being disabled causing error when the player decided to turn it off through configuration after purchasing it.

## V 3.3.0 - 2024-02-27
### Additions
- Added "Charging Booster" upgrade which allows radar boosters to charge up player's held item by looking at the base of the booster. Has a cooldown after use which can be decremented by increasing the upgrade's level.
  - Configurable charge percentage on use.
  - Configurable cooldown time on use.
  - Configurable incremental cooldown decrease on upgrade levelling. 
- Added "Drop Pod Thrusters" upgrade which decrease the amount of time you need to wait for the store's drop pod to arrive on the moon.
  - Configurable timer for the drop pod to arrive 
- Added configuration to the amount of medkits that can spawn in the extraction contract
- Added interaction with doors to lockpick them to trigger the "Locksmith" upgrade's minigame. This way, you have an alternative to bump into doors to lockpick them.
- Added configuration value in which the contract reward value is influenced by the current profit quota you need to satisfy.
  - This value will be clamped between 0% to 100% so any different input other than inside this range will be considered the closest limit.

### Fixes
- Fixed medkit breaking when Stimpack upgrade is disabled
- Fixed Night Vision showing incorrect keybind when using LethalCompanyInputUtils

## V 3.2.5 - 2024-02-26
- Fixed Sick Beats icon being a white square instead of a boombox
- Fixed a couple of the items displaying a white square when held
- Fixed some interacts showing a white square, for the example of Data Retrieval's Laptop
- Fixed Sick Beats being disabled breaking mods which patch ``BoomboxItem.Start()`` (such as DiscJockey)
- Fixed Data Retrieval Contract "floppy replication" exploit
- Fixed bomb and scavenger contract items disappearing during save reboot due its initial state initialization ignoring being stored in the ship.
- Possibly fixed the duplicated non-interactable sample when Hunter is active
  - I'm unable to reproduce this issue in any shape or form consistently, if this issue persists, I need you to make a github issue and post your logs of when this happens to understand what is causing the duplication, both the host and any clients in the session and at least Debug, Info and Error log levels activated.

## V 3.2.4 - 2024-02-25
- Fixed Sick Beats being disabled breaking damaging enemies.
- Fixed Shopping Cart resetting its scrap value on reboot, discarding outside value changes such as GamblingCompany mod.

## V 3.2.3 - 2024-02-23
- Fixed disabled contracts breaking lgu store
- Fixed resetting all values breaking due to having singleton instances disabled, leading to NRE.
- Fixed not having any contract throwing NRE when trying to purchase one
- Fixed Beekeeper breaking hive spawns due to being disabled.
- Fixed Bargain Connections throwing error when active but not upgraded past the first level.
- Fixed Sick Beats being disabled breaking the stamina regeneration (which is also used by Bigger Lungs)

## V 3.2.2 - 2024-02-22
- Actually fixing Stimpack softlock error

## V 3.2.1 - 2024-02-22
### Fixes
- Fixed Bigger Lungs error spam when loading into the game
- Fixed Stimpack throwing error when reviving players due to lack of Singleton instance

## V 3.2.0 - 2024-02-22
### Additions
- Added keybind configuration for wheelbarrow's drop all items through configuration
- Added World Building text to some upgrades/items (thanks to Nat(discordID:twinkula))
- Added upgrade "Lethal Deals" which guarantees at least one item will be on sale while browsing the item store
- Added upgrade "Market Influence" which gives a guaranteed sale percentage applied on the item that goes on sale while browsing the item store. Increasing its level increases the guaranteed sale percentage and will only go up to the maximum allowed sale percentage of the item.
- Added upgrade "Bargain Connections" which increases the amount of items that can go on sale while browsing the item stores which can be further increased on level up.
- Added upgrade "Quantum Disruptor" which increases the amount of time you can stay on a moon which can be further increased on level up.

### Changes
- Changed "Fast Encryption" behaviour to allow vanilla transmit with faster typing and character amount being only limited by how many characters the terminal lets you type.
- Spawned scrap (monster samples and contracts) now influence the totalScrapValueInLevel which is displayed at the end of game stats
- Medkit now just increases the player's health instead of using DamagePlayer with a negative value.
- Changed the time of saving LGU's data from disconnecting to autosaving. This should solve the issue of buying an upgrade, leaving and coming back with credits back and upgrade on.
- Changed config values to ConfigEntry to allow in-game configuration mods to change the values (Note: LGU is not responsible for any breaking bugs that arise from changing configuration while in-game.)
- Changed samples' particles not being played when dropped due to FPS issues when in high quantity.
- Changed Night Vision Toggle and Wheelbarrow Drop All Stored Items' control bindings to allow be configurable with [LethalCompanyInputUtils](https://thunderstore.io/c/lethal-company/p/Rune580/LethalCompany_InputUtils/), implemented by [SnackSBR](https://github.com/SnackSBR)
    - Previous configuration values won't apply due to different formatting, have a look at [Unity's Control Paths](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.0/manual/Controls.html#control-paths) to understand what to put in the configuration file when not using LethalCompanyInputUtils.

### Fixes
- Fixed wheelbarrow cost using NV's cost instead of its own
- Fixed NV being given to everyone when its considered individual instead of shared
- Fixed TotalWeight restriction not being applied due to not being updated to new weight system
- Fixed Baboon Hawks getting stuck in grabbing items stored in a wheelbarrow, leading to them camping the wheelbarrow
- Fixed Shopping Cart (Scrap Wheelbarrow) scrap value not being applied on spawn due to MapObjects not having their scrap value synced.
- Fixed Medkit's current amount of uses not being synced between players, leading to each player have three uses out of one medkit with maximum of three uses.
- Fixed "scan enemies" showing "Unkown" instead of "Unknown" for enemies without a scan node associated (e.g Ghost Girl).
- Fixed (for like second or third time) Data Disk's "grabbable" area being blocked by the PC when it spawns.
- Fixed "NV Headset Batteries" showing as level 1 after purchasing multiple levels of it without using the night vision goggles. The effect would still apply after unlocking NV.
- Fixed "Enable Contracts" configuration not being used in the code

### Code Changes (developer level)
- Spawned scrap now use a component called ``ScrapValueSyncer`` which is used to change the item's scrap value for every player in the game.
- Refactored upgrades to be more streamlined to create an upgrade and added documentation to each abstract upgrade class
- Refactored RPCs to respective handlers to relieve ``LGUStore``'s responsiblities
- Removed useless code
- Changed from storing the json alongside the game's save to storing inside the game's save (this should reduce amount of issues with mods like LCBetterSaves).
    - Any previous saves in which they have the first case will be stored in the game's save when detected and delete the outside json file so resets should not happen when updating.
- Implemented handler for ``ScanNodeProperties`` when creating or changing its attributes for easier maintenance.
- Abstracted ``WheelbarrowScript``'s ``SetupScanNodeProperties()`` to not force the base class to know which derived class it is.
- Created handler for ``TerminalNodeList`` manipulation and for "help"'s ``TerminalNode`` manipulation to add information related to LGU's commands to not clutter ``TerminalPatcher``
- Changed from each bool and int variable stored in ``UpgradeBus`` representing active and level respectively into dictionaries which allows more streamlining in upgrade implementation as they no longer need to know what variable they are referring to for manipulation.
    - Any previous saves will be attempted to gather the data from them to store into the new dictionaries to not lose upgrades.

## V 3.1.0 - 2024-1-19
Additions
- Shutter Batteries Upgrade
    - Increase the amount of time the doors stay closed.
- Scrap Insurace
    - Insure the scrap in your ship for your next moon visit.
- Contracts can be purchased for specific moons for an increased price.
- Press middlemouse button to drop all items in a wheelbarrow.

Changes
- Contract object spawn location fixes.
    - Contract objects now spawn one unit forward of enemy vents.
    - This will prevent contract objects from spawning in unreachable areas or in walls.
    - Additionally this will provide balance to objects spawning too close to the entrance sometimes.
    - By default they will spawn at the vent furthest from the entrance but can be configured to spawn at a random vent.
- Available contracts can be removed via the config.
- Lightning rod now makes it so power doesn't go out when ship is struck.
- Can no longer do the same contract twice.
- Misc section in config is now `_Misc_` so it shows at the top of the file. If you don't delete your config you will still have the old `Misc` section that won't do anything.
- Scrap Wheelbarrow rarity value has been inverted to be more intuitive. It's now 0.1 by default (10%). If you have your old config it will be 0.9 (now 90%)!
- Contracts now support modded moons.
- Better descriptions have been added to the config for shared upgrades.

Fixes
- Wheelbarrow global sound and behaviours fixed.
- Some contract details not syncing to remote clients properly.
- Exorcism failure missing a failure step on some moons.
- Active contract not going away has been fixed.
- Dropping an item on a pentagram instead of placing it will no longer make it irretrievable.
- Fix locksmith not working after loading save.
- Fix sick beats from not loading in save.
- Enable wheelbarrow = false fix.
- Particle effects on samples not disabling.

*That's all that's coming to mind but there's probably a more I'm forgetting*

## V 3.0.4 - 2024-1-16
Fixed ritual site sync (scan node and item placement), wheelbarrow weight calculation, data pc local user input, bruteforce command vanilla integer overflow, demonic tome value, multiple contract objects appearing if lobby was recreated, more probably

entering `contract` when you already have a contract now also displays the `info contract` message.

lowered volume of extraction scavenger, added config option for volume for extraction scavenger (set to 0 to mute, still attracts enemies), increased interval between voice clips for extraction scavenger, and if he is inside the ship his voice clip interval is trippled. Should make him a little less annoying.

## V 3.0.3 - 2024-1-15
More fixes, Data PC contract object not working on remote client, wheelbarrow polish, more

Bugs being worked on: Bomb can spawn kind of inside the wall and be impossible to defuse / read serial code. Apparently multiple contract objects can spawn? EX: 3 scavengers to extract will spawn.

## V 3.0.2 - 2024-1-15
Just some bug fixes, most importantly not being able to move after dying lol.

Additionally issues with disabling items in the config should be resolved.

Wheelbarrows, shopping carts, and the scavenger you rescue in the extraction contract aren't being saved by the ship at the moment even though they should be. Being investigated but we're both off for the night so I thought I'd get this out first.

## V 3.0.0 - 2024-1-14
*This is probably my favourite update so far and should breathe a lot of life into the game.*
### Contracts
Contracts are difficult team oriented missions that serve two purposes: extra income and incentive to visit non high tier moons.  
Enter contract into the terminal to receive a random contract for a random moon.  
*As usual all contracts are completely configurable.*  
Currently there are 5 contracts implemented (click to expand):
<Details>
  <summary><b>Exorcism Contract</b></summary>
    <ul>
      <li>Paranormal activity has been detected in the facility at the given moon.</li>
      <li>You must find the ritual site, discover what type of ghost you have to exorcise, enter `demon GhostType` into the terminal to get the correct ritual instructions.</li>
      <li>Collect ritual items and correctly conduct the ritual to banish the ghost and get your loot.</li>
      <li><Details><summary>Click for consequences of failure spoiler</summary>A satanic chant will start, the site will explode, and ghost girls will spawn on the site.</Details></li>
      <li>10 ghost types</li>
      <li>5 ritual items</li>
    </ul>
</Details>
<Details>
  <summary><b>Data Contract</b></summary>
    <ul>
      <li>An active device has been detected in the facility at the given moon.</li>
      <li>You must find it then 'hack' it and retrieve a valuable .db file.</li>
      <li>To do this you have to enter the devices IP address into the terminal with the bruteforce command.</li>
      <li>Then you can login with the credentials and start looking for the file through the terminal.</li>
      <li>use `ls` `cd` and `mv` ls lists the files in that directory, cd changes to a directory (.. or ~ to go back) and mv moves a file (use to win the game).</li>
      <li>EX: `cd someDirectory`, `cd ..`/`cd ~`, `ls`, `mv survey.db`</li>
    </ul>
</Details>
<Details>
  <summary><b>Exterminator Contract</b></summary>
    <ul>
      <li>The facility at the given moon has become overun with Hoarding bugs.</li>
      <li>You must find and destroy their nest.</li>
      <li>The bugs are very aggressive. Work tactifully with your team to ensure your success.</li>
      <li>Hold E on the nest to destroy it and get the loot.</li>
      <li>Pair with the hunter upgrade to get filthy rich.</li>
    </ul>
</Details>
<Details>
  <summary><b>Bomb Defusal Contract</b></summary>
    <ul>
      <li>A bomb has been planted in the facility at the given moon.</li>
      <li>You must locate and defuse it.</li>
      <li>Each bomb will have three wires, a timer, and a serial number.</li>
      <li>Enter `lookup SerialNumber` in the terminal to get defusal instructions.</li>
      <li>Cutting the wrong wire is fatal, entering the wrong serial number will give you incorrect instructions.</li>
    </ul>
</Details>
<Details>
  <summary><b>Extraction Contract</b></summary>
    <ul>
      <li>An operative from another crew has been lost in the facility on the given moon.</li>
      <li>You must find and retrieve them.</li>
      <li>You will need to bring or find a medkit to heal them then carry them out.</li>
      <li>Animated NPC with 20 custom voicelines and 3 different states.</li>
      <li>Very loud.</li>
    </ul>
</Details>

## V 2.8.8 - 2024-1-9
Just hotfixes of reported 2.8.7 bugs.

## V 2.8.7 - 2024-1-9
*Small changes for full compatibility with v47*

- additions
    - compatability: movement speed, jump height, and stamina safely uses += & -= instead of = for better mod compatability.
    - New Upgrade / purchasable: Extend deadline. Pay x amount of money to buy another working day.
    - The config for hunter is much more complex, sample values and what enemies you can gather samples from on each tier.
    - Added LGU info to help command.
    - More logging for easier debugging.

## V 2.8.3 - 2023-12-25
- additions
    - Bigger lungs new behaviours (increased stamina regen decreased jump cost final upgrade reduced fall damage)
    - Light Footed has been merged into running shoes (activates on final upgrade)
    - Beekeeper final upgrade increases hive price
    - Discombobulator does damage final upgrade
- fixes
    - Peeper fix
    - Night vision alternate behaviour fixes
    - portable tele drop items fix
    - Protein powder spider fix
    - more

## V 2.8.0 - 2023-12-23
- Hunter
    - Allows you to collect and sell samples from dead monsters
    - each tier unlocks more monsters
- Stimpack
    - Increase max health
    - completely configurable of course
- Medkit
    - Item - left click to heal
    - Limited uses (configurable)
    - Heals for 20 (configurable)
- Diving Kit
    - Heavy, two handed, low visibility
    - Can breathe underwater
- Protein Powder crits
    - Final upgrade of protein powder unlocks the chance to deal a critical hit
- Quite a few Fixes
- [LC Better Saves](https://thunderstore.io/c/lethal-company/p/Pooble/LCBetterSaves/) Compatibility!

## V 2.7.0 - 2023-12-21
- Lightning Rod
    - The closer you are to the ship, the more likely the lightning will target the ship instead of your metal object.
    - change `effective distance of lightning rod` in the cfg to tweak this.
- Fast Encryption
    - This is just the Pager if anyone remembers the pager upgrade with some minor tweaks.
    - I removed the pager when the transmitter came out but I like using this more so it's back in.
    - You need a signal translator purchased to use this, overrides the vanilla transmit so it instantly sends the message discretely to all clients and plays a sfx.
- Better Scanner Overhaul
    - The first of the complex upgrades
    - Also a bit of a nerf as scanning through walls instantly was very op
    - Level 1 - Increase scan distance
    - Level 2 - Unlocks 5 new commands
        - `scan enemies` provides a list of enemy types and their count (or just their count see cfg)
        - `scan scrap` returns the 5 most valuable scrap items in the dungeon and their coordinates
        - `scan player` returns a list of players and their coordinates sorted into dead and alive
        - `scan doors` if targeted player is inside, returns the 3 closest exits, if outside, returns the entrances
        - `scan hives` returns a list of hives, their price, and coordinates
    - Level 3 - scan scrap through walls and configurably enemies (off by default)
- Night Vision Refactor
    - The night vision system has been refactored and should be simpler to configure
    - `info nv headset batteries` also provides the exact time in seconds it will take each level to drain and regen
    - Added an option to increase the range and intensity of night vision on level up
        - You could now for example set up your night vision to be infinite but each upgrade increases how far you can see in the dark with it.
    - a new config should be generated as the config values operate in a much different range now (should happen automatically)
- Fixes including
    - Night Vision Only working for host (config serialization error)
    - Varius intern issues
    - load lgu command not using steam names

## V 2.6.0 - 2023-12-14
- Config syncing
    - All config options are now synced excluding ***item*** prices and enable/disable status.
    - Options regarding their behaviour are still synced.
    - Changes to Peeper, Teles, and Night Vision goggles will have to be manually synced!
    - Everything else is automatic.
- Intern fixes
    - Weather persisting when inside facility.
    - Not being able to teleport intern.
    - normal HUD not enabling.
- Config options
    - Enable/disable splashscreen
    - If Misc/Shared Upgrades = True, late joining players will copy the hosts upgrades.
- Small fixes
    - Protein powder saving
    - sales only refreshing for host
        - Also added it so if shared upgrades = true sales are synced
    - fixed info for a few upgrades
- New teleporter models from Sad Amazon

## V 2.5.0 - 2023-12-10

- Night Vision Changes
    - Purchase the night vision headset in the vanilla store.
    - It has a new model that will arrive in the dropship, pick it up and left click to equip it.
    - By default night vision is lost on death.

- Individual Upgrades
    - The save file is now a dictionary of steam IDs.
    - By default all upgrades will only be applied to the client that purchased them.
    - The config has a setting to make upgrades shared.

- More Config Options
    - You can now configure upgrade tiers via the `Price of each additional upgrade` field.
        - Must be formatted like so `123,321,213`
        - The above example will result in the respective upgrade having three unlockable tiers.
        - The cost of the first will be 123, the second will be 321, and so on.
        - Remove the entry to make it a non tiered upgrade.
        - This might get kinda depreciated as more complex upgrade behaviours emerge.
    - Shared or individual upgrade option.
    - Even more night vision config options.

- Incompatability Coping
    - Previous attempts to get player cap mods fully compatible didn't completely work.
    - If credit desync occurs you can type `syncCredits` to attempt the rpc again.
    - If that fails clients can type `forceCredits 123` to manually sync credits.
    - `load lgu` has been applied and tested to the new system and will reload upgrades that failed to apply.
        - Note that this should also rarely occur now.
    - `reset lgu` has been applied to the new system and now wipes only your clients upgrades.

- New splash type screen that displays only ever once.
    - other small fixes and polishing things like that.

## V 2.1.0 - 2023-12-7

- Pager Upgrade
    - Type `page <msg>` in the terminal to broadcast a message to players.
- Locksmith Upgrade
    - Activate by running into a locked door lol.
    - Strike the pins in the order they flash to unlock the door.
    - Striking a pin makes noise, failing the minigame makes a louder noise.
- Shorthand Commands
    - You can type just `lgu` for the store.
    - `cd` for discombobulator cooldown.
    - `atk` for discombobulator initattack.
- New `load LGU` command
    - Client side reapplication of spawned upgrades.
    - Use when LateCompanyClient desync has occured.
- New `reset lgu` command
    - Manually delete your LGU_x save file and cleans up.
- Netcode
    - lategame store now uses custom netcode to sync credits.
    - This hopefully resolves the credit desync issue when using player cap increasing mods.
- ModSync integration

## V 2.0.0 - 2023-12-3

- Custom Store
    - Type `lategame store` to access the custom store.
    - Prevents buying upgrades twice and tracks current upgrades.
    - Physical items (like portable tele) are still purchased through `store`.
- More Config Settings
    - Mainly the ability to change how much tiered upgrades change per level.
- Many many many fixes
    - Better safer netcode
- Custom Saving and Loading and Syncing system
    - This was nescessary for tiered upgrades.
    - Should increase compatibility and loading consistency.
    - LGU save files are in LocalLow/ZeekersRBLX/Lethal Company

## V 1.3.1 - 2023-11-29

- Mainly just fixes in this one.
- Night vision
    - Battery is consumed when turned on (Default 10%).
    - If battery is exhausted it stays depleted for 2 seconds.
- Config is now sectioned for easier editing.

## V 1.2.5 - 2023-11-28

*This version should give users the tools to make the mod feel
like it was built for their playstyle.*

*It should go without saying config settings like price and
enabled/disabled should be synced on all clients.*

*Settings like Night Vision color and whatnot can be 
client side.*

### New
- Config is here
    - 48 configurable fields.
    - Enable and disable upgrades.
    - Change prices.
    - Alter attributes of upgrades.

### Nerfs
- Beekeeper
    - Costs more ($450).
    - Now reduces instead of negates damage(-75%).
    - Cost and damage reduction are config tweakable.
- NightVision
    - Costs more ($700).
    - Has a battery life.
    - Both cost, battery duration, and more are config tweakable.

### QOL
- Discombobulator
    - If enemies are hit it counts down for how long they're stunned.
    - Night Vision color, range, and intensity can be changed via config.

## V 1.2.0 - 2023-11-27
- Proper NetCode
    - No more teleporter button desync.
    - Fixed Malware Broadcaster.
- Shortform Commands for discombobulator
    - 'atk' for 'innitattack'
    - 'cd' for 'cooldown'
- Malware Broadcaster is back
    - Instead of just despawning targets it now explodes them.
    - Tell your friends to stand back.
- Small fixes
- Back Muscles now reduces weight by 50%
- Advanced teleporter has a 20% chance to break

## V 1.1.4 - 2023-11-27
- Malware Broadcaster Patch
    - Temporarily removed until bugs are ironed out.
- Small Fixes

## V 1.1.2 - 2023-11-27
- Info commands
    - type `info <item>` in terminal to learn about it.
- Mod info
    - type `lategame` in terminal to get mod info.
- View Purchased Upgrades
    - type 'unlocks' in terminal to get a list of activated upgrades.
- Purchased Upgrades Are No Longer Removed From Store Interface
    - This is done in an attempt to remedy a bug where the store indexes incorrectly.
- Better Scanner Fix
    - Range calculation no longer changes ScanNode object maxRange value.

