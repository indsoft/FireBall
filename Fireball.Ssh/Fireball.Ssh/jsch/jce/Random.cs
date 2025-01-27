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

	public class Random : Fireball.Ssh.jsch.Random
	{
		private byte[] tmp=new byte[16];
		//private SecureRandom random;
		private System.Security.Cryptography.RNGCryptoServiceProvider rand;
		public Random()
		{
			//    random=null;
			//	  random = new SecureRandom();
			//	  
			//    try{ random=SecureRandom.getInstance("SHA1PRNG"); }
			//    catch(java.security.NoSuchAlgorithmException e){ 
			//      // System.out.println(e); 
			//
			//      // The following code is for IBM's JCE
			//      try{ random=SecureRandom.getInstance("IBMSecureRandom"); }
			//      catch(java.security.NoSuchAlgorithmException ee){ 
			//	System.out.println(ee); 
			//      }
			//    }
			rand = new System.Security.Cryptography.RNGCryptoServiceProvider();
		}
		static int times = 0;
		public void fill(byte[] foo, int start, int len)
		{
			try
			{
				if(len>tmp.Length){ tmp=new byte[len]; }
				//random.nextBytes(tmp);
				rand.GetBytes(tmp);
				Array.Copy(tmp, 0, foo, start, len);
			}
			catch(Exception e)
			{
				times++;
				Console.WriteLine(times+") Array.Copy(tmp={0}, 0, foo={1}, {2}, {3}", tmp.Length, foo.Length, start, len);
				//Console.WriteLine(e.StackTrace);
			}
		}
	}

}
