using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUILayout;
using static UnityEngine.GUILayout;
using Debug = UnityEngine.Debug;

namespace Editor
{
    public class GitTools : EditorWindow
    {
        private string gitTitle;
        private string gitMessage;

        [MenuItem("Tools/Git Tools")]
        private static void OpenWindow()
        {
            CreateWindow<GitTools>("Git Tools");
        }
    
        private void OnGUI()
        {
            gitTitle = TextField("Heading", gitTitle);
            Label("Message");
            gitMessage = EditorGUILayout.TextArea(gitMessage);
        
            if (Button("Push"))
            {
                Commit(gitTitle, gitMessage);
            }
        }

        private static void Commit(string title, string message)
        {
            Debug.Log(Application.dataPath);

            var msg = title + '\n' + message;
            Debug.Log(msg);
            
            string[] commands =
            {
                "git checkout dev",
                "git add .",
                $"git commit -m\"{msg}\"",
                //"git push",
            };

            foreach (var command in commands)
            {
                ExecuteCommand(command);
            }
        }

        private static void ExecuteCommand(string command)
        {
            var procInfo = new ProcessStartInfo("cmd", "/c" + command)
            {
                WorkingDirectory = Application.dataPath + "/../Packages/net.harrybosch.quickactions",
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            var proc = new Process();
            proc.StartInfo = procInfo;

            proc.OutputDataReceived += (sender, data) =>
            {
                if (string.IsNullOrEmpty(data.Data)) return;
                Debug.Log(data.Data);
            };
        
            proc.ErrorDataReceived += (sender, data) =>
            {
                if (string.IsNullOrEmpty(data.Data)) return;
                Debug.LogError(data.Data);
            };

            proc.Start();
            proc.BeginOutputReadLine();
            proc.BeginErrorReadLine();
            proc.WaitForExit();
        }
    }
}
