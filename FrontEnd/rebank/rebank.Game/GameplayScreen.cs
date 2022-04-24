using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using rebank.Game.Elements;

namespace rebank.Game
{
    public class GameplayScreen : Screen
    {
        BindableNumberText currencyDisplay;

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    Colour = Color4.DarkCyan,
                    RelativeSizeAxes = Axes.Both,

                },
                new Map
                {
                    RelativeSizeAxes = Axes.Both,
                    FillAspectRatio = 1,
                    FillMode = FillMode.Fit,
                    Size = new Vector2(2.5f)
                },
                new Container
                {
                    Depth = -1,
                    Children = new Drawable[]
                    {
                        new RoundedBox
                        {
                            Position = new Vector2(10),
                            Size = new Vector2(120, 40),
                            Colour = new Color4(43, 43, 44, 255)
                        },
                        new SpriteText
                        {
                            Text = "Coins:",
                            Position = new Vector2(20)
                        },
                        currencyDisplay = new BindableNumberText
                        {
                            Y = 20,
                            X = 70,
                            Colour = Color4.LimeGreen
                        },
                    }
                },
                new Container
                {
                    Depth = -1,
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Children = new Drawable[]
                    {
                        new HoverableRoundedBox
                        {
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight,
                            Y = 10,
                            X = -10,
                            Size = new Vector2(40, 40),
                            NormalColor = new Color4(43, 43, 44, 255),
                            HoverColor = new Color4(86, 86, 88, 255)
                        },
                        new SpriteText
                        {
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight,
                            Y = 20,
                            X = -25,
                            Text = "X",
                            Colour = Color4.Red
                        }
                    }
                }
            };
            currencyDisplay.Number.BindTo(GlobalState.PlayerMoney);
        }
    }
}
