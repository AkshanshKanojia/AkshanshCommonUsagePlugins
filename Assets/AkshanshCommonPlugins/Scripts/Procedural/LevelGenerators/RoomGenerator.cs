using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AkshanshKanojia.LevelEditors
{
    public class RoomGenerator : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Genral Properties")]
        [SerializeField] string roomContainerName = "Room";
        [SerializeField] int gridXSize = 5, gridYSize = 5;

        [Header("Floor Properties")]
        [SerializeField] FloorDataHolder[] AvailableFloors;
        [System.Serializable]
        struct FloorDataHolder
        {
            public GameObject FloorPrefab;
            public FloorPlacementOptions PlacementType;
        }
        #endregion

        #region Public Fields
        public enum FloorPlacementOptions { Centere,Sides,Fill}
        #endregion

        #region Private Fields
        #endregion
    }
}
