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

	public class SignatureDSA : Fireball.Ssh.jsch.SignatureDSA
	{

		//java.security.Signature signature;
		//  KeyFactory keyFactory;
		System.Security.Cryptography.DSAParameters DSAKeyInfo;
		System.Security.Cryptography.SHA1CryptoServiceProvider sha1;
		System.Security.Cryptography.CryptoStream cs;

		public void init() 
		{
			sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
			cs = new System.Security.Cryptography.CryptoStream(System.IO.Stream.Null, sha1, System.Security.Cryptography.CryptoStreamMode.Write);
		}     
		public void setPubKey(byte[] y, byte[] p, byte[] q, byte[] g) 
		{
			DSAKeyInfo.Y =  Util.stripLeadingZeros( y );
			DSAKeyInfo.P =  Util.stripLeadingZeros( p ) ;
			DSAKeyInfo.Q =  Util.stripLeadingZeros( q );
			DSAKeyInfo.G =	Util.stripLeadingZeros( g ) ;
		}
		public void setPrvKey(byte[] x, byte[] p, byte[] q, byte[] g)
		{
			DSAKeyInfo.X =  Util.stripLeadingZeros( x );
			DSAKeyInfo.P =  Util.stripLeadingZeros( p );
			DSAKeyInfo.Q =  Util.stripLeadingZeros( q );
			DSAKeyInfo.G =  Util.stripLeadingZeros( g );
		}

		//This method will probably won't work, we need to get rid of the ASN.1 format (Tamir)
		public byte[] sign() 
		{
			//byte[] sig=signature.sign();   
			cs.Close();
			System.Security.Cryptography.DSACryptoServiceProvider DSA = new System.Security.Cryptography.DSACryptoServiceProvider();
			DSA.ImportParameters(DSAKeyInfo);
			System.Security.Cryptography.DSASignatureFormatter DSAFormatter = new System.Security.Cryptography.DSASignatureFormatter(DSA);
			DSAFormatter.SetHashAlgorithm("SHA1");
	  
			byte[] sig =DSAFormatter.CreateSignature( sha1 );
			return sig;
		}
		public void update(byte[] foo) 
		{
			//signature.update(foo);
			cs.Write(  foo , 0, foo.Length);
		}
		public bool verify(byte[] sig)
		{			
			cs.Close();
			System.Security.Cryptography.DSACryptoServiceProvider DSA = new System.Security.Cryptography.DSACryptoServiceProvider();
			DSA.ImportParameters(DSAKeyInfo);
			System.Security.Cryptography.DSASignatureDeformatter DSADeformatter = new System.Security.Cryptography.DSASignatureDeformatter(DSA);
			DSADeformatter.SetHashAlgorithm("SHA1");

			long i=0;
			long j=0;
			byte[] tmp;

			//This makes sure sig is always 40 bytes?
			if(sig[0]==0 && sig[1]==0 && sig[2]==0)
			{
				long i1 = (sig[i++]<<24)&0xff000000;
				long i2 = (sig[i++]<<16)&0x00ff0000;
				long i3 = (sig[i++]<<8)&0x0000ff00;
				long i4 = (sig[i++])&0x000000ff;
				j = i1 | i2 | i3 | i4;

				i+=j;

				i1 = (sig[i++]<<24)&0xff000000;
				i2 = (sig[i++]<<16)&0x00ff0000;
				i3 = (sig[i++]<<8)&0x0000ff00;
				i4 = (sig[i++])&0x000000ff;
				j = i1 | i2 | i3 | i4;

				tmp=new byte[j]; 
				Array.Copy(sig, i, tmp, 0, j); sig=tmp;
			}
			bool res = DSADeformatter.VerifySignature(sha1, sig);
			return res;
		}
	}

}
