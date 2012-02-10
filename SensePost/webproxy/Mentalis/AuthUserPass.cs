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
using System.Text;
using System.Net;
using System.Net.Sockets;

using Org.Mentalis.Proxy;
using Org.Mentalis.Proxy.Socks;
using Org.Mentalis.Proxy.Socks.Authentication;
using Org.Mentalis.Security.Ssl;

namespace Org.Mentalis.Proxy.Socks.Authentication {

///<summary>Authenticates a user on a SOCKS5 server according to the username/password authentication subprotocol.</summary>
internal sealed class AuthUserPass : AuthBase {
	///<summary>Initializes a new instance of the AuthUserPass class.</summary>
	///<param name="AuthList">An AuthenticationList object that contains the list of all valid username/password combinations.</param>
	///<remarks>If the AuthList parameter is null, any username/password combination will be accepted.</remarks>
	public AuthUserPass(AuthenticationList AuthList) {
		this.AuthList = AuthList;
	}
	///<summary>Starts the authentication process.</summary>
	///<param name="Connection">The connection with the SOCKS client.</param>
	///<param name="Callback">The method to call when the authentication is complete.</param>
	internal override void StartAuthentication(SecureSocket Connection, AuthenticationCompleteDelegate Callback) {
		this.Connection = Connection;
		this.Callback = Callback;
		try {
			Bytes = null;
			Connection.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnRecvRequest), Connection);
		} catch {
			Callback(false);
		}
	}
	///<summary>Called when we have received the initial authentication data from the SOCKS client.</summary>
	///<param name="ar">The result of the asynchronous operation.</param>
	private void OnRecvRequest(IAsyncResult ar) {
		try {
			int Ret = Connection.EndReceive(ar);
			if (Ret <= 0) {
				Callback(false);
				return;
			}
			AddBytes(Buffer, Ret);
			if (IsValidQuery(Bytes))
				ProcessQuery(Bytes);
			else
				Connection.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnRecvRequest), Connection);
		} catch {
			Callback(false);
		}
	}
	///<summary>Checks whether the specified authentication query is a valid one.</summary>
	///<param name="Query">The query to check.</param>
	///<returns>True if the query is a valid authentication query, false otherwise.</returns>
	private bool IsValidQuery(byte [] Query) {
		try {
			return (Query.Length == Query[1] + Query[Query[1] + 2] + 3);
		} catch {
			return false;
		}
	}
	///<summary>Processes an authentication query.</summary>
	///<param name="Query">The query to process.</param>
	private void ProcessQuery(byte [] Query) {
		try {
			string User = Encoding.ASCII.GetString(Query, 2, Query[1]);
			string Pass = Encoding.ASCII.GetString(Query, Query[1] + 3, Query[Query[1] + 2]);
			byte [] ToSend;
			if (AuthList == null || AuthList.IsItemPresent(User, Pass)) {
				ToSend = new byte[]{5, 0};
				Connection.BeginSend(ToSend, 0, ToSend.Length, SocketFlags.None, new AsyncCallback(this.OnOkSent), Connection);
			} else {
				ToSend = new Byte[]{5, 1};
				Connection.BeginSend(ToSend, 0, ToSend.Length, SocketFlags.None, new AsyncCallback(this.OnUhohSent), Connection);
			}
		} catch {
			Callback(false);
		}
	}
	///<summary>Called when an OK reply has been sent to the client.</summary>
	///<param name="ar">The result of the asynchronous operation.</param>
	private void OnOkSent(IAsyncResult ar) {
		try {
			if (Connection.EndSend(ar) <= 0)
				Callback(false);
			else
				Callback(true);
		} catch {
			Callback(false);
		}
	}
	///<summary>Called when a negatiev reply has been sent to the client.</summary>
	///<param name="ar">The result of the asynchronous operation.</param>
	private void OnUhohSent(IAsyncResult ar) {
		try {
			Connection.EndSend(ar);
		} catch {}
		Callback(false);
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
	// private variables
	/// <summary>Holds the value of the AuthList property.</summary>
	private AuthenticationList m_AuthList;
}

}
