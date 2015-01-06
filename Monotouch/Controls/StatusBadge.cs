namespace MobileApp.iOS.UI.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Drawing;

    using MobileApp.Core.Models;
    using MobileApp.IOS.UI.Resources;

    using MonoTouch.Foundation;
    using MonoTouch.UIKit;

    class BadgeView : UILabel
    {
        const float height = 9;

        SizeF numberSize;

        public UIColor Color
        {
            set
            {
                Layer.BackgroundColor = value.CGColor;
            }
        }

        public BadgeView()
        {
            Text = string.Empty;
            BackgroundColor = UIColor.Clear;
            Alpha = 1;
            TextColor = ColorCollection.LightTextColor;
            Font = UIFont.BoldSystemFontOfSize(10f); ;
            UserInteractionEnabled = false;
            Layer.CornerRadius = height / 2;
            Layer.BackgroundColor = UIColor.White.CGColor;
            TextAlignment = UITextAlignment.Center;
        }

        ////void CalculateSize()
        ////{
        ////    numberSize = StringSize(" ", Font);
        ////    Frame = new RectangleF(Frame.Location, new SizeF(Math.Max(numberSize.Width, height), height));
        ////}
    }
}