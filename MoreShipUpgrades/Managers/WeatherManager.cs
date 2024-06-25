using System.Linq;

namespace MoreShipUpgrades.Managers
{
    internal static class WeatherManager
    {
        internal const string WEATHER_PROBE_COMMAND = "Weather Probe";

        internal static (string, LevelWeatherType) RandomizeWeather(ref SelectableLevel level)
        {
            if (UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_ALWAYS_CLEAR.Value) return (level.PlanetName, LevelWeatherType.None);

            LevelWeatherType selectedWeather = level.overrideWeather ? level.overrideWeatherType : level.currentWeather;
            LevelWeatherType[] allowedWeathers = level.randomWeathers.Select(x => x.weatherType).Where(x => x != selectedWeather).ToArray();
            int selectedWeatherValue = UnityEngine.Random.Range(0, allowedWeathers.Length + 1);
            if (selectedWeatherValue == allowedWeathers.Length)
            {
                if (selectedWeather == LevelWeatherType.None)
                {
                    LevelWeatherType newSelectedWeather = allowedWeathers[UnityEngine.Random.Range(0, allowedWeathers.Length)];
                    return (level.PlanetName, newSelectedWeather);
                }
                else
                {
                    return (level.PlanetName, LevelWeatherType.None);
                }
            }
            else
            {
                LevelWeatherType newSelectedWeather = allowedWeathers[selectedWeatherValue];
                return (level.PlanetName, newSelectedWeather);
            }
        }
    }
}
