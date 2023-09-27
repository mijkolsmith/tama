using Newtonsoft.Json;
using System.Text;

namespace Michaelotchi
{
	public class RemoteCreatureDataStore : IDataStore<Creature>
	{
		private HttpClient httpClient = new();
		public async Task<bool> CreateItem(Creature item)
		{
			string creatureString = JsonConvert.SerializeObject(item);
			HttpResponseMessage response = await httpClient.PostAsync(	"https://tamagotchi.hku.nl/api/Creatures", 
																		new StringContent(	creatureString, 
																							Encoding.UTF8, 
																							"application/json"));
			
			if (response.IsSuccessStatusCode)
			{
				string json = await response.Content.ReadAsStringAsync();
				Creature creature = JsonConvert.DeserializeObject<Creature>(json);
				Preferences.Set("creatureId", creature.Id);
				return true;
			}
			else return false;
		}

		public async Task<Creature> ReadItem(int id)
		{
			HttpResponseMessage response = await httpClient.GetAsync("https://tamagotchi.hku.nl/api/Creatures/" + id);
			string json = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<Creature>(json);
		}

		public async Task<bool> UpdateItem(Creature item)
		{
			string creatureString = JsonConvert.SerializeObject(item);
			HttpResponseMessage response = await httpClient.PutAsync(	"https://tamagotchi.hku.nl/api/Creatures/" + item.Id,
																		new StringContent(	creatureString,
																							Encoding.UTF8,
																							"application/json"));

			return response.IsSuccessStatusCode;
		}

		public async Task<bool> DeleteItem(Creature item)
		{
			string creatureString = JsonConvert.SerializeObject(item);
			HttpResponseMessage response = await httpClient.DeleteAsync("https://tamagotchi.hku.nl/api/Creatures/" + item.Id);

			return response.IsSuccessStatusCode;
		}
	}
}
