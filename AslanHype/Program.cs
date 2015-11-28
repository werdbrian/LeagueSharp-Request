using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

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

                
                var currentAngel1 = 90 * (float) Math.PI / 180;
                var currentAngel2 = (360-90) * (float) Math.PI / 180;
                foreach (var hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsEnemy && !hero.IsDead && hero.IsVisible && hero.IsValidTarget(8000)))
                {
                    if (Config.Item("track." + hero.ChampionName).GetValue<bool>())
                    {
                        if (hero.Distance(ObjectManager.Player.Position) <= 8000 && hero.Distance(ObjectManager.Player.Position) > 1000 )
                        {
                            Drawing.DrawLine(Drawing.WorldToScreen(ObjectManager.Player.Position), Drawing.WorldToScreen(hero.Position), 2, Color.Yellow);
                            
                            var currentScreenChamp = Drawing.WorldToScreen(ObjectManager.Player.Position);
                            var currentScreenEnemy = Drawing.WorldToScreen(hero.Position);
                            var _line = new Vector2(currentScreenChamp.X-currentScreenEnemy.X,currentScreenChamp.Y-currentScreenEnemy.Y);
                            var _line1 = new Vector2(currentScreenEnemy.X-currentScreenChamp.X,currentScreenEnemy.Y-currentScreenChamp.Y-currentScreenChamp.Y);

                           // var direction = ObjectManager.Player.Direction.To2D().Perpendicular();
                            var direction = _line.Direction();
                            var playerPosition = ObjectManager.Player.Position.To2D();
                            var conePoint1 = playerPosition + 800 * direction.Rotated(currentAngel1);
                            var conePoint2 = playerPosition + 800 * direction.Rotated(currentAngel2);
                            var conePoint13D = Drawing.WorldToScreen(conePoint1.To3D());
                            var conePoint23D = Drawing.WorldToScreen(conePoint2.To3D());
                            Drawing.DrawLine(currentScreenEnemy.X, currentScreenEnemy.Y, conePoint13D.X, conePoint13D.Y, 2, Color.Yellow);
                            Drawing.DrawLine(currentScreenEnemy.X, currentScreenEnemy.Y, conePoint23D.X,  conePoint23D.Y,2, Color.Yellow);
                            Render.Circle.DrawCircle(ObjectManager.Player.Position,800, Color.Green, 2);
                        }
                        if (hero.Distance(ObjectManager.Player.Position) < 2000 && hero.Distance(ObjectManager.Player.Position) >= 1000)
                        {
                            Drawing.DrawLine(Drawing.WorldToScreen(ObjectManager.Player.Position), Drawing.WorldToScreen(hero.Position), 2, Color.Green);
            
                            
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
