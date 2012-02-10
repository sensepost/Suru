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

///<summary>Relays data between a remote host and a local client, using the SOCKS protocols.</summary>
///<remarks>This class implements the SOCKS4, SOCKS4a and SOCKS5 protocols.</remarks>
///<remarks>If the MustAuthenticate property is set, only SOCKS5 connections are allowed and the AuthList parameter of the constructor should not be null.</remarks>
public sealed class SocksClient : Client {
	///<summary>Initializes a new instance of the SocksClient class.</summary>
	///<param name="ClientSocket">The Socket connection between this proxy server and the local client.</param>
	///<param name="Destroyer">The method to be called when this SocksClient object disconnects from the local client and the remote server.</param>
	///<param name="AuthList">The list with valid username/password combinations.</param>
	///<remarks>If the AuthList is non-null, every client has to authenticate before he can use this proxy server to relay data. If it is null, the clients don't have to authenticate.</remarks>
	public SocksClient(SecureSocket ClientSocket, DestroyDelegate Destroyer, AuthenticationList AuthList) : base(ClientSocket, Destroyer) {
		this.AuthList = AuthList;
	}
	///<summary>Gets or sets the SOCKS handler to be used when communicating with the client.</summary>
	///<value>The SocksHandler to be used when communicating with the client.</value>
	internal SocksHandler Handler {
		get {
			return m_Handler;
		}
		set {
			if (value == null)
				throw new ArgumentNullException();
			m_Handler = value;
		}
	}
	///<summary>Gets or sets the SOCKS handler to be used when communicating with the client.</summary>
	///<value>The SocksHandler to be used when communicating with the client.</value>
	public bool MustAuthenticate {
		get {
			return m_MustAuthenticate;
		}
		set {
			m_MustAuthenticate = value;
		}
	}
	///<summary>Starts communication with the client.</summary>
	public override void StartHandshake() {
		try {
			ClientSocket.BeginReceive(Buffer, 0, 1, SocketFlags.None, new AsyncCallback(this.OnStartSocksProtocol), ClientSocket);
		} catch {
			Dispose();
		}
	}
	///<summary>Called when we have received some data from the client.</summary>
	///<param name="ar">The result of the asynchronous operation.</param>
	private void OnStartSocksProtocol(IAsyncResult ar) {
		int Ret;
		try {
			Ret = ClientSocket.EndReceive(ar);
			if (Ret <= 0) {
				Dispose();
				return;
			}
			if (Buffer[0] == 4) { //SOCKS4 Protocol
				if (MustAuthenticate) {
					Dispose();
					return;
				} else {
					Handler = new Socks4Handler(ClientSocket, new NegotiationCompleteDelegate(this.OnEndSocksProtocol));
				}
			} else if(Buffer[0] == 5) { //SOCKS5 Protocol
				if (MustAuthenticate && AuthList == null) {
					Dispose();
					return;
				}
				Handler = new Socks5Handler(ClientSocket, new NegotiationCompleteDelegate(this.OnEndSocksProtocol), AuthList);
			} else {
				Dispose();
				return;
			}
			Handler.StartNegotiating();
		} catch {
			Dispose();
		}
	}
	///<summary>Called when the SOCKS protocol has ended. We can no start relaying data, if the SOCKS authentication was successful.</summary>
	///<param name="Success">Specifies whether the SOCKS negotiation was successful or not.</param>
	///<param name="Remote">The connection with the remote server.</param>
	private void OnEndSocksProtocol(bool Success, SecureSocket Remote) {
		DestinationSocket = Remote;
		if (Success)
			StartRelay();
		else
			Dispose();
	}
	///<summary>Gets or sets the AuthenticationList to use when a computer tries to authenticate on the proxy server.</summary>
	///<value>An instance of the AuthenticationList class that contains all the valid username/password combinations.</value>
	private AuthenticationList AuthList {
		get {
			return m_AuthList;
		}
		set {
			m_AuthList = value;
		}
	}
	///<summary>Returns text information about this SocksClient object.</summary>
	///<returns>A string representing this SocksClient object.</returns>
	public override string ToString() {
		try {
			if (Handler != null)
				return Handler.Username + " (" + ((IPEndPoint)ClientSocket.LocalEndPoint).Address.ToString() +") connected to " + DestinationSocket.RemoteEndPoint.ToString();
			else
				return "SOCKS connection from " + ((IPEndPoint)ClientSocket.LocalEndPoint).Address.ToString();
		} catch {
			return "Incoming SOCKS connection";
		}
	}
	// private variables
	/// <summary>Holds the value of the AuthList property.</summary>
	private AuthenticationList m_AuthList;
	/// <summary>Holds the value of the MustAuthenticate property.</summary>
	private bool m_MustAuthenticate = false;
	/// <summary>Holds the value of the Handler property.</summary>
	private SocksHandler m_Handler;
}

}
