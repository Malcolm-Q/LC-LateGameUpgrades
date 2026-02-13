using BepInEx.Configuration;
using System;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades;
using MoreShipUpgrades.UpgradeComponents.Commands;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.AttributeUpgrades;
using CSync.Lib;
using System.Runtime.Serialization;
using CSync.Extensions;
using MoreShipUpgrades.Managers;
using UnityEngine;
using MoreShipUpgrades.Misc.Util;
using MoreShipUpgrades.UpgradeComponents.Items;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Store;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Player;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Enemies;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Ship;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.WeedKiller;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.Zapgun;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.RadarBooster;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Ship;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Store;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Items;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Player;
using MoreShipUpgrades.UpgradeComponents.OneTimeUpgrades.Enemies;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.Shotgun;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Items.Jetpack;
using MoreShipUpgrades.Misc;
using MoreShipUpgrades.Configuration.Upgrades.Interfaces.OneTimeUpgrades;
using MoreShipUpgrades.Configuration.Upgrades.Interfaces.TierUpgrades;
using MoreShipUpgrades.Configuration.Upgrades.Custom;
using MoreShipUpgrades.Configuration.Upgrades.Abstractions.TIerUpgrades;
using MoreShipUpgrades.Configuration.Upgrades.Abstractions.OneTimeUpgrades;
using MoreShipUpgrades.Configuration.Contracts;

namespace MoreShipUpgrades.Configuration
{
    [DataContract]
    public class LategameConfiguration : SyncedConfig2<LategameConfiguration>
    {
        public ITierEffectUpgradeConfiguration<int> SmarterLockpickUpgradeConfiguration { get; set; }
        public ITierEffectUpgradeConfiguration<int> EffectiveBandaidsConfiguration { get; set; }
        public ITierEffectUpgradeConfiguration<int> MedicalNanobotsConfiguration { get; set; }
        public ScrapKeeperUpgradeConfiguration ScrapKeeperConfiguration { get; set; }
        public ITierEffectUpgradeConfiguration<int> ParticleInfuserConfiguration { get; set; }
        public IOneTimeUpgradeConfiguration SilverBulletsConfiguration { get; set; }
        public ITierCollectionUpgradeConfiguration FusionMatterConfiguration { get; set; }
        public ITierEffectUpgradeConfiguration<int> LongBarrelConfiguration { get; set; }
        public ITierEffectUpgradeConfiguration<int> HollowPointConfiguration { get; set; }
        public ITierEffectUpgradeConfiguration<int> JetpackThrustersConfiguration { get; set; }
        public ITierEffectUpgradeConfiguration<int> JetFuelConfiguration { get; set; }
        public ITierEffectUpgradeConfiguration<int> QuickHandsConfiguration { get; set; }
        public ITierEffectUpgradeConfiguration<int> MidasTouchConfiguration { get; set; }
        public ITierEffectUpgradeConfiguration<int> CarbonKneejointsConfiguration { get; set; }
        public ITierEffectUpgradeConfiguration<int> LifeInsuranceConfiguration { get; set; }
        public ITierEffectUpgradeConfiguration<int> RubberBootsConfiguration { get; set; }
        public ITierEffectUpgradeConfiguration<int> OxygenCanistersConfiguration {  get; set; }
        public ITierEffectUpgradeConfiguration<int> SleightOfHandConfiguration {  get; set; }
        public ITierEffectUpgradeConfiguration<int> HikingBootsConfiguration { get; set; }
        public ITierEffectUpgradeConfiguration<int> TractionBootsConfiguration { get; set; }
        public IOneTimeUpgradeConfiguration FedoraSuitConfiguration {  get; set; }
        public ITierEffectUpgradeConfiguration<int> WeedGeneticManipulationConfiguration {  get; set; }
        public ITierEffectUpgradeConfiguration<float> ClayGlassesConfiguration {  get; set; }
        public ITierEffectUpgradeConfiguration<float> MechanicalArmsConfiguration {  get; set; }
        public ITierEffectUpgradeConfiguration<int> ScavengerInstictsConfiguration {  get; set; }
        public LandingThrusterUpgradeConfiguration LandingThrustersConfiguration {  get; set; }
        public ITierEffectUpgradeConfiguration<int> ReinforcedBootsConfiguration { get; set; }
        public DeeperPocketsUpgradeConfiguration DeeperPocketsConfiguration {  get; set; }
        public ITierMultipleEffectUpgradeConfiguration<int, float> AluminiumCoilConfiguration {  get; set; }
        public ITierMultipleEffectUpgradeConfiguration<float, int> ChargingBoosterConfiguration {  get; set; }
        public ITierEffectUpgradeConfiguration<int> MarketInfluenceConfiguration { get; set; }
        public ITierEffectUpgradeConfiguration<int> BargainConnectionsConfiguration {  get; set; }
        public IOneTimeUpgradeConfiguration LethalDealsConfiguration { get; set; }
        public QuantumDisruptorUpgradeConfiguration QuantumDisruptorConfiguration {  get; set; }
        public ITierMultipleEffectUpgradeConfiguration<float> BeekeeperConfiguration {  get; set; }
        public ITierMultipleEffectUpgradeConfiguration<int, float> ProteinPowderConfiguration {  get; set; }
        public BiggerLungsUpgradeConfiguration BiggerLungsConfiguration {  get; set; }
        public ITierAlternativeEffectUpgradeConfiguration<float, BackMuscles.UpgradeMode> BackMusclesConfiguration { get; set; }
        public ITierMultipleEffectUpgradeConfiguration<float> RunningShoesConfiguration { get; set; }
        public ITierEffectUpgradeConfiguration<float> StrongLegsConfiguration {  get; set; }
        public IOneTimeAlternativeEffectUpgradeConfiguration<float, LightningRod.UpgradeMode> LightningRodConfiguration {  get; set; }
        public ITierCollectionUpgradeConfiguration HunterConfiguration {  get; set; }
        public ITierEffectUpgradeConfiguration<int> StimpackConfiguration { get; set; }
        public ITierEffectUpgradeConfiguration<float> ShutterBatteriesConfiguration {  get; set; }
        public ITierEffectUpgradeConfiguration<int> EfficientEnginesConfiguration {  get; set; }
        public ITierEffectUpgradeConfiguration<float> ClimblingGlovesConfiguration { get; set; }
        public ITierEffectUpgradeConfiguration<int> LithiumBatteriesConfiguration { get; set; }
        public IOneTimeUpgradeConfiguration LocksmithConfiguration { get; set; }
        public IOneTimeUpgradeConfiguration FastEncryptionConfiguration {  get; set; }
        public DropPodThrustersUpgradeConfiguration DropPodThrustersConfiguration { get; set; }
        public SigurdAccessUpgradeConfiguration SigurdAccessConfiguration { get; set; }
        public IOneTimeUpgradeConfiguration WalkieGpsConfiguration {  get; set; }
        public DiscombobulatorUpgradeConfiguration DiscombobulatorUpgradeConfiguration { get; set; }
        public MalwareBroadcasterUpgradeConfiguration MalwareBroadcasterUpgradeConfiguration {  get; set; }
        public NightVisionUpgradeConfiguration NightVisionUpgradeConfiguration { get; set; }
        public SickBeatsUpgradeConfiguration SickBeatsUpgradeConfiguration {  get; set; }
        public BetterScannerUpgradeConfiguration BetterScannerUpgradeConfiguration {  get; set; }
        public ITierEffectUpgradeConfiguration<int> BabyPacifierUpgradeConfiguration { get; set; }
        public ITierEffectUpgradeConfiguration<int> ItemDuplicatorUpgradeConfiguration { get; set; }
        public ContractsConfiguration ContractsConfiguration { get; set; }

        #region Attributes
        [field: SyncedEntryField] public SyncedEntry<float> INTERNS_DELAY_BEFORE_REVIVE {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> INTERNS_USAGES_PER_LANDING { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> INTERNS_INTERVAL_BETWEEN_REVIVES {  get; set; }
        [field: SyncedEntryField] public SyncedEntry<Interns.TeleportRestriction> INTERNS_TELEPORT_RESTRICTION { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> SHOW_WORLD_BUILDING_TEXT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> MEDKIT_SCAN_NODE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> OVERRIDE_UPGRADE_NAMES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> INTERN_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> INTERN_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> SALE_PERC { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> SHARED_UPGRADES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> REFUND_UPGRADES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> MEDKIT_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> MEDKIT_PRICE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> MEDKIT_HEAL_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> MEDKIT_USES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> KEEP_UPGRADES_AFTER_FIRED_CUTSCENE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> SNARE_FLEA_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> SNARE_FLEA_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> BUNKER_SPIDER_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> BUNKER_SPIDER_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> HOARDING_BUG_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> HOARDING_BUG_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> BRACKEN_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> BRACKEN_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> EYELESS_DOG_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> EYELESS_DOG_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> BABOON_HAWK_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> BABOON_HAWK_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> THUMPER_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> THUMPER_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> FOREST_KEEPER_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> FOREST_KEEPER_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> MANTICOIL_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> MANTICOIL_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> TULIP_SNAKE_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> TULIP_SNAKE_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> KIDNAPPER_FOX_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> KIDNAPPER_FOX_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> SPORE_LIZARD_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> SPORE_LIZARD_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> MANEATER_SAMPLE_MINIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> MANEATER_SAMPLE_MAXIMUM_VALUE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> SHOW_UPGRADES_CHAT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> SALE_APPLY_ONCE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> BuyableUpgradeOnce { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> ShowLockedUpgrades { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> UseDawnLib {  get; set; }

        #endregion

        #region Item Progression
        [field: SyncedEntryField] public SyncedEntry<bool> ALTERNATIVE_ITEM_PROGRESSION { get; set; }
        [field: SyncedEntryField] public SyncedEntry<ItemProgressionManager.CollectionModes> ITEM_PROGRESSION_MODE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> ITEM_PROGRESSION_CONTRIBUTION_MULTIPLIER { get; set; }
        [field: SyncedEntryField] public SyncedEntry<float> SCRAP_UPGRADE_CHANCE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<ItemProgressionManager.ChancePerScrapModes> SCRAP_UPGRADE_CHANCE_MODE { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> ITEM_PROGRESSION_BLACKLISTED_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<string> ITEM_PROGRESSION_APPARATICE_ITEMS { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> ITEM_PROGRESSION_NO_PURCHASE_UPGRADES { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> ITEM_PROGRESSION_ALWAYS_SHOW_ITEMS { get; set; }

        #endregion

        #region Randomize Upgrades
        [field: SyncedEntryField] public SyncedEntry<bool> RANDOMIZE_UPGRADES_ENABLED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<int> RANDOMIZE_UPGRADES_AMOUNT { get; set; }
        [field: SyncedEntryField] public SyncedEntry<bool> RANDOMIZE_UPGRADES_ALWAYS_SHOW_PURCHASED { get; set; }
        [field: SyncedEntryField] public SyncedEntry<RandomizeUpgradeManager.RandomizeUpgradeEvents> RANDOMIZE_UPGRADES_CHANGE_UPGRADES_EVENT { get; set; }
        #endregion

        public AlternativeCurrencyConfiguration AlternativeCurrencyConfiguration { get; set; }

        #region Configuration Bindings
        public LategameConfiguration(ConfigFile cfg) : base(Metadata.GUID)
        {
            string topSection;

			AlternativeCurrencyConfiguration = new AlternativeCurrencyConfiguration(cfg);

			#region Randomize Upgrades

			topSection = LguConstants.RANDOMIZE_UPGRADES_SECTION;
            RANDOMIZE_UPGRADES_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.RANDOMIZE_UPGRADES_ENABLED_KEY, LguConstants.RANDOMIZE_UPGRADES_ENABLED_DEFAULT, LguConstants.RANDOMIZE_UPGRADES_ENABLED_DESCRIPTION);
            RANDOMIZE_UPGRADES_AMOUNT = cfg.BindSyncedEntry(topSection, LguConstants.RANDOMIZE_UPGRADES_AMOUNT_KEY, LguConstants.RANDOMIZE_UPGRADES_AMOUNT_DEFAULT, LguConstants.RANDOMIZE_UPGRADES_AMOUNT_DESCRIPTION);
            RANDOMIZE_UPGRADES_ALWAYS_SHOW_PURCHASED = cfg.BindSyncedEntry(topSection, LguConstants.RANDOMIZE_UPGRADES_ALWAYS_SHOW_PURCHASED_KEY, LguConstants.RANDOMIZE_UPGRADES_ALWAYS_SHOW_PURCHASED_DEFAULT, LguConstants.RANDOMIZE_UPGRADES_ALWAYS_SHOW_PURCHASED_DESCRIPTION);
            RANDOMIZE_UPGRADES_CHANGE_UPGRADES_EVENT = cfg.BindSyncedEntry(topSection, LguConstants.RANDOMIZE_UPGRADES_CHANGE_UPGRADES_EVENT_KEY, LguConstants.RANDOMIZE_UPGRADES_CHANGE_UPGRADES_EVENT_DEFAULT, LguConstants.RANDOMIZE_UPGRADES_CHANGE_UPGRADES_EVENT_DESCRIPTION);

            #endregion

            #region Item Progression

            topSection = LguConstants.ITEM_PROGRESSION_SECTION;
            ALTERNATIVE_ITEM_PROGRESSION = cfg.BindSyncedEntry(topSection, LguConstants.ALTERNATIVE_ITEM_PROGRESSION_KEY, LguConstants.ALTERNATIVE_ITEM_PROGRESSION_DEFAULT, LguConstants.ALTERNATIVE_ITEM_PROGRESSION_DESCRIPTION);
            ITEM_PROGRESSION_MODE = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_MODE_KEY, LguConstants.ITEM_PROGRESSION_MODE_DEFAULT, LguConstants.ITEM_PROGRESSION_MODE_DESCRIPTION);
            ITEM_PROGRESSION_CONTRIBUTION_MULTIPLIER = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_CONTRIBUTION_MULTIPLIER_KEY, LguConstants.ITEM_PROGRESSION_CONTRIBUTION_MULTIPLIER_DEFAULT, LguConstants.ITEM_PROGRESSION_CONTRIBUTION_MULTIPLIER_DESCRIPTION);
            SCRAP_UPGRADE_CHANCE = cfg.BindSyncedEntry(topSection, LguConstants.SCRAP_UPGRADE_CHANCE_KEY, LguConstants.SCRAP_UPGRADE_CHANCE_DEFAULT, LguConstants.SCRAP_UPGRADE_CHANCE_DESCRIPTION);
            SCRAP_UPGRADE_CHANCE_MODE = cfg.BindSyncedEntry(topSection, LguConstants.SCRAP_UPGRADE_MODE_KEY, LguConstants.SCRAP_UPGRADE_MODE_DEFAULT, LguConstants.SCRAP_UPGRADE_MODE_DESCRIPTION);
            ITEM_PROGRESSION_BLACKLISTED_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_BLACKLISTED_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_BLACKLISTED_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_BLACKLISTED_ITEMS_DESCRIPTION);
            ITEM_PROGRESSION_APPARATICE_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_APPARATICE_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_APPARATICE_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_APPARATICE_ITEMS_DESCRIPTION);
            ITEM_PROGRESSION_NO_PURCHASE_UPGRADES = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_NO_PURCHASE_UPGRADES_KEY, LguConstants.ITEM_PROGRESSION_NO_PURCHASE_UPGRADES_DEFAULT, LguConstants.ITEM_PROGRESSION_NO_PURCHASE_UPGRADES_DESCRIPTION);
            ITEM_PROGRESSION_ALWAYS_SHOW_ITEMS = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_PROGRESSION_ALWAYS_SHOW_ITEMS_KEY, LguConstants.ITEM_PROGRESSION_ALWAYS_SHOW_ITEMS_DEFAULT, LguConstants.ITEM_PROGRESSION_ALWAYS_SHOW_ITEMS_DESCRIPTION);

            #endregion 

            #region Miscellaneous

            topSection = LguConstants.MISCELLANEOUS_SECTION;
            SHARED_UPGRADES = cfg.BindSyncedEntry(topSection, LguConstants.SHARE_ALL_UPGRADES_KEY, LguConstants.SHARE_ALL_UPGRADES_DEFAULT, LguConstants.SHARE_ALL_UPGRADES_DESCRIPTION);
            REFUND_UPGRADES = cfg.BindSyncedEntry(topSection, "All Upgrades Refundable", false, "If enabled, it overrides all upgrades configuration settings to allow refunding a level");
            SALE_PERC = cfg.BindSyncedEntry(topSection, LguConstants.SALE_PERCENT_KEY, LguConstants.SALE_PERCENT_DEFAULT, LguConstants.SALE_PERCENT_DESCRIPTION);
            KEEP_UPGRADES_AFTER_FIRED_CUTSCENE = cfg.BindSyncedEntry(topSection, LguConstants.KEEP_UPGRADES_AFTER_FIRED_KEY, LguConstants.KEEP_UPGRADES_AFTER_FIRED_DEFAULT, LguConstants.KEEP_UPGRADES_AFTER_FIRED_DESCRIPTION);
            SHOW_UPGRADES_CHAT = cfg.BindSyncedEntry(topSection, LguConstants.SHOW_UPGRADES_CHAT_KEY, LguConstants.SHOW_UPGRADES_CHAT_DEFAULT, LguConstants.SHOW_UPGRADES_CHAT_DESCRIPTION);
            SALE_APPLY_ONCE = cfg.BindSyncedEntry(topSection, LguConstants.SALE_APPLY_ONCE_KEY, LguConstants.SALE_APPLY_ONCE_DEFAULT, LguConstants.SALE_APPLY_ONCE_DESCRIPTION);
            SHOW_WORLD_BUILDING_TEXT = cfg.BindSyncedEntry(topSection, LguConstants.SHOW_WORLD_BUILDING_TEXT_KEY, LguConstants.SHOW_WORLD_BUILDING_TEXT_DEFAULT, LguConstants.SHOW_WORLD_BUILDING_TEXT_DESCRIPTION);
            BuyableUpgradeOnce = cfg.BindSyncedEntry(topSection, LguConstants.BUYABLE_UPGRADES_ONCE_KEY, LguConstants.BUYABLE_UPGRADES_ONCE_DEFAULT, LguConstants.BUYABLE_UPGRADES_ONCE_DESCRIPTION);
            ShowLockedUpgrades = cfg.BindSyncedEntry(topSection, LguConstants.SHOW_LOCKED_UPGRADES_KEY, LguConstants.SHOW_LOCKED_UPGRADES_DEFAULT, LguConstants.SHOW_LOCKED_UPGRADES_DESCRIPTION);
            OVERRIDE_UPGRADE_NAMES = cfg.BindSyncedEntry(topSection, LguConstants.OVERRIDE_NAMES_ENABLED_KEY, LguConstants.OVERRIDE_NAMES_ENABLED_DEFAULT, LguConstants.OVERRIDE_NAMES_ENABLED_DESCRIPTION);
            UseDawnLib = cfg.BindSyncedEntry(topSection, "Use DawnLib for Initialization", false, "Replaces initialization phase that utilizes LethalLib with DawnLib callbacks instead. Use this if you are experiencing issues with LethalLib and believe DawnLib won't have the same issues.");
            #endregion

            topSection = LguConstants.CONTRACTS_SECTION;
            ContractsConfiguration = new ContractsConfiguration(cfg, topSection);

            #region Items

            #region Medkit

            topSection = Medkit.ITEM_NAME;
            MEDKIT_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.MEDKIT_ENABLED_KEY, LguConstants.MEDKIT_ENABLED_DEFAULT, LguConstants.MEDKIT_ENABLED_DESCRIPTION);
            MEDKIT_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.MEDKIT_PRICE_KEY, LguConstants.MEDKIT_PRICE_DEFAULT, LguConstants.MEDKIT_PRICE_DESCRIPTION);
            MEDKIT_HEAL_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.MEDKIT_HEAL_AMOUNT_KEY, LguConstants.MEDKIT_HEAL_AMOUNT_DEFAULT, LguConstants.MEDKIT_HEAL_AMOUNT_DESCRIPTION);
            MEDKIT_USES = cfg.BindSyncedEntry(topSection, LguConstants.MEDKIT_USES_KEY, LguConstants.MEDKIT_USES_DEFAULT, LguConstants.MEDKIT_USES_DESCRIPTION);
            MEDKIT_SCAN_NODE = cfg.BindSyncedEntry(topSection, LguConstants.MEDKIT_SCAN_NODE_KEY, LguConstants.ITEM_SCAN_NODE_DEFAULT, LguConstants.ITEM_SCAN_NODE_DESCRIPTION);

            #endregion

            #endregion

            #region Upgrades

            topSection = SmarterLockpick.UPGRADE_NAME;
            SmarterLockpickUpgradeConfiguration = new TierPrimitiveUpgradeConfiguration<int>(cfg, topSection, LguConstants.SMARTER_LOCKPICK_ENABLED_DESCRIPTION, SmarterLockpick.PRICES_DEFAULT)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.SMARTER_LOCKPICK_INITIAL_INCREASE_KEY, LguConstants.SMARTER_LOCKPICK_INITIAL_INCREASE_DEFAULT, LguConstants.SMARTER_LOCKPICK_INITIAL_INCREASE_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.SMARTER_LOCKPICK_INCREMENTAL_INCREASE_KEY, LguConstants.SMARTER_LOCKPICK_INCREMENTAL_INCREASE_DEFAULT, LguConstants.SMARTER_LOCKPICK_INCREMENTAL_INCREASE_DESCRIPTION),
            };

            topSection = EffectiveBandaids.UPGRADE_NAME;
            EffectiveBandaidsConfiguration = new TierIndividualPrimitiveUpgradeConfiguration<int>(cfg, topSection, LguConstants.EFFECTIVE_BANDAIDS_ENABLED_DESCRIPTION, EffectiveBandaids.DEFAULT_PRICES)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.EFFECTIVE_BANDAIDS_INITIAL_HEALTH_REGEN_AMOUNT_INCREASE_KEY, LguConstants.EFFECTIVE_BANDAIDS_INITIAL_HEALTH_REGEN_AMOUNT_INCREASE_DEFAULT, LguConstants.EFFECTIVE_BANDAIDS_INITIAL_HEALTH_REGEN_AMOUNT_INCREASE_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.EFFECTIVE_BANDAIDS_INCREMENTAL_HEALTH_REGEN_AMOUNT_INCREASE_KEY, LguConstants.EFFECTIVE_BANDAIDS_INCREMENTAL_HEALTH_REGEN_AMOUNT_INCREASE_DEFAULT, LguConstants.EFFECTIVE_BANDAIDS_INCREMENTAL_HEALTH_REGEN_AMOUNT_INCREASE_DESCRIPTION),
            };

            topSection = MedicalNanobots.UPGRADE_NAME;
            MedicalNanobotsConfiguration = new TierIndividualPrimitiveUpgradeConfiguration<int>(cfg, topSection, LguConstants.MEDICAL_NANOBOTS_ENABLED_DESCRIPTION, MedicalNanobots.DEFAULT_PRICES)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.MEDICAL_NANOBOTS_INITIAL_HEALTH_REGEN_CAP_INCREASE_KEY, LguConstants.MEDICAL_NANOBOTS_INITIAL_HEALTH_REGEN_CAP_INCREASE_DEFAULT, LguConstants.MEDICAL_NANOBOTS_INITIAL_HEALTH_REGEN_CAP_INCREASE_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.MEDICAL_NANOBOTS_INCREMENTAL_HEALTH_REGEN_CAP_INCREASE_KEY, LguConstants.MEDICAL_NANOBOTS_INCREMENTAL_HEALTH_REGEN_CAP_INCREASE_DEFAULT, LguConstants.MEDICAL_NANOBOTS_INCREMENTAL_HEALTH_REGEN_CAP_INCREASE_DESCRIPTION),
            };

            topSection = ScrapKeeper.UPGRADE_NAME;
            ScrapKeeperConfiguration = new ScrapKeeperUpgradeConfiguration(cfg, topSection, LguConstants.SCRAP_KEEPER_ENABLED_DESCRIPTION, ScrapKeeper.PRICES_DEFAULT)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.SCRAP_KEEPER_INITIAL_KEEP_SCRAP_CHANCE_INCREASE_KEY, LguConstants.SCRAP_KEEPER_INITIAL_KEEP_SCRAP_CHANCE_INCREASE_DEFAULT, LguConstants.SCRAP_KEEPER_INITIAL_KEEP_SCRAP_CHANCE_INCREASE_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.SCRAP_KEEPER_INCREMENTAL_KEEP_SCRAP_CHANCE_INCREASE_KEY, LguConstants.SCRAP_KEEPER_INCREMENTAL_KEEP_SCRAP_CHANCE_INCREASE_DEFAULT, LguConstants.SCRAP_KEEPER_INCREMENTAL_KEEP_SCRAP_CHANCE_INCREASE_DESCRIPTION)
            };

            topSection = ParticleInfuser.UPGRADE_NAME;
            ParticleInfuserConfiguration = new TierPrimitiveUpgradeConfiguration<int>(cfg, topSection, LguConstants.PARTICLE_INFUSER_ENABLED_DESCRIPTION, ParticleInfuser.DEFAULT_PRICES)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.PARTICLE_INFUSER_INITIAL_TELEPORT_SPEED_INCREASE_KEY, LguConstants.PARTICLE_INFUSER_INITIAL_TELEPORT_SPEED_INCREASE_DEFAULT, LguConstants.PARTICLE_INFUSER_INITIAL_TELEPORT_SPEED_INCREASE_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.PARTICLE_INFUSER_INCREMENTAL_TELEPORT_SPEED_INCREASE_KEY, LguConstants.PARTICLE_INFUSER_INCREMENTAL_TELEPORT_SPEED_INCREASE_DEFAULT, LguConstants.PARTICLE_INFUSER_INCREMENTAL_TELEPORT_SPEED_INCREASE_DESCRIPTION),
            };

            topSection = SilverBullets.UPGRADE_NAME;
            SilverBulletsConfiguration = new OneTimeIndividualUpgradeConfiguration(cfg, topSection, LguConstants.SILVER_BULLETS_ENABLED_DESCRIPTION, LguConstants.SILVER_BULLETS_PRICE_DEFAULT);

            topSection = FusionMatter.UPGRADE_NAME;
            FusionMatterConfiguration = new TierCollectionUpgradeConfiguration(cfg, topSection, LguConstants.FUSION_MATTER_ENABLED_DESCRIPTION, FusionMatter.DEFAULT_PRICES)
            {
                TierCollection = cfg.BindSyncedEntry(topSection, LguConstants.FUSION_MATTER_TIERS_KEY, LguConstants.FUSION_MATTER_TIERS_DEFAULT, LguConstants.FUSION_MATTER_TIERS_DESCRIPTION),
            };

            topSection = LongBarrel.UPGRADE_NAME;
            LongBarrelConfiguration = new TierIndividualPrimitiveUpgradeConfiguration<int>(cfg, topSection, LguConstants.LONG_BARREL_ENABLED_DESCRIPTION, LongBarrel.DEFAULT_PRICES)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.LONG_BARREL_INITIAL_SHOTGUN_RANGE_INCREASE_KEY, LguConstants.LONG_BARREL_INITIAL_SHOTGUN_RANGE_INCREASE_DEFAULT, LguConstants.LONG_BARREL_INITIAL_SHOTGUN_RANGE_INCREASE_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.LONG_BARREL_INCREMENTAL_SHOTGUN_RANGE_INCREASE_KEY, LguConstants.LONG_BARREL_INCREMENTAL_SHOTGUN_RANGE_INCREASE_DEFAULT, LguConstants.LONG_BARREL_INCREMENTAL_SHOTGUN_RANGE_INCREASE_DESCRIPTION),
            };
            topSection = HollowPoint.UPGRADE_NAME;
            HollowPointConfiguration = new TierIndividualPrimitiveUpgradeConfiguration<int>(cfg, topSection, LguConstants.HOLLOW_POINT_ENABLED_DESCRIPTION, HollowPoint.DEFAULT_PRICES)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.HOLLOW_POINT_INITIAL_SHOTGUN_DAMAGE_INCREASE_KEY, LguConstants.HOLLOW_POINT_INITIAL_SHOTGUN_DAMAGE_INCREASE_DEFAULT, LguConstants.HOLLOW_POINT_INITIAL_SHOTGUN_DAMAGE_INCREASE_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.HOLLOW_POINT_INCREMENTAL_SHOTGUN_DAMAGE_INCREASE_KEY, LguConstants.HOLLOW_POINT_INCREMENTAL_SHOTGUN_DAMAGE_INCREASE_DEFAULT, LguConstants.HOLLOW_POINT_INCREMENTAL_SHOTGUN_DAMAGE_INCREASE_DESCRIPTION),
            };

            topSection = JetpackThrusters.UPGRADE_NAME;
            JetpackThrustersConfiguration = new TierIndividualPrimitiveUpgradeConfiguration<int>(cfg, topSection, LguConstants.JETPACK_THRUSTERS_ENABLED_DESCRIPTION, JetpackThrusters.DEFAULT_PRICES)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.JETPACK_THRUSTERS_INITIAL_MAXIMUM_POWER_INCREASE_KEY, LguConstants.JETPACK_THRUSTERS_INITIAL_MAXIMUM_POWER_INCREASE_DEFAULT, LguConstants.JETPACK_THRUSTERS_INITIAL_MAXIMUM_POWER_INCREASE_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.JETPACK_THRUSTERS_INCREMENTAL_MAXIMUM_POWER_INCREASE_KEY, LguConstants.JETPACK_THRUSTERS_INCREMENTAL_MAXIMUM_POWER_INCREASE_DEFAULT, LguConstants.JETPACK_THRUSTERS_INCREMENTAL_MAXIMUM_POWER_INCREASE_DESCRIPTION),
            };

            topSection = JetFuel.UPGRADE_NAME;
            JetFuelConfiguration = new TierIndividualPrimitiveUpgradeConfiguration<int>(cfg, topSection, LguConstants.JET_FUEL_ENABLED_DESCRIPTION, JetFuel.DEFAULT_PRICES)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.JET_FUEL_INITIAL_ACCELERATION_INCREASE_KEY, LguConstants.JET_FUEL_INITIAL_ACCELERATION_INCREASE_DEFAULT, LguConstants.JET_FUEL_INITIAL_ACCELERATION_INCREASE_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.JET_FUEL_INCREMENTAL_ACCELERATION_INCREASE_KEY, LguConstants.JET_FUEL_INCREMENTAL_ACCELERATION_INCREASE_DEFAULT, LguConstants.JET_FUEL_INCREMENTAL_ACCELERATION_INCREASE_DESCRIPTION),
            };

            topSection = QuickHands.UPGRADE_NAME;
            QuickHandsConfiguration = new TierIndividualPrimitiveUpgradeConfiguration<int>(cfg, topSection, LguConstants.QUICK_HANDS_ENABLED_DESCRIPTION, QuickHands.DEFAULT_PRICES)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.QUICK_HANDS_INITIAL_INTERACTION_SPEED_INCREASE_KEY, LguConstants.QUICK_HANDS_INITIAL_INTERACTION_SPEED_INCREASE_DEFAULT, LguConstants.QUICK_HANDS_INITIAL_INTERACTION_SPEED_INCREASE_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.QUICK_HANDS_INCREMENTAL_INTERACTION_SPEED_INCREASE_KEY, LguConstants.QUICK_HANDS_INCREMENTAL_INTERACTION_SPEED_INCREASE_DEFAULT, LguConstants.QUICK_HANDS_INCREMENTAL_INTERACTION_SPEED_INCREASE_DESCRIPTION),
            };

            topSection = MidasTouch.UPGRADE_NAME;
            MidasTouchConfiguration = new TierPrimitiveUpgradeConfiguration<int>(cfg, topSection, LguConstants.MIDAS_TOUCH_ENABLED_DESCRIPTION, MidasTouch.DEFAULT_PRICES)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.MIDAS_TOUCH_INITIAL_SCRAP_VALUE_INCREASE_KEY, LguConstants.MIDAS_TOUCH_INITIAL_SCRAP_VALUE_INCREASE_DEFAULT, LguConstants.MIDAS_TOUCH_INITIAL_SCRAP_VALUE_INCREASE_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.MIDAS_TOUCH_INCREMENTAL_SCRAP_VALUE_INCREASE_KEY, LguConstants.MIDAS_TOUCH_INCREMENTAL_SCRAP_VALUE_INCREASE_DEFAULT, LguConstants.MIDAS_TOUCH_INCREMENTAL_SCRAP_VALUE_INCREASE_DESCRIPTION),
            };

            topSection = CarbonKneejoints.UPGRADE_NAME;
            CarbonKneejointsConfiguration = new TierIndividualPrimitiveUpgradeConfiguration<int>(cfg, topSection, LguConstants.CARBON_KNEEJOINTS_ENABLED_DESCRIPTION, CarbonKneejoints.DEFAULT_PRICES)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.CARBON_KNEEJOINTS_INITIAL_CROUCH_DEBUFF_DECREASE_KEY, LguConstants.CARBON_KNEEJOINTS_INITIAL_CROUCH_DEBUFF_DECREASE_DEFAULT, LguConstants.CARBON_KNEEJOINTS_INITIAL_CROUCH_DEBUFF_DECREASE_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.CARBON_KNEEJOINTS_INCREMENTAL_CROUCH_DEBUFF_DECREASE_KEY, LguConstants.CARBON_KNEEJOINTS_INCREMENTAL_CROUCH_DEBUFF_DECREASE_DEFAULT, LguConstants.CARBON_KNEEJOINTS_INCREMENTAL_CROUCH_DEBUFF_DECREASE_DESCRIPTION),
            };

            topSection = LifeInsurance.UPGRADE_NAME;
            LifeInsuranceConfiguration = new TierPrimitiveUpgradeConfiguration<int>(cfg, topSection, LguConstants.LIFE_INSURANCE_ENABLED_DESCRIPTION, LifeInsurance.DEFAULT_PRICES)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.LIFE_INSURANCE_INITIAL_COST_PERCENTAGE_DECREASE_KEY, LguConstants.LIFE_INSURANCE_INITIAL_COST_PERCENTAGE_DECREASE_DEFAULT, LguConstants.LIFE_INSURANCE_INITIAL_COST_PERCENTAGE_DECREASE_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.LIFE_INSURANCE_INCREMENTAL_COST_PERCENTAGE_DECREASE_KEY, LguConstants.LIFE_INSURANCE_INCREMENTAL_COST_PERCENTAGE_DECREASE_DEFAULT, LguConstants.LIFE_INSURANCE_INCREMENTAL_COST_PERCENTAGE_DECREASE_DESCRIPTION),
            };

            topSection = RubberBoots.UPGRADE_NAME;
            RubberBootsConfiguration = new TierIndividualPrimitiveUpgradeConfiguration<int>(cfg, topSection, LguConstants.RUBBER_BOOTS_ENABLED_DESCRIPTION, RubberBoots.DEFAULT_PRICES)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.RUBBER_BOOTS_INITIAL_MOVEMENT_HINDERANCE_DECREASE_KEY, LguConstants.RUBBER_BOOTS_INITIAL_MOVEMENT_HINDERANCE_DECREASE_DEFAULT, LguConstants.RUBBER_BOOTS_INITIAL_MOVEMENT_HINDERANCE_DECREASE_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.RUBBER_BOOTS_INCREMENTAL_MOVEMENT_HINDERANCE_DECREASE_KEY, LguConstants.RUBBER_BOOTS_INCREMENTAL_MOVEMENT_HINDERANCE_DECREASE_DEFAULT, LguConstants.RUBBER_BOOTS_INCREMENTAL_MOVEMENT_HINDERANCE_DECREASE_DESCRIPTION),
            };

            topSection = OxygenCanisters.UPGRADE_NAME;
            OxygenCanistersConfiguration = new TierIndividualPrimitiveUpgradeConfiguration<int>(cfg, topSection, LguConstants.OXYGEN_CANISTERS_ENABLED_DESCRIPTION, OxygenCanisters.DEFAULT_PRICES)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.OXYGEN_CANISTERS_INITIAL_OXYGEN_CONSUMPTION_DECREASE_KEY, LguConstants.OXYGEN_CANISTERS_INITIAL_OXYGEN_CONSUMPTION_DECREASE_DEFAULT, LguConstants.OXYGEN_CANISTERS_INITIAL_OXYGEN_CONSUMPTION_DECREASE_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.OXYGEN_CANISTERS_INCREMENTAL_OXYGEN_CONSUMPTION_DECREASE_KEY, LguConstants.OXYGEN_CANISTERS_INCREMENTAL_OXYGEN_CONSUMPTION_DECREASE_DEFAULT, LguConstants.OXYGEN_CANISTERS_INCREMENTAL_OXYGEN_CONSUMPTION_DECREASE_DESCRIPTION),
            };
            topSection = SleightOfHand.UPGRADE_NAME;
            SleightOfHandConfiguration = new TierIndividualPrimitiveUpgradeConfiguration<int>(cfg, topSection, LguConstants.SLEIGHT_OF_HAND_ENABLED_DESCRIPTION, SleightOfHand.PRICES_DEFAULT)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.SLEIGHT_OF_HAND_INITIAL_INCREASE_KEY, LguConstants.SLEIGHT_OF_HAND_INITIAL_INCREASE_DEFAULT, LguConstants.SLEIGHT_OF_HAND_INITIAL_INCREASE_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.SLEIGHT_OF_HAND_INCREMENTAL_INCREASE_KEY, LguConstants.SLEIGHT_OF_HAND_INCREMENTAL_INCREASE_DEFAULT, LguConstants.SLEIGHT_OF_HAND_INCREMENTAL_INCREASE_DESCRIPTION),
            };

            topSection = HikingBoots.UPGRADE_NAME;
            HikingBootsConfiguration = new TierIndividualPrimitiveUpgradeConfiguration<int>(cfg, topSection, LguConstants.HIKING_BOOTS_ENABLED_DESCRIPTION, HikingBoots.PRICES_DEFAULT)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.HIKING_BOOTS_INITIAL_DECREASE_KEY, LguConstants.HIKING_BOOTS_INITIAL_DECREASE_DEFAULT, LguConstants.HIKING_BOOTS_INITIAL_DECREASE_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.HIKING_BOOTS_INCREMENTAL_DECREASE_KEY, LguConstants.HIKING_BOOTS_INCREMENTAL_DECREASE_DEFAULT, LguConstants.HIKING_BOOTS_INCREMENTAL_DECREASE_DESCRIPTION),
            };

            topSection = TractionBoots.UPGRADE_NAME;
            TractionBootsConfiguration = new TierIndividualPrimitiveUpgradeConfiguration<int>(cfg, topSection, LguConstants.TRACTION_BOOTS_ENABLED_DESCRIPTION, TractionBoots.PRICES_DEFAULT)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.TRACTION_BOOTS_INITIAL_INCREASE_KEY, LguConstants.TRACTION_BOOTS_INITIAL_INCREASE_DEFAULT, LguConstants.TRACTION_BOOTS_INITIAL_INCREASE_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.TRACTION_BOOTS_INCREMENTAL_INCREASE_KEY, LguConstants.TRACTION_BOOTS_INCREMENTAL_INCREASE_DEFAULT, LguConstants.TRACTION_BOOTS_INCREMENTAL_INCREASE_DESCRIPTION),
            };

            topSection = FedoraSuit.UPGRADE_NAME;
            FedoraSuitConfiguration = new OneTimeIndividualUpgradeConfiguration(cfg, topSection, LguConstants.FEDORA_SUIT_ENABLED_DESCRIPTION, LguConstants.FEDORA_SUIT_PRICE_DEFAULT);

            topSection = WeedGeneticManipulation.UPGRADE_NAME;
            WeedGeneticManipulationConfiguration = new TierIndividualPrimitiveUpgradeConfiguration<int>(cfg, topSection, LguConstants.WEED_GENETIC_MANIPULATION_ENABLED_DESCRIPTION, WeedGeneticManipulation.PRICES_DEFAULT)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.WEED_GENETIC_INITIAL_EFFECTIVENESS_INCREASE_KEY, LguConstants.WEED_GENETIC_INITIAL_EFFECTIVENESS_INCREASE_DEFAULT, LguConstants.WEED_GENETIC_INITIAL_EFFECTIVENESS_INCREASE_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.WEED_GENETIC_INCREMENTAL_EFFECTIVENESS_INCREASE_KEY, LguConstants.WEED_GENETIC_INCREMENTAL_EFFECTIVENESS_INCREASE_DEFAULT, LguConstants.WEED_GENETIC_INCREMENTAL_EFFECTIVENESS_INCREASE_DESCRIPTION),
            };

            topSection = ClayGlasses.UPGRADE_NAME;
            ClayGlassesConfiguration = new TierIndividualPrimitiveUpgradeConfiguration<float>(cfg, topSection, LguConstants.CLAY_GLASSES_ENABLED_DESCRIPTION, ClayGlasses.PRICES_DEFAULT)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.CLAY_GLASSES_DISTANCE_INITIAL_INCREASE_KEY, LguConstants.CLAY_GLASSES_DISTANCE_INITIAL_INCREASE_DEFAULT, LguConstants.CLAY_GLASSES_DISTANCE_INITIAL_INCREASE_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.CLAY_GLASSES_DISTANCE_INCREMENTAL_INCREASE_KEY, LguConstants.CLAY_GLASSES_DISTANCE_INCREMENTAL_INCREASE_DEFAULT, LguConstants.CLAY_GLASSES_DISTANCE_INCREMENTAL_INCREASE_DESCRIPTION),
            };

            topSection = MechanicalArms.UPGRADE_NAME;
            MechanicalArmsConfiguration = new TierIndividualPrimitiveUpgradeConfiguration<float>(cfg, topSection, LguConstants.MECHANICAL_ARMS_ENABLED_DESCRIPTION, MechanicalArms.PRICES_DEFAULT)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.MECHANICAL_ARMS_INITIAL_RANGE_INCREASE_KEY, LguConstants.MECHANICAL_ARMS_INITIAL_RANGE_INCREASE_DEFAULT, LguConstants.MECHANICAL_ARMS_INITIAL_RANGE_INCREASE_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.MECHANICAL_ARMS_INCREMENTAL_RANGE_INCREASE_KEY, LguConstants.MECHANICAL_ARMS_INCREMENTAL_RANGE_INCREASE_DEFAULT, LguConstants.MECHANICAL_ARMS_INCREMENTAL_RANGE_INCREASE_DESCRIPTION),
            };

            topSection = ScavengerInstincts.UPGRADE_NAME;
            ScavengerInstictsConfiguration = new TierPrimitiveUpgradeConfiguration<int>(cfg, topSection, LguConstants.SCAVENGER_INSTINCTS_ENABLED_DESCRIPTION, ScavengerInstincts.DEFAULT_PRICES)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.SCAVENGER_INSTINCTS_INITIAL_AMOUNT_SCRAP_INCREASE_KEY, LguConstants.SCAVENGER_INSTINCTS_INITIAL_AMOUNT_SCRAP_INCREASE_DEFAULT, LguConstants.SCAVENGER_INSTINCTS_INITIAL_AMOUNT_SCRAP_INCREASE_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.SCAVENGER_INSTINCTS_INCREMENTAL_AMOUNT_SCRAP_INCREASE_KEY, LguConstants.SCAVENGER_INSTINCTS_INCREMENTAL_AMOUNT_SCRAP_INCREASE_DEFAULT, LguConstants.SCAVENGER_INSTINCTS_INCREMENTAL_AMOUNT_SCRAP_INCREASE_DESCRIPTION),
            };

            topSection = LandingThrusters.UPGRADE_NAME;
            LandingThrustersConfiguration = new LandingThrusterUpgradeConfiguration(cfg, topSection, LguConstants.LANDING_THRUSTERS_ENABLED_DESCRIPTION, LandingThrusters.DEFAULT_PRICES)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.LANDING_THRUSTERS_INITIAL_SPEED_INCREASE_KEY, LguConstants.LANDING_THRUSTERS_INITIAL_SPEED_INCREASE_DEFAULT, LguConstants.LANDING_THRUSTERS_INITIAL_SPEED_INCREASE_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.LANDING_THRUSTERS_INCREMENTAL_SPEED_INCREASE_KEY, LguConstants.LANDING_THRUSTERS_INCREMENTAL_SPEED_INCREASE_DEFAULT, LguConstants.LANDING_THRUSTERS_INCREMENTAL_SPEED_INCREASE_DESCRIPTION),
            };

            topSection = ReinforcedBoots.UPGRADE_NAME;
            ReinforcedBootsConfiguration = new TierIndividualPrimitiveUpgradeConfiguration<int>(cfg, topSection, LguConstants.REINFORCED_BOOTS_ENABLED_DESCRIPTION, ReinforcedBoots.DEFAULT_PRICES)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.REINFORCED_BOOTS_INITIAL_DAMAGE_REDUCTION_KEY, LguConstants.REINFORCED_BOOTS_INITIAL_DAMAGE_REDUCTION_DEFAULT, LguConstants.REINFORCED_BOOTS_INITIAL_DAMAGE_REDUCTION_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.REINFORCED_BOOTS_INCREMENTAL_DAMAGE_REDUCTION_KEY, LguConstants.REINFORCED_BOOTS_INCREMENTAL_DAMAGE_REDUCTION_DEFAULT, LguConstants.REINFORCED_BOOTS_INCREMENTAL_DAMAGE_REDUCTION_DESCRIPTION),
            };
            topSection = DeepPockets.UPGRADE_NAME;
            DeeperPocketsConfiguration = new DeeperPocketsUpgradeConfiguration(cfg, topSection, LguConstants.DEEPER_POCKETS_ENABLED_DESCRIPTION, DeepPockets.DEFAULT_PRICES)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.DEEPER_POCKETS_INITIAL_TWO_HANDED_AMOUNT_KEY, LguConstants.DEEPER_POCKETS_INITIAL_TWO_HANDED_AMOUNT_DEFAULT, LguConstants.DEEPER_POCKETS_INITIAL_TWO_HANDED_AMOUNT_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.DEEPER_POCKETS_INCREMENTAL_TWO_HANDED_AMOUNT_KEY, LguConstants.DEEPER_POCKETS_INCREMENTAL_TWO_HANDED_AMOUNT_DEFAULT, LguConstants.DEEPER_POCKETS_INCREMENTAL_TWO_HANDED_AMOUNT_DESCRIPTION),
            };

            topSection = AluminiumCoils.UPGRADE_NAME;
            AluminiumCoilConfiguration = new TierIndividualMultiplePrimitiveUpgradeConfiguration<int, float>(cfg, topSection, LguConstants.ALUMINIUM_COILS_ENABLED_DESCRIPTION, AluminiumCoils.DEFAULT_PRICES)
            {
                InitialEffects =
                [
                    cfg.BindSyncedEntry(topSection, LguConstants.ALUMINIUM_COILS_INITIAL_DIFFICULTY_MULTIPLIER_KEY, LguConstants.ALUMINIUM_COILS_INITIAL_DIFFICULTY_MULTIPLIER_DEFAULT, LguConstants.ALUMINIUM_COILS_INITIAL_DIFFICULTY_MULTIPLIER_DESCRIPTION),
                    cfg.BindSyncedEntry(topSection, LguConstants.ALUMINIUM_COILS_INITIAL_COOLDOWN_KEY, LguConstants.ALUMINIUM_COILS_INITIAL_COOLDOWN_DEFAULT, LguConstants.ALUMINIUM_COILS_INITIAL_COOLDOWN_DESCRIPTION)
                ],
                IncrementalEffects =
                [
                    cfg.BindSyncedEntry(topSection, LguConstants.ALUMINIUM_COILS_INCREMENTAL_DIFFICULTY_MULTIPLIER_KEY, LguConstants.ALUMINIUM_COILS_INCREMENTAL_DIFFICULTY_MULTIPLIER_DEFAULT, LguConstants.ALUMINIUM_COILS_INCREMENTAL_DIFFICULTY_MULTIPLIER_DESCRIPTION),
                    cfg.BindSyncedEntry(topSection, LguConstants.ALUMINIUM_COILS_INCREMENTAL_COOLDOWN_KEY, LguConstants.ALUMINIUM_COILS_INCREMENTAL_COOLDOWN_DEFAULT, LguConstants.ALUMINIUM_COILS_INCREMENTAL_COOLDOWN_DESCRIPTION)
                ],
                InitialSecondEffects =
                [
                    cfg.BindSyncedEntry(topSection, LguConstants.ALUMINIUM_COILS_INITIAL_STUN_TIMER_KEY, LguConstants.ALUMINIUM_COILS_INITIAL_STUN_TIMER_DEFAULT),
                    cfg.BindSyncedEntry(topSection, LguConstants.ALUMINIUM_COILS_INITIAL_RANGE_KEY, LguConstants.ALUMINIUM_COILS_INITIAL_RANGE_DEFAULT),
                ],
                IncrementalSecondEffects =
                [
                    cfg.BindSyncedEntry(topSection, LguConstants.ALUMINIUM_COILS_INCREMENTAL_STUN_TIMER_KEY, LguConstants.ALUMINIUM_COILS_INCREMENTAL_STUN_TIMER_DEFAULT),
                    cfg.BindSyncedEntry(topSection, LguConstants.ALUMINIUM_COILS_INCREMENTAL_RANGE_KEY, LguConstants.ALUMINIUM_COILS_INCREMENTAL_RANGE_DEFAULT),
                ]
            };

            topSection = BackMuscles.UPGRADE_NAME;
            BackMusclesConfiguration = new TierIndividualAlternativePrimitiveUpgradeConfiguration<float, BackMuscles.UpgradeMode>(cfg, topSection, LguConstants.BACK_MUSCLES_ENABLED_DESCRIPTION, BackMuscles.PRICES_DEFAULT)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.BACK_MUSCLES_INITIAL_WEIGHT_MULTIPLIER_KEY, LguConstants.BACK_MUSCLES_INITIAL_WEIGHT_MULTIPLIER_DEFAULT, LguConstants.BACK_MUSCLES_INITIAL_WEIGHT_MULTIPLIER_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.BACK_MUSCLES_INCREMENTAL_WEIGHT_MULTIPLIER_KEY, LguConstants.BACK_MUSCLES_INCREMENTAL_WEIGHT_MULTIPLIER_DEFAULT, LguConstants.BACK_MUSCLES_INCREMENTAL_WEIGHT_MULTIPLIER_DESCRIPTION),
                AlternativeMode = cfg.BindSyncedEntry(topSection, LguConstants.BACK_MUSCLES_UPGRADE_MODE_KEY, LguConstants.BACK_MUSCLES_UPGRADE_MODE_DEFAULT, LguConstants.BACK_MUSCLES_UPGRADE_MODE_DESCRIPTION),
            };

            topSection = BargainConnections.UPGRADE_NAME;
            BargainConnectionsConfiguration = new TierPrimitiveUpgradeConfiguration<int>(cfg, topSection, LguConstants.BARGAIN_CONNECTIONS_ENABLED_DESCRIPTION, BargainConnections.PRICES_DEFAULT)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.BARGAIN_CONNECTIONS_INITIAL_AMOUNT_KEY, LguConstants.BARGAIN_CONNECTIONS_INITIAL_AMOUNT_DEFAULT),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.BARGAIN_CONNECTIONS_INCREMENTAL_AMOUNT_KEY, LguConstants.BARGAIN_CONNECTIONS_INCREMENTAL_AMOUNT_DEFAULT),
            };

            topSection = Beekeeper.UPGRADE_NAME;
            BeekeeperConfiguration = new TierIndividualMultiplePrimitiveUpgradeConfiguration<float>(cfg, topSection, LguConstants.BEEKEEPER_ENABLED_DESCRIPTION, Beekeeper.PRICES_DEFAULT)
            {
                InitialEffects =
                [
                    cfg.BindSyncedEntry(topSection, LguConstants.BEEKEEPER_DAMAGE_MULTIPLIER_KEY, LguConstants.BEEKEEPER_DAMAGE_MULTIPLIER_DEFAULT, LguConstants.BEEKEEPER_DAMAGE_MULTIPLIER_DESCRIPTION),
                    cfg.BindSyncedEntry(topSection, LguConstants.BEEKEEPER_HIVE_MULTIPLIER_KEY, LguConstants.BEEKEEPER_HIVE_MULTIPLIER_DEFAULT, LguConstants.BEEKEEPER_HIVE_MULTIPLIER_DESCRIPTION),
                ],
                IncrementalEffects =
                [
                    cfg.BindSyncedEntry(topSection, LguConstants.BEEKEEPER_DAMAGE_INCREMENTAL_MULTIPLIER_KEY, LguConstants.BEEKEEPER_DAMAGE_INCREMENTAL_MULTIPLIER_DEFAULT, LguConstants.BEEKEEPER_DAMAGE_INCREMENTAL_MULTIPLIER_DESCRIPTION),
                    null,
                ],
            };

            topSection = BetterScanner.UPGRADE_NAME;
            BetterScannerUpgradeConfiguration = new BetterScannerUpgradeConfiguration(cfg, topSection, LguConstants.BETTER_SCANNER_ENABLED_DESCRIPTION, LguConstants.BETTER_SCANNER_PRICE_DEFAULT);

            topSection = BiggerLungs.UPGRADE_NAME;
            BiggerLungsConfiguration = new BiggerLungsUpgradeConfiguration(cfg, topSection, LguConstants.BIGGER_LUNGS_ENABLED_DESCRIPTION, BiggerLungs.PRICES_DEFAULT)
            {
                InitialEffects =
                [
                    cfg.BindSyncedEntry(topSection, LguConstants.BIGGER_LUNGS_INITIAL_SPRINT_TIME_KEY, LguConstants.BIGGER_LUNGS_INITIAL_SPRINT_TIME_DEFAULT, LguConstants.BIGGER_LUNGS_INITIAL_SPRINT_TIME_DESCRIPTION),
                    cfg.BindSyncedEntry(topSection, LguConstants.BIGGER_LUNGS_INITIAL_STAMINA_REGEN_KEY, LguConstants.BIGGER_LUNGS_INITIAL_STAMINA_REGEN_DEFAULT, LguConstants.BIGGER_LUNGS_INITIAL_STAMINA_REGEN_DESCRIPTION),
                    cfg.BindSyncedEntry(topSection, LguConstants.BIGGER_LUNGS_INITIAL_JUMP_STAMINA_DECREASE_KEY, LguConstants.BIGGER_LUNGS_INITIAL_JUMP_STAMINA_DECREASE_DEFAULT, LguConstants.BIGGER_LUNGS_INITIAL_JUMP_STAMINA_DECREASE_DESCRIPTION)
                ],
                IncrementalEffects =
                [
                    cfg.BindSyncedEntry(topSection, LguConstants.BIGGER_LUNGS_INCREMENTAL_SPRINT_TIME_KEY, LguConstants.BIGGER_LUNGS_INCREMENTAL_SPRINT_TIME_DEFAULT, LguConstants.BIGGER_LUNGS_INCREMENTAL_SPRINT_TIME_DESCRIPTION),
                    cfg.BindSyncedEntry(topSection, LguConstants.BIGGER_LUNGS_INCREMENTAL_STAMINA_REGEN_KEY, LguConstants.BIGGER_LUNGS_INCREMENTAL_STAMINA_REGEN_DEFAULT, LguConstants.BIGGER_LUNGS_INCREMENTAL_STAMINA_REGEN_DESCRIPTION),
                    cfg.BindSyncedEntry(topSection, LguConstants.BIGGER_LUNGS_INCREMENTAL_JUMP_STAMINA_DECREASE_KEY, LguConstants.BIGGER_LUNGS_INCREMENTAL_JUMP_STAMINA_DECREASE_DEFAULT, LguConstants.BIGGER_LUNGS_INCREMENTAL_JUMP_STAMINA_DECREASE_DESCRIPTION)
                ],
                StaminaRegenerationLevel = cfg.BindSyncedEntry(topSection, LguConstants.BIGGER_LUNGS_STAMINA_APPLY_LEVEL_KEY, LguConstants.BIGGER_LUNGS_STAMINA_APPLY_LEVEL_DEFAULT, LguConstants.BIGGER_LUNGS_STAMINA_APPLY_LEVEL_DESCRIPTION),
                JumpReductionLevel = cfg.BindSyncedEntry(topSection, LguConstants.BIGGER_LUNGS_JUMP_STAMINA_DECREASE_APPLY_LEVEL_KEY, LguConstants.BIGGER_LUNGS_JUMP_STAMINA_DECREASE_APPLY_LEVEL_DEFAULT, LguConstants.BIGGER_LUNGS_JUMP_STAMINA_DECREASE_APPLY_LEVEL_DESCRIPTION),
            };

            topSection = ChargingBooster.UPGRADE_NAME;
            ChargingBoosterConfiguration = new TierMultiplePrimitiveUpgradeConfiguration<float, int>(cfg, topSection, LguConstants.CHARGING_BOOSTER_ENABLED_DESCRIPTION, LguConstants.CHARGING_BOOSTER_PRICES_DEFAULT)
            {
                InitialEffects =
                [
                    cfg.BindSyncedEntry(topSection, LguConstants.CHARGING_BOOSTER_COOLDOWN_KEY, LguConstants.CHARGING_BOOSTER_COOLDOWN_DEFAULT),
                ],
                IncrementalEffects =
                [
                    cfg.BindSyncedEntry(topSection, LguConstants.CHARGING_BOOSTER_INCREMENTAL_COOLDOWN_DECREASE_KEY, LguConstants.CHARGING_BOOSTER_INCREMENTAL_COOLDOWN_DECREASE_DEFAULT)
                ],
                InitialSecondEffects =
                [
                    cfg.BindSyncedEntry(topSection, LguConstants.CHARGING_BOOSTER_CHARGE_PERCENTAGE_KEY, LguConstants.CHARGING_BOOSTER_CHARGE_PERCENTAGE_DEFAULT)
                ],
                IncrementalSecondEffects =
                [
                    null
                ]
            };

            topSection = ClimbingGloves.UPGRADE_NAME;
            ClimblingGlovesConfiguration = new TierIndividualPrimitiveUpgradeConfiguration<float>(cfg, topSection, LguConstants.CLIMBING_GLOVES_ENABLED_DESCRIPTION, ClimbingGloves.DEFAULT_PRICES)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.CLIMBING_GLOVES_INITIAL_MULTIPLIER_KEY, LguConstants.CLIMBING_GLOVES_INITIAL_MULTIPLIER_DEFAULT, LguConstants.CLIMBING_GLOVES_INITIAL_MULTIPLIER_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.CLIMBING_GLOVES_INCREMENTAL_MULTIPLIER_KEY, LguConstants.CLIMBING_GLOVES_INCREMENTAL_MULTIPLIER_DEFAULT),
            };

            topSection = Discombobulator.UPGRADE_NAME;
            DiscombobulatorUpgradeConfiguration = new DiscombobulatorUpgradeConfiguration(cfg, topSection, LguConstants.DISCOMBOBULATOR_ENABLED_DESCRIPTION, Discombobulator.PRICES_DEFAULT)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.DISCOMBOBULATOR_STUN_DURATION_KEY, LguConstants.DISCOMBOBULATOR_STUN_DURATION_DEFAULT),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.DISCOMBOBULATOR_INCREMENTAL_STUN_TIME_KEY, LguConstants.DISCOMBOBULATOR_INCREMENTAL_STUN_TIME_DEFAULT, LguConstants.DISCOMBOBULATOR_INCREMENTAL_STUN_TIME_DESCRIPTION),
            };

            topSection = FasterDropPod.UPGRADE_NAME;
            DropPodThrustersConfiguration = new DropPodThrustersUpgradeConfiguration(cfg, topSection, LguConstants.DROP_POD_THRUSTERS_ENABLED_DESCRIPTION, LguConstants.DROP_POD_THRUSTERS_PRICE_DEFAULT);

            topSection = EfficientEngines.UPGRADE_NAME;
            EfficientEnginesConfiguration = new TierPrimitiveUpgradeConfiguration<int>(cfg, topSection, LguConstants.EFFICIENT_ENGINES_ENABLED_DESCRIPTION, EfficientEngines.DEFAULT_PRICES)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.EFFICIENT_ENGINES_INITIAL_MULTIPLIER_KEY, LguConstants.EFFICIENT_ENGINES_INITIAL_MULTIPLIER_DEFAULT),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.EFFICIENT_ENGINES_INCREMENTAL_MULTIPLIER_KEY, LguConstants.EFFICIENT_ENGINES_INCREMENTAL_MULTIPLIER_DEFAULT),
            };

            topSection = FastEncryption.UPGRADE_NAME;
            FastEncryptionConfiguration = new OneTimeUpgradeConfiguration(cfg, topSection, LguConstants.FAST_ENCRYPTION_ENABLED_DESCRIPTION, LguConstants.FAST_ENCRYPTION_PRICE_DEFAULT);

            #region Hunter

            topSection = Hunter.UPGRADE_NAME;
            HunterConfiguration = new TierCollectionUpgradeConfiguration(cfg, topSection, LguConstants.HUNTER_ENABLED_DESCRIPTION, Hunter.PRICES_DEFAULT)
            {
                TierCollection = cfg.BindSyncedEntry(topSection, LguConstants.HUNTER_SAMPLE_TIERS_KEY, LguConstants.HUNTER_SAMPLE_TIERS_DEFAULT, LguConstants.HUNTER_SAMPLE_TIERS_DESCRIPTION),
            };
            SNARE_FLEA_SAMPLE_MINIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.SNARE_FLEA_SAMPLE_MINIMUM_VALUE_KEY, LguConstants.SNARE_FLEA_SAMPLE_MINIMUM_VALUE_DEFAULT);
            SNARE_FLEA_SAMPLE_MAXIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.SNARE_FLEA_SAMPLE_MAXIMUM_VALUE_KEY, LguConstants.SNARE_FLEA_SAMPLE_MAXIMUM_VALUE_DEFAULT);
            BUNKER_SPIDER_SAMPLE_MINIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.BUNKER_SPIDER_SAMPLE_MINIMUM_VALUE_KEY, LguConstants.BUNKER_SPIDER_SAMPLE_MINIMUM_VALUE_DEFAULT);
            BUNKER_SPIDER_SAMPLE_MAXIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.BUNKER_SPIDER_SAMPLE_MAXIMUM_VALUE_KEY, LguConstants.BUNKER_SPIDER_SAMPLE_MAXIMUM_VALUE_DEFAULT);
            HOARDING_BUG_SAMPLE_MINIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.HOARDING_BUG_SAMPLE_MINIMUM_VALUE_KEY, LguConstants.HOARDING_BUG_SAMPLE_MINIMUM_VALUE_DEFAULT);
            HOARDING_BUG_SAMPLE_MAXIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.HOARDING_BUG_SAMPLE_MAXIMUM_VALUE_KEY, LguConstants.HOARDING_BUG_SAMPLE_MAXIMUM_VALUE_DEFAULT);
            BRACKEN_SAMPLE_MINIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.BRACKEN_SAMPLE_MINIMUM_VALUE_KEY, LguConstants.BRACKEN_SAMPLE_MINIMUM_VALUE_DEFAULT);
            BRACKEN_SAMPLE_MAXIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.BRACKEN_SAMPLE_MAXIMUM_VALUE_KEY, LguConstants.BRACKEN_SAMPLE_MAXIMUM_VALUE_DEFAULT);
            EYELESS_DOG_SAMPLE_MINIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.EYELESS_DOG_SAMPLE_MINIMUM_VALUE_KEY, LguConstants.EYELESS_DOG_SAMPLE_MINIMUM_VALUE_DEFAULT);
            EYELESS_DOG_SAMPLE_MAXIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.EYELESS_DOG_SAMPLE_MAXIMUM_VALUE_KEY, LguConstants.EYELESS_DOG_SAMPLE_MAXIMUM_VALUE_DEFAULT);
            BABOON_HAWK_SAMPLE_MINIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.BABOON_HAWK_SAMPLE_MINIMUM_VALUE_KEY, LguConstants.BABOON_HAWK_SAMPLE_MINIMUM_VALUE_DEFAULT);
            BABOON_HAWK_SAMPLE_MAXIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.BABOON_HAWK_SAMPLE_MAXIMUM_VALUE_KEY, LguConstants.BABOON_HAWK_SAMPLE_MAXIMUM_VALUE_DEFAULT);
            THUMPER_SAMPLE_MINIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.THUMPER_SAMPLE_MINIMUM_VALUE_KEY, LguConstants.THUMPER_SAMPLE_MINIMUM_VALUE_DEFAULT);
            THUMPER_SAMPLE_MAXIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.THUMPER_SAMPLE_MAXIMUM_VALUE_KEY, LguConstants.THUMPER_SAMPLE_MAXIMUM_VALUE_DEFAULT);
            FOREST_KEEPER_SAMPLE_MINIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.FOREST_KEEPER_SAMPLE_MINIMUM_VALUE_KEY, LguConstants.FOREST_KEEPER_SAMPLE_MINIMUM_VALUE_DEFAULT);
            FOREST_KEEPER_SAMPLE_MAXIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.FOREST_KEEPER_SAMPLE_MAXIMUM_VALUE_KEY, LguConstants.FOREST_KEEPER_SAMPLE_MAXIMUM_VALUE_DEFAULT);
            MANTICOIL_SAMPLE_MINIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.MANTICOIL_SAMPLE_MINIMUM_VALUE_KEY, LguConstants.MANTICOIL_SAMPLE_MINIMUM_VALUE_DEFAULT);
            MANTICOIL_SAMPLE_MAXIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.MANTICOIL_SAMPLE_MAXIMUM_VALUE_KEY, LguConstants.MANTICOIL_SAMPLE_MAXIMUM_VALUE_DEFAULT);
            TULIP_SNAKE_SAMPLE_MINIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.TULIP_SNAKE_SAMPLE_MINIMUM_VALUE_KEY, LguConstants.TULIP_SNAKE_SAMPLE_MINIMUM_VALUE_DEFAULT);
            TULIP_SNAKE_SAMPLE_MAXIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.TULIP_SNAKE_SAMPLE_MAXIMUM_VALUE_KEY, LguConstants.TULIP_SNAKE_SAMPLE_MAXIMUM_VALUE_DEFAULT);
            KIDNAPPER_FOX_SAMPLE_MINIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.KIDNAPPER_FOX_SAMPLE_MINIMUM_VALUE_KEY, LguConstants.KIDNAPPER_FOX_SAMPLE_MINIMUM_VALUE_DEFAULT);
            KIDNAPPER_FOX_SAMPLE_MAXIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.KIDNAPPER_FOX_SAMPLE_MAXIMUM_VALUE_KEY, LguConstants.KIDNAPPER_FOX_SAMPLE_MAXIMUM_VALUE_DEFAULT);
            SPORE_LIZARD_SAMPLE_MINIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.SPORE_LIZARD_SAMPLE_MINIMUM_VALUE_KEY, LguConstants.SPORE_LIZARD_SAMPLE_MINIMUM_VALUE_DEFAULT);
            SPORE_LIZARD_SAMPLE_MAXIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.SPORE_LIZARD_SAMPLE_MAXIMUM_VALUE_KEY, LguConstants.SPORE_LIZARD_SAMPLE_MAXIMUM_VALUE_DEFAULT);
            MANEATER_SAMPLE_MINIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.MANEATER_SAMPLE_MINIMUM_VALUE_KEY, LguConstants.MANEATER_SAMPLE_MINIMUM_VALUE_DEFAULT);
            MANEATER_SAMPLE_MAXIMUM_VALUE = cfg.BindSyncedEntry(topSection, LguConstants.MANEATER_SAMPLE_MAXIMUM_VALUE_KEY, LguConstants.MANEATER_SAMPLE_MAXIMUM_VALUE_DEFAULT);

            #endregion

            topSection = LethalDeals.UPGRADE_NAME;
            LethalDealsConfiguration = new OneTimeUpgradeConfiguration(cfg, topSection, LguConstants.LETHAL_DEALS_ENABLED_DESCRIPTION, LguConstants.LETHAL_DEALS_PRICE_DEFAULT);

            topSection = LightningRod.UPGRADE_NAME;
            LightningRodConfiguration = new OneTimeAlternativePrimitiveUpgradeConfiguration<float, LightningRod.UpgradeMode>(cfg, topSection, LightningRod.ENABLED_DESCRIPTION, LightningRod.PRICE_DEFAULT)
            {
                Effect = cfg.BindSyncedEntry(topSection, LightningRod.DIST_SECTION, LightningRod.DIST_DEFAULT, LightningRod.DIST_DESCRIPTION),
                AlternativeMode = cfg.BindSyncedEntry(topSection, LightningRod.UPGRADE_MODE_SECTION, LightningRod.UPGRADE_MODE_DEFAULT, LightningRod.UPGRADE_MODE_DESCRIPTION)
            };

            topSection = LithiumBatteries.UPGRADE_NAME;
            LithiumBatteriesConfiguration = new TierIndividualPrimitiveUpgradeConfiguration<int>(cfg, topSection, LguConstants.LITHIUM_BATTERIES_ENABLED_DESCRIPTION, LithiumBatteries.PRICES_DEFAULT)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.LITHIUM_BATTERIES_INITIAL_MULTIPLIER_KEY, LguConstants.LITHIUM_BATTERIES_INITIAL_MULTIPLIER_DEFAULT, LguConstants.LITHIUM_BATTERIES_INITIAL_MULTIPLIER_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.LITHIUM_BATTERIES_INCREMENTAL_MULTIPLIER_KEY, LguConstants.LITHIUM_BATTERIES_INCREMENTAL_MULTIPLIER_DEFAULT, LguConstants.LITHIUM_BATTERIES_INCREMENTAL_MULTIPLIER_DESCRIPTION),
            };

            topSection = LockSmith.UPGRADE_NAME;
            LocksmithConfiguration = new OneTimeIndividualUpgradeConfiguration(cfg, topSection, LguConstants.LOCKSMITH_ENABLED_DESCRIPTION, LguConstants.LOCKSMITH_PRICE_DEFAULT);

            topSection = MalwareBroadcaster.UPGRADE_NAME;
            MalwareBroadcasterUpgradeConfiguration = new(cfg, topSection, LguConstants.MALWARE_BROADCASTER_ENABLED_DESCRIPTION, LguConstants.MALWARE_BROADCASTER_PRICE_DEFAULT);

            topSection = MarketInfluence.UPGRADE_NAME;
            MarketInfluenceConfiguration = new TierPrimitiveUpgradeConfiguration<int>(cfg, topSection, LguConstants.MARKET_INFLUENCE_ENABLED_DESCRIPTION, MarketInfluence.PRICES_DEFAULT)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.MARKET_INFLUENCE_INITIAL_PERCENTAGE_KEY, LguConstants.MARKET_INFLUENCE_INITIAL_PERCENTAGE_DEFAULT),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.MARKET_INFLUENCE_INCREMENTAL_PERCENTAGE_KEY, LguConstants.MARKET_INFLUENCE_INCREMENTAL_PERCENTAGE_DEFAULT),
            };

            topSection = "Night Vision";
            NightVisionUpgradeConfiguration = new NightVisionUpgradeConfiguration(cfg, topSection, LguConstants.NIGHT_VISION_ENABLED_DESCRIPTION, NightVision.PRICES_DEFAULT)
            {
                InitialEffects =
                [
                    cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_BATTERY_MAX_KEY, LguConstants.NIGHT_VISION_BATTERY_MAX_DEFAULT, LguConstants.NIGHT_VISION_BATTERY_MAX_DESCRIPTION),
					cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_REGEN_SPEED_KEY, LguConstants.NIGHT_VISION_REGEN_SPEED_DEFAULT, LguConstants.NIGHT_VISION_REGEN_SPEED_DESCRIPTION),
					cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_DRAIN_SPEED_KEY, LguConstants.NIGHT_VISION_DRAIN_SPEED_DEFAULT, LguConstants.NIGHT_VISION_DRAIN_SPEED_DESCRIPTION),
                    cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_RANGE_KEY, LguConstants.NIGHT_VISION_RANGE_DEFAULT, LguConstants.NIGHT_VISION_RANGE_DESCRIPTION),
                    cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_INTENSITY_KEY, LguConstants.NIGHT_VISION_INTENSITY_DEFAULT, LguConstants.NIGHT_VISION_INTENSITY_DESCRIPTION),
                ],
                IncrementalEffects =
                [
                    cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_BATTERY_INCREMENT_KEY, LguConstants.NIGHT_VISION_BATTERY_INCREMENT_DEFAULT, LguConstants.NIGHT_VISION_BATTERY_INCREMENT_DESCRIPTION),
					cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_REGEN_INCREMENT_KEY, LguConstants.NIGHT_VISION_REGEN_INCREMENT_DEFAULT, LguConstants.NIGHT_VISION_REGEN_INCREMENT_DESCRIPTION),
					cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_DRAIN_INCREMENT_KEY, LguConstants.NIGHT_VISION_DRAIN_INCREMENT_DEFAULT, LguConstants.NIGHT_VISION_DRAIN_INCREMENT_DESCRIPTION),
                    cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_RANGE_INCREMENT_KEY, LguConstants.NIGHT_VISION_RANGE_INCREMENT_DEFAULT, LguConstants.NIGHT_VISION_RANGE_INCREMENT_DESCRIPTION),
                    cfg.BindSyncedEntry(topSection, LguConstants.NIGHT_VISION_INTENSITY_INCREMENT_KEY, LguConstants.NIGHT_VISION_INTENSITY_INCREMENT_DEFAULT, LguConstants.NIGHT_VISION_INTENSITY_INCREMENT_DESCRIPTION),
                ]
            };

            topSection = ProteinPowder.UPGRADE_NAME;
            ProteinPowderConfiguration = new TierIndividualMultiplePrimitiveUpgradeConfiguration<int, float>(cfg, topSection, ProteinPowder.ENABLED_DESCRIPTION, ProteinPowder.PRICES_DEFAULT)
            {
                InitialEffects =
                [
                    cfg.BindSyncedEntry(topSection, ProteinPowder.UNLOCK_FORCE_SECTION, ProteinPowder.UNLOCK_FORCE_DEFAULT, ProteinPowder.UNLOCK_FORCE_DESCRIPTION),
                ],
                IncrementalEffects =
                [
                    cfg.BindSyncedEntry(topSection, ProteinPowder.INCREMENT_FORCE_SECTION, ProteinPowder.INCREMENT_FORCE_DEFAULT, ProteinPowder.INCREMENT_FORCE_DESCRIPTION),
                ],
                InitialSecondEffects =
                [
                    cfg.BindSyncedEntry(topSection, ProteinPowder.CRIT_CHANCE_SECTION, ProteinPowder.CRIT_CHANCE_DEFAULT, ProteinPowder.CRIT_CHANCE_DESCRIPTION),
                ],
                IncrementalSecondEffects =
                [
                    null
                ],
            };

            topSection = QuantumDisruptor.UPGRADE_NAME;
            QuantumDisruptorConfiguration = new QuantumDisruptorUpgradeConfiguration(cfg, topSection, LguConstants.QUANTUM_DISRUPTOR_ENABLED_DESCRIPTION, QuantumDisruptor.PRICES_DEFAULT)
            {
                ResetMode = cfg.BindSyncedEntry(topSection, LguConstants.QUANTUM_DISRUPTOR_RESET_MODE_KEY, LguConstants.QUANTUM_DISRUPTOR_RESET_MODE_DEFAULT, LguConstants.QUANTUM_DISRUPTOR_RESET_MODE_DESCRIPTION),
                AlternativeMode = cfg.BindSyncedEntry(topSection, LguConstants.QUANTUM_DISRUPTOR_UPGRADE_MODE_KEY, LguConstants.QUANTUM_DISRUPTOR_UPGRADE_MODE_DEFAULT, LguConstants.QUANTUM_DISRUPTOR_UPGRADE_MODE_DESCRIPTION),
                InitialEffects =
                [
                    cfg.BindSyncedEntry(topSection, LguConstants.QUANTUM_DISRUPTOR_INITIAL_MULTIPLIER_KEY, LguConstants.QUANTUM_DISRUPTOR_INITIAL_MULTIPLIER_DEFAULT),
                    cfg.BindSyncedEntry(topSection, LguConstants.QUANTUM_DISRUPTOR_INITIAL_USES_KEY, LguConstants.QUANTUM_DISRUPTOR_INITIAL_USES_DEFAULT, LguConstants.QUANTUM_DISRUPTOR_INITIAL_USES_DESCRIPTION),
                    cfg.BindSyncedEntry(topSection, LguConstants.QUANTUM_DISRUPTOR_INITIAL_HOURS_ON_REVERT_KEY, LguConstants.QUANTUM_DISRUPTOR_INITIAL_HOURS_ON_REVERT_DEFAULT, LguConstants.QUANTUM_DISRUPTOR_INITIAL_HOURS_ON_REVERT_DESCRIPTION),
                ],
                IncrementalEffects =
                [
                    cfg.BindSyncedEntry(topSection, LguConstants.QUANTUM_DISRUPTOR_INCREMENTAL_MULTIPLIER_KEY, LguConstants.QUANTUM_DISRUPTOR_INCREMENTAL_MULTIPLIER_DEFAULT),
                    cfg.BindSyncedEntry(topSection, LguConstants.QUANTUM_DISRUPTOR_INCREMENTAL_USES_KEY, LguConstants.QUANTUM_DISRUPTOR_INCREMENTAL_USES_DEFAULT, LguConstants.QUANTUM_DISRUPTOR_INCREMENTAL_USES_DESCRIPTION),
                    cfg.BindSyncedEntry(topSection, LguConstants.QUANTUM_DISRUPTOR_INCREMENTAL_HOURS_ON_REVERT_KEY, LguConstants.QUANTUM_DISRUPTOR_INCREMENTAL_HOURS_ON_REVERT_DEFAULT, LguConstants.QUANTUM_DISRUPTOR_INCREMENTAL_HOURS_ON_REVERT_DESCRIPTION),
                ]
            };

            topSection = RunningShoes.UPGRADE_NAME;
            RunningShoesConfiguration = new TierIndividualMultiplePrimitiveUpgradeConfiguration<float>(cfg, topSection, LguConstants.RUNNING_SHOES_ENABLED_DESCRIPTION, RunningShoes.PRICES_DEFAULT)
            {
                InitialEffects =
                [
                    cfg.BindSyncedEntry(topSection, LguConstants.RUNNING_SHOES_INITIAL_MOVEMENT_BOOST_KEY, LguConstants.RUNNING_SHOES_INITIAL_MOVEMENT_BOOST_DEFAULT, LguConstants.RUNNING_SHOES_INITIAL_MOVEMENT_BOOST_DESCRIPTION),
                    cfg.BindSyncedEntry(topSection, LguConstants.RUNNING_SHOES_NOISE_REDUCTION_KEY, LguConstants.RUNNING_SHOES_NOISE_REDUCTION_DEFAULT, LguConstants.RUNNING_SHOES_NOISE_REDUCTION_DESCRIPTION)
                ],
                IncrementalEffects =
                [
                    cfg.BindSyncedEntry(topSection, LguConstants.RUNNING_SHOES_INCREMENTAL_MOVEMENT_BOOST_KEY, LguConstants.RUNNING_SHOES_INCREMENTAL_MOVEMENT_BOOST_DEFAULT, LguConstants.RUNNING_SHOES_INCREMENTAL_MOVEMENT_BOOST_DESCRIPTION),
                    null
                ]
            };

            topSection = ShutterBatteries.UPGRADE_NAME;
            ShutterBatteriesConfiguration = new TierPrimitiveUpgradeConfiguration<float>(cfg, topSection, ShutterBatteries.ENABLED_DESCRIPTION, ShutterBatteries.PRICES_DEFAULT)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, ShutterBatteries.INITIAL_SECTION, ShutterBatteries.INITIAL_DEFAULT, ShutterBatteries.INITIAL_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, ShutterBatteries.INCREMENTAL_SECTION, ShutterBatteries.INCREMENTAL_DEFAULT, ShutterBatteries.INCREMENTAL_DESCRIPTION),
            };

            topSection = SickBeats.UPGRADE_NAME;
            SickBeatsUpgradeConfiguration = new SickBeatsUpgradeConfiguration(cfg, topSection, LguConstants.SICK_BEATS_ENABLED_DESCRIPTION, LguConstants.SICK_BEATS_PRICE_DEFAULT);

            topSection = Sigurd.UPGRADE_NAME;
            SigurdAccessConfiguration = new SigurdAccessUpgradeConfiguration(cfg, topSection, LguConstants.SIGURD_ACCESS_ENABLED_DESCRIPTION, LguConstants.SIGURD_ACCESS_PRICE_DEFAULT)
            {
                Effect = cfg.BindSyncedEntry(topSection, LguConstants.SIGURD_ACCESS_ADDITIONAL_PERCENT_KEY, LguConstants.SIGURD_ACCESS_ADDITIONAL_PERCENT_DEFAULT),
                Chance = cfg.BindSyncedEntry(topSection, LguConstants.SIGURD_ACCESS_CHANCE_KEY, LguConstants.SIGURD_ACCESS_CHANCE_DEFAULT),
                AlternativeMode = cfg.BindSyncedEntry(topSection, "Sigurd Function Mode", Sigurd.FunctionModes.AllDays, "Supported Modes:\nAllDays: All days have a chance of increased Company Buy Rate\nLastDay: Day of the deadline has a chance of increased COmpany Buy Rate"),
            };

            topSection = Stimpack.UPGRADE_NAME;
            StimpackConfiguration = new TierIndividualPrimitiveUpgradeConfiguration<int>(cfg, topSection, Stimpack.ENABLED_DESCRIPTION, Stimpack.PRICES_DEFAULT)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, Stimpack.ADDITIONAL_HEALTH_UNLOCK_SECTION, Stimpack.ADDITIONAL_HEALTH_UNLOCK_DEFAULT, Stimpack.ADDITIONAL_HEALTH_UNLOCK_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, Stimpack.ADDITIONAL_HEALTH_INCREMENT_SECTION, Stimpack.ADDITIONAL_HEALTH_INCREMENT_DEFAULT, Stimpack.ADDITIONAL_HEALTH_INCREMENT_DESCRIPTION),
            };
            topSection = StrongLegs.UPGRADE_NAME;
            StrongLegsConfiguration = new TierIndividualPrimitiveUpgradeConfiguration<float>(cfg, topSection, LguConstants.STRONG_LEGS_ENABLED_DESCRIPTION, StrongLegs.PRICES_DEFAULT)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.STRONG_LEGS_INITIAL_JUMP_FORCE_KEY, LguConstants.STRONG_LEGS_INITIAL_JUMP_FORCE_DEFAULT, LguConstants.STRONG_LEGS_INITIAL_JUMP_FORCE_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.STRONG_LEGS_INCREMENTAL_JUMP_FORCE_KEY, LguConstants.STRONG_LEGS_INCREMENTAL_JUMP_FORCE_DEFAULT, LguConstants.STRONG_LEGS_INCREMENTAL_JUMP_FORCE_DESCRIPTION),
            };

            topSection = WalkieGPS.UPGRADE_NAME;
            WalkieGpsConfiguration = new OneTimeIndividualUpgradeConfiguration(cfg, topSection, LguConstants.WALKIE_GPS_ENABLED_DESCRIPTION, LguConstants.WALKIE_GPS_PRICE_DEFAULT);

            topSection = BabyPacifier.UPGRADE_NAME;
            BabyPacifierUpgradeConfiguration = new TierPrimitiveUpgradeConfiguration<int>(cfg, topSection, LguConstants.BABY_PACIFIER_ENABLED_DESCRIPTION, BabyPacifier.PRICES_DEFAULT)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.BABY_PACIFIER_INITIAL_PERCENTAGE_KEY, LguConstants.BABY_PACIFIER_INITIAL_PERCENTAGE_DEFAULT, LguConstants.BABY_PACIFIER_INITIAL_PERCENTAGE_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.BABY_PACIFIER_INCREMENTAL_PERCENTAGE_KEY, LguConstants.BABY_PACIFIER_INCREMENTAL_PERCENTAGE_DEFAULT, LguConstants.BABY_PACIFIER_INCREMENTAL_PERCENTAGE_DESCRIPTION)
            };

            topSection = ItemDuplicator.UPGRADE_NAME;
            ItemDuplicatorUpgradeConfiguration = new TierPrimitiveUpgradeConfiguration<int>(cfg, topSection, LguConstants.ITEM_DUPLICATOR_ENABLED_DESCRIPTION, ItemDuplicator.PRICES_DEFAULT)
            {
                InitialEffect = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_DUPLICATOR_INITIAL_PERCENTAGE_KEY, LguConstants.ITEM_DUPLICATOR_INITIAL_PERCENTAGE_DEFAULT, LguConstants.ITEM_DUPLICATOR_INITIAL_PERCENTAGE_DESCRIPTION),
                IncrementalEffect = cfg.BindSyncedEntry(topSection, LguConstants.ITEM_DUPLICATOR_INCREMENTAL_PERCENTAGE_KEY, LguConstants.ITEM_DUPLICATOR_INCREMENTAL_PERCENTAGE_DEFAULT, LguConstants.ITEM_DUPLICATOR_INCREMENTAL_PERCENTAGE_DESCRIPTION)
            };

            #endregion

            #region Commands

            #region Interns

            topSection = Interns.NAME;
            INTERN_ENABLED = cfg.BindSyncedEntry(topSection, LguConstants.INTERNS_ENABLED_KEY, LguConstants.INTERNS_ENABLED_DEFAULT, LguConstants.INTERNS_ENABLED_DESCRIPTION);
            INTERN_PRICE = cfg.BindSyncedEntry(topSection, LguConstants.INTERNS_PRICE_KEY, LguConstants.INTERNS_PRICE_DEFAULT, LguConstants.INTERNS_PRICE_DESCRIPTION);
            INTERNS_TELEPORT_RESTRICTION = cfg.BindSyncedEntry(topSection, "Teleport Restriction when using Interns", Interns.TeleportRestriction.None, "Supported modes:\nNone: No restrictions applied.\nExitBuilding: Player must exit the facility to be able to be teleported.\nEnterShip: Player must enter the ship to be able to be teleported.");
            INTERNS_USAGES_PER_LANDING = cfg.BindSyncedEntry(topSection, "Revives per Landing", -1, "Amount of times you can use the command per landing. Once the amount is reached, you can no longer revive more teammates. Use \"-1\" for infinite");
            INTERNS_INTERVAL_BETWEEN_REVIVES = cfg.BindSyncedEntry(topSection, "Interval between Revives", 0f, "Time interval between each revival");
            INTERNS_DELAY_BEFORE_REVIVE = cfg.BindSyncedEntry(topSection, "Delay before Revive", 0f, "Time after purchasing it to perform the revive");

            #endregion

            #endregion

            InitialSyncCompleted += PluginConfig_InitialSyncCompleted;
            ConfigManager.Register(this);
        }

        #endregion

        private void PluginConfig_InitialSyncCompleted(object sender, EventArgs e)
        {
            if (LguStore.Instance == null) return;
            CheckMedkit();
            UpgradeBus.Instance.Reconstruct();
        }

        void CheckMedkit()
        {
            AnimationCurve curve = new(new Keyframe(0f, UpgradeBus.Instance.PluginConfiguration.ContractsConfiguration.ExtractionConfiguration.AmountMedkits.Value), new Keyframe(1f, UpgradeBus.Instance.PluginConfiguration.ContractsConfiguration.ExtractionConfiguration.AmountMedkits.Value));
            ItemManager.SetupMapObject(AssetBundleHandler.GetItemObject("MedkitMapItem"), curve);
        }
    }
}
