Convert the Apache mime.types file to C# Dictionary<string, string> keyed by file extension. See ApacheMimeTYpes.cs for example output. The source file is located here:

http://svn.apache.org/repos/asf/httpd/httpd/trunk/docs/conf/mime.types

And is used per the permission at the top:

```
# This file maps Internet media types to unique file extension(s).
# Although created for httpd, this file is used by many software systems
# and has been placed in the public domain for unlimited redisribution.
```

The generated code looks like this:

```c-sharp
using System;
using System.Collections.Generic;

namespace ApacheMimeTypes
{
	class Apache
	{
		public static Dictionary<string, string> MimeTypes = new Dictionary<string, string>
		{
			{ "123", "application/vnd.lotus-1-2-3" },
			{ "3dml", "text/vnd.in3d.3dml" },
			{ "3g2", "video/3gpp2" },
            ...

The full output is here: https://github.com/cymen/ApacheMimeTypesToDotNet/blob/master/ApacheMimeTypes.cs
```
