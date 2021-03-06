﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Http = OpenDMS.Networking.Protocols.Http;
using Tcp = OpenDMS.Networking.Protocols.Tcp;

namespace OpenDMS.Storage.Providers.CouchDB.Transactions.Remoting
{
    public class SaveBulk : Save
    {
        public List<Commands.PostBulkDocumentsReply.Entry> Results { get; protected set; }

        public SaveBulk(IDatabase db, Model.BulkDocuments input, int sendTimeout, int receiveTimeout, int sendBufferSize, int receiveBufferSize)
            : base(db, null, input, sendTimeout, receiveTimeout, sendBufferSize, receiveBufferSize)
        {
        }

        public override void Process()
        {
            Commands.PostBulkDocuments cmd;

            try
            {
                cmd = new Commands.PostBulkDocuments(_db, (Model.BulkDocuments)_input);
            }
            catch (Exception e)
            {
                Logger.Storage.Error("An exception occurred while creating the PostBulkDocuments command.", e);
                throw;
            }

            cmd.OnComplete += delegate(Commands.Base sender, Http.Client client, Http.HttpConnection connection, Commands.ReplyBase reply)
            {
                Results = ((Commands.PostBulkDocumentsReply)reply).Results;
                TriggerOnComplete(reply);
            };
            cmd.OnError += delegate(Commands.Base sender, Http.Client client, string message, Exception exception)
            {
                TriggerOnError(message, exception);
            };
            cmd.OnProgress += delegate(Commands.Base sender, Http.Client client, Http.HttpConnection connection, Tcp.DirectionType direction, int packetSize, decimal sendPercentComplete, decimal receivePercentComplete)
            {
                TriggerOnProgress(direction, packetSize, sendPercentComplete, receivePercentComplete);
            };
            cmd.OnTimeout += delegate(Commands.Base sender, Http.Client client, Http.HttpConnection connection)
            {
                TriggerOnTimeout();
            };

            cmd.Execute(_sendTimeout, _receiveTimeout, _sendBufferSize, _receiveBufferSize);
        }
    }
}