using UnityEditor;
using UnityEngine;

namespace Dependencies.SubmodulesLoader
{
	public static class SubmodulesApplier
	{
		public static void ApplySubmodules(SubmoduleGroup[] submoduleGroups)
		{
			foreach (var submoduleGroup in submoduleGroups)
			{
				if (submoduleGroup.MainModule.Enabled)
				{
					AddSubmodule(submoduleGroup.MainModule);
					foreach (var submoduleChild in submoduleGroup.ChildModules)
					{
						var submoduleChildCopy = SubmoduleGroupChildWithParentPath(submoduleChild, submoduleGroup.MainModule);

						if (submoduleChildCopy.Enabled)
							AddSubmodule(submoduleChildCopy);
						else
							RemoveSubmodule(submoduleChildCopy);
					}
				}
				else
				{
					foreach (var submoduleChild in submoduleGroup.ChildModules)
					{
						var submoduleChildCopy = SubmoduleGroupChildWithParentPath(submoduleChild, submoduleGroup.MainModule);
						RemoveSubmodule(submoduleChildCopy);
					}
					RemoveSubmodule(submoduleGroup.MainModule);
				}
			}
		}

		static SubmoduleData SubmoduleGroupChildWithParentPath(SubmoduleData submoduleChild, SubmoduleData submoduleParent)
		{
			var submoduleGroupChildCopy = submoduleChild;
			submoduleGroupChildCopy.FolderName = submoduleParent.FolderName + "/" + submoduleChild.FolderName;
			return submoduleGroupChildCopy;
		}

		static void AddSubmodule(SubmoduleData submoduleData)
		{
			string gitAddSubmodule = $"submodule add {submoduleData.PathForGitSubmodule} ./Assets/Dependencies/{submoduleData.FolderName}";
			ProcessStarter.GitCommandWithLogs(gitAddSubmodule);
		}

		static void RemoveSubmodule(SubmoduleData submoduleData)
		{
			var submoduleDir = SubModuleDir(submoduleData.FolderName);
			var submoduleDirGit = SubModuleDirInGit(submoduleData.FolderName);

			string gitDeInitSubmodule = @"submodule deinit -f " + submoduleDir;
			string gitRemoveFromModules = @"rm -rf " + $"{submoduleDirGit}";
			string gitRemoveSubmodule = @"rm -f " + submoduleDir;

			ProcessStarter.GitCommandWithLogs(gitDeInitSubmodule);
			ProcessStarter.GitCommandWithLogs(gitRemoveFromModules);
			ProcessStarter.GitCommandWithLogs(gitRemoveSubmodule);

			RemoveSubModuleFolder(submoduleDir, submoduleDirGit);
		}
		
		static void RemoveSubModuleFolder(string submoduleDir, string submoduleDirGit)
		{
			FileUtil.DeleteFileOrDirectory(submoduleDir);
			FileUtil.DeleteFileOrDirectory(submoduleDir + ".meta");
			FileUtil.DeleteFileOrDirectory(submoduleDirGit);
		}
		
		static string ProjectFolderName()
		{
			var pathNames = Application.dataPath.Split('/');
			return pathNames[pathNames.Length - 2];
		}
		
		static string SubModuleDir(string submoduleFolder) 
			=> $"{Application.dataPath}/Dependencies/{submoduleFolder}";
		
		static string SubModuleDirInGit(string submoduleFolder)
			=> $"{Application.dataPath}/../../.git/modules/{ProjectFolderName()}/Assets/Dependencies/{submoduleFolder}";
	}
}