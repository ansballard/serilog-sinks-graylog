﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using Serilog.Sinks.Graylog.Extensions;
using Serilog.Sinks.Graylog.Helpers;

namespace Serilog.Sinks.Graylog.Transport.Udp
{
    public class UdpTransport : ITransport
    {
        private readonly ITransportClient<byte[]> _transportClient;
        private readonly IDataToChunkConverter _chunkConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpTransport"/> class.
        /// </summary>
        /// <param name="transportClient">The transport client.</param>
        /// <param name="chunkConverter"></param>
        public UdpTransport(ITransportClient<byte[]> transportClient, IDataToChunkConverter chunkConverter)
        {
            _transportClient = transportClient;
            _chunkConverter = chunkConverter;
        }


        /// <summary>
        /// Sends the specified target.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="System.ArgumentException">message was too long</exception>
        /// <exception cref="ArgumentException">message was too long</exception>
        public void Send(string message)
        {
            byte[] compressedMessage = message.Compress();
            IList<byte[]> chunks = _chunkConverter.ConvertToChunks(compressedMessage);
            foreach (byte[] chunk in chunks)
            {
                _transportClient.Send(chunk);
            }
        }
    }
}