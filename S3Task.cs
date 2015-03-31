using System;
using NAnt.Core.Attributes;
using NAnt.Core;
using Amazon.S3;
using Amazon;
using Amazon.Runtime;
using Amazon.S3.Transfer;

namespace S3NAnt
{
	public abstract class S3Task : Task
	{
		[TaskAttribute("bucket", Required = true)]
		public string Bucket { get; set;}

		[TaskAttribute("path", Required=true)]
		public string Path  { get; set; }

		[TaskAttribute("access_key", Required = false)]
		public string AccessKey {get; set;}

		[TaskAttribute("secret_key", Required= false)]
		public string SecretKey { get; set; }

		public TransferUtility CreateClient() {
			TransferUtility client;

			if (string.IsNullOrEmpty(AccessKey) || string.IsNullOrEmpty(SecretKey))
			{
				Log(Level.Info, "No AWS keys provided, using IAM provided keys");
				client = new TransferUtility(RegionEndpoint.USEast1);
			}
			else
			{
				Log(Level.Info, "AWS keys provided, loading them");
				client = new TransferUtility(AccessKey, SecretKey, RegionEndpoint.USEast1);
			}

			return client;
		}


	}
}

