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

///<summary>Listens on a specific port on the proxy server and forwards all incoming data to a specific port on another server.</summary>
public sealed class PortMapListener : Listener {
	///<summary>Initializes a new instance of the PortMapListener class.</summary>
	///<param name="Port">The port to listen on.</param>
	///<param name="MapToIP">The address to forward to.</param>
	///<remarks>The object will listen on all network addresses on the computer.</remarks>
	///<exception cref="ArgumentException"><paramref name="Port">Port</paramref> is not positive.</exception>
	///<exception cref="ArgumentNullException"><paramref name="MapToIP">MapToIP</paramref> is null.</exception>
	public PortMapListener(int Port, IPEndPoint MapToIP) : this(IPAddress.Any, Port, MapToIP) {}
	///<summary>Initializes a new instance of the PortMapListener class.</summary>
	///<param name="Port">The port to listen on.</param>
	///<param name="Address">The network address to listen on.</param>
	///<param name="MapToIP">The address to forward to.</param>
	///<remarks>For security reasons, <paramref name="Address">Address</paramref> should not be IPAddress.Any.</remarks>
	///<exception cref="ArgumentNullException">Address or <paramref name="MapToIP">MapToIP</paramref> is null.</exception>
	///<exception cref="ArgumentException">Port is not positive.</exception>
	public PortMapListener(IPAddress Address, int Port, IPEndPoint MapToIP) : base(Port, Address) {
		MapTo = MapToIP;
	}
	///<summary>Initializes a new instance of the PortMapListener class.</summary>
	///<param name="Port">The port to listen on.</param>
	///<param name="Address">The network address to listen on.</param>
	///<param name="MapToPort">The port to forward to.</param>
	///<param name="MapToAddress">The IP address to forward to.</param>
	///<remarks>For security reasons, Address should not be IPAddress.Any.</remarks>
	///<exception cref="ArgumentNullException">Address or MapToAddress is null.</exception>
	///<exception cref="ArgumentException">Port or MapToPort is invalid.</exception>
	public PortMapListener(IPAddress Address, int Port, IPAddress MapToAddress, int MapToPort) : this(Address, Port, new IPEndPoint(MapToAddress, MapToPort)) {}
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
	///<summary>Called when there's an incoming client connection waiting to be accepted.</summary>
	///<param name="ar">The result of the asynchronous operation.</param>
	public override void OnAccept(IAsyncResult ar) {
		try {
			SecureSocket NewSocket = (SecureSocket)ListenSocket.EndAccept(ar);
			if (NewSocket != null) {
				PortMapClient NewClient = new PortMapClient(NewSocket, new DestroyDelegate(this.RemoveClient), MapTo);
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
	///<summary>Returns a string representation of this object.</summary>
	///<returns>A string with information about this object.</returns>
	public override string ToString() {
		return "PORTMAP service on " + Address.ToString() + ":" + Port.ToString();
	}
	///<summary>Returns a string that holds all the construction information for this object.</summary>
	///<value>A string that holds all the construction information for this object.</value>
	public override string ConstructString {
		get {
			return "host:" + Address.ToString() + ";int:" + Port.ToString() + ";host:" + MapTo.Address.ToString() + ";int:" + MapTo.Port.ToString();
		}
	}
	// private variables
	/// <summary>Holds the value of the MapTo property.</summary>
	private IPEndPoint m_MapTo;
}

}
