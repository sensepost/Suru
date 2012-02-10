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
using System.Threading;

using Org.Mentalis.Security.Ssl;
using Org.Mentalis.Security.Certificates;



namespace Org.Mentalis.Proxy {

	/// <summary>References the callback method to be called when the <c>Client</c> object disconnects from the local client and the remote server.</summary>
	/// <param name="client">The <c>Client</c> that has closed its connections.</param>
	public delegate void DestroyDelegate(Client client);

	///<summary>Specifies the basic methods and properties of a <c>Client</c> object. This is an abstract class and must be inherited.</summary>
	///<remarks>The Client class provides an abstract base class that represents a connection to a local client and a remote server. Descendant classes further specify the protocol that is used between those two connections.</remarks>
	public abstract class Client : IDisposable {

		protected bool GOT_ENTIRE_HEADER=false;
		protected long CURRENTLEN=0;
		protected bool toggleReq=true;
		protected bool toggleReqSSL=true;
		HTTPRequest newreq = new HTTPRequest();
		protected int CurrentHTTPSport;
		protected bool isThisSSL=false;

		#region (De)Constructors
		///<summary>Initializes a new instance of the Client class.</summary>
		///<param name="ClientSocket">The <see cref ="Socket">Socket</see> connection between this proxy server and the local client.</param>
		///<param name="Destroyer">The callback method to be called when this Client object disconnects from the local client and the remote server.</param>
		public Client(SecureSocket ClientSocket, DestroyDelegate Destroyer) {
			this.ClientSocket = ClientSocket;
			this.Destroyer = Destroyer;
		}
		///<summary>Initializes a new instance of the Client object.</summary>
		///<remarks>Both the ClientSocket property and the DestroyDelegate are initialized to null.</remarks>
		public Client() {
			this.ClientSocket = null;
			this.Destroyer = null;
		}
		///<summary>Disposes of the resources (other than memory) used by the Client.</summary>
		///<remarks>Closes the connections with the local client and the remote host. Once <c>Dispose</c> has been called, this object should not be used anymore.</remarks>
		///<seealso cref ="System.IDisposable"/>
		public void Dispose() {
			try {
				ClientSocket.Shutdown(SocketShutdown.Both);
			} catch {}

			try {
				DestinationSocket.Shutdown(SocketShutdown.Both);
			} catch {}

			//Close the sockets
			if (ClientSocket != null)
				ClientSocket.Close();
			if (DestinationSocket != null)
				DestinationSocket.Close();

			//Clean up
			ClientSocket = null;
			DestinationSocket = null;

			if (Destroyer != null)
				Destroyer(this);
		}
		#endregion

		#region Properties
		///<summary>Gets or sets the Socket connection between the proxy server and the local client.</summary>
		///<value>A Socket instance defining the connection between the proxy server and the local client.</value>
		///<seealso cref ="DestinationSocket"/>
		internal SecureSocket ClientSocket {
			get {
				return m_ClientSocket;
			}
			set {
				if (m_ClientSocket != null) 
					m_ClientSocket.Close();
				m_ClientSocket = value;
			}
		}
		///<summary>Gets or sets the Socket connection between the proxy server and the remote host.</summary>
		///<value>A Socket instance defining the connection between the proxy server and the remote host.</value>
		///<seealso cref ="ClientSocket"/>
		internal SecureSocket DestinationSocket {
			get {
				return m_DestinationSocket;
			}
			set {
				if (m_DestinationSocket != null)
					m_DestinationSocket.Close();
				m_DestinationSocket = value;
			}
		}
		///<summary>Gets the buffer to store all the incoming data from the local client.</summary>
		///<value>An array of bytes that can be used to store all the incoming data from the local client.</value>
		///<seealso cref ="RemoteBuffer"/>
		protected byte[] Buffer {
			get {
				return m_Buffer;
			}
		}
		///<summary>Gets the buffer to store all the incoming data from the remote host.</summary>
		///<value>An array of bytes that can be used to store all the incoming data from the remote host.</value>
		///<seealso cref ="Buffer"/>
		protected byte[] RemoteBuffer {
			get {
				return m_RemoteBuffer;
			}
		}
		///<summary>Gets the buffer to store all the incoming data from the local client.</summary>
		///<value>An array of bytes that can be used to store all the incoming data from the local client.</value>
		///<seealso cref ="RemoteBuffer"/>
		protected byte[] SecureBuffer {
			get {
				return m_Buffer;
			}
		}
		///<summary>Gets the buffer to store all the incoming data from the remote host.</summary>
		///<value>An array of bytes that can be used to store all the incoming data from the remote host.</value>
		///<seealso cref ="Buffer"/>
		protected byte[] RemoteSecureBuffer {
			get {
				return m_RemoteSecureBuffer;
			}
		}
		#endregion

		#region Methods
		///<summary>Returns text information about this Client object.</summary>
		///<returns>A string representing this Client object.</returns>
		public override string ToString() {
			try {
				return "Incoming connection from " + ((IPEndPoint)DestinationSocket.RemoteEndPoint).Address.ToString();
			} catch {
				return "Client connection";
			}
		}
		///<summary>Starts relaying data between the remote host and the local client.</summary>
		///<remarks>This method should only be called after all protocol specific communication has been finished.</remarks>
		public void StartRelay() {
			try {
				
				ClientSocket.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnClientReceive), ClientSocket);
				DestinationSocket.BeginReceive(RemoteBuffer, 0, RemoteBuffer.Length, SocketFlags.None, new AsyncCallback(this.OnRemoteReceive), DestinationSocket);
			} catch {
				Dispose();
			}
		}
		#endregion

		#region Abstract Methods
		///<summary>Starts communication with the local client.</summary>
		public abstract void StartHandshake();
		#endregion

		protected byte[] convertTObyte(string passed){
			char[] search_chars=passed.ToCharArray();
			byte[] searchbyte = new byte[1024];
			int sbt=0;
			for (int sic=0; sic<search_chars.Length; sic++){
				if (search_chars[sic].Equals('`')){
					string sub=search_chars[sic+1].ToString()+search_chars[sic+2].ToString();
					sic+=2;
					searchbyte[sbt]=Convert.ToByte(sub,16);
					sbt++;
				} else {
					searchbyte[sbt]=Convert.ToByte(search_chars[sic]);
					sbt++;
				}
			}
			return searchbyte;
		}

		protected int ByteLength(byte[] buffer){
			for (int y=0; y<buffer.Length; y++){
				if (buffer[y]==0){
					return (y);
				}
			}
			return buffer.Length;
		}



		protected byte[] SearchandReplace(byte[] buffer,string search,string replace,int size){
			
			//string temp=Encoding.ASCII.GetString(buffer, 0, size);
			byte[] searchbyte=convertTObyte(search);
			byte[] replacebyte=convertTObyte(replace);
			int sl=ByteLength(searchbyte);
			int rl=ByteLength(replacebyte);
			int rindex=0;
			byte[] returnbuffer = new byte[buffer.Length+16384];	
			for (int index=0; (index<buffer.Length && index < size); index++,rindex++){
				bool found=true;
				if (index < (buffer.Length-sl)){
					for (int searchindex=0; searchindex<sl; searchindex++){
						if (buffer[index+searchindex]!=searchbyte[searchindex]){
							found=false;
							break;
						}
					}
				} else {found=false;}
				//ok we have the insertion point...
				if (found){
					//add the replacement
					Copy(replacebyte,0,returnbuffer,rindex,rl);
					// advance the buffer index with sl
					index+=(sl-1);
					//advance the replacement buffer with rl
					rindex+=(rl-1);
				} else {
					returnbuffer[rindex]=buffer[index];
				}

			}
			

			return returnbuffer;
		}


		#region Event Handlers
		///<summary>Called when we have received data from the local client.<br>Incoming data will immediately be forwarded to the remote host.</br></summary>
		///<param name="ar">The result of the asynchronous operation.</param>
		protected void OnClientReceive(IAsyncResult ar) {
			try {
				int Ret = ClientSocket.EndReceive(ar);
				if (Ret <= 0) {
					Dispose();
					return;
				}
				
			
				// RTRT HTTP/S S&R
				byte[] ReplacedBuffer=SearchandReplace(Buffer,"HTTP/1.1","HTTP/1.0",Ret);
				ReplacedBuffer=SearchandReplace(ReplacedBuffer,"gzip,","kaas,",Ret);
				foreach (string item in Mentalis.Proxy.WebProxy.SRList){
					string[] parts=item.Split('\t');
					ReplacedBuffer=SearchandReplace(ReplacedBuffer,parts[0],parts[1],Ret);
				
				}
				DestinationSocket.BeginSend(ReplacedBuffer, 0, Ret, SocketFlags.None, new AsyncCallback(this.OnRemoteSent), DestinationSocket);
			} catch ( Exception ex ) {
				//Console.WriteLine("Error: " + ex.Message);
				Dispose();
			}
		}
		///<summary>Called when we have sent data to the remote host.<br>When all the data has been sent, we will start receiving again from the local client.</br></summary>
		///<param name="ar">The result of the asynchronous operation.</param>
		///

		
		public static unsafe void Copy(byte[] src, int srcIndex, byte[] dst, int dstIndex, int count) {
			try{
				if (src == null || srcIndex < 0 ||
					dst == null || dstIndex < 0 || count < 0) {
					throw new System.ArgumentException();
				}

				int srcLen = src.Length;
				int dstLen = dst.Length;
				if (srcLen - srcIndex < count || dstLen - dstIndex < count) {
					throw new System.ArgumentException();
				}

				// The following fixed statement pins the location of the src and dst objects
				// in memory so that they will not be moved by garbage collection.
				fixed (byte* pSrc = src, pDst = dst) {
					byte* ps = pSrc;
					byte* pd = pDst+dstIndex;

					// Loop over the count in blocks of 4 bytes, copying an integer (4 bytes) at a time:
					for (int i = 0 ; i < count / 4 ; i++) {
						*((int*)pd) = *((int*)ps);
						pd += 4;
						ps += 4;
					}

					// Complete the copy by moving any bytes that weren't moved in blocks of 4:
					for (int i = 0; i < count % 4 ; i++) {
						*pd = *ps;
						pd++;
						ps++;
					}
				}
			} catch{
				//upload file too long
			}

		}

		protected void capture_to_others(byte[] Buffer,int Ret,bool isSSL){
			
			if (Ret > Buffer.Length){
				return;
			}
			//ok so this is a bad hack
			// Firefox -sometimes- send the stuff through another channel that gets marked as HTTPS
			//if (WebProxy.CurrentHTTPSport==80 && isSSL){
			//	isSSL=false;
			//}

			string moo = Encoding.ASCII.GetString(Buffer,0,Ret);
			//HTTPRequest newreq = new HTTPRequest();
			try{
				string[] Lines = moo.Replace("\r\n", "\n").Split('\n');
				string[] parts = Lines[0].Split(' ');
				if (parts[0].Equals("CONNECT")==false){
					
					//extract host name from header
					foreach (string line in Lines){
						if (line.StartsWith("Host:")){
							string[] parts_h = line.Split(':');
							string[] parts_a = Lines[0].Split(' ');
							if (isSSL==false){
								newreq.URL=parts_a[1];
							} else {
								newreq.URL=parts_h[1]+parts_a[1];
							}
							
							if (isSSL==false){
								string[] URLparts = parts[1].Split('/');
								newreq.host=URLparts[2];
							} else {
								newreq.host=parts_h[1].Trim(' ')+":"+CurrentHTTPSport.ToString();
							}
							break;
						}
					}
					WebProxy.globalcount++;
					newreq.header=moo;
					//newreq.isSSL=isSSL;
					newreq.DT=DateTime.Now;
					newreq.reqnum=WebProxy.globalcount;
					
				}
			} catch {}
		
		}

		public long recordedlen=0;
		public long copyspot=0;
		public byte[] postcontent = new byte[65535];


		protected long GetContentLength(byte[] Buffer,int Ret){
			lock (this){
				long contentlen=0;
				string moo = Encoding.ASCII.GetString(Buffer,0,Ret);
				string[] Lines = moo.Replace("\r\n", "\n").Split('\n');
				string[] parts=Lines[0].Split(' ');

				foreach (string line in Lines){
					if (line.StartsWith("Content-Length") || line.StartsWith("Content-length")){
						string[] looseparts = line.Split(':');
						contentlen=long.Parse(looseparts[1].Trim(' '));
						break;
					}
				}
				return contentlen;
			}
		}

		protected long is_FullRequest(byte[] Buffer,int Ret,bool newer,bool newest){
			try{
				lock (Buffer){
					string moo = Encoding.ASCII.GetString(Buffer,0,Ret);
					string[] Lines = moo.Replace("\r\n", "\n").Split('\n');
					string[] parts=Lines[0].Split(' ');
				
					if (GOT_ENTIRE_HEADER == false){
						int Cpos=moo.IndexOf("\r\n\r\n");
						if (Cpos>=0){
							GOT_ENTIRE_HEADER=true;
							CURRENTLEN=GetContentLength(Buffer,Ret);
							return Ret-Cpos-4;
						}
						else {
							return Ret;
						}
						
					}
					return Ret;
				}
			} catch {
				return -1;
			}
		}
		
		
		protected void OnRemoteSent(IAsyncResult ar) {
			try {
				int Ret = DestinationSocket.EndSend(ar);
				if (Ret > 0) {

					//Capture mostly HTTPS
					
					long kaas=is_FullRequest(Buffer,Ret,true,true);
					
					if (GOT_ENTIRE_HEADER){
						recordedlen+=kaas;
					}
					Copy(Buffer,0,postcontent,(int)copyspot,Ret);
					copyspot+=Ret;
					toggleReq=false;
					if (GOT_ENTIRE_HEADER && recordedlen>=CURRENTLEN) {
						capture_to_others(postcontent,(int)copyspot,true);
						toggleReq=true;
						recordedlen=0;
						copyspot=0;
						GOT_ENTIRE_HEADER=false;
					}
					

					ClientSocket.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(this.OnClientReceive), ClientSocket);
					return;
				}
			} catch {}
			try{
				Dispose();
			} catch{}
		}
		///<summary>Called when we have received data from the remote host.<br>Incoming data will immediately be forwarded to the local client.</br></summary>
		///<param name="ar">The result of the asynchronous operation.</param>
		protected void OnRemoteReceive(IAsyncResult ar) {
			int Ret=0;
			try {
				Ret = DestinationSocket.EndReceive(ar);
				if (Ret <= 0){
					Dispose();
					return;
				}
			}
			catch {
				try{
					Dispose();
				} catch{}
			}
			try{

				//just before it goes out, HTTP and HTTPs
				if (toggleReq){
					lock ((object)newreq){
						//lets just make sure that there is something to submit...
						if (newreq.header.Length>=2){
							int maxlen=ByteLength(RemoteBuffer);
							int cutpoint=1024;
							if (cutpoint > maxlen){
								cutpoint=maxlen;
							}
							string moo = Encoding.ASCII.GetString(RemoteBuffer,0,cutpoint);
							//just in case
							moo+="\r\n\r\n";
							newreq.response=moo.Substring(0,moo.IndexOf("\r\n\r\n"));
							newreq.isSSL=isThisSSL;
							toggleReq=false;
							WebProxy.Requests.Add(newreq);
						}
					}
				}
			} catch {
				//really - there is no request
				toggleReq=false;
			}
			try{
				//search and replace - Incoming
				byte[] ReplacedBuffer=SearchandReplace(RemoteBuffer,"o","o",Ret);
				foreach (string item in Mentalis.Proxy.WebProxy.SRListIncoming){
					string[] parts=item.Split('\t');
					ReplacedBuffer=SearchandReplace(ReplacedBuffer,parts[0],parts[1],Ret);
				}
				
				ClientSocket.BeginSend(ReplacedBuffer, 0, Ret, SocketFlags.None, new AsyncCallback(this.OnClientSent), ClientSocket);
			} catch {
				try{
					Dispose();
				} catch {}
			}
		}
		///<summary>Called when we have sent data to the local client.<br>When all the data has been sent, we will start receiving again from the remote host.</br></summary>
		///<param name="ar">The result of the asynchronous operation.</param>
		protected void OnClientSent(IAsyncResult ar) {
			try {
				int Ret = ClientSocket.EndSend(ar);
				if (Ret > 0) {
					DestinationSocket.BeginReceive(RemoteBuffer, 0, RemoteBuffer.Length, SocketFlags.None, new AsyncCallback(this.OnRemoteReceive), DestinationSocket);
					return;
				}
				
			} catch {}
			try{
				Dispose();
			} catch {}
		}
		#endregion

		#region Private Members
		// private variables
		/// <summary>Holds the address of the method to call when this client is ready to be destroyed.</summary>
		private DestroyDelegate Destroyer;
		/// <summary>Holds the value of the ClientSocket property.</summary>
		private SecureSocket m_ClientSocket;
		/// <summary>Holds the value of the DestinationSocket property.</summary>
		private SecureSocket m_DestinationSocket;
		/// <summary>Holds the value of the Buffer property.</summary>
		private byte[] m_Buffer = new byte[4096]; //0<->4095 = 4096
		/// <summary>Holds the value of the RemoteBuffer 1024 property.</summary>
		private byte[] m_RemoteBuffer = new byte[1024];
		/// <summary>Holds the value of the Buffer property.</summary>
		private byte[] m_SecureBuffer = new byte[4096]; //0<->4095 = 4096
		/// <summary>Holds the value of the RemoteBuffer 1024 property.</summary>
		private byte[] m_RemoteSecureBuffer = new byte[1024];
		/// <summary>
		/// Indicates whether the payload is secure
		/// </summary>
		protected bool		isPayloadSecure		= false;
		#endregion
	}

}