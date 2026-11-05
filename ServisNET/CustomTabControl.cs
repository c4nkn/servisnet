using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ServisNET
{
    public class CustomTabControl : TabControl
    {
        private readonly Color _headerColor = Color.FromArgb(36, 36, 36);
        private readonly Color _activeBgColor = Color.FromArgb(53, 53, 55);
        private readonly Color _inactiveBgColor = Color.FromArgb(36, 36, 36);
        private readonly Color _borderColor = Color.FromArgb(93, 94, 96);
        private readonly Color _activeTextColor = Color.White;
        private readonly Color _inactiveTextColor = Color.FromArgb(140, 151, 164);

        private readonly SolidBrush _headerBrush;
        private readonly SolidBrush _activeBgBrush;
        private readonly SolidBrush _inactiveBgBrush;
        private readonly Pen _borderPen;
        private readonly Pen _hidePen;

        public CustomTabControl()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            _headerBrush = new SolidBrush(_headerColor);
            _activeBgBrush = new SolidBrush(_activeBgColor);
            _inactiveBgBrush = new SolidBrush(_inactiveBgColor);
            _borderPen = new Pen(_borderColor, 1);
            _hidePen = new Pen(_activeBgColor, 1);

            SizeMode = TabSizeMode.Fixed;
            ItemSize = new Size(160, 40);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (DesignMode == false && LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                foreach (TabPage page in TabPages)
                {
                    page.BackColor = _activeBgColor;
                }
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            if (DesignMode == false && LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                if (e.Control is TabPage)
                {
                    e.Control.BackColor = _activeBgColor;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.None;

            Color backColor;
            if (Parent != null)
            {
                backColor = Parent.BackColor;
            }
            else
            {
                backColor = _headerColor;
            }

            g.Clear(backColor);

            int headerHeight = ItemSize.Height;
            if (TabCount > 0)
            {
                headerHeight = GetTabRect(0).Bottom;
            }

            Rectangle headerRect = new Rectangle(0, 0, Width, headerHeight);
            g.FillRectangle(_headerBrush, headerRect);

            Rectangle bodyRect = new Rectangle(0, headerHeight, Width, Height - headerHeight);
            g.FillRectangle(_activeBgBrush, bodyRect);

            for (int i = 0; i < TabCount; i++)
            {
                DrawTab(g, i);
            }

            Rectangle borderRect = new Rectangle(0, headerHeight, Width - 1, Height - headerHeight - 1);
            g.DrawRectangle(_borderPen, borderRect);

            if (SelectedIndex >= 0 && SelectedIndex < TabCount)
            {
                Rectangle activeTabRect = GetTabRect(SelectedIndex);
                if (SelectedIndex == 0)
                {
                    activeTabRect.Width = activeTabRect.Width + activeTabRect.X;
                    activeTabRect.X = 0;
                }

                g.DrawLine(_borderPen, activeTabRect.Left, activeTabRect.Top, activeTabRect.Left, activeTabRect.Bottom);
                g.DrawLine(_borderPen, activeTabRect.Left, activeTabRect.Top, activeTabRect.Right, activeTabRect.Top);
                g.DrawLine(_borderPen, activeTabRect.Right, activeTabRect.Top, activeTabRect.Right, activeTabRect.Bottom);

                g.DrawLine(_hidePen, activeTabRect.Left + 1, activeTabRect.Bottom, activeTabRect.Right - 1, activeTabRect.Bottom);
            }
        }

        private void DrawTab(Graphics g, int index)
        {
            Rectangle tabRect = GetTabRect(index);

            if (index == 0)
            {
                tabRect.Width = tabRect.Width + tabRect.X;
                tabRect.X = 0;
            }

            bool isSelected = (SelectedIndex == index);

            SolidBrush currentTabBrush;
            Color currentTextColor;

            if (isSelected)
            {
                currentTabBrush = _activeBgBrush;
                currentTextColor = _activeTextColor;
            }
            else
            {
                currentTabBrush = _inactiveBgBrush;
                currentTextColor = _inactiveTextColor;
            }

            g.FillRectangle(currentTabBrush, tabRect);

            if (isSelected == false)
            {
                if (index < TabCount - 1)
                {
                    if (SelectedIndex != index + 1)
                    {
                        int padding = 8;
                        g.DrawLine(_borderPen, tabRect.Right - 1, tabRect.Top + padding, tabRect.Right - 1, tabRect.Bottom - padding);
                    }
                }
            }

            int iconOffset = 0;
            if (ImageList != null)
            {
                if (TabPages[index].ImageIndex >= 0)
                {
                    Image icon = ImageList.Images[TabPages[index].ImageIndex];
                    int iconY = tabRect.Y + ((tabRect.Height - icon.Height) / 2);
                    Rectangle iconRect = new Rectangle(tabRect.X + 8, iconY, icon.Width, icon.Height);
                    g.DrawImage(icon, iconRect);
                    iconOffset = icon.Width + 4;
                }
                else if (string.IsNullOrEmpty(TabPages[index].ImageKey) == false)
                {
                    Image icon = ImageList.Images[TabPages[index].ImageKey];
                    if (icon != null)
                    {
                        int iconY = tabRect.Y + ((tabRect.Height - icon.Height) / 2);
                        Rectangle iconRect = new Rectangle(tabRect.X + 8, iconY, icon.Width, icon.Height);
                        g.DrawImage(icon, iconRect);
                        iconOffset = icon.Width + 4;
                    }
                }
            }

            Rectangle textRect = tabRect;
            textRect.X = textRect.X + 8 + iconOffset;

            TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine;
            TextRenderer.DrawText(g, TabPages[index].Text, Font, textRect, currentTextColor, flags);
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _headerBrush?.Dispose();
                _activeBgBrush?.Dispose();
                _inactiveBgBrush?.Dispose();
                _borderPen?.Dispose();
                _hidePen?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}