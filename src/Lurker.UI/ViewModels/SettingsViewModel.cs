﻿//-----------------------------------------------------------------------
// <copyright file="SettingsViewModel.cs" company="Wohs">
//     Missing Copyright information from a valid stylecop.json file.
// </copyright>
//-----------------------------------------------------------------------

namespace Lurker.UI.ViewModels
{
    using Caliburn.Micro;
    using Lurker.Helpers;
    using Lurker.Services;
    using Lurker.UI.Helpers;

    public class SettingsViewModel: ScreenBase
    {
        #region Fields

        private KeyboardHelper _keyboardHelper;
        private SettingsService _settingService;
        private UpdateManager _updateManager;
        private bool _needsUpdate;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// </summary>
        public SettingsViewModel(IWindowManager windowManager, KeyboardHelper keyboardHelper, SettingsService settingsService, UpdateManager updateManager)
            : base(windowManager)
        {
            this._keyboardHelper = keyboardHelper;
            this._settingService = settingsService;
            this._updateManager = updateManager;
            this.DisplayName = "Settings";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether [needs update].
        /// </summary>
        public bool NeedsUpdate
        {
            get
            {
                return this._needsUpdate;
            }

            set
            {
                this._needsUpdate = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the busy message.
        /// </summary>
        public string BusyMessage
        {
            get
            {
                return this._settingService.BusyMessage;
            }

            set
            {
                this._settingService.BusyMessage = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the sold message.
        /// </summary>
        public string SoldMessage
        {
            get
            {
                return this._settingService.SoldMessage;
            }

            set
            {
                this._settingService.SoldMessage = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the thank you message.
        /// </summary>
        public string ThankYouMessage
        {
            get
            {
                return this._settingService.ThankYouMessage;
            }

            set
            {
                this._settingService.ThankYouMessage = value;
                this.NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// Gets or sets the still interested message.
        /// </summary>
        public string StillInterestedMessage
        {
            get
            {
                return this._settingService.StillInterestedMessage;
            }

            set
            {
                this._settingService.StillInterestedMessage = value;
                this.NotifyOfPropertyChange();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Inserts the buyer name token.
        /// </summary>
        public void InsertBuyerNameToken()
        {
            this._keyboardHelper.Write(TokenHelper.BuyerName);
        }

        /// <summary>
        /// Inserts the price token.
        /// </summary>
        public void InsertPriceToken()
        {
            this._keyboardHelper.Write(TokenHelper.Price);
        }

        /// <summary>
        /// Inserts the item name token.
        /// </summary>
        public void InsertItemNameToken()
        {
            this._keyboardHelper.Write(TokenHelper.ItemName);
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public async void Update()
        {
            await this._updateManager.Update();
        }

        /// <summary>
        /// Called when deactivating.
        /// </summary>
        /// <param name="close">Inidicates whether this instance will be closed.</param>
        protected override void OnDeactivate(bool close)
        {
            if (close)
            {
                this._settingService.Save();
            }

            base.OnDeactivate(close);
        }

        /// <summary>
        /// Called when activating.
        /// </summary>
        protected override void OnActivate()
        {
            this.CheckForUpdate();
            base.OnActivate();
        }

        /// <summary>
        /// Checks for update.
        /// </summary>
        private async void CheckForUpdate()
        {
            this.NeedsUpdate = await this._updateManager.CheckForUpdate();
        }

        #endregion
    }
}
