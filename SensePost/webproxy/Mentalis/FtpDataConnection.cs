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

namespace Org.Mentalis.Proxy.Ftp {

///<summary>Relays FTP data between a remote host and a local client.</summary>
internal sealed class FtpDataConnection : Client {
	///<summary>Initializes a new instance of the FtpDataConnection class.</summary>
	public FtpDataConnection() : base() {}
	///<summary>Initializes a new instance of the FtpDataConnection class.</summary>
	///<param name="RemoteAddress">The address on the local FTP client to connect to.</param>
	///<returns>The PORT command string to send to the FTP server.</returns>
	public string ProcessPort(IPEndPoint RemoteAddress) {
		try {
			ListenSocket = new SecureSocket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			ListenSocket.Bind(new IPEndPoint(IPAddress.Any, 0));
			ListenSocket.Listen(1);
			ListenSocket.BeginAccept(new AsyncCallback(this.OnPortAccept), ListenSocket);
			ClientSocket = new SecureSocket(RemoteAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			ClientSocket.BeginConnect(RemoteAddress, new AsyncCallback(this.OnPortConnected), ClientSocket);
			return "PORT " + Listener.GetLocalExternalIP().ToString().Replace('.', ',') + "," + Math.Floor(((IPEndPoint)ListenSocket.LocalEndPoint).Port / 256).ToString() + "," + (((IPEndPoint)ListenSocket.LocalEndPoint).Port % 256).ToString() + "\r\n";
		} catch {
			Dispose();
			return "PORT 0,0,0,0,0,0\r\n";
		}
	}
	///<summary>Called when we're connected to the data port on the local FTP client.</summary>
	///<param name="ar">The result of the asynchronous operation.</param>
	private void OnPortConnected(IAsyncResult ar) {
		try {
			ClientSocket.EndConnect(ar);
			StartHandshake();
		} catch {
			Dispose();
		}
	}
	///<summary>Called when there's a connection from the remote FTP server waiting to be accepted.</summary>
	///<param name="ar">The result of the asynchronous operation.</param>
	private void OnPortAccept(IAsyncResult ar) {
		try {
			DestinationSocket = (SecureSocket)ListenSocket.EndAccept(ar);
			ListenSocket.Close();
			ListenSocket = null;
			StartHandshake();
		} catch {
			Dispose();
		}
	}
	///<summary>Starts relaying data between the remote FTP server and the local FTP client.</summary>
	public override void StartHandshake() {
		if (DestinationSocket != null && ClientSocket != null && DestinationSocket.Connected && ClientSocket.Connected)
			StartRelay();
	}
	///<summary>Gets or sets the Socket that's used to listen for incoming connections.</summary>
	///<value>A Socket that's used to listen for incoming connections.</value>
	private SecureSocket ListenSocket {
		get {
			return m_ListenSocket;
		}
		set {
			if (m_ListenSocket != null)
				m_ListenSocket.Close();
			m_ListenSocket = value;
		}
	}
	///<summary>Gets or sets the parent of this FtpDataConnection.</summary>
	///<value>The FtpClient object that's the parent of this FtpDataConnection object.</value>
	private FtpClient Parent {
		get {
			return m_Parent;
		}
		set {
			m_Parent = value;
		}
	}
	///<summary>Gets or sets a string that stores the reply that has been sent from the remote FTP server.</summary>
	///<value>A string that stores the reply that has been sent from the remote FTP server.</value>
	private string FtpReply {
		get {
			return m_FtpReply;
		}
		set {
			m_FtpReply = value;
		}
	}
	///<summary>Gets or sets a boolean value that indicates whether the FtpDataConnection expects a reply from the remote FTP server or not.</summary>
	///<value>A boolean value that indicates whether the FtpDataConnection expects a reply from the remote FTP server or not.</value>
	internal bool ExpectsReply {
		get {
			return m_ExpectsReply;
		}
		set {
			m_ExpectsReply = value;
		}
	}
	///<summary>Called when the proxy server processes a PASV command.</summary>
	///<param name="Parent">The parent FtpClient object.</param>
	public void ProcessPasv(FtpClient Parent) {
		this.Parent = Parent;
		ExpectsReply = true;
	}
	///<summary>Called when the FtpClient receives a reply on the PASV command from the server.</summary>
	///<param name="Input">The received reply.</param>
	///<returns>True if the input has been processed successfully, false otherwise.</returns>
	internal bool ProcessPasvReplyRecv(string Input) {
		FtpReply += Input;
		if (FtpClient.IsValidReply(FtpReply)) {
			ExpectsReply = false;
			ProcessPasvReply(FtpReply);
			FtpReply = "";
			return true;
		}
		return false;
	}
	///<summary>Processes a PASV reply from the server.</summary>
	///<param name="Reply">The reply to process.</param>
	private void ProcessPasvReply(string Reply) {
		try {
			IPEndPoint ConnectTo = ParsePasvIP(Reply);
			DestinationSocket = new SecureSocket(ConnectTo.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			DestinationSocket.BeginConnect(ConnectTo, new AsyncCallback(this.OnPasvConnected), DestinationSocket);
		} catch {
			Dispose();
		}
	}
	///<summary>Parses a PASV reply into an instance of the IPEndPoint class.</summary>
	///<param name="Reply">The reply to parse into an IPEndPoint.</param>
	///<returns>An instance of the IPEndPoint class when successful, null otherwise.</returns>
	private IPEndPoint ParsePasvIP(string Reply) {
		int StartIndex, StopIndex;
		string IPString;
		StartIndex = Reply.IndexOf("(");
		if (StartIndex == -1) {
			return null;
		} else {
			StopIndex = Reply.IndexOf(")", StartIndex);
			if (StopIndex == -1)
				return null;
			else
				IPString = Reply.Substring(StartIndex + 1, StopIndex - StartIndex - 1);
		}
		string [] Parts = IPString.Split(',');
		if (Parts.Length == 6)
			return new IPEndPoint(IPAddress.Parse(String.Join(".", Parts, 0, 4)), int.Parse(Parts[4]) * 256 + int.Parse(Parts[5]));
		else
			return null;
	}
	///<summary>Called when we're connected to the data port of the remote FTP server.</summary>
	///<param name="ar">The result of the asynchronous operation.</param>
	private void OnPasvConnected(IAsyncResult ar) {
		try {
			DestinationSocket.EndConnect(ar);
			ListenSocket = new SecureSocket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			ListenSocket.Bind(new IPEndPoint(IPAddress.Any, 0));
			ListenSocket.Listen(1);
			ListenSocket.BeginAccept(new AsyncCallback(this.OnPasvAccept), ListenSocket);
			Parent.SendCommand("227 Entering Passive Mode (" + Listener.GetLocalInternalIP().ToString().Replace('.', ',') + "," + Math.Floor(((IPEndPoint)ListenSocket.LocalEndPoint).Port / 256).ToString() + "," + (((IPEndPoint)ListenSocket.LocalEndPoint).Port % 256).ToString() + ").\r\n");
		} catch {
			Dispose();
		}
	}
	///<summary>Called when there's a connection from the local FTP client waiting to be accepted.</summary>
	///<param name="ar">The result of the asynchronous operation.</param>
	private void OnPasvAccept(IAsyncResult ar) {
		try {
			ClientSocket = (SecureSocket)ListenSocket.EndAccept(ar);
			StartHandshake();
		} catch {
			Dispose();
		}
	}
	// private variables
	/// <summary>Holds the value of the ListenSocket property.</summary>
	private SecureSocket m_ListenSocket;
	/// <summary>Holds the value of the Parent property.</summary>
	private FtpClient m_Parent;
	/// <summary>Holds the value of the FtpReply property.</summary>
	private string m_FtpReply = "";
	/// <summary>Holds the value of the ExpectsReply property.</summary>
	private bool m_ExpectsReply = false;
}

}
