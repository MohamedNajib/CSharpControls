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
    public class CButton : Button
    {
        private int borderSize;
        private int borderRadius;
        private Color borderColor;
        private Color baseBorderColor;
        private Color baseColor;
        private Color baseTextColor;

        // Hover
        private Color onHoverBaseColor;
        private Color onHoverTextColor;
        private Color onHoverBorderColor;


        public CButton()
        {
            init();
        }

        private void init () {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Size = new Size(150, 45);
            this.BackColor = baseColor = Color.RoyalBlue;
            borderColor = baseBorderColor = Color.LightSeaGreen;
            borderRadius = 18;
            borderSize = 0;
            this.ForeColor = baseTextColor = Color.White;
            onHoverTextColor = Color.White;
            onHoverBaseColor = Color.FromArgb(0x56, baseColor);
            onHoverBorderColor = borderColor;

            this.Resize += new EventHandler(resizeControl);
        }

        [Category("Custom Style")]
        public int BorderSize
        {
            get { return borderSize; }
            set
            {
                borderSize = value;
                this.Invalidate();
            }
        }

        [Category("Custom Style")]
        public int BorderRadius
        {
            get { return borderRadius; }
            set
            {
                if (value <= this.Height)
                    borderRadius = value;
                else borderRadius = this.Height;
                this.Invalidate();
            }
        }

        [Category("Custom Style")]
        public Color BaseBorderColor
        {
            get { return baseBorderColor; }
            set
            {
                baseBorderColor = value;
                borderColor = baseBorderColor;
                this.Invalidate();
            }
        }
        [Category("Custom Style")]
        public Color BaseColor
        {
            get { return this.baseColor; }
            set
            {
                this.baseColor = value;
                this.BackColor = baseColor;
                this.Invalidate();
            }
        }

        [Category("Custom Style")]
        public Color OnHoverBaseColor
        {
            get { return onHoverBaseColor; }
            set
            {
                onHoverBaseColor = value;
                this.Invalidate();
            }
        }

        [Category("Custom Style")]
        public Color OnHoverTextColor
        {
            get { return onHoverTextColor; }
            set
            {
                onHoverTextColor = value;
                this.Invalidate();
            }
        }
        [Category("Custom Style")]
        public Color OnHoverBorderColor
        {
            get { return onHoverBorderColor; }
            set
            {
                onHoverBorderColor = value;
                this.Invalidate();
            }
        }

        [Category("Custom Style")]
        public Color BaseTextColor
        {
            get { return this.baseTextColor; }
            set
            {
                this.baseTextColor = value;
                this.ForeColor = baseTextColor;
                this.Invalidate();
            }
        }



        private void resizeControl(object sender, EventArgs e)
        {
            if (borderRadius > this.Height)
                borderRadius = this.Height;
        }

        //Methods
        private GraphicsPath GetFigurePath(Rectangle rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            Rectangle rectSurface = this.ClientRectangle;
            Rectangle rectBorder = Rectangle.Inflate(rectSurface, -borderSize, -borderSize);
            int smoothSize = 2;
            if (borderSize > 0)
                smoothSize = borderSize;

            if (borderRadius > 2) //Rounded button
            {
                using (GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
                using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - borderSize))
                using (Pen penSurface = new Pen(this.Parent.BackColor, smoothSize))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    //Button surface
                    this.Region = new Region(pathSurface);
                    //Draw surface border for HD result
                    pevent.Graphics.DrawPath(penSurface, pathSurface);

                    //Button border                    
                    if (borderSize >= 1)
                        //Draw control border
                        pevent.Graphics.DrawPath(penBorder, pathBorder);
                }
            }
            else //Normal button
            {
                pevent.Graphics.SmoothingMode = SmoothingMode.None;
                //Button surface
                this.Region = new Region(rectSurface);
                //Button border
                if (borderSize >= 1)
                {
                    using (Pen penBorder = new Pen(borderColor, borderSize))
                    {
                        penBorder.Alignment = PenAlignment.Inset;
                        pevent.Graphics.DrawRectangle(penBorder, 0, 0, this.Width - 1, this.Height - 1);
                    }
                }
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.Parent.BackColorChanged += new EventHandler(Container_BackColorChanged);
        }

        private void Container_BackColorChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            this.BackColor = onHoverBaseColor;
            this.ForeColor = onHoverTextColor;
            borderColor = onHoverBorderColor;


            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            this.BackColor = this.baseColor;
            this.ForeColor = baseTextColor;
            borderColor = baseBorderColor;
            base.OnMouseLeave(e);
        }


    }
}
