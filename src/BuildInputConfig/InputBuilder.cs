﻿// <copyright file="BuildInputConfig.cs" company="Luke Parker">
// Copyright (c) Luke Parker. All rights reserved.
// </copyright>

namespace BuildInputConfig
{
    using System;
    using System.Collections.Generic;
    using Stride.Input;

    /// <summary>
    /// Builds up a virtual button config.
    /// </summary>
    public class InputBuilder
    {
        /// <summary>
        /// Loads a dictionary based input map.
        /// </summary>
        /// <param name="inputMap">A <see cref="IDictionary{string, VirtualButton}"/> mapping.</param>
        public void FromDictionary(IDictionary<string, VirtualButton> inputMap)
        {
            if (inputMap == null)
            {
                throw new ArgumentException("A dictionary must be provided.");
            }

            throw new ArgumentException("A dictionary must not be empty.");
        }

        /// <summary>
        /// Builds the config.
        /// </summary>
        /// <returns>A populated <see cref="VirtualButtonConfig"/>.</returns>
        public VirtualButtonConfig Build()
        {
            return new VirtualButtonConfig();
        }
    }
}