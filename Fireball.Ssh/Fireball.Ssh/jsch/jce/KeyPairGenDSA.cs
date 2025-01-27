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

	public class KeyPairGenDSA : Fireball.Ssh.jsch.KeyPairGenDSA 
	{
		byte[] x;  // private
		byte[] y;  // public
		byte[] p;
		byte[] q;
		byte[] g;

		public void init(int key_size)
		{
//			KeyPairGenerator keyGen = KeyPairGenerator.getInstance("DSA");
//			keyGen.initialize(key_size, new SecureRandom());
//			KeyPair pair = keyGen.generateKeyPair();
//			PublicKey pubKey=pair.getPublic();
//			PrivateKey prvKey=pair.getPrivate();

			System.Security.Cryptography.DSACryptoServiceProvider dsa = new System.Security.Cryptography.DSACryptoServiceProvider(key_size);
			System.Security.Cryptography.DSAParameters DSAKeyInfo = dsa.ExportParameters(true);

//			x=((DSAPrivateKey)prvKey).getX().toByteArray();
//			y=((DSAPublicKey)pubKey).getY().toByteArray();
//
//			DSAParams _params=((DSAKey)prvKey).getParams();
//			p=_params.getP().toByteArray();
//			q=_params.getQ().toByteArray();
//			g=_params.getG().toByteArray();

			x = DSAKeyInfo.X;
			y = DSAKeyInfo.Y;
			p = DSAKeyInfo.P;
			q = DSAKeyInfo.Q;
			g = DSAKeyInfo.G;
		}
		public byte[] getX(){return x;}
		public byte[] getY(){return y;}
		public byte[] getP(){return p;}
		public byte[] getQ(){return q;}
		public byte[] getG(){return g;}
	}

}
