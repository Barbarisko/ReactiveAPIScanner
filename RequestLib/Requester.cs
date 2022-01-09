using System;
using System.Collections.Generic;
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

			var byteArray = Encoding.ASCII.GetBytes("Barbarisko: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");

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
		}
	}
}
