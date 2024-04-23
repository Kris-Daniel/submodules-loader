using UnityEditor;
using UnityEngine;

namespace Dependencies.SubmodulesLoader.Editor
{
	[CustomEditor(typeof(SubmodulesToLoadSO))]
	public class SubmodulesToLoadSODrawer : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			var script = (SubmodulesToLoadSO) target;
			
			GUILayout.Space(20);
 
			if(GUILayout.Button("UpdateSubmodules", GUILayout.Height(40)))
			{
				SubmodulesApplier.ApplySubmodules(script.SubmoduleGroups);
			}
		}
	}
}