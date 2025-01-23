<details>
<summary> 3.11.2 - 2025-01-23 </summary>

- Implemented configuration for interns:
  - Amount of revives allowed per moon landing. "-1" represents infinite.
  - Delay (in seconds) between each player revive.

</details>

<details>
<summary> 3.11.1 - 2025-01-22 </summary>

- Added compatibility with [Brutal Company Minus](https://thunderstore.io/c/lethal-company/p/DrinkableWater/Brutal_Company_Minus/) related to Midas Touch upgrade where the generated scrap items (both from events and hazards) would ignore the upgrade's state. (Thank you [TI](https://github.com/tixomirof))
- Added Company Cruiser's Radio (and other possible vehicles which follow the same logic as Company Cruiser) to the Sick Beats interaction to obtain buffs from the upgrade.
- Fixed issue with host's save being changed whenever the first client joined the lobby.
- Fixed issue with NV configuration cases where it would error (leaving prices configuration as blank).
- Fixed issue with Beekeeper's increased scrap value of hives not working correctly. (Thank you [TI](https://github.com/tixomirof))
- Fixed issue with random contract not working correctly when free moons setting was enabled.
- Fixed issue with Reinforced Boots not applying when the player is using a jetpack.
- Altered interaction between Randomize Upgrades mode and Item Progression modes of UniqueScrap and CustomScrap to not add contribution to the hidden upgrades at all
  - Earlier behaviour was you would still gain contribution towards the upgrade, it would just not trigger the purchase. Problems appeared though as after being visible in the store, it would display a negative price value due to the high amount of contribution you gained. 
- Implemented item categories in Fusion Matter item configurations:
  - All: any kind of item will be kept in the player's inventory during teleportation
  - Tools: Items which are not considered scrap will be kept in the player's inventory during teleportation
  - Scrap: Items which are considered scrap will be kept in the player's inventory during teleportation
- Implemented configuration to restrict upgrades being buyable to the first ever purchasing player.
  - Upgrades which are restricted will appear disabled in the store and if prompted, will show a message stating the upgrade can only be bought by somebody else. 
  - Added configuration to show/hide upgrades restricted to other players.
</details>

<details>
<summary> 3.11.0 - 2024-12-01 </summary>

- Implemented tier upgrade which increases the health regeneration cap of the vanilla health regeneration mechanic.
- Implemented tier upgrade which increases the amount of health you regenerate through vanilla health regeneration mechanic.
- Implemented Upgrade API to facilitate modding around LGU's upgrades.
  - Allows retrieving data related to the upgrades of the session (All, Visible, Purchaseable, Rankable);
  - Allows requests of triggering upgrade rank up;
  - Allows requests of contributing towards an upgrade.
  - Requests require the name of the upgrade (either original or overriden) or the node associated to the upgrade to be considered valid.
- Implemented Discombobulator API to facilitate triggering callbacks to the Discombobulator upgrade.
  - Allows checking if the discombobulator can be fired to stun nearby enemies.
  - Allows checking if the Discombobulator upgrade is enabled in the configuration.
  - Allows requests of firing the Discombobulator mechanic (so long as it is able to fire);
  - Allows requests of setting/incrementing/decrementing the current cooldown of the Discombobulator.
    - The calculated value will not pass the boundaries of zero to the configured maximum cooldown of the Discombobulator.
- Implemented Quantum Disruptor API to facilitate triggering callbacks to the Quantum Disruptor upgrade (more specifically the revert mode)
  - Allows checking if the Quantum Disruptor upgrade is enabled in the configuration.
  - Allows checking if the Quantum Disruptor revert time command is enabled in the configuration.
  - Allows checking the reset mode used to reset the revert time usages value.
  - Allows checking the current/maximum amount of revert time usages per moon landing and amount of hours reduced per usage.
  - Allows checking if the Quantum Disruptor revert time command can be executed. (along with display message in case of error)
  - Allows requests of firing the Quantum Disruptor revert time command.
- Added world-building text to some upgrades. (Thank you Nat(discordID:twinkula))
- Fixed contribution values from Item Progression mode not being used correctly during save reboot.
- Item Progression modes of UniqueScrap and CustomScrap will not rank up upgrades hidden from Randomize Upgrades mode.
- Made Efficient Engines discount price effect also apply on [Lethal Constellations](https://thunderstore.io/c/lethal-company/p/darmuh/LethalConstellations/)'s constellation prices.
- Added configuration for Sick Beats to make the boombox music not attract nearby enemies when purchased.
- Added configuration for Discombobulator to blacklist enemies from its effect when fired.
  - It checks both the internal names stored in its data or the header text shown in the scan node for equality in the blacklist.
- Fixed issue with GoodItemScan Compat with wrong logic being applied.
- Fixed issue with Item Progression leading to upgrades not having a contribution value assigned to them.
- Fixed issue with Contract objects showing radar icons even when they are destroyed.
- Fixed issue with Fusion Matter ignoring Back Muscles weight reduction when keeping items in your inventory.
- Fixed issue with Quantum Disruptor Revert Time command not being able to be called by non-host players.
- Fixed issue with Interns Teleport Restriction configuration making the teleport cooldown ignored.

</details>

<details>
<summary> 3.10.11 - 2024-10-27 </summary>

- Hotfixed issue with patching

</details>

<details>
<summary> 3.10.10 - 2024-10-27 </summary>

- Fixed Quantum showing values times 100 instead of their normal values.
- Fixed Particle Infuser being disabled leading to incorrect behaviour.
- Fixed Night Vision Goggles not showing a scan node.
- Fixed NV Headset upgrade not being shown when purchased in Randomize Upgrades mode on.
- Fixed issue with upgrades not shown when configured first tier free and remaining with a price.
- Made Scrap Keeper affect items stored in the chute from [ShipInventory](https://thunderstore.io/c/lethal-company/p/WarperSan/ShipInventory/)

</details>
<details>
<summary> 3.10.9 - 2024-10-23 </summary>

- Fixed math issue with Quantum Disruptor due to multiplier starting at 1.4 instead of 1.0.
  - Configuration was changed to consider percentage instead of raw multiplier.
- Fixed issue with random upgrade seed not being properly loaded when booting up a game lobby.
- Fixed issue with v65 game release due to sales being changed.

</details>

<details>
<summary> 3.10.8 - 2024-10-21 </summary>

- Made a better fix for the teleporter patching due to DropAllHeldItems method being removed from vanilla code.
- Fixed issue with not being able to teleport players when revived in The Company through Interns.

</details>
<details>
<summary> 3.10.7 - 2024-10-21 </summary>

- Fixed issue with teleporter patching due to DropAllHeldItems method being removed from vanilla code.

</details>

<details>
<summary> 3.10.6 - 2024-10-20 </summary>

- Hotfixed issue with another mod.

</details>

<details>
<summary> 3.10.5 - 2024-10-20 </summary>

- Implemented one time upgrade which allows shotguns eliminate ghost girls.
- Implemented tier upgrade which allows the players to teleport with some items in their inventories according to the tier they are located.
  - The tiers mechanism is similar if not the same as the Hunter: you specify a list of items which is then separated by another delimiter to separate between tiers.
  - You specify either the names listed in the Company Store or the name displayed in the scan node (or their internal name stored in the ``Item`` instance of the game's code)
- Implemented tier upgrade which increases the shotgun's damage output.
- Implemented tier upgrade which increases the shotgun's overall range and its effective damage ranges.
- Implemented tier upgrade which increases the teleporter's speed.
- Implemented tier upgrade which in case of a full team wipe, each scrap item present in the ship has a chance of not being discarded.
- Fixed issue with medkit on Extraction contract due to different names.
- Fixed typo in some configuration entries.

</details>

<details>
<summary> 3.10.4 - 2024-10-05 </summary>

- Added configuration for interns for teleportation restriction on the revived player.
- Fixed text typo in "Jetpack Thrusters" info.
- Fixed issue with Jet Fuel configuration influencing Jetpack Thrusters in some aspects.
- Fixed text typo in Sick beats description and added configuration to allow stamina buff apply on stamina drain overtime scenarios.
- Tweaked world building text on some upgrades and added them aswell. (Again, thank you Nat(discordID:twinkula))

</details>

<details>
<summary> 3.10.3 - 2024-09-19 </summary>

- Implemented "Jetpack Thrusters" tier upgrade which increases the maximum speed of the jetpack while in flight.
- Implemented "Jet Fuel" tier upgrade which increases the acceleration of the jetpack while in flight.
- Implemented "Midas Touch" tier upgrade which increases the value of scrap found in the moons.
- Implemented "Quick Hands" tier upgrade which increases the player's interaction speed.
- Added configuration to always show items associated to upgrades when Item Progression Mode is toggled on.
- Changed Sigurd Access' configuration setup to be similar to most upgrades.
- Changed upgrade display to show the price number red if the players do not have enough credits to purchase it.
- Changed upgrade store to show first pick between shared and individual upgrades if any are configured as so and show the list of upgrades when selected.
  - If all enabled upgrades are either individual or shared, it will do the same behaviour as before.
- Fixed issue with Item Progression Mode selecting items that are not considered scrap.

</details>

<details>
<summary> 3.10.2 - 2024-09-15 </summary>

- Displayed control binding used for sorting in the store application.
- Changed colouring of maxed upgrades to dark green to distinguish between maxed upgrades and upgrades that you are unable to purchase due to lack of credits.
- Fixed issue with Landing Thrusters only applying on host.
- Fixed issue with sales re-emerging after purchase when leaving and creating a new lobby.
- Fixed issue with item names being case-sensitive in the config related to Item Progression.
- Fixed issue with clients buying upgrades wouldn't consume credits due to error in RPC calls.
- Possibly fixed issue with ChancePerScrap mode due to using integers rather than floats.
- Fixed issue with Interns enabled configuration not working correctly.
- Fixed issue with saving not working correctly in relation to individual settings.
- Code refactored.

</details>

<details>
<summary> 3.10.1 - 2024-08-29 </summary>

- Implemented "Life Insurance" tier upgrade which reduces the credit loss when leaving a body behind.
- Implemented "Carbon Kneejoints" tier upgrade which reduces the movement speed loss while crouching.
- Added configuration entry to make upgrades not purchaseable when using Item Progression Mode.
  - You will still be able to see the upgrades for their current level and related info, just not the option of purchasing it. 
- Changed logic of ChancePerScrap mode on Random to be index logic rather than loop logic which was leading to always picking the last upgrades more often than it should.
- Possibly fixed issue with Back Muscles when using Lethal Company Virtual Reality mod.
- Possibly fixed issue with Mechanical Arms when using Lethal Company Virtual Reality mod.
- Fixed issue with Oxygen Canister and Rubber Boots override name taking Reinforced Boot's, leading to confusion when chat prompt appears.
- Fixed issue with Oxygen Canister using Reinforced Boot's incremental value in its information description.
- Fixed issue with sounds with a couple items.
- Code cleanup and refactoring.

</details>

<details>
<summary> 3.10.0 - 2024-08-28 </summary>

- Implemented "Oxygen Canister" tier upgrade which reduces the oxygen consumption rate of the player.
- Implemented "Rubber boots" tier upgrade which reduces the movement hinderance from walking on water surfaces.
  - This also includes the quick sand patches during the Rainy weather.
  - If the percentage decrease reaches or exceeds 100%, you will not be slowed down when walking through it.
  - This reduces how much movement hindurance you get from walking on water surfaces, not how much you're hindered by which is always by half multiplied by outside hinder multipliers such as this one and cobwebs.
- Fixed NRE being thrown when the player is still in two-handed mode and having no object in hand. 
- Fixed another NRE issue related to upgrade terminal nodes due to Item Progression mode.
- Fixed Night Vision Goggles throwing NRE when you have the respective upgrade disabled.
- Fixed Market Influence not working correctly due to additional sale methods being introduced for vehicles.

</details>
<details>
<summary> 3.9.15 - 2024-08-19 </summary>

- Fixed Item Progression mode "ChancePerScrap" mode, specifically the "Random" one, not working correctly leading to always upgrade the first ever upgrade in the list.
- Prevented grabbable objects which have enemy scripts attached and they are not considered dead to not be pocketed to prevent issues.
- Fixed issue with Landing Thrusters not acting correctly when LethalLevelLoader is loaded by changing the patched method to apply the buff when landing.
- Fixed issue with CustomScrap and UniqueScrap modes from Item Progression mode leading to NREs due to trying to increase another level of upgrades that can only have one.
- Fixed issue with not being able to die due inconsistency between Night Vision and its upgrade.
- Added Maneater Sample to the Hunter upgrade
  - Already generated configuration files will have to update the tier configuration to include one of: maneater, cave dweller, cavedweller, baby, babyeater.  

</details>

<details>
<summary> 3.9.14 - 2024-08-17 </summary>

- Fixed NV Headset Batteries granting night vision when bought the upgrade for the first time.

</details>

<details>
<summary> 3.9.13 - 2024-08-17 </summary>

- Fixed calculations related to Item Contribution CustomScrap and UniqueScrap modes, leading to reaching level ups sooner than expected.
- Fixed Night Vision related mechanics due to upgrade starting not ranked up rather than on first level.
- Fixed Interns used on players that died while in the vehicle to be stuck in their animations.
- Made Fedora Suit not stop buttlers from brushing the floor to not just appear looking at you forever without doing anything.
- Fixed error being thrown when disconnecting on a moon and previously bought upgrades before quitting the game.

</details>

<details>
<summary> 3.9.12 - 2024-08-12 </summary>

- Split Weather Probe and Helmet item into their own mods.
- Possibly fixed issue with saving.
- Increased time for the configuration to synchronize before starting to build stuff with it.

</details>

<details>
<summary> 3.9.11 - 2024-08-06 </summary>

- Linked items/commands mods in the README.

</details>

<details>
<summary> 3.9.10 - 2024-08-06 </summary>

- Fixed issue with saving
- Added compatibility with GoodItemScan mod with Better Scanner
- Changed the medkit's name (once again) to "Medic Bag";

</details>

<details>
<summary> 3.9.9 - 2024-08-05 </summary>

- Fixed the scan nodes from the new samples to display the correct names.
- Separated Company Cruiser related upgrades into their own mod for easier setup, issue solving and for people who do not wish to alter the Company Cruiser's stats in any shape or form through this mod.
  - Player, Item, Ship, Store and Enemy upgrades will remain in this mod as they are core mechanics of the game.

</details>

<details>
<summary> 3.9.8 - 2024-08-04 </summary>

- Added Kidnapper Fox and Spore Lizard samples to be used by the Hunter upgrade
  - Though Puffer can't die by vanilla behaviour, there are mods that allow that so upgrade is ready for that
- Fixed NRE issue from compatibility with CustomItemBehaviourLibrary
- Fixed issue between shotgun and Sleight of Hand when it's disabled

</details>

<details>
<summary> 3.9.7 - 2024-08-03 </summary>

- Hotfixed issue with interact due to how the check for CustomItemBehaviourLibrary mod installed is made.
- Also fixed same issues with LethalLevelLoader.

</details>

<details>
<summary> 3.9.6 - 2024-08-03 </summary>

- Moved items into their own following mods:
  - Portable Teleporters
  - Wheelbarrow
  - Shopping Cart
  - Peeper
  - Diving Kit
- Configurable random contract upon typing ``contract`` rather than interacting with the terminal application associated with contracts.
- Implemented "Sleight Of Hand" tier upgrade which increases the reload speed of the shotgun item.
- Implemented "Hiking Boots" tier upgrade which reduces the slope effect on your movement. 

</details>


<details>
<summary> 3.9.5 - 2024-07-30 </summary>

- Fixed typo in Charging Booster's upgrade information
- Fixed extra increment on Protein Powder's damage increase in its upgrade information
- Fixed player's saves not being loaded correctly when ""upgrades being shared"" was set to false in configuration
- Fixed "Efficient Engines" discount being applied to the Company Cruiser acquisition rather than Moon Routing
- Fixed "Beekeeper" upgrade information not describing the final upgrade scrap value increase on beehives
- Fixed medkit item from this mod being disabled removing all other medkit related items from the store (through renaming to "Old Medkit")
- Included check for moon being locked (through mods such as Selene Choice) when selecting a moon for contract random selecting
- Implemented "Hiking Boots" upgrade which decrease the effect of climbing slopes in terrain
- Implemented configuration to toggle showing world-building text of some upgrades.
- Separated "Extend Deadline" and "Scrap Insurance" from this mod into their own respective mods.
  - This mod will not be dependent on them: you can decide either to use them or not. 

</details>

<details>
<summary> 3.9.3 - 2024-07-12 </summary>

- Fixed Traction Boots not applying on sprinting movement.
- Fixed upgrade store breaking due to having no entries when configured so.
- Changed Fluffy Seats to also include the instant kill when hitting too hard on an obstacle
  - This will not prevent you from death when the car gets destroyed and you die due to the explosion. Only Inertia related damages will be mitigated by this upgrade. 

</details>

<details>
<summary> 3.9.2 - 2024-07-12 </summary>

- Implemented "Traction Boots" upgrade which increases the player's traction force to the ground to allow easier movement direction change.
- Fixed showing limited amount of entries in the upgrade store due to lack of bool check for randomized upgrades mode.
- Fixed free upgrades not being applied again after players get fired from The Company.

</details>

<details>
<summary> 3.9.1 - 2024-07-11 </summary>

- Implemented "Randomized Upgrades" mode which makes the upgrades be randomly selected to appear in the shop rather than the full list
  - You can configure the amount of upgrades that can be picked to show up.
  - You can configure to show the upgrades already purchased.
  - You can configure when should the upgrades be randomized again (per quota, per moon routing or per moon landing).
- Implemented "Fluffy Seats" tier upgrade which adds player damage mitigation when driving the Company Cruiser vehicle.
- Implemented "Ignition Coil" tier upgrade which increases the chance of igniting on the Company Cruiser vehicle.
- Implemented "Weed Genetic Manipulation" tier upgrade which increases the effectiveness of the Weed Killer item on eradicating plants.
- Implemented "Fedora Suit" one time upgrade where makes the butler enemies not be angry at you when you are alone (they will still attack you if you attack them)
- Implemented "Turbo Tank" tier upgrade which increases the maximum capacity of turbo the Company Cruiser vehicle can hold.
- Fixed issue with some relevant components not being saved correctly, leading to constantly changing per save boot.

</details>

<details>
<summary> 3.9.0 - 2024-07-06 </summary>

- Implemented "Vehicle Plating" tier upgrade which increases the Company Cruiser's maximum health.
- Implemented "Rapid Motors" tier upgrade which increases the Company Cruiser's acceleration.
- Implemented "Supercharged Pistons" tier upgrade which increases the Company Cruiser's maximum speed.
- Implemented "Improved Steering" tier upgrade which increases the Company Cruiser's turning speed.
- Implemented alternative modes for Lightning Rod such as:
  - Always redirect all lightning bolts meant for items.
  - Always redirect all lightning bolts targetting random locations.
  - Always redirect all kinds of lightning bolts.
- Refactored items and config for easier maintenance.
  - Wheelbarrow was the most affected so it's likely the settings will reset for them.
- Fixed issue with Lightning Rod not functioning correctly due to the patches not being applied.
- Fixed issue with events related with explosions not functioning due to changes from v56 release of the game.

</details>

<details>
<summary> 3.8.2 - 2024-06-26 </summary>

- Implemented another Back Muscles alternative mode where you can reduce the carry weight's influence on stamina consumption when running.
- Added info for Back Muscles alternative modes.
- Fixed issue with v55 release related to item drop ship no longer leaving after getting the items.
  - Should still allow compatibility between v50 and v55 releases.
- Fixed issue with wheelbarrow weight calculation giving wrong results due to misplacement of parentheses.

</details>

<details>
<summary> 3.8.1 - 2024-06-25 </summary>

- Fixed issue with Back Muscles leading to additional 100lb being added to the player due to negative weight countermeasures.
- Added configurability of allowing items being assigned to multiple upgrades.

</details>

<details>
<summary> 3.8.0 - 2024-06-25 </summary>

- Implemented an alternative "Item Progression" mode where you can acquire upgrades based on the items you sell alongside using credits to purchase them.
  - CustomScrap: Each upgrade has a configuration option where you specifiy the several items that will contribute to the upgrade.
    - Each item contribute through their scrap value sold multiplied by a configurable multiplier for contribution.
    - When the items are sold and if they contribute to an upgrade, they will be displayed in the upgrade's screen display under "Discovered Items"
  - UniqueScrap: All items present in the game will be attributed to an upgrade, ignoring item specification configurations.
    - Each item contribute through their scrap value sold multiplied by a configurable multiplier for contribution.
    - When the items are sold and if they contribute to an upgrade, they will be displayed in the upgrade's screen display under "Discovered Items"
  - NearestValue: Whenever a quota is met, it will purchase the upgrade closest to the met quota value.
    - The quota value is rounded down meaning if there are no upgrades with a price lower than the quota, it will not purchase any upgrade.
  - ChancePerScrap: Whenever a scrap item is sold to the company, it will roll a value between 0 and 1 and checks if it's lower than the configured value for chance
    - If it meets the criteria, it will pick an upgrade to purchase based on the criteria of randomenss:
      - Random: Pure random and does not take any consideration of what the upgrades have.
      - Cheapest: Picks the upgrade with the lowest price to purchase.
      - Lowest Tier: Picks the upgrade with the lowest level to purchase.
  - Apparatice: Whenever an apparatus is sold, a random upgrade will be purchased.

- Implemented alternative mode for Back Muscles as it will only reduce the speed debuff of the carry weight on the player. The stamina debuff will still apply in this mode.

- Refactored game attribute relevant upgrades (Running Shoes, Bigger Lungs, etc.) such as the effects are not cancelled when other mod is setting some value to the same variables they are interacting with.
  - This means that any mods that requires to know the full value of the attribute variables, they no longer can just check the variable as the increments are added separately.

- Fixed issue with Defusal contract's ``Lookup`` command not working due to string manipulation.
- Fixed NRE being thrown due to PlayerManager not being initialized before relevant code is executed.
- Fixed issue with Locksmith not locking your movement, leading to happy accidents happening.
- Fixed issue with Quantum Disruptor leading to consuming extra days from deadline due to lack of resetting in the deadline.

</details>

<details>
<summary> <h2> 3.7.2 - 2024-05-27</h2> </summary>

- Added tier upgrade 'Scavenger Instincts' which increase the average amount of scrap it can spawn in a given level.
- Added tier upgrade 'Mechanical Arms' which increase the interaction range of the players (both grabbing items and opening/closing doors as example).
- Added configuration for item dropship leaving x seconds (default being zero) after being opened for their items when "Drop Pod Thrusters" upgrade is active.
- Fixed contract purchase not being synced to all clients present in the session when it's made.
- Possibly fixed the contract saving not being executed properly.
- Fixed upgrade name overrides being misplaced.

</details>

<details>
<summary> <h2> 3.7.1 - 2024-05-20</h2> </summary>

- Fixed issue with upgrades being disabled leading to crashes/unpredictable behaviour.

</details>

<details>
<summary> <h2> 3.7.0 - 2024-05-20</h2> </summary>

<details>
<summary> <h3> Additions </h3> </summary>

- Added configurable incremental price factors to ``Extend Deadline`` per quota and per day extended.
    - Due to this, an interactive UI was made for ``extend deadline`` command where you can select the amount of days you wish to extend and shows the amount of credits you will spend on that amount.
- Added interactive UI for contracts accessed through ``contracts`` command where you can select the same command prompts from typing previously.
- Added configurable toggle for Deeper Pockets allowing pocketing wheelbarrows (allowing you to switch between items when carrying a wheelbarrow) or not.
- Added configurable toggle to allow scan nodes on purchased items (Wheelbarrow, Peeper, Helmet, etc..).
- Added alternative mode for ``Quantum Disruptor`` upgrade to revert time by x hours and can only be used y times which resets at a given point (per moon landing, routing or new quota).
- Added alternative mode for ``Helmet`` item to partially mitigate damage and only break when the next hit on the player would kill them.
- Added prototype of Upgrade API to register upgrades outside of this mod.
    - Early stages of production, will need to use it to know if it works as expected.
    - You still have to do your own logic of the upgrade through patches/variable changes. You can access the status of the upgrade through ``BaseUpgrade.GetActiveUpgrade(upgradeName)`` and ``BaseUpgrade.GetUpgradeLevel(upgradeName)``
    - If any issues arise from using the API, report them in the github repository.

</details>
<details>
<summary> <h3> Changes </h3> </summary>

- Changed Probe's interactive menu to disable weather entries when conditions are not met (not enough credits or the weather is already in place)

</details>

<details>
<summary> <h3> Fixes </h3> </summary>

- Fixed Hunter samples spawning on Manticoils and Tulip Snakes when the transition from day to night happens and the daytime enemies despawned on that transiction.
- Fixed Sick Beats preventing enemies from damaging the player when disabled.
- Fixed Helmet's scan node appearing above the item rather than on the item.
- Fixed Helmet not appearing in the player's hand when held.
- Fixed some Hunter sample models not showing the name of the monster they were generated from.
- Optimized sample models to not have many (many (many)) vertices which consume unnecessary C/GPU computation power.
- Fixed issue with interacting with Data Contract's PC leading to player being able to move around when interacting with its UI.

</details>

</details>

<details>
<summary> <h2>V 3.6.5 - 2024-05-05</h2> </summary>

- Changed the upgrade store to:
    - Show inactive entries when you don't have enough credits to purchase or they have reached maximum level
    - Sort the entries by alphabetical and by price (either ascending or descending)
        - You can sort through ``InteractiveTerminalAPI``'s input binding to sort (default being 'f')
        - The current sort is displayed on the bottom right of the application screen
- Fixed item dropship landing cases where it would land faster than the players' ship, leading to the dropship leaving early and not allowing players to grab the purchased items.
	- When the upgrade is purchased/active, the dropship will wait for the players' ship to land to then check if it can land the item dropship.
- Fixed interaction between Deeper Pockets and Shopping Cart that would lead to the player being unable to interact.
- Fixed sales still showing in the upgrade store when the upgrade is already maxed.
- Fixed credits being wasted when bought upgrades and disconnecting after.
- Fixed some upgrade information texts displaying incorrect values.

</details>
<details>
<summary> <h2>V 3.6.4 - 2024-05-01</h2> </summary>

- Actually fixed Hunter not dropping new samples due to internals not being updated

</details>
<details>
<summary> <h2>V 3.6.3 - 2024-05-01</h2> </summary>

- Fixed Hunter not dropping new samples due to internals not being updated
   - Default has also been updated to account the new samples
- Added interactive screen for the "Weather Probe" effect prompted through "probe"

</details>

<details>
<summary> <h2>V 3.6.2 - 2024-04-29</h2> </summary>

- Fixed ``Deeper Pockets`` upgrade being applied even when not being bought.
- Fixed commands not working correctly due to not being registered to the network manager.
- Fixed edgecase of using ``RegisterSampleItem`` when ``Item`` scriptable object's spawn prefab already has a ``GrabbableObject`` component

</details>

<details>
<summary> <h2>V 3.6.1 - 2024-04-29</h2> </summary>

<details>
<summary> <h3> Fixes </h3> </summary>

- Bandaiding weather synchronization with a null check due to execution order.

</details>
</details>

<details>
<summary> <h2>V 3.6.0 - 2024-04-29</h2> </summary>

<details>
<summary> <h3>Additions</h3> </summary>

- Implemented "Deeper Pockets" upgrade which provides additional two-handed item carry capacity to the player, treating them like one-handed items til the last carryable two-handed.
- Implemented "Landing Thrusters" upgrade which makes the ship land and/or depart faster between moons.
- Added samples for Forest Keeper, Manticoli and Tulip Snake enemies.
- Added API for registering custom samples to the Hunter upgrade for custom/modded enemies.
	- This way, mods will only require to register their respective sample and LGU will do the spawning for them when an enemy is killed.
	- To allow multiple loot items to be able to spawn on the same monster, I changed the implementation to allow more than one item to be registered on the same enemy.
	- Weight based system to spawn a given model on kill, default value is 50 (also used in LGU's sample prefabs).

</details>

<details>
<summary> <h3>Changes</h3> </summary>

- Separated "Strong Legs" fall damage mitigation into its own tier upgrade (called "Reinforced Boots").

</details>
<details>
<summary> <h3>Fixes</h3> </summary>

- Fixed overriding upgrade names leading to a couple of unpredicted behaviour.
- Fixed Data Contract's Laptop help info showing "ls, mv, mv" rather than "ls, cd, mv", leading to people not know about the ``cd`` command
- Fixed a patch being applied incorrectly which was used to sync weathers to people joining after the probe.
- Fixed upgrades configured to be free not being applied on new saves
- Fixed radar icon appearing on the terminal due to contract items (being invisible when relevant contract is not active) by just straight out deleting them.
- Fixed player not loading their LGU save when leaving and rejoining the same save.
- Fixed Sick Beats defense attribute not being applied correctly (since its release)
- Fixed error spam when booted in the void (due to player controller not being initialized correctly)
    - This does not mean that booting in the void was because of the wheelbarrow, it just means that it would error spam as of consequence of the player's controllers not being initialized correctly.
- Fixed Interns not working correctly when Stimpack is deactivated in the configuration.
- Fixed Protein Powder's Critical Hit effect not being applied on certain configurations.
- Fixed an issue with [LethalLevelLoader](https://github.com/IAmBatby/LethalLevelLoader) due to execution order.

</details>
</details>

<details>
<summary> <h2>V 3.5.5 - 2024-04-13</h2> </summary>

<details>
<summary> <h3>Additions</h3> </summary>

- Implemented "Name Override" configuration where you can change the name of the upgrades to your liking.

</details>

<details>
<summary> <h3>Fixes</h3> </summary>

- Fixed Beekeeper not affecting Butler's Hornet's damage.

</details>
</details>

<details>
<summary> <h2>V 3.5.4 - 2024-04-11</h2> </summary>

<details>
<summary> <h3>Additions</h3> </summary>

- Implemented "Aluminium Coils" tier upgrade which provides several buffs to the zap gun item such as:
     - Reduced cooldown usage after failing its minigame
     - Reduced difficulty multiplier used to initialize the minigame's variables
     - Increased stun range
     - Increased stun time on enemies

</details>

<details>
<summary> <h3>Fixes</h3> </summary>

- Fixed Malware Broadcaster not applying on Spike Roof Traps
- Fixed monster samples not deactivating the particle system when dropped
- Fixed an error when shutting down the game.
- Fixed errors when using Lobby Control due to unity assets' meshes not being readable.

</details>
</details>

## V 3.5.3 - 2024-04-04
- Refined version checking to not load unnecessary types (and possibly lead to errors due to compatibility modes)
- Fixed Malware Broadcaster not working with explosion enabled due to changes on Landmine.SpawnExplosion signature between current release and beta release.

## V 3.5.2 - 2024-04-03
- Fixed compatibility not working when mods that modify the game version are present.

## V 3.5.1 - 2024-04-03
- Added compatibility with version 49 of the Lethal Company game.
   - You should still be able to play in version 50 (which is in public beta at the time of this release)

## V 3.5.0 - 2024-04-02
### Additions
- Implemented ``Lithium Batteries`` upgrade which decrease the rate of battery usage of the items, both passively and on use.
- ``lgu`` in the terminal will prompt a new interface which the user can interact to buy upgrades.
   - Default keybinds: 'w' for cursor up, 's' for cursor down, 'a' for previous page, 'd' for next page, 'enter' for submit prompt, 'escape' for leaving the LGU store

### Changes
- Updated to use CSync's latest 4.x.x releases for configuration synchronization.
- Made InputUtils a hard dependency rather than soft dependency.
- Made ``Protein Powder`` affect the knife's force.
- Made ``Beekeeper`` affect the buttler's bees' damage.

### Fixes
- Fixed ``Climbing Gloves`` individual status not being affected by ``Misc`` ``Share all upgrades``
- Fixed Portable Teleporter allowing you to be used when somebody else is using it.

### Code Changes
- Removed logs associated with asset loading

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

