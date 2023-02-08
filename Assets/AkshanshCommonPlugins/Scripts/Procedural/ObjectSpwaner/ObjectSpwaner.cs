using UnityEngine;

namespace AkshanshKanojia.LevelEditors
{
    public class ObjectSpwaner : MonoBehaviour
    {
        #region Public Fields
        public enum AvailableGenerationModes
        {
            RandomGrid, OrderedGrid, RandomInRadius,
            RandomBetweenVectors, RemoveRandomObjects
        }
        public AvailableGenerationModes GenerationMode;//determines how generation algorithm takes place
        //grid properties
        [HideInInspector] public float GridCellSize = 1f;
        [HideInInspector] public int GridXSize = 10,GridZSize = 10;
        [HideInInspector] public bool RandomizeInsideCell = false;
        //Generation Properties
        [HideInInspector] public bool AvoidObjectOverlaps,SkipOnOverlap;
        [HideInInspector] public float OverlapDetectionRadius = 1f;
        [HideInInspector] public int MaxOverlapItteration = 10;
        //debug properties
        [HideInInspector] public bool ShowGrid;
        [HideInInspector] public float GridVertSize = 0.1f;

        #endregion

        #region Serialized Fields
        [SerializeField,Tooltip("Total number of objects to generate/Remove")] 
        int MaxObjectsToGenerate = 10;
        #endregion

        #region Private Fields
        GridManager gridMang;
        #endregion

        void Initalize()
        {
            if (!gridMang)
                gridMang = gameObject.AddComponent<GridManager>();
            gridMang.hideFlags = HideFlags.HideInInspector;
            gridMang.vertColor = Color.red;
            gridMang.SetCellSize(GridCellSize);
            gridMang.SetGridSize(GridXSize,GridZSize);
            gridMang.vertSize = GridVertSize;
            gridMang.updateWithObjectTransform = true;
            gridMang.GenerateGrid();
        }
        public void GenerateObjects()
        {
            Initalize();
            print("done now die");
        }

        public void SetGridDebug()
        {
            Initalize();
            gridMang.showInEditor = ShowGrid;
            gridMang.vertColor = Color.red;
        }
    }
}
