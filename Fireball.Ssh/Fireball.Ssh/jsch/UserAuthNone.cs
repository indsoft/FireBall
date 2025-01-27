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

using Fireball.Ssh.java;

namespace Fireball.Ssh.jsch
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

	class UserAuthNone : UserAuth
	{
		private String methods=null;
		private UserInfo userinfo;
		internal UserAuthNone(UserInfo userinfo)
		{
			this.userinfo=userinfo;
		}

		public override bool start(Session session)
		{
			base.start(session);
			//System.out.println("UserAuthNone: start");
			Packet packet=session.packet;
			Buffer buf=session.buf;
			String username=session.username;

			byte[] _username=null;
			try{ _username=Util.getBytesUTF8(username); }
			catch
			{//(java.io.UnsupportedEncodingException e){
				_username=Util.getBytes(username);
			}

			// send
			// byte      SSH_MSG_USERAUTH_REQUEST(50)
			// string    user name
			// string    service name ("ssh-connection")
			// string    "none"
			packet.reset();
			buf.putByte((byte)Session.SSH_MSG_USERAUTH_REQUEST);
			buf.putString(_username);
			buf.putString(Util.getBytes("ssh-connection"));
			buf.putString(Util.getBytes("none"));
			session.write(packet);

			loop:
				while(true)
				{
					// receive
					// byte      SSH_MSG_USERAUTH_SUCCESS(52)
					// string    service name
					buf=session.read(buf);
					//System.out.println("UserAuthNone: read: 52 ? "+    buf.buffer[5]);
					if(buf.buffer[5]==Session.SSH_MSG_USERAUTH_SUCCESS)
					{
						return true;
					}
					if(buf.buffer[5]==Session.SSH_MSG_USERAUTH_BANNER)
					{
						buf.getInt(); buf.getByte(); buf.getByte();
						byte[] _message=buf.getString();
						byte[] lang=buf.getString();
						String message=null;
						try{ message=Util.getStringUTF8(_message); }
						catch
						{//(java.io.UnsupportedEncodingException e){
							message=Util.getString(_message);
						}
						if(userinfo!=null)
						{
							userinfo.showMessage(message);
						}
						goto loop;
					}
					if(buf.buffer[5]==Session.SSH_MSG_USERAUTH_FAILURE)
					{
						buf.getInt(); buf.getByte(); buf.getByte(); 
						byte[] foo=buf.getString();
						int partial_success=buf.getByte();
						methods=Util.getString(foo);
						//System.out.println("UserAuthNONE: "+methods+
						//		   " partial_success:"+(partial_success!=0));
						//	if(partial_success!=0){
						//	  throw new JSchPartialAuthException(new String(foo));
						//	}
						break;
					}
					else
					{
						//      System.out.println("USERAUTH fail ("+buf.buffer[5]+")");
						throw new JSchException("USERAUTH fail ("+buf.buffer[5]+")");
					}
				}
			//throw new JSchException("USERAUTH fail");
			return false;
		}
		internal String getMethods()
		{
			return methods;
		}
	}

}
