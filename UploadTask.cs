using System;
using NAnt.Core;
using NAnt.Core.Attributes;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System.Collections.Generic;

namespace S3NAnt
{
	[TaskName("s3upload")]
	public class UploadTask : S3Task
	{

		[TaskAttribute("source", Required = true)]
		public string Source { get; set;}

		protected override void ExecuteTask()
		{
			var client = CreateClient();

			Log(Level.Info, string.Format("Uploading {0} to s3://{1}/{2}", Source, Bucket, Path));

			var request = new TransferUtilityUploadRequest() {
				BucketName = Bucket,
				Key = Path,
				FilePath = Source,
			};

			var printed = new List<int>();

			request.UploadProgressEvent += (sender, args) => {
				if ( !printed.Contains(args.PercentDone) && args.PercentDone % 10 == 0 ) {
					Log(Level.Info, string.Format("Uploaded {0}% of {1}", args.PercentDone, args.FilePath));
					printed.Add(args.PercentDone);
				}
			};

			client.Upload(request);

		}

	}
}

