using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsControls.CustomControls
{
    public class CDropdownMenu : ContextMenuStrip
    {
        //Fields
        private bool isMainMenu;
        private int menuItemHeight;
        private int menuItemWidth;
        private Color menuItemTextColor;
        private Color primaryColor;
        private Color leftColumnColor;
        private Color backGroundColor;
        private Bitmap menuItemHeaderSize;
        //Constructor
        public CDropdownMenu(IContainer container)
            : base(container)
        {
            init();
        }

        private void init() {
            menuItemHeight = 20;
            menuItemWidth = 35;
            menuItemTextColor = backGroundColor = primaryColor = leftColumnColor = Color.Empty;
        }

    
        //[Browsable(true)]
        [Category("Custom Style")]
        public bool IsMainMenu
        {
            get { return isMainMenu; }
            set { isMainMenu = value;
                this.Invalidate();
            }
        }

        //[Browsable(true)]
        [Category("Custom Style")]
        public int MenuItemHeight
        {
            get { return menuItemHeight; }
            set { menuItemHeight = value;
                this.Invalidate();
            }
        }

        [Category("Custom Style")]
        public int MenuItemWidth
        {
            get { return menuItemWidth; }
            set
            {
                menuItemWidth = value;
                this.Invalidate();
            }
        }

        //[Browsable(true)]
        [Category("Custom Style")]
        public Color BackGroundColor
        {
            get { return backGroundColor; }
            set {
                backGroundColor = value;
                this.Invalidate();
            }
        }
        
        [Category("Custom Style")]
        public Color MenuItemTextColor
        {
            get { return menuItemTextColor; }
            set { menuItemTextColor = value;
                this.Invalidate();
            }
        }

        //[Browsable(true)]
        [Category("Custom Style")]
        public Color PrimaryColor
        {
            get { return primaryColor; }
            set { primaryColor = value;
                this.Invalidate();
            }
        }

        [Category("Custom Style")]
        public Color LeftColumnColor
        {
            get { return leftColumnColor; }
            set
            {
                leftColumnColor = value;
                this.Invalidate();
            }
        }


        //Private methods
        private void LoadMenuItemHeight()
        {
            //if (isMainMenu)
             menuItemHeaderSize = new Bitmap(menuItemHeight, menuItemWidth);
            //menuItemHeaderSize = new Bitmap(25, 45);
           // else menuItemHeaderSize = new Bitmap(20, menuItemHeight);
            foreach (ToolStripMenuItem menuItemL1 in this.Items)
            {
                menuItemL1.ImageScaling = ToolStripItemImageScaling.None;
                if (menuItemL1.Image == null) menuItemL1.Image = menuItemHeaderSize;
                foreach (ToolStripMenuItem menuItemL2 in menuItemL1.DropDownItems)
                {
                    menuItemL2.ImageScaling = ToolStripItemImageScaling.None;
                    if (menuItemL2.Image == null) menuItemL2.Image = menuItemHeaderSize;
                    foreach (ToolStripMenuItem menuItemL3 in menuItemL2.DropDownItems)
                    {
                        menuItemL3.ImageScaling = ToolStripItemImageScaling.None;
                        if (menuItemL3.Image == null) menuItemL3.Image = menuItemHeaderSize;
                        foreach (ToolStripMenuItem menuItemL4 in menuItemL3.DropDownItems)
                        {
                            menuItemL4.ImageScaling = ToolStripItemImageScaling.None;
                            if (menuItemL4.Image == null) menuItemL4.Image = menuItemHeaderSize;
                            ///Level 5++
                        }
                    }
                }
            }
        }
        //Overrides
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (this.DesignMode == false)
            {
                this.Renderer = new MenuRenderer(isMainMenu, primaryColor, menuItemTextColor, leftColumnColor, backGroundColor);
                LoadMenuItemHeight();
            }
        }

        }
}
