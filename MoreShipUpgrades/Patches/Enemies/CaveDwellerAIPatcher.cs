using CustomItemBehaviourLibrary.Misc;
using HarmonyLib;
using MoreShipUpgrades.UpgradeComponents.TierUpgrades.Enemies;
using System.Collections.Generic;
using System.Reflection;

namespace MoreShipUpgrades.Patches.Enemies
{
	[HarmonyPatch(typeof(CaveDwellerAI))]
	internal static class CaveDwellerAIPatcher
	{
		[HarmonyPatch(nameof(CaveDwellerAI.IncreaseBabyGrowthMeter))]
		[HarmonyTranspiler]
		static IEnumerable<CodeInstruction> IncreaseBabyGrowthMeterTranspiler(IEnumerable<CodeInstruction> instructions)
		{
			List<CodeInstruction> codes = new(instructions);
			int index = 0;
			FieldInfo growthMultiplier = typeof(CaveDwellerAI).GetField(nameof(CaveDwellerAI.growthSpeedMultiplier));
			MethodInfo GetDecreasedGrowthRate = typeof(BabyPacifier).GetMethod(nameof(BabyPacifier.GetDecreasedGrowthRate));

			Tools.FindField(ref index, ref codes, growthMultiplier, GetDecreasedGrowthRate, errorMessage: "Couldn't find growth multiplier used to increase the age of maneater");
			Tools.FindField(ref index, ref codes, growthMultiplier, GetDecreasedGrowthRate, errorMessage: "Couldn't find growth multiplier used to increase the age of maneater");

			return codes;
		}

	}
}
