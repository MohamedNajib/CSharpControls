using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace WindowsControls.CustomControls
{
    public class CToggleButton : CheckBox
    {

        private Color onBackColor;
        private Color offBackColor;

        private Color onToggleColor;
        private Color offToggleColor;
        private bool outLineStyle;

        public CToggleButton()
        {
            init();
        }
        
        private void init(){
            outLineStyle = false;
            this.MinimumSize = new Size(50, 22);
            if (!outLineStyle)
            {
                onBackColor = Color.MediumSlateBlue;
                offBackColor = Color.Gray;
                onToggleColor = Color.WhiteSmoke;
                offToggleColor = Color.Gainsboro;
            }
        }

        [Category("Custom Style")]
        public Color OnBackColor
        {
            get { return onBackColor; }
            set
            {
                onBackColor = value;
                this.Invalidate();
            }
        }

        [Category("Custom Style")]
        public Color OnToggleColor
        {
            get { return onToggleColor; }
            set
            {
                onToggleColor = value;
                this.Invalidate();
            }
        }

        [Category("Custom Style")]
        public Color OffBackColor
        {
            get { return offBackColor; }
            set
            {
                offBackColor = value;
                this.Invalidate();
            }
        }

        [Category("Custom Style")]
        public Color OffToggleColor
        {
            get { return offToggleColor; }
            set
            {
                offToggleColor = value;
                this.Invalidate();
            }
        }

        [Browsable(false)]
        public override string Text
        {
            get { return base.Text; }
            set { }
        }

        [Category("Custom Style")]
        [DefaultValue(false)]
        public bool OutLineStyle
        {
            get { return outLineStyle; }
            set
            {
                outLineStyle = value;
                if (value) {
                    onBackColor = Color.MediumSlateBlue;
                    offBackColor = Color.Gray;
                    onToggleColor = Color.MediumSlateBlue;
                    offToggleColor = Color.Gray;
                }
                this.Invalidate();
            }
        }

        //Methods
        private GraphicsPath GetFigurePath()
        {
            int arcSize = this.Height - 1;
            Rectangle leftArc = new Rectangle(0, 0, arcSize, arcSize);
            Rectangle rightArc = new Rectangle(this.Width - arcSize - 2, 0, arcSize, arcSize);

            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(leftArc, 90, 180);
            path.AddArc(rightArc, 270, 180);
            path.CloseFigure();

            return path;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            pevent.Graphics.Clear(this.Parent.BackColor);

            bool isToggleOn = this.Checked;

            if (isToggleOn) //ON
            {
                if (!outLineStyle)
                {
                    pevent.Graphics.FillPath(new SolidBrush(onBackColor), GetFigurePath());
                }
                else
                {
                    pevent.Graphics.DrawPath(new Pen(onBackColor, 2), GetFigurePath());
                }
                //Draw the toggle
                pevent.Graphics.FillEllipse(new SolidBrush(onToggleColor),
                  new Rectangle(this.Width - this.Height + 1, 2, this.Height - 5, this.Height - 5));
            }
            else //OFF
            {
                if (!outLineStyle)
                {
                    pevent.Graphics.FillPath(new SolidBrush(offBackColor), GetFigurePath());
                }
                else
                {
                    pevent.Graphics.DrawPath(new Pen(offBackColor, 2), GetFigurePath());
                }
                //Draw the toggle
                pevent.Graphics.FillEllipse(new SolidBrush(offToggleColor),
                  new Rectangle(2, 2, this.Height - 5, this.Height - 5));
            }
        }
    }
}
