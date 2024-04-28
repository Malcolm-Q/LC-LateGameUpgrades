# Hunter Registration API
<details>
<summary> <h2>Registration</h2> </summary>

<details>
<summary> <h3>Before registration</h3> </summary>

- Add ``BepInDependency`` attribute to your ``BaseUnityPlugin`` class and fill the parameters with either ``"com.malco.lethalcompany.moreshipupgrades"`` or ``MoreShipUpgrades.Misc.Metadata.GUID`` and ``DependencyFlags.SoftDependency``
	- This way, you ensure that you load after this Lategame Upgrades has loaded can use its API correctly.
	- ``SoftDependency`` is recommneded here as you won't be forced to have Lategame Upgrades as a dependency for your mod to load. However, if your mod is only intended to add items to the Hunter Upgrade, it would make sense to use ``DependencyFlags.HardDependency`` instead (or not specify the type of dependency at all)
- The names used for each enemy are the names specified in ``EnemyType.enemyName``.

</details>

<details>
<summary> <h3>Without a custom GrabbableObject script</h3> </summary>

- Use ``HunterSamples.RegisterSample(Item sampleItem, string monsterName, int hunterLevel, bool registerNetworkPrefab = false, bool grabbableToEnemies = true, double weight = 50)`` method from ``MoreShipUpgrades.API`` namespace.
	- ``Item sampleItem`` is the Scriptable Object meant to store relevant data about an item such as name, weight, scrap value, etc.
	- ``string monsterName`` is the name of the enemy you wish your item to spawn when killed. (There are alternatives of passing the name such as ``EnemyType monsterType`` and ``EnemyAI monster``, use what you need)
	- ``int hunterLevel`` is the level of the Hunter upgrade in which allows spawning scrap items on specified enemy kill.
	- ``bool registerNetworkPrefab`` is a toggle for wether you want to register the associated spawn prefab or not. If it's already registered before calling this method, you can leave it at ``false``.
	- ``bool grabbableToEnemies`` is a toggle to wether you allow enemies to grab this sample such as Hoarding Bugs or other custom enemies who use this attribute.
	- ``double weight`` is the spawn weight of the item when an enemy is killed. This is used to pick what prefab to spawn (yes, you can register multiple samples on the same enemy, ultimately only one spawns per.)
- API is sophisticated (I hope so anyways) that will warn you of any errors/warnings when registering the sample to LGU's Hunter upgrade as descriptive as possible to help fix any issues that may arise if they were spawned as such.
</details>

<details>
<summary> <h3>With a custom GrabbableObject script</h3> </summary>

- Use ``HunterSamples.RegisterSample<T>(Item sampleItem, string monsterName, int hunterLevel, bool registerNetworkPrefab = false, double weight = 50)`` method from ``MoreShipUpgrades.API`` namespace.
	- ``T`` is your custom ``GrabbableObject`` script that you wish to add to your prefab.
	- ``Item sampleItem`` is the Scriptable Object meant to store relevant data about an item such as name, weight, scrap value, etc.
	- ``string monsterName`` is the name of the enemy you wish your item to spawn when killed. (There are alternatives of passing the name such as ``EnemyType monsterType`` and ``EnemyAI monster``, use what you need)
	- ``int hunterLevel`` is the level of the Hunter upgrade in which allows spawning scrap items on specified enemy kill.
	- ``bool registerNetworkPrefab`` is a toggle for wether you want to register the associated spawn prefab or not. If it's already registered before calling this method, you can leave it at ``false``.
	- ``double weight`` is the spawn weight of the item when an enemy is killed. This is used to pick what prefab to spawn (yes, you can register multiple samples on the same enemy, ultimately only one spawns per.)
- You can also use the one where you don't specify the type if your ``Item`` scriptable object's spawn prefab already contains the custom ``GrabbableObject`` script
- API is sophisticated (I hope so anyways) that will warn you of any errors/warnings when registering the sample to LGU's Hunter upgrade as descriptive as possible to help fix any issues that may arise if they were spawned as such.

</details>

<details>
<summary> <h3> Examples </h3> </summary>

<details>
<summary> Without custom GrabbableObject script </summary>

```c#
	Item item = LoadItemFromAssetBundle();
	// RegisterNetworkPrefab(item.spawnPrefab); if you're gonna register before hand, toggle registerNetworkPrefab to false
	MoreShipUpgrades.API.HunterSamples.RegisterSample(sampleItem: item, monsterName: "Hoarding Bug", hunterLevel: 1, registerNetworkPrefab: true, grabbableToEnemies: true, weight: 50);
```
- ``LoadItemFromAssetBundle()`` is defined by you and the purpose of this method is to load an ``Item`` Scriptable Object from outside resource such as an asset bundle.
- ``RegisterNetworkPrefab()`` can be either defined by you or using some other API in which it registers the associated prefab to the game's network manager.
- You don't need to specify the parameter names, it's just easier to understand what each value is corresponded to.
- This way, when the players have reached level 1 of Hunter upgrade, whenever a ``Hoarding Bug`` is killed, it has a 50/50 chance of either spawning your item or LGU's respective sample item.
- If the ``Item`` scriptable object's spawn prefab already contains a GrabbableObject component, you can use this method instead.

</details>
<details>
<summary> With custom GrabbableObject script </summary>

```c#
	Item item = LoadItemFromAssetBundle();
	// RegisterNetworkPrefab(item.spawnPrefab); if you're gonna register before hand, toggle registerNetworkPrefab to false
	MoreShipUpgrades.API.HunterSamples.RegisterSample<CustomGrabbableObject>(sampleItem: item, monsterName: "Crawler", hunterLevel: 2, registerNetworkPrefab: true, weight: 50);
```
- ``LoadItemFromAssetBundle()`` is defined by you and the purpose of this method is to load an ``Item`` Scriptable Object from outside resource such as an asset bundle.
- ``RegisterNetworkPrefab()`` can be either defined by you or using some other API in which it registers the associated prefab to the game's network manager.
- You don't need to specify the parameter names, it's just easier to understand what each value is corresponded to.
- This way, when the players have reached level 2 of Hunter upgrade, whenever a ``Thumper/Half`` is killed, it has a 50/50 chance of either spawning your item or LGU's respective sample item.

</details>

</details>