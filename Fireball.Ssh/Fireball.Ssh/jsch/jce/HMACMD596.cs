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

namespace Fireball.Ssh.jsch.jce
{
	/* -*-mode:java; c-basic-offset:2; -*- */
	/*
	Copyright (c) 2002,2003,2004 ymnk, JCraft,Inc. All rights reserved.

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are met:

	  1. Redistributions of source code must retain the above copyright notice,
		 this list of conditions and the following disclaimer.

	  2. Redistributions in binary form must reproduce the above copyright 
		 notice, this list of conditions and the following disclaimer in 
		 the documentation and/or other materials provided with the distribution.

	  3. The names of the authors may not be used to endorse or promote products
		 derived from this software without specific prior written permission.

	THIS SOFTWARE IS PROVIDED ``AS IS'' AND ANY EXPRESSED OR IMPLIED WARRANTIES,
	INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
	FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL JCRAFT,
	INC. OR ANY CONTRIBUTORS TO THIS SOFTWARE BE LIABLE FOR ANY DIRECT, INDIRECT,
	INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
	LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA,
	OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
	LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
	NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE,
	EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
	*/

	public class HMACMD596 : MAC
	{
		private const String name="hmac-md5-96";
		private const int bsize=12;
		private Org.Mentalis.Security.Cryptography.HMAC mentalis_mac;
		private System.Security.Cryptography.CryptoStream cs;
		//private Mac mac;
		public int getBlockSize(){return bsize;}
		public void init(byte[] key) 
		{
			if(key.Length>16)
			{
				byte[] tmp=new byte[16];
				Array.Copy(key, 0, tmp, 0, 16);	  
				key=tmp;
			}
			//    SecretKeySpec skey=new SecretKeySpec(key, "HmacMD5");
			//    mac=Mac.getInstance("HmacMD5");
			//    mac.init(skey);
			mentalis_mac = new Org.Mentalis.Security.Cryptography.HMAC(new System.Security.Cryptography.MD5CryptoServiceProvider(), key);
			cs = new System.Security.Cryptography.CryptoStream( System.IO.Stream.Null, mentalis_mac, System.Security.Cryptography.CryptoStreamMode.Write);
		} 
		private byte[] tmp=new byte[4];
		public void update(int i)
		{
			tmp[0]=(byte)(i>>24);
			tmp[1]=(byte)(i>>16);
			tmp[2]=(byte)(i>>8);
			tmp[3]=(byte)i;
			update(tmp, 0, 4);
		}
		public void update(byte[] foo, int s, int l)
		{
			//mac.update(foo, s, l); 
			cs.Write(  foo, s, l);
		}
		private byte[] buf=new byte[12];
		public byte[] doFinal()
		{
			cs.Close();
			Array.Copy( mentalis_mac.Hash, 0, buf, 0, 12);
			return buf;
		}
		public String getName()
		{
			return name;
		}
	}
}
