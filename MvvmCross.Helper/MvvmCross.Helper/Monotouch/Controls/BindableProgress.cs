namespace MobileApp.iOS.UI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using MonoTouch.Foundation;
    using MonoTouch.UIKit;

    using MBProgressHUD;

    public class BindableProgress
    {
        private MTMBProgressHUD progress;
        private readonly UIView parent;

        private string text;

        public BindableProgress(UIView parent, string text)
        {
            this.parent = parent;
            this.Text = text;
        }

        public bool Visible
        {
            get { return this.progress != null; }
            set
            {
                if (Visible == value)
                    return;

                if (value)
                {
                    this.progress = new MTMBProgressHUD(this.parent)
                    {
                        LabelText = this.Text,
                        RemoveFromSuperViewOnHide = true
                    };

                    parent.UserInteractionEnabled = false;
                    this.parent.AddSubview(this.progress);
                    this.progress.Show(true);
                }
                else
                {
                    parent.UserInteractionEnabled = true;
                    this.progress.Hide(true);
                    this.progress = null;
                }
            }
        }

        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }
    }
}