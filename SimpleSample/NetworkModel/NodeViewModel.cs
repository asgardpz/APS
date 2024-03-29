﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using Utils;

namespace NetworkModel
{
    /// <summary>
    /// Defines a node in the view-model.
    /// Nodes are connected to other nodes through attached connectors (aka connection points).
    /// </summary>
    public sealed class NodeViewModel : AbstractModelBase
    {
        #region Internal Data Members

        /// <summary>
        /// The name of the node.
        /// </summary>
        private string name = string.Empty;

        /// <summary>
        /// The X coordinate for the position of the node.
        /// </summary>
        private double x = 0;

        /// <summary>
        /// The Y coordinate for the position of the node.
        /// </summary>
        private double y = 0;

        /// <summary>
        /// Set to 'true' when the node is selected.
        /// </summary>
        private bool isSelected = false;

        //設定圖檔
        private string image = string.Empty;
        //設定連結次數
        private int source = 0;
        private int dest = 0;
        /// <summary>
        /// List of input connectors (connections points) attached to the node.
        /// </summary>
        private ImpObservableCollection<ConnectorViewModel> connectors = null;

        #endregion Internal Data Members

        public NodeViewModel()
        {
        }

        public NodeViewModel(string name,string image)
        {
            this.name = name;
            this.image = image;
        }

        /// <summary>
        /// The name of the node.
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name == value)
                {
                    return;
                }

                name = value;

                OnPropertyChanged("Name");
            }
        }

        public string Image
        {
            get
            {
                return image;
            }
            set
            {
                if (image == value)
                {
                    return;
                }

                image = value;

                OnPropertyChanged("Image");
            }
        }
        /// <summary>
        /// The X coordinate for the position of the node.
        /// </summary>
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                if (value >= 0)
                {
                    if (x == value)
                    {
                        return;
                    }

                    x = value;

                    OnPropertyChanged("X");
                }

            }
        }

        /// <summary>
        /// The Y coordinate for the position of the node.
        /// </summary>
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                if (value >= 0)
                {
                    if (y == value)
                    {
                        return;
                    }

                    y = value;

                    OnPropertyChanged("Y");
                }

            }
        }

        public int Source
        {
            get
            {
                return source;
            }
            set
            {
                if (value >= 0)
                {
                    if (source == value)
                    {
                        return;
                    }

                    source = value;

                    OnPropertyChanged("Source");
                }

            }
        }
        public int Dest
        {
            get
            {
                return dest;
            }
            set
            {
                if (value >= 0)
                {
                    if (dest == value)
                    {
                        return;
                    }

                    dest = value;

                    OnPropertyChanged("Dest");
                }

            }
        }
        /// <summary>
        /// List of connectors (connection anchor points) attached to the node.
        /// </summary>
        public ImpObservableCollection<ConnectorViewModel> Connectors
        {
            get
            {
                if (connectors == null)
                {
                    connectors = new ImpObservableCollection<ConnectorViewModel>();
                    connectors.ItemsAdded += new EventHandler<CollectionItemsChangedEventArgs>(connectors_ItemsAdded);
                    connectors.ItemsRemoved += new EventHandler<CollectionItemsChangedEventArgs>(connectors_ItemsRemoved);
                }

                return connectors;
            }
        }

        /// <summary>
        /// A helper property that retrieves a list (a new list each time) of all connections attached to the node. 
        /// </summary>
        public ICollection<ConnectionViewModel> AttachedConnections
        {
            get
            {
                List<ConnectionViewModel> attachedConnections = new List<ConnectionViewModel>();

                foreach (var connector in this.Connectors)
                {
                    if (connector.AttachedConnection != null)
                    {
                        attachedConnections.Add(connector.AttachedConnection);
                    }
                }

                return attachedConnections;
            }
        }

        /// <summary>
        /// Set to 'true' when the node is selected.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                if (isSelected == value)
                {
                    return;
                }

                isSelected = value;

                OnPropertyChanged("IsSelected");
            }
        }

        #region Private Methods

        /// <summary>
        /// Event raised when connectors are added to the node.
        /// </summary>
        private void connectors_ItemsAdded(object sender, CollectionItemsChangedEventArgs e)
        {
            foreach (ConnectorViewModel connector in e.Items)
            {
                connector.ParentNode = this;
            }
        }

        /// <summary>
        /// Event raised when connectors are removed from the node.
        /// </summary>
        private void connectors_ItemsRemoved(object sender, CollectionItemsChangedEventArgs e)
        {
            foreach (ConnectorViewModel connector in e.Items)
            {
                connector.ParentNode = null;
            }
        }

        #endregion Private Methods
    }
}
