﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EchoClient.cs">
//   Copyright (c) 2020 Johannes Deml. All rights reserved.
// </copyright>
// <author>
//   Johannes Deml
//   public@deml.io
// </author>
// --------------------------------------------------------------------------------------------------------------------

using System.Threading.Tasks;

namespace NetworkBenchmark.Enet
{
	internal class EchoClient: EnetClient
	{
		private Task listenTask;

		public EchoClient(int id, BenchmarkSetup config, BenchmarkData benchmarkData): base(id, config, benchmarkData)
		{
		}

		public override void Start()
		{
			listenTask = Task.Factory.StartNew(ConnectAndListen, TaskCreationOptions.LongRunning);
		}

		public override async void Dispose()
		{
			while (!listenTask.IsCompleted)
			{
				await Task.Delay(10);
			}

			base.Dispose();
		}
	}
}
