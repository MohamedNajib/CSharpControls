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
    class CDateTimePicker : DateTimePicker
    {
        //Fields
        //-> Appearance
        private Color skinColor;
        private Color textColor;
        private Color borderColor;
        private int borderSize;
        private int borderRadius;

        //-> Other Values
        private bool droppedDown;
        private Image calendarIcon;
        private RectangleF iconButtonArea;
        private const int calendarIconWidth = 34;
        private const int arrowIconWidth = 17;

        //Constructor
        public CDateTimePicker()
        {
            init();
        }

        private void init() {
            skinColor = Color.MediumSlateBlue;
            textColor = Color.White;
            borderColor = Color.MediumSlateBlue;
            droppedDown = false;
            calendarIcon = Properties.Resources.cw;
            borderSize = 0;
            borderRadius = 2;

            this.SetStyle(ControlStyles.UserPaint, true);
            this.MinimumSize = new Size(0, 35);
            this.Font = new Font(this.Font.Name, 9.5F);

            this.Resize += new EventHandler(resizeControl);
        }

        //Properties
        [Category("Custom Style")]
        public Color SkinColor
        {
            get { return skinColor; }
            set
            {
                skinColor = value;
                if (skinColor.GetBrightness() >= 0.8F)
                    calendarIcon = Properties.Resources.cb;
                else calendarIcon = Properties.Resources.cw;
                this.Invalidate();
            }
        }

        [Category("Custom Style")]
        public Color TextColor
        {
            get { return textColor; }
            set
            {
                textColor = value;
                this.Invalidate();
            }
        }

        [Category("Custom Style")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                this.Invalidate();
            }
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
                /*  if (value < 2)
                      borderRadius = 2;
                  else if (value > this.Height)
                      borderRadius = this.Height;
                  else borderRadius = value;

                  this.Invalidate();*/
            }
        }

        private void resizeControl(object sender, EventArgs e)
        {
            if (borderRadius > this.Height)
                borderRadius = this.Height;
        }

        //Overridden methods
        protected override void OnDropDown(EventArgs eventargs)
        {
            base.OnDropDown(eventargs);
            droppedDown = true;
        }
        protected override void OnCloseUp(EventArgs eventargs)
        {
            base.OnCloseUp(eventargs);
            droppedDown = false;
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            e.Handled = true;
        }

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
                using (Graphics graphics = this.CreateGraphics())
                using (GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
                using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - borderSize))
                using (Pen penSurface = new Pen(this.Parent.BackColor, smoothSize))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                using (SolidBrush skinBrush = new SolidBrush(skinColor))
                using (SolidBrush openIconBrush = new SolidBrush(Color.FromArgb(50, 64, 64, 64)))
                using (SolidBrush textBrush = new SolidBrush(textColor))
                using (StringFormat textFormat = new StringFormat())
                {
                    pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    //DateTime surface
                    this.Region = new Region(pathSurface);
                    //Draw surface border for HD result
                    pevent.Graphics.DrawPath(penSurface, pathSurface);

                    //DateTime border                    
                    if (borderSize >= 1)
                        //Draw control border
                        pevent.Graphics.DrawPath(penBorder, pathBorder);


                    RectangleF clientArea = new RectangleF(0, 0, this.Width - 0.5F, this.Height - 0.5F);
                    RectangleF iconArea = new RectangleF(clientArea.Width - calendarIconWidth, 0, calendarIconWidth, clientArea.Height);
                    penBorder.Alignment = PenAlignment.Inset;
                    textFormat.LineAlignment = StringAlignment.Center;

                    //Draw surface
                    graphics.FillRectangle(skinBrush, clientArea);
                    //Draw text
                    graphics.DrawString("   " + this.Text, this.Font, textBrush, clientArea, textFormat);
                    //Draw open calendar icon highlight
                    if (droppedDown == true) graphics.FillRectangle(openIconBrush, iconArea);
                    //Draw border 
                    if (borderSize >= 1) graphics.DrawRectangle(penBorder, clientArea.X, clientArea.Y, clientArea.Width, clientArea.Height);
                    //Draw icon
                    graphics.DrawImage(calendarIcon, this.Width - calendarIcon.Width - 9, (this.Height - calendarIcon.Height) / 2);
                }
            }
            else //Normal button
            {
                using (Graphics graphics = this.CreateGraphics())
                using (Pen penBorder = new Pen(borderColor, borderSize))
                using (SolidBrush skinBrush = new SolidBrush(skinColor))
                using (SolidBrush openIconBrush = new SolidBrush(Color.FromArgb(50, 64, 64, 64)))
                using (SolidBrush textBrush = new SolidBrush(textColor))
                using (StringFormat textFormat = new StringFormat())
                {
                    RectangleF clientArea = new RectangleF(0, 0, this.Width - 0.5F, this.Height - 0.5F);
                    RectangleF iconArea = new RectangleF(clientArea.Width - calendarIconWidth, 0, calendarIconWidth, clientArea.Height);
                    penBorder.Alignment = PenAlignment.Inset;
                    textFormat.LineAlignment = StringAlignment.Center;
                    //Draw surface
                    graphics.FillRectangle(skinBrush, clientArea);
                    //Draw text
                    graphics.DrawString("   " + this.Text, this.Font, textBrush, clientArea, textFormat);
                    //Draw open calendar icon highlight
                    if (droppedDown == true) graphics.FillRectangle(openIconBrush, iconArea);
                    //Draw border 
                    if (borderSize >= 1) graphics.DrawRectangle(penBorder, clientArea.X, clientArea.Y, clientArea.Width, clientArea.Height);
                    //Draw icon
                    graphics.DrawImage(calendarIcon, this.Width - calendarIcon.Width - 9, (this.Height - calendarIcon.Height) / 2);
                }
                /* pevent.Graphics.SmoothingMode = SmoothingMode.None;
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
                 }*/
            }
        }
        /*protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rectSurface = this.ClientRectangle;
            Rectangle rectBorder = Rectangle.Inflate(rectSurface, -borderSize, -borderSize);
            int smoothSize = 2;
            if (borderSize > 0)
                smoothSize = borderSize;

            using (Graphics graphics = this.CreateGraphics())
            using (GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
            using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - borderSize))
            using (Pen penSurface = new Pen(this.Parent.BackColor, smoothSize))
            using (Pen penBorder = new Pen(borderColor, borderSize)) 
            using (SolidBrush skinBrush = new SolidBrush(skinColor))
            using (SolidBrush openIconBrush = new SolidBrush(Color.FromArgb(50, 64, 64, 64)))
            using (SolidBrush textBrush = new SolidBrush(textColor))
            using (StringFormat textFormat = new StringFormat())
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                //DateTime surface
                this.Region = new Region(pathSurface);
                //Draw surface border for HD result
                e.Graphics.DrawPath(penSurface, pathSurface);

                //DateTime border                    
                if (borderSize >= 1)
                    //Draw control border
                    e.Graphics.DrawPath(penBorder, pathBorder);


                RectangleF clientArea = new RectangleF(0, 0, this.Width - 0.5F, this.Height - 0.5F);
                RectangleF iconArea = new RectangleF(clientArea.Width - calendarIconWidth, 0, calendarIconWidth, clientArea.Height);
                penBorder.Alignment = PenAlignment.Inset;
                textFormat.LineAlignment = StringAlignment.Center;

                //Draw surface
                graphics.FillRectangle(skinBrush, clientArea);
                //Draw text
                graphics.DrawString("   " + this.Text, this.Font, textBrush, clientArea, textFormat);
                //Draw open calendar icon highlight
                if (droppedDown == true) graphics.FillRectangle(openIconBrush, iconArea);
                //Draw border 
                if (borderSize >= 1) graphics.DrawRectangle(penBorder, clientArea.X, clientArea.Y, clientArea.Width, clientArea.Height);
                //Draw icon
                graphics.DrawImage(calendarIcon, this.Width - calendarIcon.Width - 9, (this.Height - calendarIcon.Height) / 2);

            }
        }*/
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            int iconWidth = GetIconButtonWidth();
            iconButtonArea = new RectangleF(this.Width - iconWidth, 0, iconWidth, this.Height);
            
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (iconButtonArea.Contains(e.Location))
                this.Cursor = Cursors.Hand;
            else this.Cursor = Cursors.Default;
        }

        //Private methods
        private int GetIconButtonWidth()
        {
            int textWidh = TextRenderer.MeasureText(this.Text, this.Font).Width;
            if (textWidh <= this.Width - (calendarIconWidth + 20))
                return calendarIconWidth;
            else return arrowIconWidth;
        }
    }
}
