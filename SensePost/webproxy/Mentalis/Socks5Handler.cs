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
using System.Text;

using Org.Mentalis.Proxy;
using Org.Mentalis.Proxy.Socks.Authentication;
using Org.Mentalis.Security.Ssl;


namespace Org.Mentalis.Proxy.Socks {

///<summary>Implements the SOCKS5 protocol.</summary>
internal sealed class Socks5Handler : SocksHandler {
	///<summary>Initializes a new instance of the Socks5Handler class.</summary>
	///<param name="ClientConnection">The connection with the client.</param>
	///<param name="Callback">The method to call when the SOCKS negotiation is complete.</param>
	///<param name="AuthList">The authentication list to use when clients connect.</param>
	///<exception cref="ArgumentNullException"><c>Callback</c> is null.</exception>
	///<remarks>If the AuthList parameter is null, no authentication will be required when a client connects to the proxy server.</remarks>
	public Socks5Handler(SecureSocket ClientConnection, NegotiationCompleteDelegate Callback, AuthenticationList AuthList) : base(ClientConnection, Callback) {
		this.AuthList = AuthList;
	}
	///<summary>Initializes a new instance of the Socks5Handler class.</summary>
	///<param name="ClientConnection">The connection with the client.</param>
	///<param name="Callback">The method to call when the SOCKS negotiation is complete.</param>
	///<exception cref="ArgumentNullException"><c>Callback</c> is null.</exception>
	public Socks5Handler(SecureSocket ClientConnection, NegotiationCompleteDelegate Callback) : this(ClientConnection, Callback, null) {}
	///<summary>Checks whether a specific request is a valid SOCKS request or not.</summary>
	///<param name="Request">The request array to check.</param>
	///<returns>True is the specified request is valid, false otherwise</returns>
	protected override bool IsValidRequest(byte [] Request) {
		try {
			return (Request.Length == Request[0] + 1);
		} catch {
			return false;
		}
	}
	///<summary>Processes a SOCKS request from a client and selects an authentication method.</summary>
	///<param name="Request">The request to process.</param>
	protected override void ProcessRequest(byte [] Request) {
		try {
			byte Ret = 255;
			for (int Cnt = 1; Cnt < Request.Length; Cnt++) {
				if (Request[Cnt] == 0 && AuthList == null) { //0 = No authentication
					Ret = 0;
					AuthMethod = new AuthNone();
					break;
				} else if (Request[Cnt] == 2 && AuthList != null) { //2 = user/pass
					Ret = 2;
					AuthMethod = new AuthUserPass(AuthList);
					if (AuthList != null)
						break;
				}
			}
			Connection.BeginSend(new byte[]{5, Ret}, 0, 2, SocketFlags.None, new AsyncCallback(this.OnAuthSent), Connection);
		} catch {
			Dispose(false);
		}
	}
	///<summary>Called when client has been notified of the selected authentication method.</summary>
	///<param name="ar">The result of the asynchronous operation.</param>
	private void OnAuthSent(IAsyncResult ar) {
		try {
			if (Connection.EndSend(ar) <= 0 || AuthMethod == null) {
				Dispose(false);
				return;
			}
			AuthMethod.StartAuthentication(Connection, new AuthenticationCompleteDelegate(this.OnAuthenticationComplete));
		} catch {
			Dispose(false);
		}
	}
	///<summary>Called when the authentication is complete.</summary>
	///<param name="Success">Indicates whether the authentication was successful ot not.</param>
	private void OnAuthenticationComplete(bool Success) {
		try {
			if (Success) {
				Bytes = null;
				Connection.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnRecvRequest), Connection);
			} else {
				Dispose(false);
			}
		} catch {
			Dispose(false);
		}
	}
	///<summary>Called when we received the request of the client.</summary>
	///<param name="ar">The result of the asynchronous operation.</param>
	private void OnRecvRequest(IAsyncResult ar) {
		try {
			int Ret = Connection.EndReceive(ar);
			if (Ret <= 0) {
				Dispose(false);
				return;
			}
			AddBytes(Buffer, Ret);
			if (IsValidQuery(Bytes))
				ProcessQuery(Bytes);
			else
				Connection.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnRecvRequest), Connection);
		} catch {
			Dispose(false);
		}
	}
	///<summary>Checks whether a specified query is a valid query or not.</summary>
	///<param name="Query">The query to check.</param>
	///<returns>True if the query is valid, false otherwise.</returns>
	private bool IsValidQuery(byte [] Query) {
		try {
			switch(Query[3]) {
				case 1: //IPv4 address
					return (Query.Length == 10);
				case 3: //Domain name
					return (Query.Length == Query[4] + 7);
				case 4: //IPv6 address
					//Not supported
					Dispose(8);
					return false;
				default:
					Dispose(false);
					return false;
			}
		} catch {
			return false;
		}
	}
	///<summary>Processes a received query.</summary>
	///<param name="Query">The query to process.</param>
	private void ProcessQuery(byte [] Query) {
		try {
			switch(Query[1]) {
				case 1: //CONNECT
					IPAddress RemoteIP = null;
					int RemotePort = 0;
					if (Query[3] == 1) {
						RemoteIP = IPAddress.Parse(Query[4].ToString() + "." + Query[5].ToString() + "." + Query[6].ToString() + "." + Query[7].ToString());
						RemotePort = Query[8] * 256 + Query[9];
					} else if( Query[3] == 3) {
						RemoteIP = Dns.Resolve(Encoding.ASCII.GetString(Query, 5, Query[4])).AddressList[0];
						RemotePort = Query[4] + 5;
						RemotePort = Query[RemotePort] * 256 + Query[RemotePort + 1];
					}
					RemoteConnection = new SecureSocket(RemoteIP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
					RemoteConnection.BeginConnect(new IPEndPoint(RemoteIP, RemotePort), new AsyncCallback(this.OnConnected), RemoteConnection);
					break;
				case 2: //BIND
					byte [] Reply = new byte[10];
					long LocalIP = Listener.GetLocalExternalIP().Address;
					AcceptSocket = new SecureSocket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
					AcceptSocket.Bind(new IPEndPoint(IPAddress.Any, 0));
					AcceptSocket.Listen(50);
					Reply[0] = 5;  //Version 5
					Reply[1] = 0;  //Everything is ok :)
					Reply[2] = 0;  //Reserved
					Reply[3] = 1;  //We're going to send a IPv4 address
					Reply[4] = (byte)(Math.Floor((LocalIP % 256)));  //IP Address/1
					Reply[5] = (byte)(Math.Floor((LocalIP % 65536) / 256));  //IP Address/2
					Reply[6] = (byte)(Math.Floor((LocalIP % 16777216) / 65536));  //IP Address/3
					Reply[7] = (byte)(Math.Floor(LocalIP / 16777216));  //IP Address/4
					Reply[8] = (byte)(Math.Floor(((IPEndPoint)AcceptSocket.LocalEndPoint).Port / 256));  //Port/1
					Reply[9] = (byte)(((IPEndPoint)AcceptSocket.LocalEndPoint).Port % 256);  //Port/2
					Connection.BeginSend(Reply, 0, Reply.Length, SocketFlags.None, new AsyncCallback(this.OnStartAccept), Connection);
					break;
				case 3: //ASSOCIATE
					//ASSOCIATE is not implemented (yet?)
					Dispose(7);
					break;
				default:
					Dispose(7);
					break;
			}
		} catch {
			Dispose(1);
		}
	}
	///<summary>Called when we're successfully connected to the remote host.</summary>
	///<param name="ar">The result of the asynchronous operation.</param>
	private void OnConnected(IAsyncResult ar) {
		try {
			RemoteConnection.EndConnect(ar);
			Dispose(0);
		} catch {
			Dispose(1);
		}
	}
	///<summary>Called when there's an incoming connection in the AcceptSocket queue.</summary>
	///<param name="ar">The result of the asynchronous operation.</param>
	protected override void OnAccept(IAsyncResult ar) {
		try {
			RemoteConnection = (SecureSocket)AcceptSocket.EndAccept(ar);
			AcceptSocket.Close();
			AcceptSocket = null;
			Dispose(0);
		} catch {
			Dispose(1);
		}
	}
	///<summary>Sends a reply to the client connection and disposes it afterwards.</summary>
	///<param name="Value">A byte that contains the reply code to send to the client.</param>
	protected override void Dispose(byte Value) {
		byte [] ToSend;
		try {
			ToSend = new byte[]{5, Value, 0, 1,
						(byte)(((IPEndPoint)RemoteConnection.LocalEndPoint).Address.Address % 256),
						(byte)(Math.Floor((((IPEndPoint)RemoteConnection.LocalEndPoint).Address.Address % 65536) / 256)),
						(byte)(Math.Floor((((IPEndPoint)RemoteConnection.LocalEndPoint).Address.Address % 16777216) / 65536)),
						(byte)(Math.Floor(((IPEndPoint)RemoteConnection.LocalEndPoint).Address.Address / 16777216)),
						(byte)(Math.Floor(((IPEndPoint)RemoteConnection.LocalEndPoint).Port / 256)),
						(byte)(((IPEndPoint)RemoteConnection.LocalEndPoint).Port % 256)};
		} catch {
			ToSend = new byte[] {5, 1, 0, 1, 0, 0, 0, 0, 0, 0};
		}
		try {
			Connection.BeginSend(ToSend, 0, ToSend.Length, SocketFlags.None, (AsyncCallback)(ToSend[1] == 0 ? new AsyncCallback(this.OnDisposeGood) : new AsyncCallback(this.OnDisposeBad)), Connection);
		} catch {
			Dispose(false);
		}
	}
	///<summary>Gets or sets the the AuthBase object to use when trying to authenticate the SOCKS client.</summary>
	///<value>The AuthBase object to use when trying to authenticate the SOCKS client.</value>
	///<exception cref="ArgumentNullException">The specified value is null.</exception>
	private AuthBase AuthMethod {
		get {
			return m_AuthMethod;
		}
		set {
			if (value == null)
				throw new ArgumentNullException();
			m_AuthMethod = value;
		}
	}
	///<summary>Gets or sets the AuthenticationList object to use when trying to authenticate the SOCKS client.</summary>
	///<value>The AuthenticationList object to use when trying to authenticate the SOCKS client.</value>
	private AuthenticationList AuthList {
		get {
			return m_AuthList;
		}
		set {
			m_AuthList = value;
		}
	}
	// private variables
	/// <summary>Holds the value of the AuthList property.</summary>
	private AuthenticationList m_AuthList;
	/// <summary>Holds the value of the AuthMethod property.</summary>
	private AuthBase m_AuthMethod;
}

}
