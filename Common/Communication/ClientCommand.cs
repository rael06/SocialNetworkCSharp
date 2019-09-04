using System;

namespace Common.Communication
{
    public class ClientCommand
    {
        public string CommandType { get; set; }
        public object CommandContent { get; set; }
    }
}
