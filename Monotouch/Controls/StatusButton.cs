namespace MobileApp.iOS.UI.Views
{
    using System;
    using System.Drawing;

    using MobileApp.Core.Models;
    using MobileApp.IOS.UI.Resources;

    using MonoTouch.CoreAnimation;
    using MonoTouch.Foundation;
    using MonoTouch.UIKit;

    internal class StatusButton : UIControl
    {
        #region Constants

        private const float Padding = 10;

        #endregion

        #region Static Fields

        private static readonly Lazy<UIImage> IconImage = new Lazy<UIImage>(() => UIImage.FromBundle("status"));

        #endregion

        #region Fields

        private readonly BadgeView badge;

        private readonly UIImageView imageView;

        private ContactStatus status;

        #endregion

        #region Constructors and Destructors

        public StatusButton()
        {
            this.imageView = new UIImageView(IconImage.Value.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate)) { TintColor = ColorCollection.TintColor };

            this.AddSubview(this.imageView);

            this.badge = new BadgeView { Frame = new RectangleF(41, 26.5f, 8, 8) };
            this.AddSubview(this.badge);
        }

        #endregion

        #region Public Properties

        public ContactStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                this.status = value;

                switch (value)
                {
                    case ContactStatus.Online:
                        this.badge.Color = ColorCollection.OnlineColor;
                        break;
                    case ContactStatus.EarlyInbound:
                    case ContactStatus.EarlyOutbound:
                        this.badge.Color = ColorCollection.EarlyColor;
                        break;
                    case ContactStatus.Dnd:
                        this.badge.Color = ColorCollection.OfflineColor;
                        break;
                    case ContactStatus.Confirmed:
                        this.badge.Color = ColorCollection.EarlyColor;
                        break;
                    case ContactStatus.Offline:
                        this.badge.Color = ColorCollection.TintColor;
                        break;
                }

                CAKeyFrameAnimation pathAnimation = CAKeyFrameAnimation.GetFromKeyPath("transform");
                pathAnimation.CalculationMode = CAAnimation.AnimationPaced;
                pathAnimation.FillMode = CAFillMode.Forwards;
                pathAnimation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseOut);
                //			pathAnimation.RemovedOnCompletion = false;
                pathAnimation.Duration = .2;

                CATransform3D transform = CATransform3D.MakeScale(2f, 2f, 1);
                pathAnimation.Values = new[]
                                           {
                                               NSValue.FromCATransform3D(CATransform3D.Identity), NSValue.FromCATransform3D(transform),
                                               NSValue.FromCATransform3D(CATransform3D.Identity)
                                           };
                this.badge.Layer.AddAnimation(pathAnimation, "pulse");
            }
        }

        #endregion

        #region Public Methods and Operators

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            RectangleF bounds = this.Bounds;
            bounds.X += Padding + 15;
            bounds.Y += Padding;
            bounds.Width -= Padding * 2;
            bounds.Height -= Padding * 2;
            this.imageView.Frame = bounds;
        }

        #endregion
    }
}