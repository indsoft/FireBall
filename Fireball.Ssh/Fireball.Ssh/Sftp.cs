#region Licences
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
#endregion Licences

using System;
using Fireball.Ssh.jsch;

/* 
 * Sftp.cs
 * 
 * Copyright (c) 2006 Tamir Gal, http://www.tamirgal.com, All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 *  	1. Redistributions of source code must retain the above copyright notice,
 *		this list of conditions and the following disclaimer.
 *
 *	    2. Redistributions in binary form must reproduce the above copyright 
 *		notice, this list of conditions and the following disclaimer in 
 *		the documentation and/or other materials provided with the distribution.
 *
 *	    3. The names of the authors may not be used to endorse or promote products
 *		derived from this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED ``AS IS'' AND ANY EXPRESSED OR IMPLIED WARRANTIES,
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE AUTHOR
 *  *OR ANY CONTRIBUTORS TO THIS SOFTWARE BE LIABLE FOR ANY DIRECT, INDIRECT,
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
 * LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA,
 * OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
 * EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 * 
 **/

namespace Fireball.Ssh
{
	public class Sftp : SshTransferProtocolBase
	{
		MyProgressMonitor m_monitor;

		public Sftp(string sftpHost, string user, string password)
			: base(sftpHost, user, password)
		{
			Init();
		}

		public Sftp(string sftpHost, string user)
			: base(sftpHost, user)
		{
			Init();
		}

		private void Init()
		{
			m_monitor = new MyProgressMonitor(this);
		}

		protected override string ChannelType
		{
			get { return "sftp"; }
		}

		private ChannelSftp SftpChannel
		{
			get { return (ChannelSftp)m_channel; }
		}

		//Get

		public void Get(string fromFilePath)
		{
			Get(fromFilePath, ".");
		}

		public void Get(string[] fromFilePaths)
		{
			for (int i = 0; i < fromFilePaths.Length; i++)
			{
				Get(fromFilePaths[i]);
			}
		}

		public void Get(string[] fromFilePaths, string toDirPath)
		{
			for (int i = 0; i < fromFilePaths.Length; i++)
			{
				Get(fromFilePaths[i], toDirPath);
			}
		}

		public override void Get(string fromFilePath, string toFilePath)
		{
			SftpChannel.get(fromFilePath, toFilePath, m_monitor, ChannelSftp.OVERWRITE);
		}

		//Put

		public void Put(string fromFilePath)
		{
			Put(fromFilePath, ".");
		}

		public void Put(string[] fromFilePaths)
		{
			for (int i = 0; i < fromFilePaths.Length; i++)
			{
				Put(fromFilePaths[i]);
			}
		}

		public void Put(string[] fromFilePaths, string toDirPath)
		{
			for (int i = 0; i < fromFilePaths.Length; i++)
			{
				Put(fromFilePaths[i], toDirPath);
			}
		}

		public override void Put(string fromFilePath, string toFilePath)
		{
			SftpChannel.put(fromFilePath, toFilePath, m_monitor, ChannelSftp.OVERWRITE);
		}

		//MkDir

		public override  void Mkdir(string directory)
		{
			SftpChannel.mkdir(directory);
		}

		#region ProgressMonitor Implementation

		private class MyProgressMonitor : SftpProgressMonitor
		{
			private long transferred = 0;
			private long total = 0;
			private int elapsed = -1;
			private Sftp m_sftp;
			private string src;
			private string dest;

			System.Timers.Timer timer;

			public MyProgressMonitor(Sftp sftp)
			{
				m_sftp = sftp;
			}

			public override void init(int op, String src, String dest, long max)
			{
				this.src=src;
				this.dest=dest;
				this.elapsed = 0;
				this.total = max;
				timer = new System.Timers.Timer(1000);
				timer.Start();
				timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);

				string note;
				if (op.Equals(GET))
				{
					note = "Downloading " + System.IO.Path.GetFileName( src ) + "...";
				}
				else
				{
					note = "Uploading " + System.IO.Path.GetFileName( src ) + "...";
				}
				m_sftp.SendStartMessage(src, dest, (int)total, note);
			}
			public override bool count(long c)
			{
				this.transferred += c;
				string note = ("Transfering... [Elapsed time: " + elapsed + "]");
				m_sftp.SendProgressMessage(src, dest, (int)transferred, (int)total, note);
				return true;
			}
			public override void end()
			{
				timer.Stop();
				timer.Dispose();
				string note = ("Done in " + elapsed + " seconds!");
				m_sftp.SendEndMessage(src, dest, (int)transferred, (int)total, note);
				transferred = 0;
				total = 0;
				elapsed = -1;
				src=null;
				dest=null;
			}

			private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
			{
				this.elapsed++;
			}
		}

		#endregion ProgressMonitor Implementation
	}	
}
