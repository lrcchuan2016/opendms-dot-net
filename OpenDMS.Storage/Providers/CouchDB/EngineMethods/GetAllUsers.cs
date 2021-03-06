﻿using System;

namespace OpenDMS.Storage.Providers.CouchDB.EngineMethods
{
    public class GetAllUsers : Base
    {
        public GetAllUsers(EngineRequest request)
            : base(request)
        {
        }

        public override void Execute()
        {
            Transactions.Transaction t;
            Transactions.Processes.GetAllUsers process;

            process = new Transactions.Processes.GetAllUsers(_request.Database,
                _request.RequestingPartyType, _request.Session, _request.Database.Server.Timeout,
                _request.Database.Server.Timeout, _request.Database.Server.BufferSize, _request.Database.Server.BufferSize);
            t = new Transactions.Transaction(process);

            AttachSubscriber(process, _request.OnActionChanged);
            AttachSubscriber(process, _request.OnAuthorizationDenied);
            AttachSubscriber(process, _request.OnComplete);
            AttachSubscriber(process, _request.OnError);
            AttachSubscriber(process, _request.OnProgress);
            AttachSubscriber(process, _request.OnTimeout);

            t.Execute();
        }
    }
}
