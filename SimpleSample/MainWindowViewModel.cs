using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkModel;
using Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.IO;


namespace SampleCode
{
    /// <summary>
    /// The view-model for the main window.
    /// </summary>
    public class MainWindowViewModel : AbstractModelBase
    {
        #region Internal Data Members
        public NetworkViewModel network = null;

        #endregion Internal Data Members
        public MainWindowViewModel()
        {
            //預覽
            //PopulateWithTestData();
        }

        //回傳NetWork
        public NetworkViewModel Network
        {
            get
            {
                return network;
            }
            set
            {
                network = value;

                OnPropertyChanged("Network");
            }
        }

        /// <summary>
        /// Called when the user has started to drag out a connector, thus creating a new connection.
        /// </summary>
        public ConnectionViewModel ConnectionDragStarted(ConnectorViewModel draggedOutConnector, Point curDragPoint)
        {
            if (draggedOutConnector.AttachedConnection != null)
            {
                //
                // There is an existing connection attached to the connector that has been dragged out.
                // Remove the existing connection from the view-model.
                //
                this.Network.Connections.Remove(draggedOutConnector.AttachedConnection);
            }

            //
            // Create a new connection to add to the view-model.
            //
            var connection = new ConnectionViewModel();

            //
            // Link the source connector to the connector that was dragged out.
            //
            connection.SourceConnector = draggedOutConnector;

            //
            // Set the position of destination connector to the current position of the mouse cursor.
            //
            connection.DestConnectorHotspot = curDragPoint;

            //
            // Add the new connection to the view-model.
            //
            this.Network.Connections.Add(connection);

            return connection;
        }

        /// <summary>
        /// Called as the user continues to drag the connection.
        /// </summary>
        public void ConnectionDragging(ConnectionViewModel connection, Point curDragPoint)
        {
            //
            // Update the destination connection hotspot while the user is dragging the connection.
            //
            connection.DestConnectorHotspot = curDragPoint;
        }

        /// <summary>
        /// Called when the user has finished dragging out the new connection.
        /// </summary>
        public void ConnectionDragCompleted(ConnectionViewModel newConnection, ConnectorViewModel connectorDraggedOut, ConnectorViewModel connectorDraggedOver)
        {
            if (connectorDraggedOver == null)
            {
                //
                // The connection was unsuccessful.
                // Maybe the user dragged it out and dropped it in empty space.
                //
                this.Network.Connections.Remove(newConnection);
                return;
            }

            //
            // The user has dragged the connection on top of another valid connector.
            //

            var existingConnection = connectorDraggedOver.AttachedConnection;
            if (existingConnection != null)
            {
                //
                // There is already a connection attached to the connector that was dragged over.
                // Remove the existing connection from the view-model.
                //
                this.Network.Connections.Remove(existingConnection);
            }

            //
            // Finalize the connection by attaching it to the connector
            // that the user dropped the connection on.
            //
            newConnection.DestConnector = connectorDraggedOver;
        }

        /// <summary>
        /// Delete the currently selected nodes from the view-model.
        /// </summary>
        public void DeleteSelectedNodes()
        {
            // Take a copy of the nodes list so we can delete nodes while iterating.
            var nodesCopy = this.Network.Nodes.ToArray();

            foreach (var node in nodesCopy)
            {
                if (node.IsSelected)
                {
                    DeleteNode(node);
                }
            }
        }

        /// <summary>
        /// Delete the node from the view-model.
        /// Also deletes any connections to or from the node.
        /// </summary>
        public void DeleteNode(NodeViewModel node)
        {
            //
            // Remove all connections attached to the node.
            //
            this.Network.Connections.RemoveRange(node.AttachedConnections);

            //
            // Remove the node from the network.
            //
            this.Network.Nodes.Remove(node);
        }

        public void ConnSelectedNodes()
        {
            // Take a copy of the nodes list so we can delete nodes while iterating.
            var nodesCopy = this.Network.Nodes.ToArray();

            foreach (var node in nodesCopy)
            {
                if (node.IsSelected)
                {
                    ConnNode(node, node);
                }
            }
        }

        /// <summary>
        /// Delete the node from the view-model.
        /// Also deletes any connections to or from the node.
        /// </summary>
        public void ConnNode(NodeViewModel node_source, NodeViewModel node_dest)
        {

            //判定第二次的Source
            var connection = new ConnectionViewModel();
            if (node_source.Source > 0)
            {
                connection.Color = "#FFF4BC0E";
            }
            connection.SourceConnector = node_source.Connectors[0+ node_source.Source];
            connection.SourceConnectorHotspot = node_source.Connectors[0+ node_source.Source].Hotspot;
            connection.DestConnector = node_dest.Connectors[1+ node_dest.Dest];
            connection.DestConnectorHotspot = node_dest.Connectors[1+ node_dest.Dest].Hotspot;
            this.Network.Connections.Add(connection);
            node_source.Source += 2;
            node_dest.Dest += 2;

        }
        /// <summary>
        /// Create a node and add it to the view-model.
        /// </summary>
        public NodeViewModel CreateNode(string name, Point nodeLocation,string image)
        {
            var node = new NodeViewModel(name, image);
            node.X = nodeLocation.X;
            node.Y = nodeLocation.Y;

            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            node.Connectors.Add(new ConnectorViewModel());
            this.Network.Nodes.Add(node);

            return node;
        }

        #region Private Methods

        /// <summary>
        /// A function to conveniently populate the view-model with test data.
        /// </summary>
        private void PopulateWithTestData()
        {
            //
            // Create a network, the root of the view-model.
            //
            this.Network = new NetworkViewModel();
            //
            // Create some nodes and add them to the view-model.
            //
            var node1 = CreateNode("開始", new Point(10, 10),"");
            var node2 = CreateNode("結束", new Point(200, 10),"");

            //
            // Create a connection between the nodes.
            //
            var connection = new ConnectionViewModel();
            connection.SourceConnector = node1.Connectors[1];
            connection.DestConnector = node2.Connectors[3];

            //
            // Add the connection to the view-model.
            //
            this.Network.Connections.Add(connection);

        }

        #endregion Private Methods
    }
}
