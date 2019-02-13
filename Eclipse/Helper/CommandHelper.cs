using System;
using System.Collections.Generic;
using UnityEngine;
using Eclipse.Backend;
using Eclipse.Components.Command;
using Eclipse.Managers;
using Eclipse.Base.Struct;
using Eclipse.Base;

namespace Eclipse
{
    public class CommandHelper
    {
        public static CommandBase[] GetAllUseableCommandList()
        {
            List<CommandBase> commands = new List<CommandBase>();

            CommandBase[] BCL = GetEngineBasicCommandList();
            CommandBase[] PCL = GetPluginCommandList();
            for (int i = 0; i < BCL.Length; i++)
            {
                commands.Add(BCL[i]);
            }
            for (int i = 0; i < PCL.Length; i++)
            {
                commands.Add(PCL[i]);
            }
            return commands.ToArray();
        }

        public static CommandBase[] GetEngineBasicCommandList()
        {
            List<CommandBase> commands = new List<CommandBase>();
            /* Category: command, all the command control command */
            #region Category Command
            /* Clean console */
            #region Clean Console
            CommandProperty CP_CleanConsole1 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "clear", "clear"));
            CommandBase CB_CleanConsole = new CommandBase("Clean Console",
                new EngineGUIString("清除控制台列表", "Clean The Console Window.").ToString(),
                new List<CommandProperty>(new CommandProperty[] { CP_CleanConsole1 }));
            CB_CleanConsole.call = new CommandBase.Call(() => {
                for (int i = 0; i < CommandBackend.CommandRecord.Length; i++)
                {
                    CommandBackend.CommandRecord[i] = null;
                }
                return null;
            });
            commands.Add(CB_CleanConsole);
            /* Event setting */
            #endregion
            /* Check permission */
            #region Permission
            CommandBase CB_Permission = new CommandBase();
            CB_Permission.FunctionName = "Permission";
            CB_Permission.FunctionComment = new EngineGUIString("查看權限的指令", "Check Permission Command.").ToString();
            /* Varaible setting */
            CommandProperty CP_Permission1 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "permission", "Permission"));
            CB_Permission.CP_Lists.Add(CP_Permission1);
            CB_Permission.call = new CommandBase.Call(() => {
                PermissionBase permissionBase = EngineManager.EngineControl.GetPermission();
                List<string> output = new List<string>();
                output.Add("Current permission have:");
                for (int i = 0; i < permissionBase.GetSize(); i++)
                {
                    output.Add("> " + permissionBase.GetUser(i));
                }
                return output.ToArray();
            });
            commands.Add(CB_Permission);
            #endregion
            /* Cheat */
            #region Cheat
            CommandBase CB_Cheat = new CommandBase();
            CB_Cheat.FunctionName = "Permission";
            CB_Cheat.FunctionComment = new EngineGUIString("查看權限的指令", "Check Permission Command.").ToString();
            /* Varaible setting */
            CommandProperty CP_Cheat1 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "admin", "Permission"));
            CommandProperty CP_Cheat2 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Dynamic, "", "Trigger"));
            CB_Cheat.CP_Lists.Add(CP_Cheat1);
            CB_Cheat.CP_Lists.Add(CP_Cheat2);
            CB_Cheat.call = new CommandBase.Call(() => {
                PermissionBase permissionBase = EngineManager.EngineControl.GetPermission();
                if (CB_Cheat.CP_Lists[1].property.variableValue != "0" && CB_Cheat.CP_Lists[1].property.variableValue != "1")
                {
                    return new string[1] { "You have to enter either 0 or 1 to enable and disable." };
                }
                if (permissionBase.CheckUserExist("Admin"))
                {
                    if (CB_Cheat.CP_Lists[1].property.variableValue == "0")
                    {
                        EngineManager.EngineControl.GetPermission().DeleteUser("Admin");
                        return new string[1] { "Remove Admin permission successfully." };
                    }
                    else
                    {
                        return new string[1] { "You are already Administor." };
                    }
                }
                else
                {
                    if (CB_Cheat.CP_Lists[1].property.variableValue == "1")
                    {
                        EngineManager.EngineControl.GetPermission().AddUser("Admin");
                        return new string[1] { "Add Admin permission successfully." };
                    }
                    else
                    {
                        return new string[1] { "You are already not Administor." };
                    }
                }
            });
            commands.Add(CB_Cheat);
            #endregion
            /* Help command */
            #region Help
            /* Varaible setting */
            CommandProperty CP_HelpConsole1 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "help", "Help"));
            CommandBase CB_HelpConsole = new CommandBase("Help",
                new EngineGUIString("幫助的指令", "Help Command.").ToString(),
                new List<CommandProperty>(new CommandProperty[] { CP_HelpConsole1 }));
            CB_HelpConsole.call = new CommandBase.Call(() => {
                return new string[] { "Here is the list commands category you can use:",
                    "Enter different category see the detail of the command.",
                    "> help core",
                    "> help plugin",
                    "> help world",
                    "> help audio",
                    "> help entity",
                    "> help player",
                    "> help character" };
            });
            commands.Add(CB_HelpConsole);
            #endregion
            /* Help core */
            #region Help Core
            CommandBase CB_HelpCore = new CommandBase();
            CB_HelpCore.FunctionName = "Help Core";
            CB_HelpCore.FunctionComment = new EngineGUIString("核心的幫助指令.", "Core Help Command.").ToString();
            CommandProperty CP_HelpCore1 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "help", "Help"));
            CommandProperty CP_HelpCore2 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "core", "Core"));
            CB_HelpCore.CP_Lists.Add(CP_HelpCore1);
            CB_HelpCore.CP_Lists.Add(CP_HelpCore2);
            CB_HelpCore.call = new CommandBase.Call(() => {
                return new string[3] { "The core commands you can use:",
                "> permission => check your current permission.",
                "> clear => clean the console window."};
            });
            commands.Add(CB_HelpCore);
            #endregion
            /* Help plugin */
            #region Help Plugin
            CommandBase CB_HelpPlugin = new CommandBase();
            CB_HelpPlugin.FunctionName = "Help Plugin";
            CB_HelpPlugin.FunctionComment = new EngineGUIString("插件的幫助指令.", "Plugin Help Command.").ToString();
            CommandProperty CP_HelpPlugin1 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "help", "Help"));
            CommandProperty CP_HelpPlugin2 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "plugin", "Plugin"));
            CB_HelpPlugin.CP_Lists.Add(CP_HelpPlugin1);
            CB_HelpPlugin.CP_Lists.Add(CP_HelpPlugin2);
            CB_HelpPlugin.call = new CommandBase.Call(() => {
                return new string[] { "The core commands you can use:",
                "> plugin list => output use plugin list." };
            });
            commands.Add(CB_HelpPlugin);
            #endregion
            /* Help world */
            #region Help World
            CommandBase CB_HelpWorld = new CommandBase();
            CB_HelpWorld.FunctionName = "Help World";
            CB_HelpWorld.FunctionComment = new EngineGUIString("世界生成幫助的指令", "World Related Help Command.").ToString();
            /* Varaible setting */
            CommandProperty CP_HelpWorld1 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "help", "Help"));
            CommandProperty CP_HelpWorld2 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "world", "Help"));
            CB_HelpWorld.CP_Lists.Add(CP_HelpWorld1);
            CB_HelpWorld.CP_Lists.Add(CP_HelpWorld2);
            CB_HelpWorld.call = new CommandBase.Call(() => {
                return new string[] { "Here is the list world commands you can use:",
                    "> world default [Character ID] => set world default character as player (Admin).",
                    "> world list => Get the world list ID.",
                    "> world spawn [MapName] => spawn the world replace the current world.",
                    "> world respawn [x] [y] [z] => set the world respawn position (Admin).",
                    "> world clear => destroy the current world.",
                    "> world sky list => get sky ID list.",
                    "> world sky set [Sky ID] => change the current sky (Admin).",
                    "> gravity [x] [y] [z] => set vector as gravity (Admin).",
                    "> gravity [y] => set value as gravity (Admin)."};
            });
            commands.Add(CB_HelpWorld);
            #endregion
            /* Help audio */
            #region Help Audio
            CommandProperty CP_HelpAudio1 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "help", "Help"));
            CommandProperty CP_HelpAudio2 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "audio", "Audio"));
            CommandBase CB_HelpAudio = new CommandBase("Help Audio",
                new EngineGUIString("音源的幫助指令.", "Audio Help Command.").ToString(),
                new List<CommandProperty>(new CommandProperty[] { CP_HelpAudio1, CP_HelpAudio2 }));
            CB_HelpAudio.call = new CommandBase.Call(() => {
                return new string[] {"Audio related commands have:",
                    "> audio music list => output music ID list.",
                    "> audio sfx list => output sfx ID list.",
                    "> audio music play [Audio ID] => play the music by ID.",
                    "> audio music stop => kill all music.",
                    "> audio music [0 - 1] => change the music volume.",
                    "> audio sfx play [Audio ID] => play the sfx by ID.",
                    "> audio sfx [0 - 1] => change the sfx volume."};
            });
            commands.Add(CB_HelpAudio);
            #endregion
            /* Help entity */
            #region Help Entity
            CommandProperty CP_HelpEntity1 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "help", "Help"));
            CommandProperty CP_HelpEntity2 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "entity", "Entity"));
            CommandBase CB_HelpEntity = new CommandBase("Help Entity", new EngineGUIString("實體物件幫助的指令", "Entity Help Commands").ToString(),
                new List<CommandProperty>(new CommandProperty[] { CP_HelpEntity1, CP_HelpEntity2 }));
            CB_HelpEntity.call = new CommandBase.Call(() => {
                return new string[] { "Entity commands list below:",
                "> entity spawn [Entity ID] => spawn entity in front of you (Admin).",
                "> entity spawn [Entity ID] [x] [y] [z] => spawn entity to the position (Admin).",
                "> entity clear => clear all exist entity (Admin)."};
            });
            commands.Add(CB_HelpEntity);
            #endregion
            /* Help player */
            #region Help Player
            CommandProperty CP_HelpPlayer1 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "help", "Help"));
            CommandProperty CP_HelpPlayer2 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "player", "Player"));
            CommandBase CB_HelpPlayer = new CommandBase("Help Player", new EngineGUIString("玩家幫助的指令", "Player Help Commands").ToString(),
                new List<CommandProperty>(new CommandProperty[] { CP_HelpPlayer1, CP_HelpPlayer2 }));
            CB_HelpPlayer.call = new CommandBase.Call(() => {
                return new string[] { "Player use command below:",
                    "> noclip => turn on fly mode (Admin).",
                    "> spawn player [x] [y] [z] => spawn default player to the position (Admin).",
                    "> teleport [x] [y] [z] => teleport player to the position (Admin)."};
            });
            commands.Add(CB_HelpPlayer);
            #endregion
            /* Help character */
            #endregion
            /* Category: Plugin, all the plugin related command */
            #region Category Plugin
            /* Plugin list */
            #region Plugin List
            CommandBase CB_PluginList = new CommandBase();
            CB_PluginList.FunctionName = "Plugin List";
            CB_PluginList.FunctionComment = new EngineGUIString("輸出插件列表.", "Output Plugin List.").ToString();
            CommandProperty CP_PluginList1 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "plugin", "World"));
            CommandProperty CP_PluginList2 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "list", "List"));
            CB_PluginList.CP_Lists.Add(CP_PluginList1);
            CB_PluginList.CP_Lists.Add(CP_PluginList2);
            CB_PluginList.call = new CommandBase.Call(() => {
                PluginManagerBase[] PMB = LinkerHelper.ToManager.GetManagerByType<PluginManager>().GetPluginManagers();
                List<string> output = new List<string>();
                output.Add("Current plugin list have:");
                for (int i = 0; i < PMB.Length; i++)
                {
                    output.Add("> " + PMB[i].GetManagerName());
                }
                return output.ToArray();
            });
            commands.Add(CB_PluginList);
            #endregion
            #endregion
            /* Category: world, all the world related command */
            #region Category World
            /* Map list */
            #region Map List
            CommandBase CB_MapList = new CommandBase();
            CB_MapList.FunctionName = "Map List";
            CB_MapList.FunctionComment = new EngineGUIString("輸出地圖列表.", "Output Map List.").ToString();
            CommandProperty CP_MapList1 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "world", "World"));
            CommandProperty CP_MapList2 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "list", "List"));
            CB_MapList.CP_Lists.Add(CP_MapList1);
            CB_MapList.CP_Lists.Add(CP_MapList2);
            CB_MapList.call = new CommandBase.Call(() => {
                List<GameObjectAsset> GA = LinkerHelper.ToManager.GetManagerByType<MapManager>().GetAsset();
                List<string> output = new List<string>();
                output.Add("Current map list have:");
                for (int i = 0; i < GA.Count; i++)
                {
                    output.Add("> " + GA[i].AssetID);
                }
                return output.ToArray();
            });
            commands.Add(CB_MapList);
            #endregion
            /* Change default character */
            #region Default Character
            /* Basic name and object */
            CommandBase CB_DefaultCharacter = new CommandBase();
            CB_DefaultCharacter.FunctionName = "Default Character";
            CB_DefaultCharacter.FunctionComment = new EngineGUIString("改變預設角色 ID", "Change default character string").ToString();
            CB_DefaultCharacter.permissionBase = new PermissionBase(new string[1] { "Admin" });
            /* Varaible setting */
            CommandProperty CP_DefaultCharacter1 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "world", "World"));
            CommandProperty CP_DefaultCharacter2 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "default", "Default"));
            CommandProperty CP_DefaultCharacter3 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Dynamic, "", "ID"));
            CB_DefaultCharacter.CP_Lists.Add(CP_DefaultCharacter1);
            CB_DefaultCharacter.CP_Lists.Add(CP_DefaultCharacter2);
            CB_DefaultCharacter.CP_Lists.Add(CP_DefaultCharacter3);
            CB_DefaultCharacter.call = new CommandBase.Call(() => {
                CharacterManager CM = LinkerHelper.ToManager.GetManagerByType<CharacterManager>();
                string id = CB_DefaultCharacter.CP_Lists[2].property.variableValue;
                GameObjectAsset GA = CM.GetAssetByID(id);
                if (GA == null) return new string[1] { "Cannot find character id: " + id };
                if (GA.Asset == null) return new string[1] { "Character didn't register in the slot: " + id };
                CM.SetDefaultCharacter(id);
                return new string[1] { "Default character successfully change to: " + id };
            });
            commands.Add(CB_DefaultCharacter);
            #endregion
            /* Spawn world */
            #region Spawn
            /* Basic name and object */
            CommandBase CB_SpawnWorld = new CommandBase();
            CB_SpawnWorld.FunctionName = "Spawn World";
            CB_SpawnWorld.FunctionComment = new EngineGUIString("從地圖控制腳本中, 生成地圖, 注意 ! 你會覆蓋現有地圖.",
                "Spawn map from manager. notice, you will replace the current map.").ToString();
            /* Varaible setting */
            CommandProperty CP_SpawnWorld1 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "world", "World"));
            CommandProperty CP_SpawnWorld2 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "spawn", "Spawn"));
            CommandProperty CP_SpawnWorld3 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Dynamic, "", "MapName"));
            CB_SpawnWorld.CP_Lists.Add(CP_SpawnWorld1);
            CB_SpawnWorld.CP_Lists.Add(CP_SpawnWorld2);
            CB_SpawnWorld.CP_Lists.Add(CP_SpawnWorld3);
            /* Event setting */
            CB_SpawnWorld.call = new CommandBase.Call(() => {
                bool Success = MapManager.MapControl.SpawnMap(CB_SpawnWorld.CP_Lists[2].property.variableValue);
                string CommandResponse = Success ? "Succussfully spawn map." : "Failed to find the map %MapName%.";
                return new string[1] { CommandResponse };
            });
            commands.Add(CB_SpawnWorld);
            #endregion
            /* Set respawn */
            #region Set Respawn
            CommandBase CB_SpawnPoint = new CommandBase();
            CB_SpawnPoint.permissionBase = new PermissionBase(new string[1] { "Admin" });
            CB_SpawnPoint.FunctionName = "Set Respawn Point";
            CB_SpawnPoint.FunctionComment = new EngineGUIString("設置重生點.", "Set The Respawn Point.").ToString();
            /* Varaible setting */
            CommandProperty CP_SpawnPoint1 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "world", "World"));
            CommandProperty CP_SpawnPoint2 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "respawn", "Respawn"));
            CommandProperty CP_SpawnPoint3 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Dynamic, "", "x"));
            CommandProperty CP_SpawnPoint4 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Dynamic, "", "y"));
            CommandProperty CP_SpawnPoint5 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Dynamic, "", "z"));
            CB_SpawnPoint.CP_Lists.Add(CP_SpawnPoint1);
            CB_SpawnPoint.CP_Lists.Add(CP_SpawnPoint2);
            CB_SpawnPoint.CP_Lists.Add(CP_SpawnPoint3);
            CB_SpawnPoint.CP_Lists.Add(CP_SpawnPoint4);
            CB_SpawnPoint.CP_Lists.Add(CP_SpawnPoint5);
            CB_SpawnPoint.call = new CommandBase.Call(() => {
                float[] pos = new float[3];
                bool Pass = false;
                for (int i = 2; i < 5; i++)
                {
                    Pass = Single.TryParse(CB_SpawnPoint.CP_Lists[i].property.variableValue, out pos[i - 2]);
                    if (!Pass)
                        return new string[1] { "You have to enter a vector3 position." };
                }
                MapManager.MapControl.SetMapRespawnPoint(new Vector3(pos[0], pos[1], pos[2]));
                return new string[1] { "Successfully set the respawn point." };
            });
            commands.Add(CB_SpawnPoint);
            #endregion
            /* Destroy world */
            #region Destroy
            CommandBase CB_DestroyWorld = new CommandBase();
            CB_DestroyWorld.FunctionName = "Destroy World";
            CB_DestroyWorld.FunctionComment = new EngineGUIString("刪除目前世界.", "Destroy Current World.").ToString();
            /* Varaible setting */
            CommandProperty CP_DestroyWorld1 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "world", "World"));
            CommandProperty CP_DestroyWorld2 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "clear", "Clear"));
            CB_DestroyWorld.CP_Lists.Add(CP_DestroyWorld1);
            CB_DestroyWorld.CP_Lists.Add(CP_DestroyWorld2);
            CB_DestroyWorld.call = new CommandBase.Call(() => {
                MapManager.MapControl.CleanMap();
                EntityManager.EntityControl.CleanAllEntity();
                CharacterManager.CharacterControl.KillAll();
                return new string[1] { "World clean successfully." };
            });
            commands.Add(CB_DestroyWorld);
            #endregion
            /* Skybox list */
            #region Skybox List
            CommandProperty CP_SkyboxList1 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "world", "World"));
            CommandProperty CP_SkyboxList2 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "sky", "Sky"));
            CommandProperty CP_SkyboxList3 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "list", "List"));
            CommandBase CB_SkyboxList = new CommandBase("Sky List", new EngineGUIString("顯示天空列表.", "Show Sky List ID.").ToString(),
                new List<CommandProperty>(new CommandProperty[] { CP_SkyboxList1, CP_SkyboxList2, CP_SkyboxList3 }));
            CB_SkyboxList.call = new CommandBase.Call(() => {
                List<GameObjectAsset> MA = MapManager.SkyControl.GetSkyboxAsset();
                List<string> skylist = new List<string>();
                skylist.Add("Sky ID list below:");
                for (int i = 0; i < MA.Count; i++)
                {
                    skylist.Add("> " + MA[i].AssetID);
                }
                return skylist.ToArray();
            });
            commands.Add(CB_SkyboxList);
            #endregion
            /* Set skybox */
            #region Skybox Set
            CommandProperty CP_SkyboxSet1 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "world", "World"));
            CommandProperty CP_SkyboxSet2 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "sky", "Sky"));
            CommandProperty CP_SkyboxSet3 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "set", "Set"));
            CommandProperty CP_SkyboxSet4 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Dynamic, "", "SkyID"));
            CommandBase CB_SkyboxSet = new CommandBase("Sky Set", new EngineGUIString("設定天空.", "Set Sky.").ToString(),
                new PermissionBase(new string[] { "Admin" }),
                new List<CommandProperty>(new CommandProperty[] { CP_SkyboxSet1, CP_SkyboxSet2, CP_SkyboxSet3, CP_SkyboxSet4 }));
            CB_SkyboxSet.call = new CommandBase.Call(() => {
                bool success = MapManager.SkyControl.SetSkyBox(CB_SkyboxSet.CP_Lists[3].property.variableValue);
                if (success)
                    return new string[1] { "Successfully change sky to: " + CB_SkyboxSet.CP_Lists[3].property.variableValue };
                else
                    return new string[1] { "Failed to change the sky." };
            });
            commands.Add(CB_SkyboxSet);
            #endregion
            /* Check sun dir */
            #region Sun Direction
            CommandProperty CP_SunDir1 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "check", "Check"));
            CommandProperty CP_SunDir2 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "sun", "Sun"));
            CommandBase CB_SunDir = new CommandBase("Sun Direction", new EngineGUIString("輸出太陽夾角", "Output Sun Angle").ToString(),
                new List<CommandProperty>(new CommandProperty[] { CP_SunDir1, CP_SunDir2 }));
            CB_SunDir.call = new CommandBase.Call(() => {
                return new string[1] { "Current sun degree: " + MapManager.SkyControl.GetCurrentSunDegree().ToString() };
            });
            commands.Add(CB_SunDir);
            #endregion
            /* Gravity vec3 */
            #region Gravity Vec3
            CommandProperty CP_GraVe31 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "gravity", "Gravity"));
            CommandProperty CP_GraVe32 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Dynamic, "", "x"));
            CommandProperty CP_GraVe33 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Dynamic, "", "y"));
            CommandProperty CP_GraVe34 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Dynamic, "", "z"));
            CommandBase CB_GraVe3 = new CommandBase("Gravity Vector3", new EngineGUIString("重力調整, 三維座標.", "Gravity With Vector3").ToString(),
                new PermissionBase(new string[] { "Admin" }),
                new List<CommandProperty>(new CommandProperty[] { CP_GraVe31, CP_GraVe32, CP_GraVe33, CP_GraVe34 }));
            CB_GraVe3.call = new CommandBase.Call(() => {
                float[] vec = new float[3];
                for(int i = 0; i < 3; i++)
                {
                    bool pass = float.TryParse(CB_GraVe3.CP_Lists[i + 1].property.variableValue, out vec[i]);
                    if (!pass) return new string[1] { "Please enter vector3 as gravity value." };
                }
                MapManager.PhysicsControl.SetGravity(new Vector3(vec[0], vec[1], vec[2]));
                return new string[1] { "Gravity set successfully." };
            });
            commands.Add(CB_GraVe3);
            #endregion
            /* Gravity float */
            #region Gravity Float
            CommandProperty CP_GraFloat1 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "gravity", "Gravity"));
            CommandProperty CP_GraFloat2 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Dynamic, "", "value"));
            CommandBase CB_GraFloat = new CommandBase("Gravity Float", new EngineGUIString("重力調整, Y座標.", "Gravity With Only Y Axis").ToString(),
                new PermissionBase(new string[] { "Admin" }),
                new List<CommandProperty>(new CommandProperty[] { CP_GraFloat1, CP_GraFloat2 }));
            CB_GraFloat.call = new CommandBase.Call(() => {
                float val = 0.0f;
                bool pass = float.TryParse(CB_GraVe3.CP_Lists[1].property.variableValue, out val);
                if (!pass) return new string[1] { "Please enter number as gravity value." };
                MapManager.PhysicsControl.SetGravity(val);
                return new string[1] { "Gravity set successfully." };
            });
            commands.Add(CB_GraFloat);
            #endregion
            #endregion
            /* Category: audio, all the audio related command */
            #region Category Audio
            /* Music list */
            #region Music list
            CommandProperty CP_MusicList1 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "audio", "Audio"));
            CommandProperty CP_MusicList2 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "music", "Music"));
            CommandProperty CP_MusicList3 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "list", "List"));
            CommandBase CB_MusicList = new CommandBase("Music List", new EngineGUIString("列出音樂清單", "Output Music List").ToString(),
                new List<CommandProperty>(new CommandProperty[] { CP_MusicList1, CP_MusicList2, CP_MusicList3 }));
            CB_MusicList.call = new CommandBase.Call(() => {
                List<string> musiclist = new List<string>();
                musiclist.Add("Music ID list:");
                List<AudioAsset> audioAssets = AudioManager.MusicControl.GetListMusicAudio();
                for(int i = 0; i < audioAssets.Count; i++)
                {
                    musiclist.Add("> " + audioAssets[i].AssetID);
                }
                return musiclist.ToArray();
            });
            commands.Add(CB_MusicList);
            #endregion
            /* SFX list */
            #region SFX list
            CommandProperty CP_SFXList1 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "audio", "Audio"));
            CommandProperty CP_SFXList2 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "sfx", "SFX"));
            CommandProperty CP_SFXList3 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "list", "List"));
            CommandBase CB_SFXList = new CommandBase("Music List", new EngineGUIString("列出音樂清單", "Output Music List").ToString(),
                new List<CommandProperty>(new CommandProperty[] { CP_SFXList1, CP_SFXList2, CP_SFXList3 }));
            CB_SFXList.call = new CommandBase.Call(() =>
            {
                List<string> musiclist = new List<string>();
                musiclist.Add("Music ID list:");
                List<AudioAsset> audioAssets = AudioManager.SFXControl.GetListSFXAudio();
                for (int i = 0; i < audioAssets.Count; i++)
                {
                    musiclist.Add("> " + audioAssets[i].AssetID);
                }
                return musiclist.ToArray();
            });
            commands.Add(CB_SFXList);
            #endregion
            /* Music play */
            #region Music Play
                CommandBase CB_MusicPlay = new CommandBase();
            CB_MusicPlay.FunctionName = "Play Music";
            CB_MusicPlay.FunctionComment = new EngineGUIString("播放音樂.", "Music Play.").ToString();
            /* Varaible setting */
            CommandProperty CP_MusicPlay1 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "audio", "Audio"));
            CommandProperty CP_MusicPlay2 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "music", "Music"));
            CommandProperty CP_MusicPlay3 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "play", "Play"));
            CommandProperty CP_MusicPlay4 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Dynamic, "", "AudioID"));
            CB_MusicPlay.CP_Lists.Add(CP_MusicPlay1);
            CB_MusicPlay.CP_Lists.Add(CP_MusicPlay2);
            CB_MusicPlay.CP_Lists.Add(CP_MusicPlay3);
            CB_MusicPlay.CP_Lists.Add(CP_MusicPlay4);
            CB_MusicPlay.call = new CommandBase.Call(() => {
                bool success = AudioManager.MusicControl.PlayMusic(CB_MusicPlay.CP_Lists[3].property.variableValue);
                if (success)
                    return new string[1] { "Music successfully play." };
                else
                    return new string[1] { "Cannot play music" };
            });
            /* Event setting */
            commands.Add(CB_MusicPlay);
            #endregion
            /* Music stop */
            #region Music Stop
            CommandBase CB_MusicStop = new CommandBase();
            CB_MusicStop.FunctionName = "Stop Music";
            CB_MusicStop.FunctionComment = new EngineGUIString("停止播放所有音樂.", "Stop All The Music.").ToString();
            /* Varaible setting */
            CommandProperty CP_MusicStop1 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "audio", "Audio"));
            CommandProperty CP_MusicStop2 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "music", "Music"));
            CommandProperty CP_MusicStop3 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "stop", "Stop"));
            CB_MusicStop.CP_Lists.Add(CP_MusicStop1);
            CB_MusicStop.CP_Lists.Add(CP_MusicStop2);
            CB_MusicStop.CP_Lists.Add(CP_MusicStop3);
            CB_MusicStop.call = new CommandBase.Call(() => {
                AudioManager.MusicControl.StopAllMusic();
                return new string[1] { "Delete all music entity." };
            });
            /* Event setting */
            commands.Add(CB_MusicStop);
            #endregion
            /* Music volume */
            #region Music Volume
            CommandBase CB_MusicVolume = new CommandBase();
            CB_MusicVolume.FunctionName = "Change Music Volume";
            CB_MusicVolume.FunctionComment = new EngineGUIString("變更音樂音量, 音樂音量介於 0 - 1 之間.",
                "Change The Music Volume, Volume Must Between 0 - 1.").ToString();
            /* Varaible setting */
            CommandProperty CP_MusicVolume1 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "audio", "audio"));
            CommandProperty CP_MusicVolume2 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "music", "music"));
            CommandProperty CP_MusicVolume3 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Dynamic, "", "volume"));
            CB_MusicVolume.CP_Lists.Add(CP_MusicVolume1);
            CB_MusicVolume.CP_Lists.Add(CP_MusicVolume2);
            CB_MusicVolume.CP_Lists.Add(CP_MusicVolume3);
            CB_MusicVolume.call = new CommandBase.Call(() => {
                float volume = 0.0f;
                bool Success = Single.TryParse(CP_MusicVolume3.property.variableValue, out volume);
                if (!Success)
                    return new string[1] { "The music volume must be number." };
                bool adjustSuccess = AudioManager.MusicControl.AdjustVolume(volume);
                if (!adjustSuccess)
                    return new string[1] { "The music volume must between 0 - 1." };
                else
                    return new string[1] { "Succussfully adjust volume to %volume%." };
            });
            /* Event setting */
            commands.Add(CB_MusicVolume);
            #endregion
            /* SFX play */
            #region SFX Play
            CommandBase CB_SFXPlay = new CommandBase();
            CB_SFXPlay.FunctionName = "Play SFX";
            CB_SFXPlay.FunctionComment = new EngineGUIString("播放音效.", "Play SFX.").ToString();
            /* Varaible setting */
            CommandProperty CP_SFXPlay1 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "audio", "Audio"));
            CommandProperty CP_SFXPlay2 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "sfx", "SFX"));
            CommandProperty CP_SFXPlay3 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "play", "Play"));
            CommandProperty CP_SFXPlay4 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Dynamic, "", "AudioID"));
            CB_SFXPlay.CP_Lists.Add(CP_SFXPlay1);
            CB_SFXPlay.CP_Lists.Add(CP_SFXPlay2);
            CB_SFXPlay.CP_Lists.Add(CP_SFXPlay3);
            CB_SFXPlay.CP_Lists.Add(CP_SFXPlay4);
            CB_SFXPlay.call = new CommandBase.Call(() => {
                bool success = AudioManager.SFXControl.PlaySFX(CB_SFXPlay.CP_Lists[3].property.variableValue);
                if (success)
                    return new string[1] { "SFX successfully play." };
                else
                    return new string[1] { "Cannot play sfx" };
            });
            /* Event setting */
            commands.Add(CB_SFXPlay);
            #endregion
            /* SFX volume */
            #region SFX Volume
            CommandBase CB_SFXVolume = new CommandBase();
            CB_SFXVolume.FunctionName = "Change SFX Volume";
            CB_SFXVolume.FunctionComment = new EngineGUIString("變更音效音量, 音效音量介於 0 - 1 之間.",
                "Change SFX Volume, Volume Must Between 0 - 1.").ToString();
            /* Varaible setting */
            CommandProperty CP_SFXVolume1 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "audio", "audio"));
            CommandProperty CP_SFXVolume2 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "sfx", "sfx"));
            CommandProperty CP_SFXVolume3 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Dynamic, "", "volume"));
            CB_SFXVolume.CP_Lists.Add(CP_SFXVolume1);
            CB_SFXVolume.CP_Lists.Add(CP_SFXVolume2);
            CB_SFXVolume.CP_Lists.Add(CP_SFXVolume3);
            CB_SFXVolume.call = new CommandBase.Call(() => {
                float volume = 0.0f;
                bool Success = Single.TryParse(CP_SFXVolume3.property.variableValue, out volume);
                if (!Success)
                    return new string[1] { "The sfx volume must be number." };
                bool adjustSuccess = AudioManager.MusicControl.AdjustVolume(volume);
                if (!adjustSuccess)
                    return new string[1] { "The sfx volume must between 0 - 1." };
                else
                    return new string[1] { "Succussfully adjust volume to %volume%." };
            });
            /* Event setting */
            commands.Add(CB_SFXVolume);
            #endregion
            #endregion
            /* Category: entity, all the entity editor related command */
            #region Category Entity
            /* Spawn entity */
            #region Spawn
            /* Basic name and object */
            CommandBase CB_SpawnEntity = new CommandBase();
            CB_SpawnEntity.FunctionName = "Spawn Entity";
            CB_SpawnEntity.FunctionComment = new EngineGUIString("生成物件至玩家面前.",
                "Spawn Entity In Front Of Player").ToString();
            CB_SpawnEntity.permissionBase = new PermissionBase(new string[1] { "Admin" });
            /* Varaible setting */
            CommandProperty CP_SpawnEntity1 = new CommandProperty
               (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "entity", "Entity"));
            CommandProperty CP_SpawnEntity2 = new CommandProperty
               (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "spawn", "Spawn"));
            CommandProperty CP_SpawnEntity3 = new CommandProperty
               (new CommandProperty.PropertyType(CommandProperty.VariableType.Dynamic, "", "EntityID"));
            CB_SpawnEntity.CP_Lists.Add(CP_SpawnEntity1);
            CB_SpawnEntity.CP_Lists.Add(CP_SpawnEntity2);
            CB_SpawnEntity.CP_Lists.Add(CP_SpawnEntity3);
            CB_SpawnEntity.call = new CommandBase.Call(() => {
                bool success = EntityManager.EntityControl.SpawnEntity(CB_SpawnEntity.CP_Lists[2].property.variableValue);
                if (success)
                    return new string[1] { "Successfully spawn entity: " + CB_SpawnEntity.CP_Lists[2].property.variableValue };
                else
                    return new string[1] { "Failed to spawn entity: " + CB_SpawnEntity.CP_Lists[2].property.variableValue };
            });
            commands.Add(CB_SpawnEntity);
            #endregion
            /* Spawn entity but with position */
            #region Spawn With Position
            /* Basic name and object */
            CommandBase CB_SpawnEntityPos = new CommandBase();
            CB_SpawnEntityPos.FunctionName = "Spawn Entity With Position";
            CB_SpawnEntityPos.FunctionComment = new EngineGUIString("生成物件至目標處.",
                "Spawn Entity To Position").ToString();
            CB_SpawnEntityPos.permissionBase = new PermissionBase(new string[1] { "Admin" });
            /* Varaible setting */
            CommandProperty CP_SpawnEntityPos1 = new CommandProperty
               (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "entity", "Entity"));
            CommandProperty CP_SpawnEntityPos2 = new CommandProperty
               (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "spawn", "Spawn"));
            CommandProperty CP_SpawnEntityPos3 = new CommandProperty
               (new CommandProperty.PropertyType(CommandProperty.VariableType.Dynamic, "", "EntityID"));
            CommandProperty CP_SpawnEntityPos4 = new CommandProperty
               (new CommandProperty.PropertyType(CommandProperty.VariableType.Dynamic, "", "x"));
            CommandProperty CP_SpawnEntityPos5 = new CommandProperty
               (new CommandProperty.PropertyType(CommandProperty.VariableType.Dynamic, "", "y"));
            CommandProperty CP_SpawnEntityPos6 = new CommandProperty
               (new CommandProperty.PropertyType(CommandProperty.VariableType.Dynamic, "", "z"));
            CB_SpawnEntityPos.CP_Lists.Add(CP_SpawnEntityPos1);
            CB_SpawnEntityPos.CP_Lists.Add(CP_SpawnEntityPos2);
            CB_SpawnEntityPos.CP_Lists.Add(CP_SpawnEntityPos3);
            CB_SpawnEntityPos.CP_Lists.Add(CP_SpawnEntityPos4);
            CB_SpawnEntityPos.CP_Lists.Add(CP_SpawnEntityPos5);
            CB_SpawnEntityPos.CP_Lists.Add(CP_SpawnEntityPos6);
            CB_SpawnEntityPos.call = new CommandBase.Call(() => {
                float[] pos = new float[3];
                bool canParse = false;
                for (int i = 3; i < 6; i++)
                {
                    canParse = Single.TryParse(CB_SpawnEntityPos.CP_Lists[i].property.variableValue, out pos[i - 3]);
                    if (!canParse)
                        return new string[1] { "Please enter vector3 position" };
                }
                bool success = EntityManager.EntityControl.SpawnEntity(CB_SpawnEntityPos.CP_Lists[2].property.variableValue, new Vector3(pos[0], pos[1], pos[2]));
                if (success)
                    return new string[1] { "Successfully spawn entity: " + CB_SpawnEntityPos.CP_Lists[2].property.variableValue };
                else
                    return new string[1] { "Failed to spawn entity: " + CB_SpawnEntityPos.CP_Lists[2].property.variableValue };
            });
            commands.Add(CB_SpawnEntityPos);
            #endregion
            /* Kill all entity */
            #region Kill All
            CommandBase CB_KillAllEntity = new CommandBase();
            CB_KillAllEntity.FunctionName = "Kill All Entity";
            CB_KillAllEntity.FunctionComment = new EngineGUIString("刪除所有場景中實體.", "Kill All The Entity In Map").ToString();
            CB_KillAllEntity.permissionBase = new PermissionBase(new string[1] { "Admin" });
            CommandProperty CP_KillAllEntity1 =
                new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "entity", "Entity"));
            CommandProperty CP_KillAllEntity2 =
                new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "clear", "Clear"));
            CB_KillAllEntity.CP_Lists.Add(CP_KillAllEntity1);
            CB_KillAllEntity.CP_Lists.Add(CP_KillAllEntity2);
            CB_KillAllEntity.call = new CommandBase.Call(() => {
                EntityManager.EntityControl.CleanAllEntity();
                return new string[1] { "Clean all entity." };
            });
            commands.Add(CB_KillAllEntity);
            #endregion
            #endregion
            /* Category: player, all the player related command */
            #region Category Player
            /* NoClip */
            #region NoClip
            CommandProperty CP_NoClip1 = new CommandProperty(new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "noclip", "NoClip"));
            CommandBase CB_NoClip = new CommandBase("NoClip", new EngineGUIString("無碰撞浮空.", "NoClip Enable.").ToString(),
                new PermissionBase(new string[] { "Admin" }),
                new List<CommandProperty>(new CommandProperty[] { CP_NoClip1 }));
            CB_NoClip.call = new CommandBase.Call(() => {
                ControlManager.ControllerAssign.GetCurrentController().NoClip();
                return new string[] { "No clip trigger successfully." };
            });
            commands.Add(CB_NoClip);
            #endregion
            /* Spawn character */
            #region Spawn Player
            /* Basic name and object */
            CommandBase CB_SpawnCharacter = new CommandBase();
            CB_SpawnCharacter.FunctionName = "Spawn Player";
            CB_SpawnCharacter.FunctionComment = new EngineGUIString("生成玩家至世界中.", "Spawn Player Into The World.").ToString();
            CB_SpawnCharacter.permissionBase = new PermissionBase(new string[1] { "Admin" });
            /* Varaible setting */
            CommandProperty CP_SpawnCharacter1 = new CommandProperty
               (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "spawn", "Spawn"));
            CommandProperty CP_SpawnCharacter2 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "player", "Player"));
            CommandProperty CP_SpawnCharacter3 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Dynamic, "", "x"));
            CommandProperty CP_SpawnCharacter4 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Dynamic, "", "y"));
            CommandProperty CP_SpawnCharacter5 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Dynamic, "", "z"));
            CB_SpawnCharacter.CP_Lists.Add(CP_SpawnCharacter1);
            CB_SpawnCharacter.CP_Lists.Add(CP_SpawnCharacter2);
            CB_SpawnCharacter.CP_Lists.Add(CP_SpawnCharacter3);
            CB_SpawnCharacter.CP_Lists.Add(CP_SpawnCharacter4);
            CB_SpawnCharacter.CP_Lists.Add(CP_SpawnCharacter5);
            CB_SpawnCharacter.call = new CommandBase.Call(() => {
                float[] pos = new float[3];
                for(int i = 2; i < 5; i++)
                {
                    bool Pass = Single.TryParse(CB_SpawnCharacter.CP_Lists[i].property.variableValue, out pos[i - 2]);
                    if (!Pass)
                        return new string[1] { "You have to enter a vector3 position." };
                }
                bool success = CharacterManager.CharacterControl.SpawnPlayerCharacter(new Vector3(pos[0], pos[1], pos[2]));
                if(success)
                    return new string[1] { "Spawn player successfully." };
                else
                    return new string[1] { "Player spawn failed." };
            });
            commands.Add(CB_SpawnCharacter);
            #endregion
            /* Teleport cursor */
            #region Teleport Cursor

            #endregion
            /* Teleport */
            #region Player Teleport
            /* Basic name and object */
            CommandBase CB_TeleportCharacter = new CommandBase();
            CB_TeleportCharacter.FunctionName = "Teleport Player";
            CB_TeleportCharacter.FunctionComment = new EngineGUIString("傳送玩家至指定位置.",
            "Teleport Player To A Position.").ToString();
            CB_TeleportCharacter.permissionBase = new PermissionBase(new string[1] { "Admin" });
            /* Varaible setting */
            CommandProperty CP_TeleportCharacter1 = new CommandProperty
               (new CommandProperty.PropertyType(CommandProperty.VariableType.Static, "teleport", "Teleport"));
            CommandProperty CP_TeleportCharacter2 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Dynamic, "", "x"));
            CommandProperty CP_TeleportCharacter3 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Dynamic, "", "y"));
            CommandProperty CP_TeleportCharacter4 = new CommandProperty
                (new CommandProperty.PropertyType(CommandProperty.VariableType.Dynamic, "", "z"));
            CB_TeleportCharacter.CP_Lists.Add(CP_TeleportCharacter1);
            CB_TeleportCharacter.CP_Lists.Add(CP_TeleportCharacter2);
            CB_TeleportCharacter.CP_Lists.Add(CP_TeleportCharacter3);
            CB_TeleportCharacter.CP_Lists.Add(CP_TeleportCharacter4);
            CB_TeleportCharacter.call = new CommandBase.Call(() => {
                if(CharacterManager.CharacterControl.GetPlayerCharacter() == null)
                {
                    return new string[1] { "Player is not spawn yet." };
                }
                Transform t = CharacterManager.CharacterControl.GetPlayerCharacter().transform;
                float[] pos = new float[3];
                for (int i = 1; i < 4; i++)
                {
                    bool Pass = Single.TryParse(CB_TeleportCharacter.CP_Lists[i].property.variableValue, out pos[i - 1]);
                    if (!Pass)
                        return new string[1] { "You have to enter a vector3 position." };
                }
                t.position = new Vector3(pos[0], pos[1], pos[2]);
                return new string[1] { "Successfully teleport to position: " + new Vector3(pos[0], pos[1], pos[2]).ToString() + "." };
            });
            commands.Add(CB_TeleportCharacter);
            #endregion
            #endregion
            /* Category: character, all the character ralated commands */
            #region Category Character

            #endregion
            return commands.ToArray();
        }

        public static CommandBase[] GetPluginCommandList()
        {
            return PluginManager.GetAllPluginCommand();
        }
    }
}
