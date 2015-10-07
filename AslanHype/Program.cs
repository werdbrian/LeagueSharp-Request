using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;



namespace AslanHype
{

    class Program
    {
        public static Menu Config;

        public static string[] highChamps =
            {
                "Ahri", "Akali", "Amumu", "Annie", "Blitzcrank", "Darius", "Diana", "Ekko", "Elise", "Evelynn",
                "Fiddlesticks", "Fizz", "Gragas", "Hecarim", "JarvanIV", "Jayce", "Katarina", "KhaZix", "Leblanc", "LeeSin",
                "Lissandra", "Leona", "Malphite", "MasterYi", "Nautilus", "Nidalee", "Pantheon", "RekSai", "Rengar", "Sejuani",
                "Shaco", "Skarner", "Thresh", "TwistedFate", "VelKoz", "Veigar", "Vi", "MoneyKing", "XinZhao", "Zac",
                "Zed", "Zyra"
            };

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {

            Config = new Menu("Aslan Hype", "Aslan Hype", true);
            {
                Config.AddItem(new MenuItem("aslan.hype.track", "Aslan Hype! Status").SetValue(true));
                Config.AddItem(new MenuItem("vision.range", "Gang Search Range").SetValue(new Slider(3000, 1, 3000)));

                var enemiesMenu = new Menu("Enemy Settings", "Enemy Settings");
                {
                    foreach (var enemy in HeroManager.Enemies.Where(o => o.IsValid))
                    {
                        enemiesMenu.AddItem(new MenuItem("track." + enemy.ChampionName, "Track: " + enemy.ChampionName).SetValue(highChamps.Contains(enemy.ChampionName)));
                    }
                    Config.AddSubMenu(enemiesMenu);
                }
                var drawMenu = new Menu("Draw Settings", "Draw Settings");
                {
                    drawMenu.AddItem(new MenuItem("line.draw", "Draw Line").SetValue(true));
                    Config.AddSubMenu(drawMenu);
                }
            }
            
            Config.AddToMainMenu();
            
            
            Drawing.OnDraw += Drawing_OnDraw;

        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Config.Item("aslan.hype.track").GetValue<bool>() && Config.Item("line.draw").GetValue<bool>())
            {
                foreach (var hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsEnemy && !hero.IsDead && hero.IsVisible && hero.IsValidTarget(3000)))
                {
                    if (Config.Item("track." + hero.ChampionName).GetValue<bool>())
                    {
                        if (hero.Distance(ObjectManager.Player.Position) <= 3000 && hero.Distance(ObjectManager.Player.Position) > 2000)
                        {
                            Drawing.DrawLine(Drawing.WorldToScreen(ObjectManager.Player.Position), Drawing.WorldToScreen(hero.Position), 1, Color.Green);
                        }
                        if (hero.Distance(ObjectManager.Player.Position) < 2000 && hero.Distance(ObjectManager.Player.Position) >= 1000)
                        {
                            Drawing.DrawLine(Drawing.WorldToScreen(ObjectManager.Player.Position), Drawing.WorldToScreen(hero.Position), 2, Color.Yellow);
                        }
                        if (hero.Distance(ObjectManager.Player.Position) < 1000 && hero.Distance(ObjectManager.Player.Position) >= 1)
                        {
                            Drawing.DrawLine(Drawing.WorldToScreen(ObjectManager.Player.Position), Drawing.WorldToScreen(hero.Position), 3, Color.Red);
                        }
                    }
                }
            }
        }
    }
}