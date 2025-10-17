using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

namespace Commands
{

    public partial class CommandsManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;
        private Dictionary<string, MethodInfo> _commands = new();
        private string _input;

        private void Awake()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                foreach (MethodInfo methodInfo in assembly.GetTypes().SelectMany(classType => classType.GetMethods()))
                {
                    var attributes = methodInfo.GetCustomAttributes<CommandAttribute>().ToList();
                    if(attributes.Count == 0) continue;

                    foreach(CommandAttribute attribute in attributes)
                    {
                        Debug.Log($"{ attribute.CommandName } | {methodInfo.Name}");
                        _commands.Add(attribute.CommandName, methodInfo);
                    }
                }
            }
            _inputField.onSubmit.AddListener(OnSubmit);
        }
        private void OnSubmit(string text)
        {
            _input = text;
            ProcessCommand();
            _input = "";
            _inputField.text = "";
        }

        private void ProcessCommand()
        {
            Debug.Log("Process comand");
            string[] tokens = _input.Split(' ');
            string[] parameterTokens = tokens.Skip(1).ToArray();
            if(tokens.Length == 0) return;

            if(!_commands.TryGetValue(tokens[0], out var methodInfo))
            {
                Debug.LogError($"Nie znaleziono komendy: {tokens[0]}");
                return;
            }

            ParameterInfo[] parametersInfos = methodInfo.GetParameters();
            if(parametersInfos.Length != parameterTokens.Length)
            {
                Debug.LogError($"Nieprawid³owa liczba parametrów dla komendy: {tokens[0]}. Oczekiwano {parametersInfos.Length}, otrzymano {parameterTokens.Length}");
                return;
            }

            List<object> invocationParams = new List<object>();
            for (int i = 0; i < parametersInfos.Length; i++)
            {
                var parameterInfo = parametersInfos[i];
                invocationParams.Add(Convert.ChangeType(parameterTokens[i], parameterInfo.ParameterType));
            }

            methodInfo.Invoke(this, invocationParams.ToArray());
        }

    }
}
