using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dependencies.SubmodulesLoader
{
	[CreateAssetMenu(fileName = "SubmodulesToLoad", menuName = "Tools/SubmodulesToLoad", order = 0)]
	public class SubmodulesToLoadSO : ScriptableObject
	{
		public SubmoduleGroup[] SubmoduleGroups;
	}
	
	[Serializable]
	public struct SubmoduleData
	{
		[FormerlySerializedAs("FolderNameInProject")] [FormerlySerializedAs("PathInProject")] public string FolderName;
		public string PathForGitSubmodule;
		public bool Enabled;
	}

	[Serializable]
	public struct SubmoduleGroup
	{
		public SubmoduleData MainModule;
		public SubmoduleData[] ChildModules;
	}
}