#1. Name
Suru Web Proxy
#2. Author
Roelof Temmingh
#3. License, version & release date
License : 2-clause BSD license  
Version : v1.1.0  
Release Date : Unknown

#4. Description
Suru is a Man In The Middle (MITM) proxy that sits between the user's browser and the web application. It receives all the requests made by a the browser and records it. The requests can be modified in any way and replayed. Suru not only catches requests that were made by the user, but also requests that use the IE object, such as rich applications using web services, MSN ads, Google Earth requests, application auto-updates etc. The proxy understands multi part POSTs (MPPs) and XML POSTs (used for web services).

##4.1 Web application fuzzer
Suru gives the analyst the ability to fuzz ANY part of the HTTP request. This obviously includes GET and POST parameters, but can also be extended to Host: fields, Content-length: etc. The analyst can choose to fuzz any point of the HTTP request header or body. These "Fuzz control points" can be fuzzed with any value - and Suru includes some sample fuzz strings by default. After fuzzing, the analyst can choose to "auto group" the responses. This means that the application will compare the response to a base response (similar to what CrowBar does) and automatically group the responses according to its difference to the base response. In simple terms that means that Suru will tell you how many different responses were received - slightly different responses (e.g. when the response only differ by one character) will be grouped together. The analyst has the ability to set the tolerance of this grouper and the granularity of the grouping.

##4.2 Reconnaissance engine
The SensePost Suru WebProxy has the ability to perform the same type of 'Back-End' functionality as Wikto has. But it goes one step further: As you browse, Suru automatically detects when a new directory is used (e.g. when the user surfed to http://abc\_corp/abc/ the directory /abc/ is automatically searched). This means that, as the analyst is surfing the application, Suru will learn more and more about the application and perform more in-depth discovery of the site. This "smart" discovery includes functionality like automatically searching a known file name with all extensions (nice for finding abc\_corp\_login.old), and using known directory names in future searches (e.g. when /abc/ was found it would search future directories also for /abc/ - thereby also finding /cgi-bin/abc/ automatically).

##4.3 Ease of use
* Suru was written in-house by SensePost. We perform hundreds of web application assessments every year and Suru is thus a web proxy written by people that use web proxies every day. Some of our favourite features include: The ability to save and load sessions.
* Requests that were edited are marked so you can easily distinguish them from normal requests.
*There is Search and Replace on both outgoing and incoming streams (with the ability to also change binary streams).
* We have one-click directory and file discovery.
* The ability to update filenames, extensions and directory names online.
* There is a Fuzz-String editor within the application so you don't need to go and find it on the file system.
* The tools section gives MD5 and SHA1 hashes, base64, encoding and decoding and HEX, all with one click.
* The interface shows number of POST and GET parameters for every request so you don't need to go find where you actually submitted information.
* There is automatic relationship discovery, which compares all parameter with the SHA1,MD5 hashes, B64 encoded and B64 decoded representation of all the other parameters to see if there is a correlation...
* Support for upstream proxies.
* Support for response timing analysis.

#5. Usage
Complite and run
#6. Requirements
Microsoft dot net framework 1.1
