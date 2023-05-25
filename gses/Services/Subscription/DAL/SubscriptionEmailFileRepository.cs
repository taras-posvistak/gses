using Gses.Services.Common.Tools;
using System.Text;
using System.Text.Json;

namespace Gses.Services.Subscription.DAL
{
	public interface ISubscriptionEmailFileRepository
	{
		string[] GetAll();

		bool Add(string email);
	}

	public class SubscriptionEmailFileRepository : ISubscriptionEmailFileRepository
	{
		private const string _filePath = $"{FilePathConstant.FileDbFolder}/SubscriptionEmails.json";

		public string[] GetAll()
		{
			if (!File.Exists(_filePath))
			{
				return Array.Empty<string>();
			}

			using var streamReader = new StreamReader(_filePath, Encoding.UTF8);
			var contentResult = streamReader.ReadToEnd();
			if (string.IsNullOrEmpty(contentResult))
			{
				return Array.Empty<string>();
			}

			return JsonSerializer.Deserialize<string[]>(contentResult) ?? Array.Empty<string>();
		}

		public bool Add(string email)
		{
			var allEmails = new HashSet<string>(GetAll());
			if (allEmails.Contains(email))
			{
				return false;
			}

			allEmails.Add(email);

			var directoryPath = Path.GetDirectoryName(_filePath);
			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}

			using var fileStream = File.Open(_filePath, FileMode.OpenOrCreate);
			using var writer = new StreamWriter(fileStream);
			var json = JsonSerializer.Serialize(allEmails);
			writer.WriteLine(json);

			return true;
		}
	}
}
