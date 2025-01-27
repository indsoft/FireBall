
//    Copyright (C) 2005  Sebastian Faltoni <sebastian@dotnetfireball.net>
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

using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Fireball.Timers
{

	#region WeakTimer

	[ToolboxItem(true)]
	public class WeakTimer : Component
	{
		private int _TickCount = 0;

		#region public property Interval

		//Created By RogerJ - 28-03-2003

		//member variable
		private int _Interval = 100;

		public int Interval
		{
			get { return _Interval; }

			set { _Interval = value; }
		}

		#endregion //END PROPERTY Interval

		#region public property Enabled

		//Created By RogerJ - 28-03-2003

		//member variable
		private bool _Enabled = true;

		public bool Enabled
		{
			get { return _Enabled; }

			set { _Enabled = value; }
		}

		#endregion //END PROPERTY Enabled

		public event EventHandler Tick = null;
		private Container components = null;

		public WeakTimer(IContainer container)
		{
			container.Add(this);
			InitializeComponent();
			Init();
		}

		public WeakTimer()
		{
			InitializeComponent();
			Init();
		}

		private void Init()
		{
			WeakTimerHelper.Attach(this);

		}

		public void DoTick(object s, EventArgs e)
		{
			_TickCount++;
			if (Enabled)
			{
				if (_TickCount >= (this.Interval/WeakTimerHelper.Speed))
				{
					_TickCount = 0;

					try
					{
						if (Tick != null)
							Tick(s, e);
					}
					catch
					{
					}
				}
			}
			//	GC.KeepAlive(Tick.Target);
			//	GC.KeepAlive(this);
		}

		~WeakTimer()
		{
			//Console.WriteLine ("killing WeakTimer");
			WeakTimerHelper.Detach(this);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}

		#endregion
	}

	#endregion

	#region Helper

	public class WeakTimerHelper
	{
		public static Timer _timer = GetTimer();
		private static ArrayList _listeners = new ArrayList();
		public const int Speed = 10;


		public static void Attach(WeakTimer t)
		{
			WeakReference wr = new WeakReference(t);

			_listeners.Add(wr);
		}

		public static void Detach(WeakTimer t)
		{
			for (int i = _listeners.Count - 1; i >= 0; i--)
			{
				WeakReference wr = (WeakReference) _listeners[i];
				try
				{
					if (wr.Target == t)
						_listeners.RemoveAt(i);
				}
				catch
				{
				}
			}
		}

		private static Timer GetTimer()
		{
			Timer t = new Timer();
			t.Interval = Speed;
			t.Tick += new EventHandler(DoTick);
			t.Enabled = true;
			return t;
		}

		private static void DoTick(object s, EventArgs e)
		{
			foreach (WeakReference wr in _listeners)
			{
				if (wr.IsAlive)
				{
					try
					{
						((WeakTimer) wr.Target).DoTick(null, EventArgs.Empty);
					}
					catch
					{
					}
				}
			}
		}

	}

	#endregion
}