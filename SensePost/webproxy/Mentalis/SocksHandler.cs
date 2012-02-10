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
using Org.Mentalis.Security.Ssl;

namespace Org.Mentalis.Proxy.Socks {

///<summary>Defines the signature of the method that's called when the SOCKS negotiation is complete.</summary>
///<param name="Success">Indicates whether the negotiation was successful or not.</param>
///<param name="Remote">The connection with the remote server.</param>
internal delegate void NegotiationCompleteDelegate(bool Success, SecureSocket Remote);

///<summary>Implements a specific version of the SOCKS protocol.</summary>
internal abstract class SocksHandler {
	///<summary>Initializes a new instance of the SocksHandler class.</summary>
	///<param name="ClientConnection">The connection with the client.</param>
	///<param name="Callback">The method to call when the SOCKS negotiation is complete.</param>
	///<exception cref="ArgumentNullException"><c>Callback</c> is null.</exception>
	public SocksHandler(SecureSocket ClientConnection, NegotiationCompleteDelegate Callback) {
		if (Callback == null)
			throw new ArgumentNullException();
		Connection = ClientConnection;
		Signaler = Callback;
	}
	///<summary>Gets or sets the username of the SOCKS user.</summary>
	///<value>A String representing the username of the logged on user.</value>
	///<exception cref="ArgumentNullException">The specified value is null.</exception>
	internal string Username {
		get {
			return m_Username;
		}
		set {
			if (value == null)
				throw new ArgumentNullException();
			m_Username = value;
		}
	}
	///<summary>Gets or sets the connection with the client.</summary>
	///<value>A Socket representing the connection between the proxy server and the SOCKS client.</value>
	///<exception cref="ArgumentNullException">The specified value is null.</exception>
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
	///<summary>Gets a buffer that can be used when receiving bytes from the client.</summary>
	///<value>A byte array that can be used when receiving bytes from the client.</value>
	protected byte[] Buffer {
		get {
			return m_Buffer;
		}
	}
	///<summary>Gets or sets a byte array that can be used to store received bytes from the client.</summary>
	///<value>A byte array that can be used to store bytes from the client.</value>
	protected byte[] Bytes {
		get {
			return m_Bytes;
		}
		set {
			m_Bytes = value;
		}
	}
	///<summary>Gets or sets the connection with the remote host.</summary>
	///<value>A Socket representing the connection between the proxy server and the remote host.</value>
	///<exception cref="ArgumentNullException">The specified value is null.</exception>
	protected SecureSocket RemoteConnection {
		get {
			return m_RemoteConnection;
		}
		set {
			m_RemoteConnection = value;
			try {
				m_RemoteConnection.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, 1);
			} catch {}
		}
	}
	///<summary>Gets or sets the socket that is used to accept incoming connections.</summary>
	///<value>A Socket that is used to accept incoming connections.</value>
	protected SecureSocket AcceptSocket {
		get {
			return m_AcceptSocket;
		}
		set {
			m_AcceptSocket = value;
		}
	}
	///<summary>Gets or sets the IP address of the requested remote server.</summary>
	///<value>An IPAddress object specifying the address of the requested remote server.</value>
	protected IPAddress RemoteBindIP {
		get {
			return m_RemoteBindIP;
		}
		set {
			m_RemoteBindIP = value;
		}
	}
	///<summary>Closes the listening socket if present, and signals the parent object that SOCKS negotiation is complete.</summary>
	///<param name="Success">Indicates whether the SOCKS negotiation was successful or not.</param>
	protected void Dispose(bool Success) {
		if (AcceptSocket != null)
			AcceptSocket.Close();
		Signaler(Success, RemoteConnection);
	}
	///<summary>Starts accepting bytes from the client.</summary>
	public void StartNegotiating() {
		try {
			Connection.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnReceiveBytes), Connection);
		} catch {
			Dispose(false);
		}
	}
	///<summary>Called when we receive some bytes from the client.</summary>
	///<param name="ar">The result of the asynchronous operation.</param>
	protected void OnReceiveBytes(IAsyncResult ar) {
		try {
			int Ret = Connection.EndReceive(ar);
			if (Ret <= 0)
				Dispose(false);
			AddBytes(Buffer, Ret);
			if (IsValidRequest(Bytes))
				ProcessRequest(Bytes);
			else
				Connection.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnReceiveBytes), Connection);
		} catch {
			Dispose(false);
		}
	}
	///<summary>Called when an OK reply has been sent to the client.</summary>
	///<param name="ar">The result of the asynchronous operation.</param>
	protected void OnDisposeGood(IAsyncResult ar) {
		try {
			if (Connection.EndSend(ar) > 0) {
				Dispose(true);
				return;
			}
		} catch {}
		Dispose(false);
	}
	///<summary>Called when a negative reply has been sent to the client.</summary>
	///<param name="ar">The result of the asynchronous operation.</param>
	protected void OnDisposeBad(IAsyncResult ar) {
		try {
			Connection.EndSend(ar);
		} catch {}
		Dispose(false);
	}
	///<summary>Adds some bytes to a byte aray.</summary>
	///<param name="NewBytes">The new bytes to add.</param>
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
	///<summary>Called when the AcceptSocket should start accepting incoming connections.</summary>
	///<param name="ar">The result of the asynchronous operation.</param>
	protected void OnStartAccept(IAsyncResult ar) {
		try {
			if (Connection.EndSend(ar) <= 0)
				Dispose(false);
			else
				AcceptSocket.BeginAccept(new AsyncCallback(this.OnAccept), AcceptSocket);
		} catch {
			Dispose(false);
		}
	}
	///<summary>Called when there's an incoming connection in the AcceptSocket queue.</summary>
	///<param name="ar">The result of the asynchronous operation.</param>
	protected abstract void OnAccept(IAsyncResult ar);
	///<summary>Sends a reply to the client connection and disposes it afterwards.</summary>
	///<param name="Value">A byte that contains the reply code to send to the client.</param>
	protected abstract void Dispose(byte Value);
	///<summary>Checks whether a specific request is a valid SOCKS request or not.</summary>
	///<param name="Request">The request array to check.</param>
	///<returns>True is the specified request is valid, false otherwise</returns>
	protected abstract bool IsValidRequest(byte [] Request);
	///<summary>Processes a SOCKS request from a client.</summary>
	///<param name="Request">The request to process.</param>
	protected abstract void ProcessRequest(byte [] Request);
	// private variables
	/// <summary>Holds the value of the Username property.</summary>
	private string m_Username;
	/// <summary>Holds the value of the Buffer property.</summary>
	private byte [] m_Buffer = new byte[1024];
	/// <summary>Holds the value of the Bytes property.</summary>
	private byte [] m_Bytes;
	/// <summary>Holds the value of the RemoteConnection property.</summary>
	private SecureSocket m_RemoteConnection;
	/// <summary>Holds the value of the Connection property.</summary>
	private SecureSocket m_Connection;
	/// <summary>Holds the value of the AcceptSocket property.</summary>
	private SecureSocket m_AcceptSocket;
	/// <summary>Holds the value of the RemoteBindIP property.</summary>
	private IPAddress m_RemoteBindIP;
	/// <summary>Holds the address of the method to call when the SOCKS negotiation is complete.</summary>
	private NegotiationCompleteDelegate Signaler;
}

}
