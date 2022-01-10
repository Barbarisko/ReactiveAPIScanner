﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RequestLib
{
	public class Requester
	{
		protected readonly HttpClient client = new HttpClient();

		public Requester()
		{
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(
				new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
			client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

			var token = System.IO.File.ReadAllText(@".\token.txt");

			var byteArray = Encoding.ASCII.GetBytes("Barbarisko:" + token);

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
		}
	}
}
