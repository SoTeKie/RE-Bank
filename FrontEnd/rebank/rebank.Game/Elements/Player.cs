using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace rebank.Game.Elements
{
    public class Player : CompositeDrawable
    {
        private float velocity = 0;
        private Vector2 moveTarget = new Vector2(0);
        private bool isIdle = true;

        private readonly Map map;

        public Player(Map map)
        {
            this.map = map;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            RelativePositionAxes = Axes.Both;
            Size = new Vector2(0.03125f);
            InternalChild = new Box
            {
                Colour = Color4.Purple,
                RelativeSizeAxes = Axes.Both,
            };
        }

        protected override void Update()
        {
            if (isIdle)
            {
                base.Update();
                return;
            }

            if (velocity < 1)
                velocity *= 2f;
            if (velocity > 1)
                velocity = 1;

            var dir = moveTarget - Position;
            dir.Normalize();

            dir *= 0.001f * velocity;

            if (dir.LengthSquared > (Position - moveTarget).LengthSquared)
            {
                TryMoveTo(moveTarget);
                isIdle = true;
                velocity = 0;
            }
            else
            {
                TryMoveTo(Position + dir);
            }

            base.Update();
        }

        private void TryMoveTo(Vector2 target)
        {
            var us = new Quad(target.X, target.Y, Width, Height);
            foreach (var q in map.CollisionBoxes)
            {
                if(us.AABBFloat.IntersectsWith(q.AABBFloat))
                {
                    var diff = us.AABBFloat;
                    diff.Intersect(q.AABBFloat);

                    if (diff.Width < diff.Height)
                    {
                        var calcX = (target.X - Position.X) - diff.Width;
                        var div = Math.Abs(calcX / (target.X - Position.X)) - float.Epsilon;
                        var nextY = Position.Y + ((target.Y - Position.Y) * div);
                        var nextX = Position.X + ((target.X - Position.X) * div);
                        isIdle = true;
                        velocity = 0;
                        TryMoveTo(new Vector2(nextX, nextY));
                    }
                    else if (diff.Height > diff.Width)
                    {
                        var calcY = (target.Y - Position.Y) - diff.Width;
                        var div = Math.Abs(calcY / (target.Y - Position.Y)) - float.Epsilon;
                        var nextX = Position.X + ((target.X - Position.X) * div);
                        var nextY = Position.Y + ((target.Y - Position.Y) * div);
                        isIdle = true;
                        velocity = 0;
                        TryMoveTo(new Vector2(nextX, nextY));
                    }
                    return;
                }
            }

            if (target.X == float.NaN || target.Y == float.NaN)
                return;
            Position = target;
            map.OnPlayerMove();
        }

        public void MoveTowards(Vector2 target)
        {
            isIdle = false;
            moveTarget = target;
            if (velocity < 0.1f)
                velocity = 0.1f;
        }
    }
}
