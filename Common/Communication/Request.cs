using System;

namespace Common.Communication
{
	public class Request
	{
		public string RequestType { get; set; }
		public string RequestTarget { get; set; }
		public dynamic RequestContent { get; set; }
		public bool RequestSuccess { get; set; }

		public string ToString()
		{
			return $"Request = \n" +
				$"RequestType : {RequestType},\n" +
				$"RequestTarget : {RequestTarget}, \n" +
				$"RequestContent : {RequestContent}, \n" +
				$"RequestSuccess : {RequestSuccess.ToString()}";
		}
	}
}
