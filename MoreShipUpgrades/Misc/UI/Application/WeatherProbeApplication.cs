using InteractiveTerminalAPI.UI;
using InteractiveTerminalAPI.UI.Application;
using InteractiveTerminalAPI.UI.Cursor;
using InteractiveTerminalAPI.UI.Screen;
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
                        Confirm(level.PlanetName, string.Format(LGUConstants.CONFIRM_WEATHER_FORMAT, level.PlanetName, weather.weatherType.ToString(), UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PICKED_WEATHER_PRICE.Value), () => BeforeChangeWeather(level, weather.weatherType),() => SwitchScreen(screen, cursorMenu, true));
                    }
                };

            }
            if (level.currentWeather != LevelWeatherType.None)
            {
                elements[possibleWeathers.Length] = new CursorElement()
                {
                    Name = "Clear",
                    Action = () =>
                    {
                        Confirm(level.PlanetName, string.Format(LGUConstants.CONFIRM_CLEAR_WEATHER_FORMAT, level.PlanetName, UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_ALWAYS_CLEAR ? UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PRICE.Value : UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PICKED_WEATHER_PRICE.Value), () => BeforeChangeWeather(level, LevelWeatherType.None), () => SwitchScreen(screen, cursorMenu, true));
                    }
                };
            }
            if (!UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_ALWAYS_CLEAR)
            {
                elements[possibleWeathers.Length + 1] = new CursorElement()
                {
                    Name = "Random",
                    Action = () =>
                    {
                        Confirm(level.PlanetName, string.Format(LGUConstants.CONFIRM_RANDOM_WEATHER_FORMAT, level.PlanetName, UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PRICE.Value), () => BeforeRandomizeWeather(level), () => SwitchScreen(screen, cursorMenu, true));
                    }
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
        void BeforeChangeWeather(SelectableLevel level, LevelWeatherType type)
        {
            int groupCredits = UpgradeBus.Instance.GetTerminal().groupCredits;
            if (groupCredits < UpgradeBus.Instance.PluginConfiguration.WEATHER_PROBE_PICKED_WEATHER_PRICE)
            {
                ErrorMessage(level.PlanetName, PreviousScreen(), LGUConstants.NOT_ENOUGH_CREDITS_SPECIFIED_PROBE);
                return;
            }
            ChangeWeather(level, type);
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
            RandomizeWeather(level);
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
