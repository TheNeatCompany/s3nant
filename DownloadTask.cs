using System;
using System.IO;
using NAnt.Core;
using NAnt.Core.Attributes;
using Amazon.S3;
using Amazon;
using Amazon.Runtime;
using Amazon.S3.Transfer;
using System.Collections.Generic;

namespace S3NAnt
{

	[TaskName("s3download")]
	public class DownloadTask : S3Task 
	{

		[TaskAttribute("destination", Required = false)]
		public string Destination { get; set; }

		protected override void ExecuteTask()
		{
			var client = CreateClient();

			if ( string.IsNullOrWhiteSpace(Destination) ) {
				Destination = System.IO.Path.GetFileName(Path);
				Log(Level.Info, string.Format("Destination was not set, using [{0}] as the value", Destination));
			}

			Log(Level.Info, string.Format("Downloading s3://{0}/{1} to {2}", Bucket, Path, Destination));

			var request = new TransferUtilityDownloadRequest() {
				BucketName = Bucket,
				Key = Path,
				FilePath = Destination,
			};

			var printed = new List<int>();

			request.WriteObjectProgressEvent += (sender, args) => {
				if ( !printed.Contains(args.PercentDone) && args.PercentDone % 10 == 0 ) {
					Log(Level.Info, string.Format("Finished {0}% of {1}", args.PercentDone, Destination) );
					printed.Add(args.PercentDone);
				}
			};

			client.Download(request);
		}

	}
}

