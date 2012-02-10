/*
    Copyright © 2002, The KPD-Team
    All rights reserved.
    http://www.mentalis.org/

  Redistribution and use in source and binary forms, with or without
  modification, are permitted provided that the following conditions
  are met:

    - Redistributions of source code must retain the above copyright
       notice, this list of conditions and the following disclaimer. 

    - Neither the name of the KPD-Team, nor the names of its contributors
       may be used to endorse or promote products derived from this
       software without specific prior written permission. 

  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
  FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL
  THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
  INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
  (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
  SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
  HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
  STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
  ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
  OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Net;
using System.Net.Sockets;

using Org.Mentalis.Proxy;
using Org.Mentalis.Proxy.Socks;
using Org.Mentalis.Security.Ssl;

namespace Org.Mentalis.Proxy.Socks.Authentication {

///<summary>Defines the signature of the method to be called when the authentication is complete.</summary>
///<param name="Success">Specifies whether the authentication was successfull or not.</param>
internal delegate void AuthenticationCompleteDelegate(bool Success);

///<summary>Authenticates a user on a SOCKS5 server according to the implemented subprotocol.</summary>
///<remarks>This is an abstract class. The subprotocol that's used to authenticate a user is specified in the subclasses of this base class.</remarks>
internal abstract class AuthBase {
	///<summary>Initializes a new instance of the AuthBase class.</summary>
	public AuthBase() {}
	///<summary>Starts the authentication process.</summary>
	///<remarks>This abstract method must be implemented in the subclasses, according to the selected subprotocol.</remarks>
	///<param name="Connection">The connection with the SOCKS client.</param>
	///<param name="Callback">The method to call when the authentication is complete.</param>
	internal abstract void StartAuthentication(SecureSocket Connection, AuthenticationCompleteDelegate Callback);
	///<summary>Gets or sets the Socket connection between the proxy server and the SOCKS client.</summary>
	///<value>A Socket instance defining the connection between the proxy server and the local client.</value>
	protected SecureSocket Connection {
		get {
			return m_Connection;
		}
		set {
			if (value == null)
				throw new ArgumentNullException();
			m_Connection = value;
		}
	}
	///<summary>Gets a buffer that can be used to receive data from the client connection.</summary>
	///<value>An array of bytes that can be used to receive data from the client connection.</value>
	protected byte [] Buffer {
		get {
			return m_Buffer;
		}
	}
	///<summary>Gets or sets an array of bytes that can be used to store all received data.</summary>
	///<value>An array of bytes that can be used to store all received data.</value>
	protected byte [] Bytes {
		get {
			return m_Bytes;
		}
		set {
			m_Bytes = value;
		}
	}
	///<summary>Adds bytes to the array returned by the Bytes property.</summary>
	///<param name="NewBytes">The bytes to add.</param>
	///<param name="Cnt">The number of bytes to add.</param>
	protected void AddBytes(byte [] NewBytes, int Cnt) {
		if (Cnt <= 0 || NewBytes == null || Cnt > NewBytes.Length)
			return;
		if (Bytes == null) {
			Bytes = new byte[Cnt];
		} else {
			byte [] tmp = Bytes;
			Bytes = new byte[Bytes.Length  + Cnt];
			Array.Copy(tmp, 0, Bytes, 0, tmp.Length);
		}
		Array.Copy(NewBytes, 0, Bytes, Bytes.Length - Cnt, Cnt);
	}
	///<summary>The method to call when the authentication is complete.</summary>
	protected AuthenticationCompleteDelegate Callback;
	// private variables
	/// <summary>Holds the value of the Connection property.</summary>
	private SecureSocket m_Connection;
	/// <summary>Holds the value of the Buffer property.</summary>
	private byte [] m_Buffer = new byte[1024];
	/// <summary>Holds the value of the Bytes property.</summary>
	private byte [] m_Bytes;
}

}
