# Hunter Registration API
<details>
<summary> <h2>Registration</h2> </summary>

<details>
<summary> <h3>Without a custom GrabbableObject script</h3> </summary>

- Use ``RegisterSample(Item sampleItem, string monsterName, int hunterLevel, bool registerNetworkPrefab = false, bool grabbableToEnemies = true, double weight = 50)`` method from ``MoreShipUpgrades.API`` namespace.
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

- Use ``RegisterSample<T>(Item sampleItem, string monsterName, int hunterLevel, bool registerNetworkPrefab = false, double weight = 50)`` method from ``MoreShipUpgrades.API`` namespace.
	- ``T`` is your custom ``GrabbableObject`` script that you wish to add to your prefab.
	- ``Item sampleItem`` is the Scriptable Object meant to store relevant data about an item such as name, weight, scrap value, etc.
	- ``string monsterName`` is the name of the enemy you wish your item to spawn when killed. (There are alternatives of passing the name such as ``EnemyType monsterType`` and ``EnemyAI monster``, use what you need)
	- ``int hunterLevel`` is the level of the Hunter upgrade in which allows spawning scrap items on specified enemy kill.
	- ``bool registerNetworkPrefab`` is a toggle for wether you want to register the associated spawn prefab or not. If it's already registered before calling this method, you can leave it at ``false``.
	- ``double weight`` is the spawn weight of the item when an enemy is killed. This is used to pick what prefab to spawn (yes, you can register multiple samples on the same enemy, ultimately only one spawns per.)
- You can also use the one where you don't specify the type if your ``Item`` scriptable object's spawn prefab already contains the custom ``GrabbableObject`` script
- API is sophisticated (I hope so anyways) that will warn you of any errors/warnings when registering the sample to LGU's Hunter upgrade as descriptive as possible to help fix any issues that may arise if they were spawned as such.

</details>


</details>