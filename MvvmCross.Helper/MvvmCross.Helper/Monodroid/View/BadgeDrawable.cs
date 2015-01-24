namespace MobileApp.Android.Views
{
    using MobileApp.Core.Models;

    using global::Android.Animation;
    using global::Android.Graphics;
    using global::Android.Graphics.Drawables;

    public class BadgeDrawable : Drawable
    {
        private readonly Drawable child;

        private readonly Paint badgePaint;

        private readonly Paint textPaint;

        private readonly RectF badgeBounds = new RectF();

        private readonly Rect txtBounds = new Rect();

        private int count = 0;

        private int alpha = 0x8F;

        private ValueAnimator alphaAnimator;

        private ContactStatus status = ContactStatus.Online;

        private Color color = Color.ParseColor("#5d778d");

        public BadgeDrawable(Drawable child)
        {
            this.child = child;
            badgePaint = new Paint { AntiAlias = true, Color = Color.Blue, };
            textPaint = new Paint { AntiAlias = true, Color = Color.White, TextSize = 16, TextAlign = Paint.Align.Center };
        }

        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
                InvalidateSelf();
            }
        }

        public void SetCountAnimated(int count)
        {
            if (alphaAnimator != null)
            {
                alphaAnimator.Cancel();
                alphaAnimator = null;
            }

            const int Duration = 300;

            alphaAnimator = ObjectAnimator.OfInt(this, "alpha", 0xFF, 0);
            alphaAnimator.SetDuration(Duration);
            alphaAnimator.RepeatMode = ValueAnimatorRepeatMode.Reverse;
            alphaAnimator.RepeatCount = 1;
            alphaAnimator.AnimationRepeat += (sender, e) =>
                {
                    ((Animator)sender).RemoveAllListeners();
                    this.count = count;
                };
            alphaAnimator.Start();
        }

        public ContactStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                if (alphaAnimator != null)
                {
                    alphaAnimator.Cancel();
                    alphaAnimator = null;
                }

                switch (value)
                {
                    case ContactStatus.EarlyInbound:
                    case ContactStatus.EarlyOutbound:
                        this.color = Color.ParseColor("#edc91f");
                        break;
                    case ContactStatus.Confirmed:
                        this.color = Color.ParseColor("#FF8800");
                        break;
                    case ContactStatus.Dnd:
                        this.color = Color.ParseColor("#ff5349");
                        break;
                    case ContactStatus.Online:
                        this.color = Color.ParseColor("#28b232");
                        break;
                    case ContactStatus.Offline:
                        this.color = Color.ParseColor("#5d778d");
                        break;
                }

                const int Duration = 400;

                alphaAnimator = ObjectAnimator.OfInt(this, "alpha", 0xFF, 0);
                alphaAnimator.SetDuration(Duration);
                alphaAnimator.RepeatMode = ValueAnimatorRepeatMode.Reverse;
                alphaAnimator.RepeatCount = 1;
                alphaAnimator.AnimationRepeat += (sender, e) =>
                    {
                        ((Animator)sender).RemoveAllListeners();
                        this.badgePaint.Color = this.color;
                    };
                alphaAnimator.Start();
            }
        }

        public override void Draw(Canvas canvas)
        {
            child.Draw(canvas);

            badgePaint.Alpha = textPaint.Alpha = alpha;
            badgePaint.Color = this.color;
            var width = Bounds.Width();
            var partH = width / 8;
            var height = Bounds.Height();
            var partV = height / 8;
            badgeBounds.Set(width / 2 + partH, height / 2 + partV, width - partH, height - partV);

            canvas.DrawRoundRect(badgeBounds, 8, 8, badgePaint);
            textPaint.TextSize = (8 * badgeBounds.Height()) / 10;
        }

        protected override void OnBoundsChange(Rect bounds)
        {
            base.OnBoundsChange(bounds);
            child.SetBounds(bounds.Left, bounds.Top, bounds.Right, bounds.Bottom);
        }

        public override int IntrinsicWidth
        {
            get
            {
                return child.IntrinsicWidth;
            }
        }

        public override int IntrinsicHeight
        {
            get
            {
                return child.IntrinsicHeight;
            }
        }

        public override void SetAlpha(int alpha)
        {
            this.alpha = alpha;
            InvalidateSelf();
        }

        public override void SetColorFilter(ColorFilter cf)
        {
            child.SetColorFilter(cf);
        }

        public override int Opacity
        {
            get
            {
                return child.Opacity;
            }
        }
    }
}