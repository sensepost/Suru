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
using Org.Mentalis.Proxy.Socks.Authentication;
using Org.Mentalis.Security.Ssl;

namespace Org.Mentalis.Proxy.Socks {

///<summary>Listens on a specific port on the proxy server for incoming SOCKS4 and SOCKS5 requests.</summary>
///<remarks>This class also implements the SOCKS4a protocol.</remarks>
public sealed class SocksListener : Listener {
	///<summary>Initializes a new instance of the SocksListener class.</summary>
	///<param name="Port">The port to listen on.</param>
	///<remarks>The SocksListener will listen on all available network cards and it will not use an AuthenticationList.</remarks>
	public SocksListener(int Port) : this(IPAddress.Any, Port, null) {}
	///<summary>Initializes a new instance of the SocksListener class.</summary>
	///<param name="Port">The port to listen on.</param>
	///<param name="Address">The address to listen on. You can specify IPAddress.Any to listen on all installed network cards.</param>
	///<remarks>For the security of your server, try to avoid to listen on every network card (IPAddress.Any). Listening on a local IP address is usually sufficient and much more secure.</remarks>
	///<remarks>The SocksListener object will not use an AuthenticationList.</remarks>
	public SocksListener(IPAddress Address, int Port) : this(Address, Port, null) {}
	///<summary>Initializes a new instance of the SocksListener class.</summary>
	///<param name="Port">The port to listen on.</param>
	///<param name="AuthList">The list of valid login/password combinations. If you do not need password authentication, set this parameter to null.</param>
	///<remarks>The SocksListener will listen on all available network cards.</remarks>
	public SocksListener(int Port, AuthenticationList AuthList) : this(IPAddress.Any, Port, AuthList) {}
	///<summary>Initializes a new instance of the SocksListener class.</summary>
	///<param name="Port">The port to listen on.</param>
	///<param name="Address">The address to listen on. You can specify IPAddress.Any to listen on all installed network cards.</param>
	///<param name="AuthList">The list of valid login/password combinations. If you do not need password authentication, set this parameter to null.</param>
	///<remarks>For the security of your server, try to avoid to listen on every network card (IPAddress.Any). Listening on a local IP address is usually sufficient and much more secure.</remarks>
	public SocksListener(IPAddress Address, int Port, AuthenticationList AuthList) : base(Port, Address) {
		this.AuthList = AuthList;
	}
	///<summary>Called when there's an incoming client connection waiting to be accepted.</summary>
	///<param name="ar">The result of the asynchronous operation.</param>
	public override void OnAccept(IAsyncResult ar) {
		try {
			SecureSocket NewSocket = (SecureSocket)ListenSocket.EndAccept(ar);
			if (NewSocket != null) {
				SocksClient NewClient = new SocksClient(NewSocket, new DestroyDelegate(this.RemoveClient), AuthList);
				AddClient(NewClient);
				NewClient.StartHandshake();
			}
		} catch {}
		try {
			//Restart Listening
			ListenSocket.BeginAccept(new AsyncCallback(this.OnAccept), ListenSocket);
		} catch {
			Dispose();
		}
	}
	///<summary>Gets or sets the AuthenticationList to be used when a SOCKS5 client connects.</summary>
	///<value>An AuthenticationList that is to be used when a SOCKS5 client connects.</value>
	///<remarks>This value can be null.</remarks>
	private AuthenticationList AuthList {
		get {
			return m_AuthList;
		}
		set {
			m_AuthList = value;
		}
	}
	///<summary>Returns a string representation of this object.</summary>
	///<returns>A string with information about this object.</returns>
	public override string ToString() {
		return "SOCKS service on " + Address.ToString() + ":" + Port.ToString();
	}
	///<summary>Returns a string that holds all the construction information for this object.</summary>
	///<value>A string that holds all the construction information for this object.</value>
	public override string ConstructString {
		get {
			if (AuthList == null)
				return "host:" + Address.ToString() + ";int:" + Port.ToString()+ ";null";
			else
				return "host:" + Address.ToString() + ";int:" + Port.ToString()+ ";authlist";
		}
	}
	// private variables
	/// <summary>Holds the value of the AuthList property.</summary>
	private AuthenticationList m_AuthList;
}

}
