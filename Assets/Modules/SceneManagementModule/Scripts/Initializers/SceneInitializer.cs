using System;
using System.Collections;
using System.Collections.Generic;

using SDRGames.Whist.HelpersModule;
using SDRGames.Whist.LocalizationModule.Models;

using UnityEngine;

namespace SDRGames.Whist.SceneManagementModule.Initializers
{
    public abstract class SceneInitializer : MonoBehaviour
    {
        private float _currentWeight;
        
        protected float _totalWeight;
        protected Dictionary<string, LocalizedString> _sceneInitializationStringParameters;
        protected Dictionary<string, float> _sceneInitializationNumericParameters;
        protected Dictionary<string, UnityEngine.Object> _sceneInitializationReferenceParameters;

        public event EventHandler<PartInitializedEventArgs> PartInitialized;

        public abstract IEnumerator InitializeCoroutine();

        public abstract void Run();

        public void AddInitializationParameter(string key, LocalizedString value)
        {
            if(_sceneInitializationStringParameters == null)
            {
                _sceneInitializationStringParameters = new Dictionary<string, LocalizedString>();
            }
            if(_sceneInitializationStringParameters.ContainsKey(key))
            {
                return;
            }
            _sceneInitializationStringParameters.Add(key, value);
        }

        public void AddInitializationParameter(string key, float value)
        {
            if (_sceneInitializationNumericParameters == null)
            {
                _sceneInitializationNumericParameters = new Dictionary<string, float>();
            }
            if (_sceneInitializationNumericParameters.ContainsKey(key))
            {
                return;
            }
            _sceneInitializationNumericParameters.Add(key, value);
        }

        public void AddInitializationParameter(string key, UnityEngine.Object value)
        {
            if (_sceneInitializationReferenceParameters == null)
            {
                _sceneInitializationReferenceParameters = new Dictionary<string, UnityEngine.Object>();
            }
            if (_sceneInitializationReferenceParameters.ContainsKey(key))
            {
                return;
            }
            _sceneInitializationReferenceParameters.Add(key, value);
        }

        public void SetInitializationParameters(Dictionary<string, LocalizedString> sceneInitializationParameters)
        {
            _sceneInitializationStringParameters = sceneInitializationParameters;
        }

        public void SetInitializationParameters(Dictionary<string, float> sceneInitializationParameters)
        {
            _sceneInitializationNumericParameters = sceneInitializationParameters;
        }

        public void SetInitializationParameters(Dictionary<string, UnityEngine.Object> sceneInitializationParameters)
        {
            _sceneInitializationReferenceParameters = sceneInitializationParameters;
        }

        public void SetInitializationParameters(SerializableDictionary<string, LocalizedString> sceneInitializationParameters)
        {
            _sceneInitializationStringParameters = new Dictionary<string, LocalizedString>();
            foreach(KeyValuePair<string, LocalizedString> parameter in sceneInitializationParameters)
            {
                _sceneInitializationStringParameters.Add(parameter.Key, parameter.Value);
            }
        }

        public void SetInitializationParameters(SerializableDictionary<string, float> sceneInitializationParameters)
        {
            _sceneInitializationNumericParameters = new Dictionary<string, float>();
            foreach (KeyValuePair<string, float> parameter in sceneInitializationParameters)
            {
                _sceneInitializationNumericParameters.Add(parameter.Key, parameter.Value);
            }
        }

        public void SetInitializationParameters(SerializableDictionary<string, UnityEngine.Object> sceneInitializationParameters)
        {
            _sceneInitializationReferenceParameters = new Dictionary<string, UnityEngine.Object>();
            foreach (KeyValuePair<string, UnityEngine.Object> parameter in sceneInitializationParameters)
            {
                _sceneInitializationReferenceParameters.Add(parameter.Key, parameter.Value);
            }
        }

        public Dictionary<string, LocalizedString> GetInitializationStringParameters()
        {
            return _sceneInitializationStringParameters;
        }

        public Dictionary<string, float> GetInitializationNumericParameters()
        {
            return _sceneInitializationNumericParameters;
        }

        public Dictionary<string, UnityEngine.Object> GetInitializationReferenceParameters()
        {
            return _sceneInitializationReferenceParameters;
        }

        protected IEnumerator InitializePart(Action methodName, float weight)
        {
            methodName();
            yield return null;
            _currentWeight += weight;
            PartInitialized?.Invoke(this, new PartInitializedEventArgs(_currentWeight / _totalWeight));
        }
    }
}
