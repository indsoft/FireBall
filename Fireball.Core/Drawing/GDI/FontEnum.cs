#region Copyright

//    Copyright (C) 2005  Sebastian Faltoni  <sebastian@dotnetfireball.net>
//
//    This library is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Lesser General Public
//    License as published by the Free Software Foundation; either
//    version 2.1 of the License, or (at your option) any later version.
//
//    This library is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Lesser General Public
//    License along with this library; if not, write to the Free Software
//    Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

#endregion

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Fireball.Win32;


namespace Fireball.Drawing.GDI
{
    public class FontList : UITypeEditor
    {
        private IWindowsFormsEditorService edSvc = null;
        private ListBox FontListbox;

        private void LB_DrawItem(object sender, DrawItemEventArgs e)
        {
            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            if (e.Index == -1)
                return;


            object li = FontListbox.Items[e.Index];
            string text = li.ToString();
            Brush bg, fg;

            if (selected)
            {
                bg = SystemBrushes.Highlight;
                fg = SystemBrushes.HighlightText;
                //fg=Brushes.Black;
            }
            else
            {
                bg = SystemBrushes.Window;
                fg = SystemBrushes.WindowText;
            }

            //e.Graphics.FillRectangle (SystemBrushes.Window,0,e.Bounds.Top,e.Bounds.Width ,FontListbox.ItemHeight); 			
            if (selected)
            {
                int ofs = 37;
                e.Graphics.FillRectangle(SystemBrushes.Window, new Rectangle(ofs, e.Bounds.Top, e.Bounds.Width - ofs, FontListbox.ItemHeight));
                e.Graphics.FillRectangle(SystemBrushes.Highlight, new Rectangle(ofs + 1, e.Bounds.Top + 1, e.Bounds.Width - ofs - 2, FontListbox.ItemHeight - 2));
                System.Windows.Forms.ControlPaint.DrawFocusRectangle(e.Graphics, new Rectangle(ofs, e.Bounds.Top, e.Bounds.Width - ofs, FontListbox.ItemHeight));
            }
            else
            {
                e.Graphics.FillRectangle(SystemBrushes.Window, 0, e.Bounds.Top, e.Bounds.Width, FontListbox.ItemHeight);
            }


            e.Graphics.DrawString(text, e.Font, fg, 38, e.Bounds.Top + 4);

            e.Graphics.SetClip(new Rectangle(1, e.Bounds.Top + 2, 34, FontListbox.ItemHeight - 4));


            e.Graphics.FillRectangle(SystemBrushes.Highlight, new Rectangle(1, e.Bounds.Top + 2, 34, FontListbox.ItemHeight - 4));

            IntPtr hdc = e.Graphics.GetHdc();
            GDIFont gf = new GDIFont(text, 9);
            int a = 0;
            IntPtr res = NativeGdi32Api.SelectObject(hdc, gf.hFont);
            NativeGdi32Api.SetTextColor(hdc, ColorTranslator.ToWin32(SystemColors.Window));
            NativeGdi32Api.SetBkMode(hdc, 0);
            NativeUser32Api.TabbedTextOut(hdc, 3, e.Bounds.Top + 5, "abc", 3, 0, ref a, 0);
            NativeGdi32Api.SelectObject(hdc, res);
            gf.Dispose();
            e.Graphics.ReleaseHdc(hdc);
            e.Graphics.DrawRectangle(Pens.Black, new Rectangle(1, e.Bounds.Top + 2, 34, FontListbox.ItemHeight - 4));
            e.Graphics.ResetClip();

        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context != null
                && context.Instance != null
                && provider != null)
            {
                edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));


                
                if (edSvc != null)
                {
                    // Create a CheckedListBox and populate it with all the enum values
                    FontListbox = new ListBox();
                    FontListbox.DrawMode = DrawMode.OwnerDrawFixed;
                    FontListbox.BorderStyle = BorderStyle.None;
                    FontListbox.Sorted = true;
                    FontListbox.MouseDown += new MouseEventHandler(this.OnMouseDown);
                    FontListbox.DoubleClick += new EventHandler(this.ValueChanged);
                    FontListbox.DrawItem += new DrawItemEventHandler(this.LB_DrawItem);
                    FontListbox.ItemHeight = 20;
                    FontListbox.MouseMove += new MouseEventHandler(this.OnMouseMoved);
                    FontListbox.Height = 200;
                    FontListbox.Width = 180;

                    ICollection fonts = new FontEnum().EnumFonts();
                    foreach (string font in fonts)
                    {
                        FontListbox.Items.Add(font);
                    }
                    edSvc.DropDownControl(FontListbox);
                    if (FontListbox.SelectedItem != null)
                        return FontListbox.SelectedItem.ToString();
                }
            }

            return value;
        }


        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        private bool handleLostfocus = false;

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (!handleLostfocus && FontListbox.ClientRectangle.Contains(FontListbox.PointToClient(new Point(e.X, e.Y))))
            {
                FontListbox.LostFocus += new EventHandler(this.ValueChanged);
                handleLostfocus = true;
            }
        }

        private void OnMouseMoved(object sender, MouseEventArgs e)
        {
        }

        private void ValueChanged(object sender, EventArgs e)
        {
            if (edSvc != null)
            {
                edSvc.CloseDropDown();
            }
        }

        public override void PaintValue(PaintValueEventArgs e)
        {
            string text = e.Value.ToString();
            Bitmap bp = new Bitmap(e.Bounds.Width, e.Bounds.Height);
            Graphics g = Graphics.FromImage(bp);

            Brush bg, fg;

            bg = SystemBrushes.Window;
            fg = SystemBrushes.WindowText;

            g.FillRectangle(SystemBrushes.Highlight, e.Bounds);

            IntPtr hdc = g.GetHdc();
            GDIFont gf = new GDIFont(text, 9);
            int a = 0;
            IntPtr res = NativeGdi32Api.SelectObject(hdc, gf.hFont);
            NativeGdi32Api.SetTextColor(hdc, ColorTranslator.ToWin32(SystemColors.Window));
            NativeGdi32Api.SetBkMode(hdc, 0);
            NativeUser32Api.TabbedTextOut(hdc, 1, 1, "abc", 3, 0, ref a, 0);
            NativeGdi32Api.SelectObject(hdc, res);
            gf.Dispose();
            g.ReleaseHdc(hdc);
            e.Graphics.DrawImage(bp, e.Bounds.Left, e.Bounds.Top);

            //	e.Graphics.DrawString ("abc",new Font (text,10f),SystemBrushes.Window,3,0);
        }

        public override bool GetPaintValueSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }


    public class FontEnum
    {
        private Hashtable Fonts = null;


        public FontEnum()
        {
        }

        public ICollection EnumFonts()
        {
            Bitmap bmp = new Bitmap(10, 10);
            Graphics g = Graphics.FromImage(bmp);

            IntPtr hDC = g.GetHdc();
            Fonts = new Hashtable();
            LogFont lf = new LogFont();
            lf.lfCharSet = 1;
            FONTENUMPROC callback = new FONTENUMPROC(this.CallbackFunc);
            NativeGdi32Api.EnumFontFamiliesEx(hDC, lf, callback, 0, 0);

            g.ReleaseHdc(hDC);
            g.Dispose();
            bmp.Dispose();
            return Fonts.Keys;
        }

        private int CallbackFunc(ENUMLOGFONTEX f, int a, int b, int LParam)
        {
            Fonts[f.elfLogFont.lfFaceName] = "abc";
            return 1;
        }

    }

}