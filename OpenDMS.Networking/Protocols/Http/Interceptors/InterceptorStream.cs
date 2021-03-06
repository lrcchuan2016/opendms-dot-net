﻿using System;
using System.IO;
namespace OpenDMS.Networking.Protocols.Http.Interceptors
{
    public class InterceptorStream : Stream
    {
        private long _length;
        private InterceptorBase _interceptor;

        public override bool CanRead
        {
            get { return _interceptor.CanRead; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return _interceptor.CanWrite; }
        }

        public override long Length
        {
            get { return _length; }
        }

        public override long Position
        {
            get;
            set;
        }

        public InterceptorStream(InterceptorBase interceptor)
            : base()
        {
            _interceptor = interceptor;
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int bytesRead = _interceptor.Read(buffer, offset, count);
            Position += bytesRead;
            return bytesRead;
        }

        public void ReadToEndAsync()
        {
            _interceptor.ReadToEndAsync();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            _length = value;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

    }
}
