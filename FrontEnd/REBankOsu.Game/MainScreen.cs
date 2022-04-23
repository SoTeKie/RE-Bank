using System;
using System.Net.Http;
using System.Net.Http.Headers;
using NuGet.Common;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osuTK.Graphics;

namespace REBankOsu.Game
{
    public class MainScreen : Screen
    {
        Checkbox showPasswordCheckbox;
        PasswordTextBox passwordBox;
        BasicTextBox usernameBox;

        public object Background { get; private set; }

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
                /*new SpriteText
                {
                    Y = 20,
                    Text = "Main Screen",
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Font = FontUsage.Default.With(size: 40)
                },
                new SpinningBox
                {
                    Anchor = Anchor.Centre,
                }*/
                        
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

                                HttpResponseMessage response = Program.Client.PostAsJsonAsync($"auth/login", user).GetAwaiter().GetResult();
                                response.EnsureSuccessStatusCode();

                                //Deserialize
                                Program.AccessToken = response.Content.ReadAsAsync<Token>().GetAwaiter().GetResult();
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

        public class User
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class Token
        {
            public string AccessToken { get; set; }
        }

        class Program
        {
            public static HttpClient Client = new HttpClient();
            public static Token AccessToken;

            static Program()
            {
                Client.BaseAddress = new Uri("http://localhost:8000/");
                Client.DefaultRequestHeaders.Accept.Clear();
                Client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }
    }
}
