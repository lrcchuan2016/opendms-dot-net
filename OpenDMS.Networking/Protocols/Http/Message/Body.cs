﻿using System;

namespace OpenDMS.Networking.Protocols.Http.Message
{
    public class Body
    {
        public bool IsRepeatable { get; set; }
        public bool IsChunked { get; set; }
        public HttpNetworkStream Stream { get; set; }

    }
}