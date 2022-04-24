using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK.Graphics;

namespace rebank.Game.Elements
{
    public class AccountBanner : CompositeDrawable
    {
        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            GlobalState.Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", GlobalState.AccessToken);
            var response = GlobalState.Client.GetAsync("api/v1/bank-accounts/").GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();

            var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var accounts = JArray.Parse(json);
            string mainIBAN = "";
            double mainBal = 0;
            string savingsIBAN = "";
            double savingsBal = 0;
            foreach (var account in accounts)
            {
                if ((string)account["type"] == "default")
                {
                    mainIBAN = (string)account["iban"];
                    mainBal = double.Parse((string)account["balance"]);
                }
                else if ((string)account["type"] == "savings")
                {
                    savingsIBAN = (string)account["iban"];
                    savingsBal = double.Parse((string)account["balance"]);
                }
            }

            InternalChild = new Container
            {
                Children = new[] {
                    new Container
                    {
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                Size = new osuTK.Vector2(640, 170),
                                Colour = new Color4(43, 43, 44, 255)
                            },
                            new SpriteText
                            {
                                Text = "Main Account",
                                Position = new osuTK.Vector2(20)
                            },
                            new SpriteText
                            {
                                Text = mainIBAN + " = " + mainBal + " EUR",
                                Position = new osuTK.Vector2(20, 45),
                                Colour = new Color4(75, 75, 77, 255)
                            },
                            new Box
                            {
                                RelativeSizeAxes = Axes.X,
                                RelativePositionAxes = Axes.Both,
                                Position = new osuTK.Vector2(0, 0.5f),
                                Size = new osuTK.Vector2(1),
                                Colour = Color4.Black
                            },
                            new SpriteText
                            {
                                Text = "Savings",
                                Position = new osuTK.Vector2(20, 100)
                            },
                            new SpriteText
                            {
                                Text = savingsIBAN + " = " + savingsBal + " EUR",
                                Position = new osuTK.Vector2(20, 125),
                                Colour = new Color4(75, 75, 77, 255)
                            }
                        },
                        RelativeSizeAxes = Axes.X,
                        Size = new osuTK.Vector2(1),
                        AutoSizeAxes = Axes.Y,
                        Masking = true,
                        CornerRadius = 15,
                    }
                },
                Padding = new MarginPadding(10),
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Width = 1
            };
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Width = 1;
        }
    }
}
