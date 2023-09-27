using Newtonsoft.Json;

namespace Michaelotchi
{
	public class CreatureDataStore : IDataStore<Creature>
	{
		private readonly string preferencesTag = "Creature";

		public Task<bool> CreateItem(Creature item)
		{
			if (Preferences.ContainsKey(preferencesTag))
			{
				return Task.FromResult(false);
			}

			string creatureString = JsonConvert.SerializeObject(item);
			Preferences.Set(preferencesTag, creatureString);

			return Task.FromResult(Preferences.ContainsKey(preferencesTag));
		}

		public Task<Creature> ReadItem(int id)
		{
			string creatureString = Preferences.Get(preferencesTag, "");
			return Task.FromResult(JsonConvert.DeserializeObject<Creature>(creatureString));
		}

		public Task<bool> UpdateItem(Creature item)
		{
			if (Preferences.ContainsKey(preferencesTag))
			{
				Preferences.Remove(preferencesTag);
			}

			string creatureString = JsonConvert.SerializeObject(item);
			Preferences.Set(preferencesTag, creatureString);

			return Task.FromResult(Preferences.ContainsKey(preferencesTag));
		}

		public Task<bool> DeleteItem(Creature item)
		{
			Preferences.Remove(preferencesTag);
			return Task.FromResult(!Preferences.ContainsKey(preferencesTag));
		}
	}
}
