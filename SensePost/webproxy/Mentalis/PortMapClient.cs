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

namespace Org.Mentalis.Proxy.PortMap {

///<summary>Relays data between a remote host and a local client.</summary>
public sealed class PortMapClient : Client {
	///<summary>Initializes a new instance of the PortMapClient class.</summary>
	///<param name="ClientSocket">The <see cref ="Socket">Socket</see> connection between this proxy server and the local client.</param>
	///<param name="Destroyer">The callback method to be called when this Client object disconnects from the local client and the remote server.</param>
	///<param name="MapTo">The IP EndPoint to send the incoming data to.</param>
	public PortMapClient(SecureSocket ClientSocket, DestroyDelegate Destroyer, IPEndPoint MapTo) : base(ClientSocket, Destroyer) {
		this.MapTo = MapTo;
	}
	///<summary>Gets or sets the IP EndPoint to map all incoming traffic to.</summary>
	///<value>An IPEndPoint that holds the IP address and port to use when redirecting incoming traffic.</value>
	///<exception cref="ArgumentNullException">The specified value is null.</exception>
	///<returns>An IP EndPoint specifying the host and port to map all incoming traffic to.</returns>
	private IPEndPoint MapTo {
		get {
			return m_MapTo;
		}
		set {
			if (value == null)
				throw new ArgumentNullException();
			m_MapTo = value;
		}
	}
	///<summary>Starts connecting to the remote host.</summary>
	override public void StartHandshake() {
		try {
			DestinationSocket = new SecureSocket(MapTo.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			DestinationSocket.BeginConnect(MapTo, new AsyncCallback(this.OnConnected), DestinationSocket);
		} catch {
			Dispose();
		}
	}
	///<summary>Called when the socket is connected to the remote host.</summary>
	///<remarks>When the socket is connected to the remote host, the PortMapClient begins relaying traffic between the host and the client, until one of them closes the connection.</remarks>
	///<param name="ar">The result of the asynchronous operation.</param>
	private void OnConnected(IAsyncResult ar) {
		try {
			DestinationSocket.EndConnect(ar);
			StartRelay();
		} catch {
			Dispose();
		}
	}
	///<summary>Returns text information about this PortMapClient object.</summary>
	///<returns>A string representing this PortMapClient object.</returns>
	public override string ToString() {
		try {
			return "Forwarding port from " + ((IPEndPoint)ClientSocket.RemoteEndPoint).Address.ToString() + " to " + MapTo.ToString();
		} catch {
			return "Incoming Port forward connection";
		}
	}
	// private variables
	/// <summary>Holds the value of the MapTo property.</summary>
	private IPEndPoint m_MapTo;
}

}
