//ORIGINAL LGPL SOURCE CODE FINDED ON COMPONA LGPL SOURCE CODE

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Fireball.Windows.Forms
{
	/// <summary>
	/// Summary description for SplitViewChildControl.
	/// </summary>
	public class SplitViewChildWidget : Widget
	{
		public SplitViewThumbControl LeftThumb;
		public SplitViewThumbControl TopThumb;

		protected HScrollBar hScroll;
		protected VScrollBar vScroll;
		private Panel Filler;

		private Container components = null;

		public SplitViewChildWidget()
		{
			InitializeComponent();

//			Puzzle.Windows.NativeMethods.OpenThemeData (this.Handle,"EDIT");
//			Puzzle.Windows.NativeMethods.OpenThemeData (this.vScroll.Handle,"SCROLLBAR");
//			Puzzle.Windows.NativeMethods.OpenThemeData (this.hScroll.Handle,"SCROLLBAR");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof (SplitViewChildWidget));
			this.hScroll = new System.Windows.Forms.HScrollBar();
			this.vScroll = new System.Windows.Forms.VScrollBar();
			this.Filler = new System.Windows.Forms.Panel();
			this.TopThumb = new Fireball.Windows.Forms.SplitViewThumbControl();
			this.LeftThumb = new Fireball.Windows.Forms.SplitViewThumbControl();


			this.SuspendLayout();
			// 
			// hScroll
			// 
			this.hScroll.Location = new System.Drawing.Point(-4, 292);
			this.hScroll.Name = "hScroll";
			this.hScroll.Size = new System.Drawing.Size(440, 16);
			this.hScroll.TabIndex = 0;

			// 
			// vScroll
			// 
			this.vScroll.Location = new System.Drawing.Point(440, 0);
			this.vScroll.Maximum = 300;
			this.vScroll.Minimum = 0;
			this.vScroll.Name = "vScroll";
			this.vScroll.Size = new System.Drawing.Size(16, 360);
			this.vScroll.TabIndex = 1;
			// 
			// Filler
			// 
			this.Filler.BackColor = System.Drawing.SystemColors.Control;
			this.Filler.Location = new System.Drawing.Point(64, 260);
			this.Filler.Name = "Filler";
			this.Filler.Size = new System.Drawing.Size(20, 20);
			this.Filler.TabIndex = 3;
			// 
			// TopThumb
			// 
			this.TopThumb.BackColor = System.Drawing.SystemColors.Control;
			this.TopThumb.Cursor = System.Windows.Forms.Cursors.HSplit;
			this.TopThumb.Location = new System.Drawing.Point(101, 17);
			this.TopThumb.Name = "TopThumb";
			this.TopThumb.Size = new System.Drawing.Size(16, 8);
			this.TopThumb.TabIndex = 3;
			this.TopThumb.Visible = false;
			// 
			// LeftThumb
			// 
			this.LeftThumb.BackColor = System.Drawing.SystemColors.Control;
			this.LeftThumb.Cursor = System.Windows.Forms.Cursors.VSplit;
			this.LeftThumb.Location = new System.Drawing.Point(423, 17);
			this.LeftThumb.Name = "LeftThumb";
			this.LeftThumb.Size = new System.Drawing.Size(8, 16);
			this.LeftThumb.TabIndex = 3;
			this.LeftThumb.Visible = false;

			// 
			// SplitViewChildControl
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[]
				{
					this.TopThumb,
					this.LeftThumb,
					this.Filler,
					this.vScroll,
					this.hScroll
				});
			this.Name = "SplitViewChildControl";
			this.Size = new System.Drawing.Size(456, 376);
			this.ResumeLayout(false);

		}

		#endregion

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			DoResize();
		}


		protected Rectangle ClientArea
		{
			get
			{
				Rectangle r = this.ClientRectangle;
				r.Width -= vScroll.Width;
				r.Height -= hScroll.Height;
				return r;
			}
		}


		private void DoResize()
		{
			/*try
			{*/
                this.SuspendLayout();
				if (TopThumb == null)
					return;

				TopThumb.Width = SystemInformation.VerticalScrollBarWidth;
				LeftThumb.Height = SystemInformation.HorizontalScrollBarHeight;
				vScroll.Width = SystemInformation.VerticalScrollBarWidth;
				hScroll.Height = SystemInformation.HorizontalScrollBarHeight;

				if (TopThumbVisible)
				{
					vScroll.Top = TopThumb.Height;
					if (hScroll.Visible)
						vScroll.Height = this.ClientHeight - hScroll.Height - TopThumb.Height;
					else
						vScroll.Height = this.ClientHeight - TopThumb.Height;

				}
				else
				{
					if (hScroll.Visible)
						vScroll.Height = this.ClientHeight - hScroll.Height;
					else
						vScroll.Height = this.ClientHeight;

					vScroll.Top = 0;
				}

				if (LeftThumbVisible)
				{
					hScroll.Left = LeftThumb.Width;
					if (vScroll.Visible)
						hScroll.Width = this.ClientWidth - vScroll.Width - LeftThumb.Width;
					else
						hScroll.Width = this.ClientWidth - LeftThumb.Width;


				}
				else
				{
					if (vScroll.Visible)
						hScroll.Width = this.ClientWidth - vScroll.Width;
					else
						hScroll.Width = this.ClientWidth;

					hScroll.Left = 0;
				}


				vScroll.Left = this.ClientWidth - vScroll.Width;
				hScroll.Top = this.ClientHeight - hScroll.Height;

				LeftThumb.Left = 0;
				LeftThumb.Top = hScroll.Top;
				TopThumb.Left = vScroll.Left;
				TopThumb.Top = 0;


				Filler.Left = vScroll.Left;
				Filler.Top = hScroll.Top;
				Filler.Width = vScroll.Width;
				Filler.Height = hScroll.Height;
			/*}
			catch
			{
			}*/

                this.ResumeLayout(false);
		}

		/// <summary>
		/// Gets or Sets if the Left side thumb control is visible or not.
		/// </summary>
		public bool LeftThumbVisible
		{
			get { return this.LeftThumb.Visible; }
			set
			{
				this.LeftThumb.Visible = value;
				DoResize();
			}
		}

		/// <summary>
		/// Getd ot Sets if the Top thumb control is visible or not.
		/// </summary>
		public bool TopThumbVisible
		{
			get { return this.TopThumb.Visible; }
			set
			{
				this.TopThumb.Visible = value;
				DoResize();
			}
		}

		[Browsable(false)]
		public int VisibleClientHeight
		{
			get
			{
				if (hScroll.Visible)
					return this.ClientHeight - hScroll.Height;
				else
					return this.ClientHeight;
			}
		}

		[Browsable(false)]
		public int VisibleClientWidth
		{
			get
			{
				if (hScroll.Visible)
					return this.ClientWidth - vScroll.Width;
				else
					return this.ClientWidth;
			}
		}

		#region public property ScrollBars

		private ScrollBars _ScrollBars;

		public ScrollBars ScrollBars
		{
			get { return _ScrollBars; }

			set
			{
				_ScrollBars = value;
				if (ScrollBars == ScrollBars.Both)
				{
					hScroll.Visible = true;
					vScroll.Visible = true;
				}
				if (ScrollBars == ScrollBars.None)
				{
					hScroll.Visible = false;
					vScroll.Visible = false;
				}
				if (ScrollBars == ScrollBars.Horizontal)
				{
					hScroll.Visible = true;
					vScroll.Visible = false;
				}
				if (ScrollBars == ScrollBars.Vertical)
				{
					hScroll.Visible = false;
					vScroll.Visible = true;
				}
				Filler.Visible = hScroll.Visible & vScroll.Visible;

				if (vScroll.Visible && HasThumbs)
				{
					TopThumb.Height = 8;
				}
				else
				{
					TopThumb.Height = 0;
				}
				if (hScroll.Visible && HasThumbs)
				{
					LeftThumb.Width = 8;
				}
				else
				{
					LeftThumb.Width = 0;
				}


				DoResize();
				this.Refresh();
			}
		}


		private bool HasThumbs;

		public void HideThumbs()
		{
			TopThumb.Height = 0;
			LeftThumb.Width = 0;
			HasThumbs = false;
			DoResize();
		}

		public void ShowThumbs()
		{
			TopThumb.Height = 8;
			LeftThumb.Width = 8;
			HasThumbs = true;
			DoResize();
		}

		#endregion 
	}
}