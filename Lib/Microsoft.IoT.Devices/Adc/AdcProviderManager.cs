﻿// Copyright (c) Microsoft. All rights reserved.
//
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Adc;
using Windows.Devices.Adc.Provider;
using Windows.Foundation;

namespace Microsoft.IoT.Devices.Adc
{
    /// <summary>
    /// An implementation of <see cref="IAdcProvider"/> that allows multiple ADC 
    /// controllers to be registered as a simple collection.
    /// </summary>
    /// <remarks>
    /// All controllers should be added to the <see cref="ControllerProviders"/> collection 
    /// before calling <see cref="AdcController.GetControllersAsync"/>.
    /// </remarks>
    public sealed class AdcProviderManager : IAdcProvider, IDisposable
    {
        #region Member Variables
        private Collection<IAdcControllerProvider> controllerProviders;
        #endregion // Member Variables

        #region Constructors
        /// <summary>
        /// Initializes a new <see cref="AdcProviderManager"/>.
        /// </summary>
        public AdcProviderManager()
        {
            controllerProviders = new Collection<IAdcControllerProvider>();
        }
        #endregion // Constructors

        #region Public Methods
        public void Dispose()
        {
            // Dispose and remove each provider
            for (int i = ControllerProviders.Count - 1; i > 0; i--)
            {
                var provider = ControllerProviders[i] as IDisposable;
                if (provider != null) { provider.Dispose(); }
                ControllerProviders.RemoveAt(i);
            }
        }

        /// <summary>
        /// Gets the <see cref="AdcController"/> instances for each controller provider.
        /// </summary>
        /// <returns>
        /// An <see cref="IAsyncOperation"/> that yields the list of controllers.
        /// </returns>
        public IAsyncOperation<IReadOnlyList<AdcController>> GetControllersAsync()
        {
            return AdcController.GetControllersAsync(this);
        }
        #endregion // Public Methods

        #region Public Properties
        /// <summary>
        /// Gets the collection of controller providers stored in the manager.
        /// </summary>
        /// <value>
        /// The collection of controller providers stored in the manager.
        /// </value>
        public IList<IAdcControllerProvider> ControllerProviders
        {
            get
            {
                return controllerProviders;
            }
        }
        #endregion // Public Properties

        #region IAdcProvider Interface
        IReadOnlyList<IAdcControllerProvider> IAdcProvider.GetControllers()
        {
            return controllerProviders;
        }
        #endregion // IAdcProvider Interface
    }
}
