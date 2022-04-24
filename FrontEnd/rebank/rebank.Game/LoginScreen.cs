using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osuTK.Graphics;
using rebank.Game.Elements;
using rebank.Game.Models;

namespace rebank.Game
{
    public class LoginScreen : Screen
    {
        Checkbox showPasswordCheckbox;
        PasswordTextBox passwordBox;
        BasicTextBox usernameBox;

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Colour = Color4.Black,
                    RelativeSizeAxes = Axes.Both,
                },
                new Container
                {
                    Children = new Drawable[]
                    {
                        new SpriteText
                        {
                            Text = "Login Page",
                            Font = new FontUsage("Roboto", 35),
                            Y = -60,
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre
                        },

                        new SpriteText
                        {
                            Text = "Username:",
                            Font = new FontUsage("Roboto", 18),
                            Size = new osuTK.Vector2(200, 30),
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre
                        },

                        usernameBox = new BasicTextBox
                        {
                            Y = 20,
                            Size = new osuTK.Vector2(200,30),
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre
                        },

                        new SpriteText
                        {
                            Y = 60,
                            Text = "Password:",
                            Font = new FontUsage("Roboto", 18),
                            Size = new osuTK.Vector2(200, 30),
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre
                        },

                        passwordBox = new PasswordTextBox
                        {
                            Y = 80,
                            Size = new osuTK.Vector2(200, 30),
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre
                        },

                        new FillFlowContainer {
                            Y = 120,
                            Direction = FillDirection.Horizontal,
                            Children = new Drawable[] {
                                showPasswordCheckbox = new BasicCheckbox
                                {
                                    CheckedColor = FrameworkColour.Yellow,

                                },
                                new SpriteText
                                {
                                    Text = "Show password",
                                    Font = new FontUsage("Roboto", 18),
                                    Margin = new MarginPadding(5)
                                }
                            },

                            Size = new osuTK.Vector2(200, 30),
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre
                        },

                        new WhiteTextButton
                        {
                            Y = 180,
                            Text = "Login",


                            BackgroundColour = new Color4(44, 43, 43, 255),
                            HoverColour = new Color4(88, 84, 84, 255),
                            FlashColour = new Color4(133, 125, 125, 255),

                            Size = new osuTK.Vector2(300, 40),
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,

                            Action = () => {
                                User user = new User
                                {
                                    Username = usernameBox.Text,
                                    Password = passwordBox.Text,
                                };

                                var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                                var content = new StringContent(JsonConvert.SerializeObject(user, settings), Encoding.UTF8, "application/json");

                                HttpResponseMessage response = GlobalState.Client.PostAsync($"api/v1/login/", content).GetAwaiter().GetResult();
                                response.EnsureSuccessStatusCode();

                                GlobalState.AccessToken = JsonConvert.DeserializeObject<AccessToken>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult(), settings).Token;

                                this.Push(new HomeScreen());
                            }

                        }
                    },
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    AutoSizeAxes = Axes.Both
                }
            };

            showPasswordCheckbox.Current.BindTo(passwordBox.ShowPassword);
        }
    }
}
