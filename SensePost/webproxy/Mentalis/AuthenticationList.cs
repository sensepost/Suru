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
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Specialized;

namespace Org.Mentalis.Proxy.Socks.Authentication {

///<summary>Stores a dictionary with username/password combinations.</summary>
///<remarks>This class can be used by a SOCKS5 listener.</remarks>
///<remarks>This class uses an MD5 has to store the passwords in a secure manner.</remarks>
///<remarks>The username is treated in a case-insensitive manner, the password is treated case-sensitive.</remarks>
public class AuthenticationList {
	///<summary>Initializes a new instance of the AuthenticationList class.</summary>
	public AuthenticationList() {}
	///<summary>Adds an item to the list.</summary>
	///<param name="Username">The username to add.</param>
	///<param name="Password">The corresponding password to add.</param>
	///<exception cref="ArgumentNullException">Either Username or Password is null.</exception>
	public void AddItem(string Username, string Password) {
		if (Password == null)
			throw new ArgumentNullException();
		AddHash(Username, Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(Password))));
	}
	///<summary>Adds an item to the list.</summary>
	///<param name="Username">The username to add.</param>
	///<param name="PassHash">The hashed password to add.</param>
	///<exception cref="ArgumentNullException">Either Username or Password is null.</exception>
	public void AddHash(string Username, string PassHash) {
		if (Username == null || PassHash == null)
			throw new ArgumentNullException();
		if (Listing.ContainsKey(Username)) {
			Listing[Username] = PassHash;
		} else {
			Listing.Add(Username, PassHash);
		}
	}
	///<summary>Removes an item from the list.</summary>
	///<param name="Username">The username to remove.</param>
	///<exception cref="ArgumentNullException">Username is null.</exception>
	public void RemoveItem(string Username) {
		if (Username == null)
			throw new ArgumentNullException();
		Listing.Remove(Username);
	}
	///<summary>Checks whether a user/pass combination is present in the collection or not.</summary>
	///<param name="Username">The username to search for.</param>
	///<param name="Password">The corresponding password to search for.</param>
	///<returns>True when the user/pass combination is present in the collection, false otherwise.</returns>
	public bool IsItemPresent(string Username, string Password) {
		return IsHashPresent(Username, Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(Password))));
	}
	///<summary>Checks whether a username is present in the collection or not.</summary>
	///<param name="Username">The username to search for.</param>
	///<returns>True when the username is present in the collection, false otherwise.</returns>
	public bool IsUserPresent(string Username) {
		return Listing.ContainsKey(Username);
	}
	///<summary>Checks whether a user/passhash combination is present in the collection or not.</summary>
	///<param name="Username">The username to search for.</param>
	///<param name="PassHash">The corresponding password hash to search for.</param>
	///<returns>True when the user/passhash combination is present in the collection, false otherwise.</returns>
	public bool IsHashPresent(string Username, string PassHash) {
		return Listing.ContainsKey(Username) && Listing[Username].Equals(PassHash);
	}
	///<summary>Gets the StringDictionary that's used to store the user/pass combinations.</summary>
	///<value>A StringDictionary object that's used to store the user/pass combinations.</value>
	protected StringDictionary Listing {
		get {
			return m_Listing;
		}
	}
	///<summary>Gets an array with all the keys in the authentication list.</summary>
	///<value>An array of strings containing all the keys in the authentication list.</value>
	public string[] Keys {
		get {
			ICollection keys = Listing.Keys;
			string [] ret = new string[keys.Count];
			keys.CopyTo(ret, 0);
			return ret;
		}
	}
	///<summary>Gets an array with all the hashes in the authentication list.</summary>
	///<value>An array of strings containing all the hashes in the authentication list.</value>
	public string[] Hashes {
		get {
			ICollection values = Listing.Values;
			string [] ret = new string[values.Count];
			values.CopyTo(ret, 0);
			return ret;
		}
	}
	///<summary>Clears the authentication list.</summary>
	public void Clear() {
		Listing.Clear();
	}
	// private variables
	/// <summary>Holds the value of the Listing property.</summary>
	private StringDictionary m_Listing = new StringDictionary();
}

}