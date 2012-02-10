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

using System.Net.Sockets;

using Org.Mentalis.Proxy.Socks.Authentication;
using Org.Mentalis.Security.Ssl;

namespace Org.Mentalis.Proxy.Socks.Authentication {

///<summary>Authenticates a user on a SOCKS5 server according to the 'No Authentication' subprotocol.</summary>
internal sealed class AuthNone : AuthBase {
	///<summary>Initializes a new instance of the AuthNone class.</summary>
	public AuthNone() {}
	///<summary>Calls the parent class to inform it authentication is complete.</summary>
	///<param name="Connection">The connection with the SOCKS client.</param>
	///<param name="Callback">The method to call when the authentication is complete.</param>
	internal override void StartAuthentication(SecureSocket Connection, AuthenticationCompleteDelegate Callback) {
		Callback(true);
	}
}

}
