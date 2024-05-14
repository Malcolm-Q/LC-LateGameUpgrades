using InteractiveTerminalAPI.UI;
using InteractiveTerminalAPI.UI.Application;
using InteractiveTerminalAPI.UI.Cursor;
using InteractiveTerminalAPI.UI.Screen;
using LethalLib.Modules;
using MoreShipUpgrades.Managers;
using MoreShipUpgrades.Misc.Util;
using System;
using System.Linq;

namespace MoreShipUpgrades.Misc.UI.Application
{
    internal class WeatherProbeApplication : PageApplication
    {
        public override void Initialization()
        {
            SelectableLevel[] levels = StartOfRound.Instance.levels.Where(x => x.randomWeathers.Length > 0).ToArray();
            (SelectableLevel[][], CursorMenu[], IScreen[]) entries = GetPageEntries(levels);

            SelectableLevel[][] pagesLevels = entries.Item1;
            CursorMenu[] cursorMenus = entries.Item2;
            IScreen[] screens = entries.Item3;

            for (int i = 0; i < pagesLevels.Length; i++)
            {
                SelectableLevel[] levelList = pagesLevels[i];
                CursorElement[] elements = new CursorElement[levelList.Length];
                cursorMenus[i] = CursorMenu.Create(startingCursorIndex: 0, elements: elements);
                CursorMenu cursorMenu = cursorMenus[i];
                ITextElement[] textElements =
                    [
                        TextElement.Create(text: LGUConstants.MAIN_WEATHER_PROBE_TOP_TEXT),
                        TextElement.Create(text: " "),
                        cursorMenu
                    ];
                screens[i] = BoxedScreen.Create(title: LGUConstants.MAIN_WEATHER_PROBE_SCREEN_TITLE, elements: textElements);

                for (int j = 0; j < levelList.Length; j++)
                {
                    SelectableLevel level = levelList[j];
                    if (level == null) continue;
                    elements[j] = CursorElement.Create(level.PlanetName, action: () => SelectedPlanet(level, PreviousScreen()));
                }
            }
            currentPage = initialPage;
            currentCursorMenu = initialPage.GetCurrentCursorMenu();
            currentScreen = initialPage.GetCurrentScreen();
        }
        void SelectedPlanet(SelectableLevel level, Action cancelAction)
        {
            RandomWeatherWithVariables[] possibleWeathers = level.randomWeathers.Where(x => x.weatherType != level.currentWeather).ToArray();
            CursorElement[] elements = new CursorElement[possibleWeathers.Length+3];

            CursorMenu cursorMenu = new CursorMenu()
            {
                elements = elements,
            };
            IScreen screen = new BoxedScreen()
            {
                Title = level.PlanetName,
                elements =
                [
                    new TextElement()
                        {
                            Text = string.Format(LGUConstants.SELECT_WEATHER_FORMAT, level.PlanetName)
                        },
                        new TextElement()
                        {
                            Text = " "
                        },
                        new TextElement()
                        {
                            Text = string.Format(LGUConstants.CURRENT_WEATHER_FORMAT, level.currentWeather == LevelWeatherType.None ? "Clear" : level.currentWeather)
                        },
                        new TextElement()
                        {
                            Text = " "
                        },
                        cursorMenu
                ]
            };
            for (int i = 0; i < possibleWeathers.Length; i++)
            {
                RandomWeatherWithVariables weather = possibleWeathers[i];
                elements[i] = new CursorElement()
                {
                    Name = weather.weatherType.ToString(),
                    Action = () =>
                    {
                        BeforeChangeWeather(level, weather.weatherType);
                    },
                    Active = (x) => CanSelectWeather(level, weather.weatherType, UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PICKED_WEATHER_PRICE),
                };

            }
            elements[possibleWeathers.Length] = new CursorElement()
            {
                Name = "Clear",
                Action = () =>
                {
                    BeforeChangeWeather(level, LevelWeatherType.None);
                },
                Active = (x) => CanSelectWeather(level, LevelWeatherType.None, UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_ALWAYS_CLEAR ? UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PRICE : UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PICKED_WEATHER_PRICE),
            };
            if (!UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_ALWAYS_CLEAR)
            {
                elements[possibleWeathers.Length + 1] = new CursorElement()
                {
                    Name = "Random",
                    Action = () =>
                    {
                        BeforeRandomizeWeather(level);
                    },
                    Active = (x) => CanSelectRandomWeather(possibleWeathers, UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PRICE),
                };

                elements[possibleWeathers.Length + 2] = new CursorElement()
                {
                    Name = LGUConstants.CANCEL_PROMPT,
                    Action = cancelAction
                };
            }
            else
            {
                elements[possibleWeathers.Length + 1] = new CursorElement()
                {
                    Name = LGUConstants.CANCEL_PROMPT,
                    Action = cancelAction
                };
            }
            SwitchScreen(screen, cursorMenu, true);
        }
        static bool CanSelectRandomWeather(RandomWeatherWithVariables[] weathers, int price)
        {
            int groupCredits = UpgradeBus.Instance.GetTerminal().groupCredits;
            if (price > groupCredits) return false;

            return weathers.Length >= 1;
        }
        static bool CanSelectWeather(SelectableLevel level, LevelWeatherType levelWeatherType, int price)
        {
            int groupCredits = UpgradeBus.Instance.GetTerminal().groupCredits;
            if (price > groupCredits) return false;

            bool sameWeather = level.currentWeather == levelWeatherType || (level.overrideWeather && level.overrideWeatherType == levelWeatherType);
            return !sameWeather;
        }
        void BeforeChangeWeather(SelectableLevel level, LevelWeatherType type)
        {
            int groupCredits = UpgradeBus.Instance.GetTerminal().groupCredits;
            if (groupCredits < UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PICKED_WEATHER_PRICE)
            {
                ErrorMessage(level.PlanetName, PreviousScreen(), LGUConstants.NOT_ENOUGH_CREDITS_SPECIFIED_PROBE);
                return;
            }

            bool sameWeather = level.currentWeather == type || (level.overrideWeather && level.overrideWeatherType == type);
            if (sameWeather)
            {
                ErrorMessage(level.PlanetName, PreviousScreen(), string.Format(LGUConstants.SAME_WEATHER_FORMAT, level.PlanetName, type == LevelWeatherType.None ? "clear" : type));
                return;
            }
            int price = type == LevelWeatherType.None && UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_ALWAYS_CLEAR ? UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PRICE : UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PICKED_WEATHER_PRICE.Value;

            Confirm(level.PlanetName, string.Format(LGUConstants.CONFIRM_WEATHER_FORMAT, level.PlanetName, type, price), () => ChangeWeather(level, type), PreviousScreen());
        }
        void ChangeWeather(SelectableLevel level, LevelWeatherType weatherType)
        {
            int price = weatherType == LevelWeatherType.None && UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_ALWAYS_CLEAR ? UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PRICE : UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PICKED_WEATHER_PRICE.Value;
            terminal.groupCredits -= price;
            LguStore.Instance.SyncCreditsServerRpc(terminal.groupCredits);
            LguStore.Instance.SyncWeatherServerRpc(level.PlanetName, weatherType);
            CursorElement exit = new CursorElement()
            {
                Name = "Exit",
                Action = PreviousScreen()
            };
            CursorMenu cursorMenu = new CursorMenu()
            {
                elements = [exit]
            };
            IScreen screen = new BoxedScreen()
            {
                Title = level.PlanetName,
                elements = [
                        new TextElement()
                        {
                            Text = string.Format(LGUConstants.WEATHER_CHANGED_FORMAT, level.PlanetName, weatherType == LevelWeatherType.None ? "clear" : weatherType),
                        },
                        new TextElement()
                        {
                            Text = " "
                        },
                        cursorMenu
                    ]
            };
            SwitchScreen(screen, cursorMenu, false);
        }
        void BeforeRandomizeWeather(SelectableLevel level)
        {
            int groupCredits = UpgradeBus.Instance.GetTerminal().groupCredits;
            if (groupCredits < UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PRICE)
            {
                ErrorMessage(level.PlanetName, PreviousScreen(), LGUConstants.NOT_ENOUGH_CREDITS_PROBE);
                return;
            }
            Confirm(level.PlanetName, string.Format(LGUConstants.CONFIRM_RANDOM_WEATHER_FORMAT, level.PlanetName, UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PRICE.Value), () => RandomizeWeather(level), PreviousScreen());
        }
        void RandomizeWeather(SelectableLevel level)
        {
            (string, LevelWeatherType weather) weather = WeatherManager.RandomizeWeather(ref level);
            terminal.groupCredits -= UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PRICE.Value;
            LguStore.Instance.SyncCreditsServerRpc(terminal.groupCredits);
            LguStore.Instance.SyncWeatherServerRpc(level.PlanetName, weather.Item2);
            CursorElement exit = new CursorElement()
            {
                Name = "Exit",
                Action = PreviousScreen()
            };
            CursorMenu cursorMenu = new CursorMenu()
            {
                elements = [exit]
            };
            IScreen screen = new BoxedScreen()
            {
                Title = level.PlanetName,
                elements = [
                        new TextElement()
                        {
                            Text = string.Format(LGUConstants.WEATHER_CHANGED_FORMAT, level.PlanetName, weather.Item2),
                        },
                        new TextElement()
                        {
                            Text = " "
                        },
                        cursorMenu
                    ]
            };
            SwitchScreen(screen, cursorMenu, false);
        }
    }
}
