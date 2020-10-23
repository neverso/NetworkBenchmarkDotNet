﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetCoreNetworkBenchmark.LiteNetLib
{
	internal class LiteNetLibBenchmark: INetworkBenchmark
	{
		private BenchmarkConfiguration _config;
		private EchoServer _echoServer;
		private List<EchoClient> _echoClients;


		public void Initialize(BenchmarkConfiguration config)
		{
			_config = config;
			_echoServer = new EchoServer(config);
			_echoClients = new List<EchoClient>();
		}

		public Task StartServer()
		{
			return _echoServer.StartServerThread();
		}

		public Task StartClients()
		{
			for (int i = 0; i < _config.NumClients; i++)
			{
				_echoClients.Add(new EchoClient(i, _config));
			}

			return Task.CompletedTask;
		}

		public Task ConnectClients()
		{
			for (int i = 0; i < _config.NumClients; i++)
			{
				_echoClients[i].Start();
			}

			var clientsConnected = Task.Run(() =>
			{
				for (int i = 0; i < _config.NumClients; i++)
				{
					while (!_echoClients[i].IsConnected)
					{
						Task.Delay(10);
					}
				}

			});
			return clientsConnected;
		}

		public void StartBenchmark()
		{
			for (int i = 0; i < _echoClients.Count; i++)
			{
				_echoClients[i].StartSendingMessages();
			}
		}

		public void StopBenchmark()
		{

		}

		public Task DisconnectClients()
		{
			for (int i = 0; i < _echoClients.Count; i++)
			{
				_echoClients[i].Disconnect();
			}
			return Task.CompletedTask;
		}

		public Task StopServer()
		{
			return _echoServer.StopServerThread();
		}

		public Task StopClients()
		{
			return Task.CompletedTask;
		}

		public Task DisposeClients()
		{
			for (int i = 0; i < _echoClients.Count; i++)
			{
				_echoClients[i].Dispose();
			}

			var allDisposed = Task.Run(() =>
			{
				for (int i = 0; i < _echoClients.Count; i++)
				{
					while (!_echoClients[i].IsDisposed)
					{
						Task.Delay(10);
					}
				}

			});
			return allDisposed;
		}

		public Task DisposeServer()
		{
			_echoServer.Dispose();

			return Task.CompletedTask;
		}
	}
}