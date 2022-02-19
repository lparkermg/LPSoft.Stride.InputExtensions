// <copyright file="BuildInputConfig.cs" company="Luke Parker">
// Copyright (c) Luke Parker. All rights reserved.
// </copyright>

namespace BuildInputConfig
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Stride.Input;

    /// <summary>
    /// Builds up a virtual button config.
    /// </summary>
    public class InputBuilder
    {
        private IDictionary<string, VirtualButton[]> _builderState = new Dictionary<string, VirtualButton[]>();

        /// <summary>
        /// Loads a dictionary based input map.
        /// </summary>
        /// <param name="inputMap">A <see cref="IDictionary{string, VirtualButton}"/> mapping.</param>
        /// <returns>The <see cref="InputBuilder"/> object.</returns>
        public InputBuilder FromDictionary(IDictionary<string, VirtualButton[]> inputMap)
        {
            if (inputMap == null)
            {
                throw new ArgumentException("A dictionary must be provided.");
            }

            if (inputMap.Count == 0)
            {
                throw new ArgumentException("A dictionary must not be empty.");
            }

            if (inputMap.Any(i => string.IsNullOrWhiteSpace(i.Key)))
            {
                throw new ArgumentException("A dictionary cannot have empty or whitespace keys.");
            }

            if (inputMap.Any(i => i.Value.Any(v => v == null)))
            {
                throw new ArgumentException("A dictionary cannot have a null value.");
            }

            foreach (var item in inputMap)
            {
                _builderState.Add(item.Key, item.Value);
            }

            return this;
        }

        /// <summary>
        /// Builds the config.
        /// </summary>
        /// <returns>A populated <see cref="VirtualButtonConfig"/>.</returns>
        public VirtualButtonConfig Build()
        {
            var config = new VirtualButtonConfig();

            foreach (var item in _builderState)
            {
                foreach (var input in item.Value)
                {
                    config.Add(new VirtualButtonBinding(item.Key, input));
                }
            }

            return config;
        }
    }
}
