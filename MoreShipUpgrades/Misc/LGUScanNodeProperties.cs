using UnityEngine;

namespace MoreShipUpgrades.Misc
{
    internal class LGUScanNodeProperties
    {
        internal enum NodeType
        {
            GENERAL = 0,
            DANGER = 1,
            SCRAP = 2,
        }
        public static void UpdateScrapValue(ref GrabbableObject grabbableObject, int scrapValue = -1)
        {
            ScanNodeProperties scanNodeProperties = grabbableObject.GetComponentInChildren<ScanNodeProperties>();
            ChangeScanNode(scanNodeProperties: ref scanNodeProperties, nodeType: (NodeType)scanNodeProperties.nodeType, header: scanNodeProperties.headerText, subText: $"Value: {scrapValue}", scrapValue: scrapValue);
        }
        public static void ChangeScanNode(ref ScanNodeProperties scanNodeProperties, NodeType nodeType, string header = "LGU Scan Node", string subText = "Used for LGU stuff", int creatureScanID = -1, int scrapValue = 0, int minRange = 5, int maxRange = 7)
        {
            scanNodeProperties.headerText = header;
            scanNodeProperties.subText = subText;
            scanNodeProperties.nodeType = (int)nodeType;
            scanNodeProperties.creatureScanID = creatureScanID;
            scanNodeProperties.scrapValue = scrapValue;
            scanNodeProperties.minRange = minRange;
            scanNodeProperties.maxRange = maxRange;
        }
        static GameObject CreateCanvasScanNode(ref GameObject objectToAddScanNode)
        {
            GameObject scanNodeObject = Object.Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), objectToAddScanNode.transform.position, Quaternion.Euler(Vector3.zero), objectToAddScanNode.transform);
            scanNodeObject.name = "ScanNode";
            scanNodeObject.layer = LayerMask.NameToLayer("ScanNode");
            Object.Destroy(scanNodeObject.GetComponent<MeshRenderer>());
            Object.Destroy(scanNodeObject.GetComponent<MeshFilter>());
            return scanNodeObject;
        }
        static void AddScanNode(GameObject objectToAddScanNode, NodeType nodeType, string header = "LGU Scan Node", string subText = "Used for LGU stuff", int creatureScanID = -1, int minRange = 5, int maxRange = 7)
        {
            GameObject scanNodeObject = CreateCanvasScanNode(ref objectToAddScanNode);
            ScanNodeProperties scanNodeProperties = scanNodeObject.AddComponent<ScanNodeProperties>();
            ChangeScanNode(ref scanNodeProperties, nodeType, header, subText, creatureScanID, minRange, maxRange);
        }
        public static void AddGeneralScanNode(GameObject objectToAddScanNode, string header = "LGU Scan Node", string subText = "Used for LGU stuff", int creatureScanID = -1, int minRange = 5, int maxRange = 7)
        {
            AddScanNode(objectToAddScanNode: objectToAddScanNode, nodeType: NodeType.GENERAL, header: header, subText: subText, creatureScanID: creatureScanID, minRange: minRange, maxRange: maxRange);
        }

        public static void AddScrapScanNode(GameObject objectToAddScanNode, string header = "LGU Scan Node", string subText = "Used for LGU stuff", int creatureScanID = -1, int minRange = 5, int maxRange = 7)
        {
            AddScanNode(objectToAddScanNode: objectToAddScanNode, nodeType: NodeType.SCRAP, header: header, subText: subText, creatureScanID: creatureScanID, minRange: minRange, maxRange: maxRange);
        }
        public static void AddDangerScanNode(GameObject objectToAddScanNode, string header = "LGU Scan Node", string subText = "Used for LGU stuff", int creatureScanID = -1, int minRange = 5, int maxRange = 7)
        {
            AddScanNode(objectToAddScanNode: objectToAddScanNode, nodeType: NodeType.DANGER, header: header, subText: subText, creatureScanID: creatureScanID, minRange: minRange, maxRange: maxRange);
        }
        public static void RemoveScanNode(GameObject objectToRemoveScanNode)
        {
            Object.Destroy(objectToRemoveScanNode.GetComponentInChildren<ScanNodeProperties>());
        }
    }
}
