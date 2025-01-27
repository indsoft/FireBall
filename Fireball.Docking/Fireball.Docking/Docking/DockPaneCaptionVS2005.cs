#region Fireball License
//    Copyright (C) 2005  Sebastian Faltoni sebastian{at}dotnetfireball{dot}net
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
#region Original License
// *****************************************************************************
// 
//  Copyright 2004, Weifen Luo
//  All rights reserved. The software and associated documentation 
//  supplied hereunder are the proprietary information of Weifen Luo
//  and are supplied subject to licence terms.
// 
//  WinFormsUI Library Version 1.0
// *****************************************************************************
#endregion

using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace Fireball.Docking
{
	/// <include file='CodeDoc/DockPaneCaptionVS2003.xml' path='//CodeDoc/Class[@name="DockPaneCaptionVS2003"]/ClassDef/*'/>
	[ToolboxItem(false)]
	public class DockPaneCaptionVS2005 : DockPaneCaptionBase
	{
		#region consts
		private const int _TextGapTop = 2;
		private const int _TextGapBottom = 0;
		private const int _TextGapLeft = 3;
		private const int _TextGapRight = 3;
		private const int _ButtonGapTop = 2;
		private const int _ButtonGapBottom = 1;
		private const int _ButtonGapBetween = 1;
		private const int _ButtonGapLeft = 1;
		private const int _ButtonGapRight = 2;
		private const string _ResourceImageCloseEnabled = "DockPaneCaption.CloseEnabled.bmp";
		private const string _ResourceImageCloseDisabled = "DockPaneCaption.CloseDisabled.bmp";
		private const string _ResourceImageAutoHideYes = "DockPaneCaption.AutoHideYes.bmp";
		private const string _ResourceImageAutoHideNo = "DockPaneCaption.AutoHideNo.bmp";
		private const string _ResourceToolTipClose = "DockPaneCaption.ToolTipClose";
		private const string _ResourceToolTipAutoHide = "DockPaneCaption.ToolTipAutoHide";
		#endregion

		private InertButton m_buttonClose;
		private InertButton m_buttonAutoHide;

		/// <include file='CodeDoc/DockPaneCaptionVS2003.xml' path='//CodeDoc/Class[@name="DockPaneCaptionVS2003"]/Construct[@name="(DockPane)"]/*'/>
        protected internal DockPaneCaptionVS2005(DockPane pane)
            : base(pane)
		{
			SuspendLayout();

			Font = SystemInformation.MenuFont;

			m_buttonClose = new InertButton(ImageCloseEnabled, ImageCloseDisabled);
			m_buttonAutoHide = new InertButton();

			m_buttonClose.ToolTipText = ToolTipClose;
			m_buttonClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			m_buttonClose.Click += new EventHandler(this.Close_Click);

			m_buttonAutoHide.ToolTipText = ToolTipAutoHide;
			m_buttonAutoHide.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			m_buttonAutoHide.Click += new EventHandler(AutoHide_Click);

			Controls.AddRange(new Control[]	{	m_buttonClose, m_buttonAutoHide });

			ResumeLayout();
		}

		#region Customizable Properties
		/// <include file='CodeDoc/DockPaneCaptionVS2003.xml' path='//CodeDoc/Class[@name="DockPaneCaptionVS2003"]/Property[@name="TextGapTop"]/*'/>
		protected virtual int TextGapTop
		{
			get	{	return _TextGapTop;	}
		}

		/// <include file='CodeDoc/DockPaneCaptionVS2003.xml' path='//CodeDoc/Class[@name="DockPaneCaptionVS2003"]/Property[@name="TextGapBottom"]/*'/>
		protected virtual int TextGapBottom
		{
			get	{	return _TextGapBottom;	}
		}

		/// <include file='CodeDoc/DockPaneCaptionVS2003.xml' path='//CodeDoc/Class[@name="DockPaneCaptionVS2003"]/Property[@name="TextGapLeft"]/*'/>
		protected virtual int TextGapLeft
		{
			get	{	return _TextGapLeft;	}
		}

		/// <include file='CodeDoc/DockPaneCaptionVS2003.xml' path='//CodeDoc/Class[@name="DockPaneCaptionVS2003"]/Property[@name="TextGapRight"]/*'/>
		protected virtual int TextGapRight
		{
			get	{	return _TextGapRight;	}
		}

		/// <include file='CodeDoc/DockPaneCaptionVS2003.xml' path='//CodeDoc/Class[@name="DockPaneCaptionVS2003"]/Property[@name="ButtonGapTop"]/*'/>
		protected virtual int ButtonGapTop
		{
			get	{	return _ButtonGapTop;	}
		}

		/// <include file='CodeDoc/DockPaneCaptionVS2003.xml' path='//CodeDoc/Class[@name="DockPaneCaptionVS2003"]/Property[@name="ButtonGapBottom"]/*'/>
		protected virtual int ButtonGapBottom
		{
			get	{	return _ButtonGapBottom;	}
		}

		/// <include file='CodeDoc/DockPaneCaptionVS2003.xml' path='//CodeDoc/Class[@name="DockPaneCaptionVS2003"]/Property[@name="ButtonGapLeft"]/*'/>
		protected virtual int ButtonGapLeft
		{
			get	{	return _ButtonGapLeft;	}
		}

		/// <include file='CodeDoc/DockPaneCaptionVS2003.xml' path='//CodeDoc/Class[@name="DockPaneCaptionVS2003"]/Property[@name="ButtonGapRight"]/*'/>
		protected virtual int ButtonGapRight
		{
			get	{	return _ButtonGapRight;	}
		}

		/// <include file='CodeDoc/DockPaneCaptionVS2003.xml' path='//CodeDoc/Class[@name="DockPaneCaptionVS2003"]/Property[@name="ButtonGapBetween"]/*'/>
		protected virtual int ButtonGapBetween
		{
			get	{	return _ButtonGapBetween;	}
		}

		private static Image _imageCloseEnabled = null;
		/// <include file='CodeDoc/DockPaneCaptionVS2003.xml' path='//CodeDoc/Class[@name="DockPaneCaptionVS2003"]/Property[@name="ImageCloseEnabled"]/*'/>
		protected virtual Image ImageCloseEnabled
		{
			get
			{	
				if (_imageCloseEnabled == null)
					_imageCloseEnabled = ResourceHelper.LoadBitmap(_ResourceImageCloseEnabled);
				return _imageCloseEnabled;
			}
		}

		private static Image _imageCloseDisabled = null;
		/// <include file='CodeDoc/DockPaneCaptionVS2003.xml' path='//CodeDoc/Class[@name="DockPaneCaptionVS2003"]/Property[@name="ImageCloseDisabled"]/*'/>
		protected virtual Image ImageCloseDisabled
		{
			get
			{	
				if (_imageCloseDisabled == null)
					_imageCloseDisabled = ResourceHelper.LoadBitmap(_ResourceImageCloseDisabled);
				return _imageCloseDisabled;
			}
		}

		private static Image _imageAutoHideYes = null;
		/// <include file='CodeDoc/DockPaneCaptionVS2003.xml' path='//CodeDoc/Class[@name="DockPaneCaptionVS2003"]/Property[@name="ImageAutoHideYes"]/*'/>
		protected virtual Image ImageAutoHideYes
		{
			get
			{	
				if (_imageAutoHideYes == null)
					_imageAutoHideYes = ResourceHelper.LoadBitmap(_ResourceImageAutoHideYes);
				return _imageAutoHideYes;
			}
		}

		private static Image _imageAutoHideNo = null;
		/// <include file='CodeDoc/DockPaneCaptionVS2003.xml' path='//CodeDoc/Class[@name="DockPaneCaptionVS2003"]/Property[@name="ImageAutoHideNo"]/*'/>
		protected virtual Image ImageAutoHideNo
		{
			get
			{	
				if (_imageAutoHideNo == null)
					_imageAutoHideNo = ResourceHelper.LoadBitmap(_ResourceImageAutoHideNo);
				return _imageAutoHideNo;
			}
		}

		private static string _toolTipClose = null;
		/// <include file='CodeDoc/DockPaneCaptionVS2003.xml' path='//CodeDoc/Class[@name="DockPaneCaptionVS2003"]/Property[@name="ToolTipClose"]/*'/>
		protected virtual string ToolTipClose
		{
			get
			{	
				if (_toolTipClose == null)
					_toolTipClose = ResourceHelper.GetString(_ResourceToolTipClose);
				return _toolTipClose;
			}
		}

		private static string _toolTipAutoHide = null;
		/// <include file='CodeDoc/DockPaneCaptionVS2003.xml' path='//CodeDoc/Class[@name="DockPaneCaptionVS2003"]/Property[@name="ToolTipAutoHide"]/*'/>
		protected virtual string ToolTipAutoHide
		{
			get
			{	
				if (_toolTipAutoHide == null)
					_toolTipAutoHide = ResourceHelper.GetString(_ResourceToolTipAutoHide);
				return _toolTipAutoHide;
			}
		}

		/// <include file='CodeDoc/DockPaneCaptionVS2003.xml' path='//CodeDoc/Class[@name="DockPaneCaptionVS2003"]/Property[@name="ActiveBackColor"]/*'/>
		protected virtual Brush ActiveBackBrush
		{
			get	{

                Fireball.Docking.Drawing.ColorHLS hls = new Fireball.Docking.Drawing.ColorHLS(SystemColors.InactiveCaption);

                hls.Lighten(0.02f);

                Rectangle rect = this.ClientRectangle;

                if(rect == Rectangle.Empty)
                    rect = new Rectangle(0,0,1,1);

                LinearGradientBrush lnbr = new LinearGradientBrush(rect,
                 hls.Color,SystemColors.ActiveCaption, LinearGradientMode.Vertical);
                return lnbr;

            }
		}

		/// <include file='CodeDoc/DockPaneCaptionVS2003.xml' path='//CodeDoc/Class[@name="DockPaneCaptionVS2003"]/Property[@name="InactiveBackColor"]/*'/>
        protected virtual Brush InactiveBackBrush
		{
			get	{	return SystemBrushes.ControlDark;	}
		}

		/// <include file='CodeDoc/DockPaneCaptionVS2003.xml' path='//CodeDoc/Class[@name="DockPaneCaptionVS2003"]/Property[@name="ActiveTextColor"]/*'/>
		protected virtual Color ActiveTextColor
		{
			get	{	return SystemColors.ActiveCaptionText;	}
		}

		/// <include file='CodeDoc/DockPaneCaptionVS2003.xml' path='//CodeDoc/Class[@name="DockPaneCaptionVS2003"]/Property[@name="InactiveTextColor"]/*'/>
		protected virtual Color InactiveTextColor
		{
			get	{	return SystemColors.ControlText;	}
		}

		/// <include file='CodeDoc/DockPaneCaptionVS2003.xml' path='//CodeDoc/Class[@name="DockPaneCaptionVS2003"]/Property[@name="InactiveBorderColor"]/*'/>
		protected virtual Color InactiveBorderColor
		{
			get	{	return SystemColors.GrayText; }
		}

		/// <include file='CodeDoc/DockPaneCaptionVS2003.xml' path='//CodeDoc/Class[@name="DockPaneCaptionVS2003"]/Property[@name="ActiveButtonBorderColor"]/*'/>
		protected virtual Color ActiveButtonBorderColor
		{
			get	{	return ActiveTextColor;	}
		}

		/// <include file='CodeDoc/DockPaneCaptionVS2003.xml' path='//CodeDoc/Class[@name="DockPaneCaptionVS2003"]/Property[@name="InactiveButtonBorderColor"]/*'/>
		protected virtual Color InactiveButtonBorderColor
		{
			get	{	return Color.Empty;	}
		}

		private static StringFormat _textStringFormat = null;
		/// <include file='CodeDoc/DockPaneCaptionVS2003.xml' path='//CodeDoc/Class[@name="DockPaneCaptionVS2003"]/Property[@name="TextStringFormat"]/*'/>
		protected virtual StringFormat TextStringFormat
		{
			get
			{	
				if (_textStringFormat == null)
				{
					_textStringFormat = new StringFormat();
					_textStringFormat.Trimming = StringTrimming.EllipsisCharacter;
					_textStringFormat.LineAlignment = StringAlignment.Center;
					_textStringFormat.FormatFlags = StringFormatFlags.NoWrap;
				}
				return _textStringFormat;
			}
		}

		#endregion

		/// <exclude/>
		protected internal override int MeasureHeight()
		{
			int height = Font.Height + TextGapTop + TextGapBottom;

			if (height < ImageCloseEnabled.Height + ButtonGapTop + ButtonGapBottom)
				height = ImageCloseEnabled.Height + ButtonGapTop + ButtonGapBottom;

			return height;
		}

		/// <exclude/>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);
            //e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			DrawCaption(e.Graphics);
		}

		private void DrawCaption(Graphics g)
		{
			//BackColor = DockPane.IsActivated ? ActiveBackColor : InactiveBackColor;

            if (DockPane.IsActivated)
                g.FillRectangle(this.ActiveBackBrush, this.ClientRectangle);
            else
                g.FillRectangle(this.InactiveBackBrush, this.ClientRectangle);

			Rectangle rectCaption = ClientRectangle;

			if (!DockPane.IsActivated)
			{
				using (Pen pen = new Pen(InactiveBorderColor))
				{
					g.DrawLine(pen, rectCaption.X + 1, rectCaption.Y, rectCaption.X + rectCaption.Width - 2, rectCaption.Y);
					g.DrawLine(pen, rectCaption.X + 1, rectCaption.Y + rectCaption.Height - 1, rectCaption.X + rectCaption.Width - 2, rectCaption.Y + rectCaption.Height - 1);
					g.DrawLine(pen, rectCaption.X, rectCaption.Y + 1, rectCaption.X, rectCaption.Y + rectCaption.Height - 2);
					g.DrawLine(pen, rectCaption.X + rectCaption.Width - 1, rectCaption.Y + 1, rectCaption.X + rectCaption.Width - 1, rectCaption.Y + rectCaption.Height - 2);
				}
			}

			m_buttonClose.ForeColor = m_buttonAutoHide.ForeColor = (DockPane.IsActivated ? ActiveTextColor : InactiveTextColor);
			m_buttonClose.BorderColor = m_buttonAutoHide.BorderColor = (DockPane.IsActivated ? ActiveButtonBorderColor : InactiveButtonBorderColor);

			Rectangle rectCaptionText = rectCaption;
			rectCaptionText.X += TextGapLeft;
			if (ShouldShowCloseButton && ShouldShowAutoHideButton)
				rectCaptionText.Width = rectCaption.Width - ButtonGapRight
					- ButtonGapLeft	- TextGapLeft - TextGapRight -
					(m_buttonAutoHide.Width + ButtonGapBetween + m_buttonClose.Width);
			else if (ShouldShowCloseButton || ShouldShowAutoHideButton)
				rectCaptionText.Width = rectCaption.Width - ButtonGapRight
					- ButtonGapLeft	- TextGapLeft - TextGapRight - m_buttonClose.Width;
			else
				rectCaptionText.Width = rectCaption.Width - TextGapLeft - TextGapRight;
			rectCaptionText.Y += TextGapTop;
			rectCaptionText.Height -= TextGapTop + TextGapBottom;

			using (Brush brush = new SolidBrush(DockPane.IsActivated ? ActiveTextColor : InactiveTextColor))
			{
				g.DrawString(DockPane.CaptionText, Font, brush, rectCaptionText, TextStringFormat);
			}
		}

		/// <exclude/>
		protected override void OnLayout(LayoutEventArgs levent)
		{
			SetButtonsPosition();
			base.OnLayout (levent);
		}

		/// <exclude/>
		protected override void OnRefreshChanges()
		{
			SetButtons();
			Invalidate();
		}

		private bool ShouldShowCloseButton
		{
			get	{	return (DockPane.ActiveContent != null)? DockPane.ActiveContent.DockHandler.CloseButton : false;	}
		}

		private bool ShouldShowAutoHideButton
		{
			get	{	return !DockPane.IsFloat;	}
		}

		private void SetButtons()
		{
			m_buttonClose.Visible = ShouldShowCloseButton;
			m_buttonAutoHide.Visible = ShouldShowAutoHideButton;
			m_buttonAutoHide.ImageEnabled = DockPane.IsAutoHide ? ImageAutoHideYes : ImageAutoHideNo;
			
			SetButtonsPosition();
		}

		private void SetButtonsPosition()
		{
			// set the size and location for close and auto-hide buttons
			Rectangle rectCaption = ClientRectangle;
			int buttonWidth = ImageCloseEnabled.Width;
			int buttonHeight = ImageCloseEnabled.Height;
			int height = rectCaption.Height - ButtonGapTop - ButtonGapBottom;
			if (buttonHeight < height)
			{
				buttonWidth = buttonWidth * (height / buttonHeight);
				buttonHeight = height;
			}
			m_buttonClose.SuspendLayout();
			m_buttonAutoHide.SuspendLayout();
			Size buttonSize = new Size(buttonWidth, buttonHeight);
			m_buttonClose.Size = m_buttonAutoHide.Size = buttonSize;
			int x = rectCaption.X + rectCaption.Width - 1 - ButtonGapRight - m_buttonClose.Width;
			int y = rectCaption.Y + ButtonGapTop;
			Point point = m_buttonClose.Location = new Point(x, y);
			if (ShouldShowCloseButton)
				point.Offset(-(m_buttonAutoHide.Width + ButtonGapBetween), 0);
			m_buttonAutoHide.Location = point;
			m_buttonClose.ResumeLayout();
			m_buttonAutoHide.ResumeLayout();
		}

		private void Close_Click(object sender, EventArgs e)
		{
			DockPane.CloseActiveContent();
		}

		private void AutoHide_Click(object sender, EventArgs e)
		{
			DockPane.DockState = DockHelper.ToggleAutoHideState(DockPane.DockState);
			if (!DockPane.IsAutoHide)
				DockPane.Activate();
		}
	}
}
