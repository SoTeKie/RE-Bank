using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Events;
using osuTK.Graphics;

namespace rebank.Game.Elements
{
    public class GameBanner : CompositeDrawable
    {
        BindableNumberText currencyDisplay;

        public Action ClickAction;

        protected override bool OnClick(ClickEvent e)
        {
            ClickAction?.Invoke();

            return true;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            InternalChild = new Container
            {
                Children = new[] {
                    new Container
                    {
                        Children = new Drawable[]
                        {
                            new Sprite
                            {
                                Texture = textures.Get("game_banner_bg"),
                                TextureRelativeSizeAxes = Axes.Both,
                                TextureRectangle = new osu.Framework.Graphics.Primitives.RectangleF(0, 0, 1, 1),
                                Size = new osuTK.Vector2(640, 300),
                                Colour = Color4.White
                            },
                            new Box
                            {
                                RelativeSizeAxes = Axes.Y,
                                RelativePositionAxes = Axes.Both,
                                Position = new osuTK.Vector2(0.5f, 0),
                                Size = new osuTK.Vector2(1),
                                Colour = new Color4(43, 43, 44, 255)
                            },
                            new Box
                            {
                                RelativeSizeAxes = Axes.X,
                                RelativePositionAxes = Axes.Both,
                                Position = new osuTK.Vector2(0, 0.125f),
                                Size = new osuTK.Vector2(0.5f, 1),
                                Colour = new Color4(43, 43, 44, 255)
                            },
                            new SpriteText
                            {
                                Text = "Coins:",
                                Position = new osuTK.Vector2(10)
                            },
                            currencyDisplay = new BindableNumberText
                            {
                                Y = 10,
                                X = 60,
                                Colour = Color4.Orange
                            },
                            new SpriteText
                            {
                                Text = "--- Quests ---",
                                RelativePositionAxes = Axes.X,
                                Position = new osuTK.Vector2(0.25f, 50),
                                Origin = Anchor.TopCentre
                            },
                            new RoundedBox
                            {
                                Y = 90,
                                X = 15,
                                Size = new osuTK.Vector2(280, 40),
                                Colour = new Color4(43, 43, 44, 128)
                            },
                            new SpriteText
                            {
                                Text = "Log into the game today",
                                Position = new osuTK.Vector2(25, 100),
                            },
                            new SpriteText
                            {
                                Text = "√",
                                Position = new osuTK.Vector2(265, 100),
                                Colour = Color4.LimeGreen
                            },
                            new RoundedBox
                            {
                                Y = 155,
                                X = 15,
                                Size = new osuTK.Vector2(280, 40),
                                Colour = new Color4(43, 43, 44, 128)
                            },
                            new SpriteText
                            {
                                Text = "Find 3 stars",
                                Position = new osuTK.Vector2(25, 165),
                            },
                            new SpriteText
                            {
                                Text = "X",
                                Position = new osuTK.Vector2(265, 165),
                                Colour = Color4.Red
                            },
                            new RoundedBox
                            {
                                Y = 220,
                                X = 15,
                                Size = new osuTK.Vector2(280, 40),
                                Colour = new Color4(43, 43, 44, 128)
                            },
                            new SpriteText
                            {
                                Text = "Mow your lawn",
                                Position = new osuTK.Vector2(25, 230),
                            },
                            new SpriteText
                            {
                                Text = "X",
                                Position = new osuTK.Vector2(265, 230),
                                Colour = Color4.Red
                            },
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

            currencyDisplay.Number.BindTo(GlobalState.PlayerMoney);
        }

    }
}
