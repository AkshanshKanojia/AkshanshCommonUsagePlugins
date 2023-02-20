using UnityEngine;
using System.Collections.Generic;

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
        [HideInInspector] public bool RandomizeInsideCell = false,MatchSurfaceHeight = false;
        //Generation Properties
        [HideInInspector] public bool AvoidObjectOverlaps,SkipOnOverlap,GenerateInExistingObject;
        [HideInInspector] public int MaxOverlapItteration = 10;

        [System.Serializable]
        public struct SpwanableObjectHolder
        {
            public GameObject ObjectPrefab;
            public float ObjectAvoidanceRadius;
        }
        public SpwanableObjectHolder[] SpwanablePrefabs;
        [HideInInspector] public string HolderName = "Generated props";
        //debug properties
        [HideInInspector] public bool ShowGrid;
        [HideInInspector] public float GridVertSize = 0.1f;

        #endregion

        #region Serialized Fields
        [SerializeField,Tooltip("Total number of objects to generate/Remove")] 
        int MaxObjectsToGenerate = 10;
        #endregion

        #region Private Fields
        [System.Serializable]
        struct GeneratedObjectData
        {
            public GameObject TargetObj;
            public Vector3 SpawnPos;
        }

        List<GeneratedObjectData> CurtActiveObjects;
        GridManager gridMang;
        enum ObjectGenrationFilters { FixedInRadius,RandomInRadius}
        #endregion

        void Initalize()//generates basic componenets required for generation
        {
            gridMang = GetComponent<GridManager>();
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

        /// <summary>
        /// Generates a single object depending onnvalues passed initilly on list
        /// </summary>
        bool GenerateSingleObject(ObjectGenrationFilters _filter)
        {
            if (CurtActiveObjects.Count >= gridMang.cells.Count)
            {
                Debug.LogWarning("Breaking Generation at " + CurtActiveObjects.Count + " beacuse it exceeds grid size!");
                return false;
            }
            int _genIndex = Random.Range(0, SpwanablePrefabs.Length);
            Transform _tempObj = Instantiate(SpwanablePrefabs[_genIndex].ObjectPrefab).transform;
            _tempObj.position = gridMang.cells[CurtActiveObjects.Count].midPos;
            if(_filter == ObjectGenrationFilters.RandomInRadius)
            {//Note: do surface raycasting first :D
                //randomize pos
              // _tempObj.position += 
            }
            CurtActiveObjects.Add(new GeneratedObjectData
            {
                TargetObj = _tempObj.gameObject,
            });
            return true;
        }

        public void GenerateObjects()//generates object based on generation type
        {
            Initalize();
            CurtActiveObjects = new List<GeneratedObjectData>();
            switch(GenerationMode)
            {
                case AvailableGenerationModes.OrderedGrid:
                    #region Ordered grid Gen
                    //generation
                    for(int i=0;i<MaxObjectsToGenerate;i++)
                    {
                        GenerateSingleObject(ObjectGenrationFilters.FixedInRadius);
                    }
                    #endregion
                    break;
                default:
                    break;
            }
        }

        public void SetGridDebug()
        {
            Initalize();
            gridMang.showInEditor = ShowGrid;
            gridMang.vertColor = Color.red;
        }
    }
}
