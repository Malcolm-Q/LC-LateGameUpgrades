using UnityEngine;

namespace MoreShipUpgrades.Misc
{
    /// <summary>
    /// Handler that's responsible to manipulate the game's scan node properties (which are displayed when right-clicking which displays several nodes with information such as scrap value or description of the items)
    /// </summary>
    internal class LguScanNodeProperties
    {
        /// <summary>
        /// Enumerator used to describe each node type used by the game
        /// </summary>
        internal enum NodeType
        {
            /// <summary>
            /// The blue background scan node used to mark the ship and main entrance
            /// </summary>
            GENERAL = 0,
            /// <summary>
            /// The red background scan node used to mark map hazards and enemies
            /// </summary>
            DANGER = 1,
            /// <summary>
            /// The green background scan node used to mark scrap items
            /// </summary>
            SCRAP = 2,
        }
        /// <summary>
        /// Updates the scan node of the GrabbableObject component to the new provided scrap value
        /// </summary>
        /// <preCondition>The GrabbableObject must contain a component in its children which has the ScanNodeProperties component</preCondition>
        /// <postCondition>The ScanNodeProperties component will have the new set scrap value</postCondition>
        /// <param name="grabbableObject">The scrap item we wish to change the scan node's displayed value of</param>
        /// <param name="scrapValue">The new scrap value to be set in the scrap item</param>
        public static void UpdateScrapValue(ref GrabbableObject grabbableObject, int scrapValue = -1)
        {
            ScanNodeProperties scanNodeProperties = grabbableObject.GetComponentInChildren<ScanNodeProperties>();
            ChangeScanNode(scanNodeProperties: ref scanNodeProperties, nodeType: (NodeType)scanNodeProperties.nodeType, header: scanNodeProperties.headerText, subText: $"Value: {scrapValue}", scrapValue: scrapValue);
        }
        /// <summary>
        /// Alters the respective attributes of the scan node properties to the provided ones
        /// </summary>
        /// <param name="scanNodeProperties">The component we wish to change the attributes of</param>
        /// <param name="nodeType">The type of node we wish to set on the component</param>
        /// <param name="header">The header text we wish to set on the component</param>
        /// <param name="subText">The text displayed below the header to set on the component</param>
        /// <param name="creatureScanID">The identifier of a creature to trigger a bestiary entry to set on the component</param>
        /// <param name="scrapValue">The value of the item in being sold to set on the component</param>
        /// <param name="minRange">The minimum range necessary to be displayed in the player's HUD to set on the component</param>
        /// <param name="maxRange">The maximum range allowed to be displayed in the player's HUD to set on the component</param>
        public static void ChangeScanNode(ref ScanNodeProperties scanNodeProperties, NodeType nodeType, string header = "LGU Scan Node", string subText = "Used for LGU stuff", int creatureScanID = -1, int scrapValue = 0, int minRange = 2, int maxRange = 7)
        {
            scanNodeProperties.headerText = header;
            scanNodeProperties.subText = subText;
            scanNodeProperties.nodeType = (int)nodeType;
            scanNodeProperties.creatureScanID = creatureScanID;
            scanNodeProperties.scrapValue = scrapValue;
            scanNodeProperties.minRange = minRange;
            scanNodeProperties.maxRange = maxRange;
        }
        /// <summary>
        /// Creates a new gameObject which will contain the scan node and adds it to the provided gameObject
        /// </summary>
        /// <param name="objectToAddScanNode">Gameobject that's gonna be added a scan node</param>
        /// <returns>Reference to the created gameObject with the scan node</returns>
        static GameObject CreateCanvasScanNode(ref GameObject objectToAddScanNode)
        {
            GameObject scanNodeObject = Object.Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), objectToAddScanNode.transform.position, Quaternion.Euler(Vector3.zero), objectToAddScanNode.transform);
            scanNodeObject.name = "ScanNode";
            scanNodeObject.layer = LayerMask.NameToLayer("ScanNode");
            Object.Destroy(scanNodeObject.GetComponent<MeshRenderer>());
            Object.Destroy(scanNodeObject.GetComponent<MeshFilter>());
            return scanNodeObject;
        }
        /// <summary>
        /// Adds a scan node to the provided gameObject and sets its attributes to the provided ones
        /// </summary>
        /// <param name="objectToAddScanNode">GameObject we wish to add the scan node on</param>
        /// <param name="nodeType">The type of scan node</param>
        /// <param name="header">Text displayed on the header of the scan node</param>
        /// <param name="subText">Text displayed below the header of the scan node</param>
        /// <param name="creatureScanID">Identifier of the bestiary entry to unlock when scanning this node</param>
        /// <param name="minRange">Minimum range required to display this node</param>
        /// <param name="maxRange">Maximum range allowed to display this node</param>
        static void AddScanNode(GameObject objectToAddScanNode, NodeType nodeType, string header = "LGU Scan Node", string subText = "Used for LGU stuff", int creatureScanID = -1, int minRange = 2, int maxRange = 7)
        {
            GameObject scanNodeObject = CreateCanvasScanNode(ref objectToAddScanNode);
            ScanNodeProperties scanNodeProperties = scanNodeObject.AddComponent<ScanNodeProperties>();
            ChangeScanNode(scanNodeProperties: ref scanNodeProperties, nodeType: nodeType, header: header, subText: subText, creatureScanID: creatureScanID, minRange: minRange, maxRange: maxRange);
        }
        /// <summary>
        /// Adds a general scan node to the provided gameObject and set its attributes to the provided ones
        /// </summary>
        /// <param name="objectToAddScanNode">GameObject we wish to add the scan node on</param>
        /// <param name="header">Text displayed on the header of the scan node</param>
        /// <param name="subText">Text displayed below the header of the scan node</param>
        /// <param name="creatureScanID">Identifier of the bestiary entry to unlock when scanning this node</param>
        /// <param name="minRange">Minimum range required to display this node</param>
        /// <param name="maxRange">Maximum range allowed to display this node</param>
        public static void AddGeneralScanNode(GameObject objectToAddScanNode, string header = "LGU Scan Node", string subText = "Used for LGU stuff", int creatureScanID = -1, int minRange = 2, int maxRange = 7)
        {
            AddScanNode(objectToAddScanNode: objectToAddScanNode, nodeType: NodeType.GENERAL, header: header, subText: subText, creatureScanID: creatureScanID, minRange: minRange, maxRange: maxRange);
        }
        /// <summary>
        /// Adds a scrap scan node to the provided gameObject and set its attributes to the provided ones
        /// </summary>
        /// <param name="objectToAddScanNode">GameObject we wish to add the scan node on</param>
        /// <param name="header">Text displayed on the header of the scan node</param>
        /// <param name="subText">Text displayed below the header of the scan node</param>
        /// <param name="creatureScanID">Identifier of the bestiary entry to unlock when scanning this node</param>
        /// <param name="minRange">Minimum range required to display this node</param>
        /// <param name="maxRange">Maximum range allowed to display this node</param>
        public static void AddScrapScanNode(GameObject objectToAddScanNode, string header = "LGU Scan Node", string subText = "Used for LGU stuff", int creatureScanID = -1, int minRange = 2, int maxRange = 7)
        {
            AddScanNode(objectToAddScanNode: objectToAddScanNode, nodeType: NodeType.SCRAP, header: header, subText: subText, creatureScanID: creatureScanID, minRange: minRange, maxRange: maxRange);
        }
        /// <summary>
        /// Adds a danger scan node to the provided gameObject and set its attributes to the provided ones
        /// </summary>
        /// <param name="objectToAddScanNode">GameObject we wish to add the scan node on</param>
        /// <param name="header">Text displayed on the header of the scan node</param>
        /// <param name="subText">Text displayed below the header of the scan node</param>
        /// <param name="creatureScanID">Identifier of the bestiary entry to unlock when scanning this node</param>
        /// <param name="minRange">Minimum range required to display this node</param>
        /// <param name="maxRange">Maximum range allowed to display this node</param>
        public static void AddDangerScanNode(GameObject objectToAddScanNode, string header = "LGU Scan Node", string subText = "Used for LGU stuff", int creatureScanID = -1, int minRange = 2, int maxRange = 7)
        {
            AddScanNode(objectToAddScanNode: objectToAddScanNode, nodeType: NodeType.DANGER, header: header, subText: subText, creatureScanID: creatureScanID, minRange: minRange, maxRange: maxRange);
        }
        /// <summary>
        /// Destroys the component associated with scan node
        /// </summary>
        /// <param name="objectToRemoveScanNode">GameObject we want to remove the scan node from</param>
        public static void RemoveScanNode(GameObject objectToRemoveScanNode)
        {
            Object.Destroy(objectToRemoveScanNode.GetComponentInChildren<ScanNodeProperties>());
        }
    }
}
