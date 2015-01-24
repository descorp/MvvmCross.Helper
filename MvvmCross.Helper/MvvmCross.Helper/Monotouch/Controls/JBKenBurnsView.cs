//  Created by Javier Berlana on 9/23/11.
//  Copyright (c) 2011, Javier Berlana
//  Ported to C# by James Clancey, Xamarin
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of this 
//  software and associated documentation files (the "Software"), to deal in the Software 
//  without restriction, including without limitation the rights to use, copy, modify, merge, 
//  publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons 
//  to whom the Software is furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in all copies 
//  or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
//  PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
//  FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//  ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS 
//  IN THE SOFTWARE.
//

namespace XamarinStore
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;

    using MonoTouch.CoreAnimation;
    using MonoTouch.CoreGraphics;
    using MonoTouch.Foundation;
    using MonoTouch.UIKit;

    public class JBKenBurnsView : UIView
    {
        #region Constants

        private const float enlargeRatio = 1.1f;

        private const int imageBuffer = 1;

        #endregion

        #region Fields

        public List<UIImage> Images = new List<UIImage>();

        private readonly Queue<UIView> Views = new Queue<UIView>();

        private readonly Random random = new Random();

        private int currentIndex;

        private NSTimer timer;

        #endregion

        #region Constructors and Destructors

        public JBKenBurnsView()
        {
            this.ImageDuration = 12;
            this.ShouldLoop = true;
            this.IsLandscape = true;
            this.BackgroundColor = UIColor.Clear;
            this.Layer.MasksToBounds = true;
            this.IsActive = false;
        }

        #endregion

        #region Public Events

        public event EventHandler Finished;

        public event Action<int> ImageIndexChanged;

        #endregion

        #region Public Properties

        public int CurrentIndex
        {
            get
            {
                return this.currentIndex;
            }
            set
            {
                this.currentIndex = value;
                if (this.Images.Count < this.currentIndex)
                {
                    this.Animate();
                }
            }
        }

        public double ImageDuration { get; set; }

        public bool IsActive { get; private set; }

        public bool IsLandscape { get; set; }

        public bool ShouldLoop { get; set; }

        #endregion

        #region Public Methods and Operators

        public void Animate()
        {
            this.IsActive = true;
            if (this.timer != null)
            {
                this.timer.Invalidate();
            }
            this.timer = NSTimer.CreateRepeatingScheduledTimer(this.ImageDuration, this.NextImage);
            this.timer.Fire();
        }

        #endregion

        #region Methods

        private void NextImage()
        {
            if (this.Images.Count == 0 || this.currentIndex >= this.Images.Count && !this.ShouldLoop)
            {
                if (this.Finished != null)
                {
                    this.Finished(this, EventArgs.Empty);
                }
                return;
            }

            if (this.currentIndex >= this.Images.Count)
            {
                this.currentIndex = 0;
            }

            var image = this.Images[this.currentIndex];
            this.currentIndex++;
            if (image == null)
            {
                if (image == null || image.Size == Size.Empty)
                {
                    return;
                }
            }

            float resizeRatio = -1;
            float widthDiff = -1;
            float heightDiff = -1;
            float originX = -1;
            float originY = -1;
            float zoomInX = -1;
            float zoomInY = -1;
            float moveX = -1;
            float moveY = -1;
            var frameWidth = this.IsLandscape ? this.Bounds.Width : this.Bounds.Height;
            var frameHeight = this.IsLandscape ? this.Bounds.Height : this.Bounds.Width;

            // Wider than screen 
            var imageWidth = image.Size.Width == 0 ? 100 : image.Size.Width;
            var imageHeight = image.Size.Height == 0 ? 100 : image.Size.Height;

            if (imageWidth > frameWidth)
            {
                widthDiff = imageWidth - frameWidth;

                // Higher than screen
                if (imageHeight > frameHeight)
                {
                    heightDiff = imageHeight - frameHeight;

                    if (widthDiff > heightDiff)
                    {
                        resizeRatio = frameHeight / imageHeight;
                    }
                    else
                    {
                        resizeRatio = frameWidth / imageWidth;
                    }

                    // No higher than screen [OK]
                }
                else
                {
                    heightDiff = frameHeight - imageHeight;

                    if (widthDiff > heightDiff)
                    {
                        resizeRatio = frameWidth / imageWidth;
                    }
                    else
                    {
                        resizeRatio = this.Bounds.Height / imageHeight;
                    }
                }

                // No wider than screen
            }
            else
            {
                widthDiff = frameWidth - imageWidth;

                // Higher than screen [OK]
                if (imageHeight > frameHeight)
                {
                    heightDiff = imageHeight - frameHeight;

                    if (widthDiff > heightDiff)
                    {
                        resizeRatio = imageHeight / frameHeight;
                    }
                    else
                    {
                        resizeRatio = frameWidth / imageWidth;
                    }

                    // No higher than screen [OK]
                }
                else
                {
                    heightDiff = frameHeight - imageHeight;

                    if (widthDiff > heightDiff)
                    {
                        resizeRatio = frameWidth / imageWidth;
                    }
                    else
                    {
                        resizeRatio = frameHeight / imageHeight;
                    }
                }
            }

            // Resize the image.
            var optimusWidth = (imageWidth * resizeRatio) * enlargeRatio;
            var optimusHeight = (imageHeight * resizeRatio) * enlargeRatio;
            var imageView = new UIView { Frame = new RectangleF(0, 0, optimusWidth, optimusHeight), BackgroundColor = UIColor.Clear, };

            var maxMoveX = Math.Min(optimusWidth - frameWidth, 50f);
            var maxMoveY = Math.Min(optimusHeight - frameHeight, 50f) * 2 / 3;

            float rotation = (this.random.Next(9)) / 100;

            switch (this.random.Next(3))
            {
                case 0:
                    originX = 0;
                    originY = 0;
                    zoomInX = 1.25f;
                    zoomInY = 1.25f;
                    moveX = -maxMoveX;
                    moveY = -maxMoveY;
                    break;

                case 1:
                    originX = 0;
                    originY = 0; // Math.Max(frameHeight - (optimusHeight),frameHeight * 1/3);
                    zoomInX = 1.1f;
                    zoomInY = 1.1f;
                    moveX = -maxMoveX;
                    moveY = maxMoveY;
                    break;

                case 2:
                    originX = frameWidth - optimusWidth;
                    originY = 0;
                    zoomInX = 1.3f;
                    zoomInY = 1.3f;
                    moveX = maxMoveX;
                    moveY = -maxMoveY;
                    break;

                default:
                    originX = frameWidth - optimusWidth;
                    originY = 0; //Math.Max(frameHeight - (optimusHeight),frameHeight * 1/3);
                    zoomInX = 1.2f;
                    zoomInY = 1.2f;
                    moveX = maxMoveX;
                    moveY = maxMoveY;
                    break;
            }

            var picLayer = new CALayer { Contents = image.CGImage, AnchorPoint = PointF.Empty, Bounds = imageView.Bounds, Position = new PointF(originX, originY) };
            imageView.Layer.AddSublayer(picLayer);

            var animation = new CATransition { Duration = 1, Type = CAAnimation.TransitionFade, };
            this.Layer.AddAnimation(animation, null);

            this.Views.Enqueue(imageView);
            while (this.Views.Count > imageBuffer)
            {
                this.Views.Dequeue().RemoveFromSuperview();
            }

            this.AddSubview(imageView);

            Animate(
                this.ImageDuration + 2,
                0,
                UIViewAnimationOptions.CurveEaseIn,
                () =>
                    {
                        var t = CGAffineTransform.MakeRotation(rotation);
                        t.Translate(moveX, moveY);
                        t.Scale(zoomInX, zoomInY);
                        imageView.Transform = t;
                    },
                null);

            if (this.ImageIndexChanged != null)
            {
                this.ImageIndexChanged(this.currentIndex);
            }
        }

        #endregion
    }
}