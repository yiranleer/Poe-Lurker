﻿//-----------------------------------------------------------------------
// <copyright file="PlayerViewModel.cs" company="Wohs Inc.">
//     Copyright © Wohs Inc.
// </copyright>
//-----------------------------------------------------------------------

namespace Lurker.UI.ViewModels
{
    using System.Collections.ObjectModel;
    using Lurker.Models;
    using Lurker.Services;

    /// <summary>
    /// Represent a player.
    /// </summary>
    public class PlayerViewModel : Caliburn.Micro.PropertyChangedBase
    {
        #region Fields

        private PlayerService _service;
        private Player _activePlayer;
        private Player _selectedPlayer;
        private bool _selectionVisible;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerViewModel"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public PlayerViewModel(PlayerService service)
        {
            this._service = service;
            this._activePlayer = service.FirstPlayer;
            this.Players = new ObservableCollection<Player>();

            foreach (var player in service.Players)
            {
                this.Players.Add(player);
            }

            this._service.PlayerChanged += this.Service_PlayerChanged;
            this._service.PlayerListChanged += this.Service_PlayerListChanged;

            this.PropertyChanged += this.PlayerViewModel_PropertyChanged;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the players.
        /// </summary>
        public ObservableCollection<Player> Players { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name => this._activePlayer?.Name;

        /// <summary>
        /// Gets the level.
        /// </summary>
        public int Level => this._activePlayer.GetCurrentLevel();

        /// <summary>
        /// Gets or sets the selected player.
        /// </summary>
        public Player SelectedPlayer
        {
            get
            {
                return this._selectedPlayer;
            }

            set
            {
                if (this._selectedPlayer != value)
                {
                    this._selectedPlayer = value;
                    this.NotifyOfPropertyChange();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [selection visible].
        /// </summary>
        public bool SelectionVisible
        {
            get
            {
                return this._selectionVisible;
            }

            set
            {
                if (this._selectionVisible != value)
                {
                    this._selectionVisible = value;
                    this.NotifyOfPropertyChange();
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Toggles the selection.
        /// </summary>
        public void ToggleSelection()
        {
            this.SelectionVisible = !this.SelectionVisible;
        }

        /// <summary>
        /// Handles the PropertyChanged event of the PlayerViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void PlayerViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.SelectedPlayer) && this.SelectedPlayer != null)
            {
                this.ToggleSelection();
                this._service.ChangePlayer(this.SelectedPlayer);
            }
        }

        /// <summary>
        /// Services the player changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void Service_PlayerChanged(object sender, Player e)
        {
            this._activePlayer = e;
            this.NotifyOfPropertyChange(() => this.Name);
            this.NotifyOfPropertyChange(() => this.Level);
        }

        /// <summary>
        /// Services the player list changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void Service_PlayerListChanged(object sender, System.Collections.Generic.IEnumerable<Player> e)
        {
            this.Players.Clear();

            foreach (var player in e)
            {
                this.Players.Add(player);
            }
        }

        #endregion
    }
}