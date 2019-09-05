using System;

namespace Common.Communication
{
	public class Request
	{
		public string RequestType { get; set; }
		public string RequestTarget { get; set; }
		public object RequestContent { get; set; }
	}
}
