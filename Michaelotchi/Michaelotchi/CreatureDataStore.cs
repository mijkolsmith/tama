using Newtonsoft.Json;

namespace Michaelotchi
{
	public class CreatureDataStore : IDataStore<Creature>
	{
		private readonly string preferencesTag = "Creature";

		public bool CreateItem(Creature item)
		{
			if (Preferences.ContainsKey(preferencesTag))
			{
				return false;
			}

			string creatureString = JsonConvert.SerializeObject(item);
			Preferences.Set(preferencesTag, creatureString);

			return Preferences.ContainsKey(preferencesTag);
		}

		public Creature ReadItem()
		{
			string creatureString = Preferences.Get(preferencesTag, "");
			return JsonConvert.DeserializeObject<Creature>(creatureString);
		}

		public bool UpdateItem(Creature item)
		{
			if (Preferences.ContainsKey(preferencesTag))
			{
				Preferences.Remove(preferencesTag);
			}

			string creatureString = JsonConvert.SerializeObject(item);
			Preferences.Set(preferencesTag, creatureString);

			return Preferences.ContainsKey(preferencesTag);
		}

		public bool DeleteItem(Creature item)
		{
			Preferences.Remove(preferencesTag);
			return !Preferences.ContainsKey(preferencesTag);
		}
	}
}
