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
using Org.Mentalis.Security.Ssl;

namespace Org.Mentalis.Proxy.Socks {

///<summary>Implements the SOCKS4 and SOCKS4a protocols.</summary>
internal sealed class Socks4Handler : SocksHandler {
	///<summary>Initializes a new instance of the Socks4Handler class.</summary>
	///<param name="ClientConnection">The connection with the client.</param>
	///<param name="Callback">The method to call when the SOCKS negotiation is complete.</param>
	///<exception cref="ArgumentNullException"><c>Callback</c> is null.</exception>
	public Socks4Handler(SecureSocket ClientConnection, NegotiationCompleteDelegate Callback) : base(ClientConnection, Callback) {}
	///<summary>Checks whether a specific request is a valid SOCKS request or not.</summary>
	///<param name="Request">The request array to check.</param>
	///<returns>True is the specified request is valid, false otherwise</returns>
	protected override bool IsValidRequest(byte [] Request) {
		try {
			if (Request[0] != 1 && Request[0] != 2) { //CONNECT or BIND
				Dispose(false);
			} else {
				if (Request[3] == 0 && Request[4] == 0 && Request[5] == 0 && Request[6] != 0) { //Use remote DNS
					int Ret = Array.IndexOf(Request, (byte)0, 7);
					if (Ret > -1)
						return Array.IndexOf(Request, (byte)0, Ret + 1) != -1;
				} else {
					return Array.IndexOf(Request, (byte)0, 7) != -1;
				}
			}
		} catch {}
		return false;
	}
	///<summary>Processes a SOCKS request from a client.</summary>
	///<param name="Request">The request to process.</param>
	protected override void ProcessRequest(byte [] Request) {
		int Ret;
		try {
			if (Request[0] == 1) { // CONNECT
				IPAddress RemoteIP;
				int RemotePort = Request[1] * 256 + Request[2];
				Ret = Array.IndexOf(Request, (byte)0, 7);
				Username = Encoding.ASCII.GetString(Request, 7, Ret - 7);
				if (Request[3] == 0 && Request[4] == 0 && Request[5] == 0 && Request[6] != 0) {// Use remote DNS
					Ret = Array.IndexOf(Request, (byte)0, Ret + 1);
					RemoteIP = Dns.Resolve(Encoding.ASCII.GetString(Request, Username.Length + 8, Ret - Username.Length - 8)).AddressList[0];
				} else { //Do not use remote DNS
					RemoteIP = IPAddress.Parse(Request[3].ToString() + "." + Request[4].ToString() + "." + Request[5].ToString() + "." + Request[6].ToString());
				}
				RemoteConnection = new SecureSocket(RemoteIP.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				RemoteConnection.BeginConnect(new IPEndPoint(RemoteIP, RemotePort), new AsyncCallback(this.OnConnected), RemoteConnection);
			} else if (Request[0] == 2) { // BIND
				byte [] Reply = new byte[8];
				long LocalIP = Listener.GetLocalExternalIP().Address;
				AcceptSocket = new SecureSocket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				AcceptSocket.Bind(new IPEndPoint(IPAddress.Any, 0));
				AcceptSocket.Listen(50);
				RemoteBindIP = IPAddress.Parse(Request[3].ToString() + "." + Request[4].ToString() + "." + Request[5].ToString() + "." + Request[6].ToString());
				Reply[0] = 0;  //Reply version 0
				Reply[1] = 90;  //Everything is ok :)
				Reply[2] = (byte)(Math.Floor(((IPEndPoint)AcceptSocket.LocalEndPoint).Port / 256));  //Port/1
				Reply[3] = (byte)(((IPEndPoint)AcceptSocket.LocalEndPoint).Port % 256);  //Port/2
				Reply[4] = (byte)(Math.Floor((LocalIP % 256)));  //IP Address/1
				Reply[5] = (byte)(Math.Floor((LocalIP % 65536) / 256));  //IP Address/2
				Reply[6] = (byte)(Math.Floor((LocalIP % 16777216) / 65536));  //IP Address/3
				Reply[7] = (byte)(Math.Floor(LocalIP / 16777216));  //IP Address/4
				Connection.BeginSend(Reply, 0, Reply.Length, SocketFlags.None, new AsyncCallback(this.OnStartAccept), Connection);
			}
		} catch {
			Dispose(91);
		}
	}
	///<summary>Called when we're successfully connected to the remote host.</summary>
	///<param name="ar">The result of the asynchronous operation.</param>
	private void OnConnected(IAsyncResult ar) {
		try {
			RemoteConnection.EndConnect(ar);
			Dispose(90);
		} catch {
			Dispose(91);
		}
	}
	///<summary>Sends a reply to the client connection and disposes it afterwards.</summary>
	///<param name="Value">A byte that contains the reply code to send to the client.</param>
	protected override void Dispose(byte Value) {
		byte [] ToSend;
		try {
			ToSend = new byte[]{0, Value, (byte)(Math.Floor(((IPEndPoint)RemoteConnection.RemoteEndPoint).Port / 256)),
		                                   (byte)(((IPEndPoint)RemoteConnection.RemoteEndPoint).Port % 256),
		                                   (byte)(Math.Floor((((IPEndPoint)RemoteConnection.RemoteEndPoint).Address.Address % 256))),
		                                   (byte)(Math.Floor((((IPEndPoint)RemoteConnection.RemoteEndPoint).Address.Address % 65536) / 256)),
		                                   (byte)(Math.Floor((((IPEndPoint)RemoteConnection.RemoteEndPoint).Address.Address % 16777216) / 65536)),
		                                   (byte)(Math.Floor(((IPEndPoint)RemoteConnection.RemoteEndPoint).Address.Address / 16777216))};
		} catch {
			ToSend = new byte[]{0, 91, 0, 0, 0, 0, 0, 0};
		}
		try {
			Connection.BeginSend(ToSend, 0, ToSend.Length, SocketFlags.None, (AsyncCallback)(ToSend[1] == 90 ? new AsyncCallback(this.OnDisposeGood) : new AsyncCallback(this.OnDisposeBad)), Connection);
		} catch {
			Dispose(false);
		}
	}
	///<summary>Called when there's an incoming connection in the AcceptSocket queue.</summary>
	///<param name="ar">The result of the asynchronous operation.</param>
	protected override void OnAccept(IAsyncResult ar) {
		try {
			RemoteConnection = (SecureSocket)AcceptSocket.EndAccept(ar);
			AcceptSocket.Close();
			AcceptSocket = null;
			if (RemoteBindIP.Equals(((IPEndPoint)RemoteConnection.RemoteEndPoint).Address))
				Dispose(90);
			else
				Dispose(91);
		} catch {
			Dispose(91);
		}
	}
}

}
