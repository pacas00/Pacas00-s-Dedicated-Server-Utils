using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace plugin_pacas00_server.commands
{
	class CustomConsoleCommands: FortressCraftMod
	{
		public void Start()
		{
			this.CreateCommand("resourceLevel", "Set World Resource Level", CmdParameterType.String, "ResourceDifficulty");
			this.CreateCommand("powerLevel", "Set World Power Level", CmdParameterType.String, "PowerDifficulty");
			this.CreateCommand("dayNightCycle", "Set Day/Night Cycle", CmdParameterType.String, "DayNightCycle");
			this.CreateCommand("convSpeed", "Set Conveyor Speed", CmdParameterType.String, "ConvSpeed");
			this.CreateCommand("mobDifficulty", "Set Mob Difficulty", CmdParameterType.String, "MobDifficulty");
			this.CreateCommand("deathMode", "Set the Death Mode", CmdParameterType.String, "DeathEffect");


			this.CreateCommand("ServerName", "Set what shows as the name in the server browser", CmdParameterType.String, "ServerName");
			this.CreateCommand("MaxPlayers", "Set Server's Max Players", CmdParameterType.String, "MaxPlayers");

			//this.CreateCommand("SharedResearch", "Set Server's Research as shared", CmdParameterType.String, "SharedResearch");
			this.CreateCommand("ItemLookup", "FIRE DEBUGGER", CmdParameterType.String, "ItemLookup");
			this.CreateCommand("ItemAdd", "FIRE DEBUGGER", CmdParameterType.String, "ItemAdd");
			this.CreateCommand("CubeLookup", "FIRE DEBUGGER", CmdParameterType.String, "CubeLookup");
			this.CreateCommand("CubeAdd", "FIRE DEBUGGER", CmdParameterType.String, "CubeAdd");


		}

		private void TestSpawn(string parameters)
		{
			global::Console.LogTargetFunction("ATTEMPTING TO ItemSearch", ConsoleMessageType.Trace);



			global::Console.LogTargetFunction("ATTEMPTed TO ItemSearch", ConsoleMessageType.Trace);

		}

		private void CubeLookup(string parameters)
		{
			int pixel = 16744512;
			byte red = (byte)(((pixel >> 16) & 0xff) / 16);
			byte green = (byte)(((pixel >> 8) & 0xff) / 16);
			byte blue = (byte)(((pixel) & 0xff) / 16);
			global::Console.LogTargetFunction(" " + red + " " + green + " " + blue, ConsoleMessageType.Trace);

			string text = parameters;
			parameters = parameters.ToLowerInvariant();
			string commandPar1 = parameters;
			if(parameters.Contains(" "))
			{
				int num = parameters.IndexOf(' ');
				commandPar1 = parameters.Substring(0, num);
			}
			else if(parameters.Length < 2)
			{
				//Dump out here, need more param
				global::Console.LogTargetFunction("cubelookup text    - where text is a name or partial name", ConsoleMessageType.Trace);
				return;
			}

			var cubes = TerrainData.mEntriesByKey.Keys.ToList();
			cubes = cubes.FindAll(
				    delegate (String key)
				    {
					    return key.ToLower().Contains(commandPar1.ToLower()) || TerrainData.mEntriesByKey[key].Name.ToLower().Contains(commandPar1.ToLower());
				    }
				);

			foreach(string s in cubes)
			{
				global::Console.LogTargetFunction(TerrainData.mEntriesByKey[s].Name + " : " + TerrainData.mEntriesByKey[s].CubeType, ConsoleMessageType.Trace);
			}
		}

		private void ItemLookup(string parameters)
		{
			string text = parameters;
			parameters = parameters.ToLowerInvariant();
			string commandPar1 = parameters;
			if(parameters.Contains(" "))
			{
				int num = parameters.IndexOf(' ');
				commandPar1 = parameters.Substring(0, num);
			}
			else if(parameters.Length < 2)
			{
				//Dump out here, need more param
				global::Console.LogTargetFunction("cubelookup text    - where text is a name or partial name", ConsoleMessageType.Trace);
				return;
			}

			var items = ItemEntry.mEntriesByKey.Keys.ToList();
			items = items.FindAll(
				    delegate (String key)
				    {
					    return key.ToLower().Contains(commandPar1.ToLower()) || ItemEntry.mEntriesByKey[key].Name.ToLower().Contains(commandPar1.ToLower());
				    }
				);

			foreach(string s in items)
			{
				global::Console.LogTargetFunction(ItemEntry.mEntriesByKey[s].Name + " : " + ItemEntry.mEntriesByKey[s].ItemID, ConsoleMessageType.Trace);
			}
		}

		private void CubeAdd(string parameters)
		{
			string text = parameters;
			parameters = parameters.ToLowerInvariant();
			string playername = parameters;
			string[] commandPars = null;
			if(parameters.Contains(" "))
			{
				int num = parameters.IndexOf(' ');
				playername = parameters.Substring(0, num);
				commandPars = parameters.Substring(parameters.IndexOf(' ') + 1, parameters.Length - num - 1).Split(' ');
			}
			else {
				//Dump out here, need more params
				global::Console.LogTargetFunction("cubeadd playername cubeid amount [metavalue]", ConsoleMessageType.Trace);
				return;
			}
			if(commandPars.Length < 2)
			{
				//Dump out here, need more params
				global::Console.LogTargetFunction("cubeadd playername cubeid amount [metavalue]", ConsoleMessageType.Trace);
				return;
			}

			int id = Convert.ToInt32(commandPars[0]);
			int ammount = Convert.ToInt32(commandPars[1]);

			bool cube = false;

			ItemBase itemStack = null;

			ushort cubeid = Convert.ToUInt16(id);
			ushort cubeValue = TerrainData.GetDefaultValue(cubeid);
			if(commandPars.Length > 2) cubeValue = Convert.ToUInt16(commandPars[2]);
			itemStack = new ItemCubeStack(cubeid, cubeValue, ammount);

			if(itemStack != null)
			{
				cube = true;
			}

			if(cube)
			{
				int count = NetworkManager.instance.mServerThread.connections.Count;
				for(int i = 0;i < count;i++)
				{
					NetworkServerConnection networkServerConnection = NetworkManager.instance.mServerThread.connections[i];
					if(networkServerConnection.mState == eNetworkConnectionState.Playing)
					{
						if(networkServerConnection.mPlayer.mUserName.ToLower() == playername.ToLower())
						{
							if(networkServerConnection.mPlayer.mInventory.CanFit(itemStack))
							{
								networkServerConnection.mPlayer.mInventory.AddItem(itemStack);
								if(itemStack.mType == ItemType.ItemCubeStack)
								{
									var itemCubeStack = itemStack as ItemCubeStack;
									global::Console.LogTargetFunction("Gave " + itemCubeStack.mnAmount + " of " + TerrainData.GetNameForValue(itemCubeStack.mCubeType, itemCubeStack.mCubeValue) + " to " + playername, ConsoleMessageType.Trace);
								}
								return;
							}
							else {
								global::Console.LogTargetFunction("Inventory full", ConsoleMessageType.Trace);
								return;
							}
						}
					}
				}
				global::Console.LogTargetFunction("Player Not Found", ConsoleMessageType.Trace);
				return;
			}
			else {
				global::Console.LogTargetFunction("Cube Not Found", ConsoleMessageType.Trace);
				return;
			}

		}

		private void ItemAdd(string parameters)
		{
			string text = parameters;
			parameters = parameters.ToLowerInvariant();
			string playername = parameters;
			string[] commandPars = null;
			if(parameters.Contains(" "))
			{
				int num = parameters.IndexOf(' ');
				playername = parameters.Substring(0, num);
				commandPars = parameters.Substring(parameters.IndexOf(' ') + 1, parameters.Length - num - 1).Split(' ');
			}
			else {
				//Dump out here, need more params
				global::Console.LogTargetFunction("itemadd playername itemid amount [metavalue]", ConsoleMessageType.Trace);
				return;
			}
			if(commandPars.Length < 2)
			{
				//Dump out here, need more params
				global::Console.LogTargetFunction("itemadd playername itemid amount [metavalue]", ConsoleMessageType.Trace);
				return;
			}

			int id = Convert.ToInt32(commandPars[0]);
			int ammount = Convert.ToInt32(commandPars[1]);

			bool item = false;

			ItemBase itemStack = null;
			if(ItemEntry.mEntriesById[id] != null)
			{
				//its an item
				itemStack = new ItemStack(id, ammount);
				item = true;
			}

			if(item)
			{
				int count = NetworkManager.instance.mServerThread.connections.Count;
				for(int i = 0;i < count;i++)
				{
					NetworkServerConnection networkServerConnection = NetworkManager.instance.mServerThread.connections[i];
					if(networkServerConnection.mState == eNetworkConnectionState.Playing)
					{
						if(networkServerConnection.mPlayer.mUserName.ToLower() == playername.ToLower())
						{
							if(networkServerConnection.mPlayer.mInventory.CanFit(itemStack))
							{
								networkServerConnection.mPlayer.mInventory.AddItem(itemStack);
								if(itemStack.mType != ItemType.ItemCubeStack)
								{
									global::Console.LogTargetFunction("Gave " + (itemStack as ItemStack).mnAmount + " of " + ItemEntry.mEntriesById[itemStack.mnItemID].Name + " to " + playername, ConsoleMessageType.Trace);
								}
								return;
							}
							else {
								global::Console.LogTargetFunction("Inventory full", ConsoleMessageType.Trace);
								return;
							}
						}
					}
				}
				global::Console.LogTargetFunction("Player Not Found", ConsoleMessageType.Trace);
				return;
			}
			else {
				global::Console.LogTargetFunction("Item Not Found", ConsoleMessageType.Trace);
				return;
			}

		}

		public static void SaveWorldSettings()
		{
			DifficultySettings.SetSettingsFromWorldData(WorldScript.instance.mWorldData);
			WorldScript.instance.SaveWorldSettings();
		}
		private void SharedResearch(string parameters)
		{
			if(!(WorldScript.mbIsServer == true))
			{
				//We only run commands on the server.
				global::Console.LogTargetFunction("Only the Server can run this command.", ConsoleMessageType.Trace);
				return;
			}

			bool old = PlayerResearch.mbShareResearch;
			PlayerResearch.mbShareResearch = old;

			Settings.Instance.settings.ServerName = parameters;

			Settings.SaveSettings();
			Settings.ApplyServerSettings();

			global::Console.LogTargetFunction("Set Shared Research to [" + !old + "].", ConsoleMessageType.Trace);

		}

		private void ServerName(string parameters)
		{
			if(!(WorldScript.mbIsServer == true))
			{
				//We only run commands on the server.
				global::Console.LogTargetFunction("Only the Dedicated Server can run this command.", ConsoleMessageType.Trace);
				return;
			}


			Settings.Instance.settings.ServerName = parameters;

			Settings.SaveSettings();
			Settings.ApplyServerSettings();

			global::Console.LogTargetFunction("Set Server Name to [" + parameters + "].", ConsoleMessageType.Trace);

		}

		private void MaxPlayers(string parameters)
		{
			if(!(WorldScript.mbIsServer == true))
			{
				//We only run commands on the server.
				global::Console.LogTargetFunction("Only the Dedicated Server can run this command.", ConsoleMessageType.Trace);
				return;
			}

			int num = -1;
			if(int.TryParse(parameters, out num))
			{
				Settings.Instance.settings.MaxPlayerCount = (num);

				Settings.SaveSettings();
				Settings.ApplyServerSettings();

				global::Console.LogTargetFunction("Set MaxPlayers to [" + parameters + "].", ConsoleMessageType.Trace);
			}
			else {
				global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only numbers are supported", ConsoleMessageType.Trace);
			}
		}

		private void PowerDifficulty(string parameters)
		{
			if(!(WorldScript.mbIsServer == true))
			{
				//We only run commands on the server.
				global::Console.LogTargetFunction("Only the Dedicated Server can run this command.", ConsoleMessageType.Trace);
				return;
			}

			int num = -1;
			if(int.TryParse(parameters, out num))
			{
				if(num == 0 || num == 1)
				{
					WorldScript.instance.mWorldData.mnPowerLevel = num;
					SaveWorldSettings();
					global::Console.LogTargetFunction("Power Difficulty set to [" + parameters + "], Restart Server to take effect.", ConsoleMessageType.Trace);
				}
				else {
					global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only 0 (Plentiful) and 1 (Scarce) are supported", ConsoleMessageType.Trace);
				}
			}
			else {
				global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only 0 (Plentiful) and 1 (Scarce) are supported", ConsoleMessageType.Trace);
			}
		}

		private void ResourceDifficulty(string parameters)
		{
			if(!(WorldScript.mbIsServer == true))
			{
				//We only run commands on the server.
				global::Console.LogTargetFunction("Only the Dedicated Server can run this command.", ConsoleMessageType.Trace);
				return;
			}

			int num = -1;
			if(int.TryParse(parameters, out num))
			{
				if(num == 0 || num == 1 || num == 2 || num == 3)
				{
					WorldScript.instance.mWorldData.mnResourceLevel = num;
					SaveWorldSettings();
					global::Console.LogTargetFunction("Resource Difficulty set to [" + parameters + "], Restart Server to take effect.", ConsoleMessageType.Trace);
				}
				else {
					global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only 0 (Plentiful), 1 (Scarce), 2 (Greg), and 3 (Casual) are supported", ConsoleMessageType.Trace);
				}
			}
			else {
				global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only 0 (Plentiful), 1 (Scarce), 2 (Greg), and 3 (Casual) are supported", ConsoleMessageType.Trace);
			}
		}

		private void DayNightCycle(string parameters)
		{
			if(!(WorldScript.mbIsServer == true))
			{
				//We only run commands on the server.
				global::Console.LogTargetFunction("Only the Dedicated Server can run this command.", ConsoleMessageType.Trace);
				return;
			}

			int num = -1;
			if(int.TryParse(parameters, out num))
			{
				if(num == 0 || num == 1 || num == 2)
				{
					WorldScript.instance.mWorldData.mnDayLevel = num;
					SaveWorldSettings();
					global::Console.LogTargetFunction("Day Night Cycle set to [" + parameters + "], Restart Server to take effect.", ConsoleMessageType.Trace);
				}
				else {
					global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only 0 (Eternal Day), 1 (Normal) and 2 (Eternal Night) are supported", ConsoleMessageType.Trace);
				}
			}
			else {
				global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only 0 (Eternal Day), 1 (Normal) and 2 (Eternal Night) are supported", ConsoleMessageType.Trace);
			}
		}

		private void DeathEffect(string parameters)
		{
			if(!(WorldScript.mbIsServer == true))
			{
				//We only run commands on the server.
				global::Console.LogTargetFunction("Only the Dedicated Server can run this command.", ConsoleMessageType.Trace);
				return;
			}

			int num = -1;
			if(int.TryParse(parameters, out num))
			{
				if(num == 0 || num == 1 || num == 2 || num == 3 || num == 4)
				{
					WorldScript.instance.mWorldData.meDeathEffect = (DifficultySettings.DeathEffect)num;
					SaveWorldSettings();
					global::Console.LogTargetFunction("Death Effect set to [" + parameters + "], Restart Server to take effect.", ConsoleMessageType.Trace);
				}
				else {
					global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only 0 (Easy), 1 (Clumsy), 2 (IronMan), 3 (SquishCore) and 4 (HardCore) are supported", ConsoleMessageType.Trace);
				}
			}
			else {
				global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only 0 (Easy), 1 (Clumsy), 2 (IronMan), 3 (SquishCore) and 4 (HardCore) are supported", ConsoleMessageType.Trace);
			}
		}

		private void MobDifficulty(string parameters)
		{
			if(!(WorldScript.mbIsServer == true))
			{
				//We only run commands on the server.
				global::Console.LogTargetFunction("Only the Dedicated Server can run this command.", ConsoleMessageType.Trace);
				return;
			}

			int num = -1;
			if(int.TryParse(parameters, out num))
			{
				if(num == 0 || num == 1 || num == 2)
				{
					WorldScript.instance.mWorldData.mnMobLevel = num;
					SaveWorldSettings();
					global::Console.LogTargetFunction("Mob Difficulty set to [" + parameters + "], Restart Server to take effect.", ConsoleMessageType.Trace);
				}
				else {
					global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only 0 (Trivial), 1 (Normal) and 2 (Hard) are supported", ConsoleMessageType.Trace);
				}
			}
			else {
				global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only 0 (Trivial), 1 (Normal) and 2 (Hard) are supported", ConsoleMessageType.Trace);
			}
		}

		private void ConvSpeed(string parameters)
		{
			if(!(WorldScript.mbIsServer == true))
			{

				//We only run commands on the server.
				global::Console.LogTargetFunction("Only the Dedicated Server can run this command.", ConsoleMessageType.Trace);
				return;
			}

			int num = -1;
			if(int.TryParse(parameters, out num))
			{
				if(num == 0 || num == 1)
				{
					WorldScript.instance.mWorldData.mnConveyorLevel = num;
					SaveWorldSettings();
					global::Console.LogTargetFunction("Conveyor Speed set to [" + parameters + "], Restart Server to take effect.", ConsoleMessageType.Trace);
				}
				else {
					global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only 0 (Fast) and 1 (Slow) are supported", ConsoleMessageType.Trace);
				}
			}
			else {
				global::Console.LogTargetFunction("Invalid Value [" + parameters + "], Only 0 (Fast) and 1 (Slow) are supported", ConsoleMessageType.Trace);
			}
		}



		private void CreateCommand(string command, string description, CmdParameterType type, string functionName)
		{
			global::Console.AddCommand(new ConsoleCommand(command, description, type, base.gameObject, functionName));
		}
	}
}
