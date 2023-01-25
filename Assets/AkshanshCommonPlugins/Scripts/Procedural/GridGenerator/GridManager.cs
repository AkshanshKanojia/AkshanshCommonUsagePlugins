using System.Collections.Generic;
using UnityEngine;

namespace AkshanshKanojia.LevelEditors
{
    public class GridManager : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Grid Properties")]
        [SerializeField] float cellSize = 1f;
        [SerializeField] bool updateWithObjectTransform;

        [SerializeField]
        int xSize = 10, zSize = 10;

        [Header("Editor Properties")]
        [SerializeField] bool showInEditor = true, showMidPoint = false;
        [SerializeField] Color vertColor;
        [SerializeField] float vertSize = 1f;
        #endregion

        #region Private Fields
        Vector3[] gridVertices;
        #endregion

        //can be used to genrate faces if needed later
        [System.Serializable]
        public class CellHolder
        {
            public Vector3 bottomLeft, topLeft, bottomRight, topRight, midPos;
            public int EntitiesInCell = 0;//assign this manually (for game need)
            public void GetMidPoint()
            {
                midPos.x = (topLeft - bottomLeft).magnitude / 2;
                midPos.z = (topLeft - bottomLeft).magnitude / 2;
                midPos += bottomLeft;//origin of cell start from bottom left
            }
        }
        public List<CellHolder> cells;

        #region private methods
        private void Start()
        {
            GenerateGrid();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = vertColor;
            if (showInEditor)
            {
                try
                {
                    foreach (var v in gridVertices)
                    {
                        if (v != null)
                        {
                            Gizmos.DrawSphere(v, vertSize);
                        }
                    }
                    if(showMidPoint)
                    {
                        foreach(var v in cells)
                        {
                            Gizmos.DrawSphere(v.midPos, vertSize);
                        }
                    }
                }
                catch
                {
                    GenerateGrid();
                }
            }

        }
        void GetCellData()
        {
            cells.Clear();
            int _zFixIndex = 0;
            for (int i = 0; i < (xSize - 1) * (zSize - 1); i++)
            {
                if (i % (zSize - 1) == 0 && i != 0)
                {
                    _zFixIndex++;
                }
                CellHolder _temp = new CellHolder()
                {
                    bottomLeft = gridVertices[i + _zFixIndex],
                    topLeft = gridVertices[i + 1 + _zFixIndex],
                    topRight = gridVertices[i + zSize + 1 + _zFixIndex],
                    bottomRight = gridVertices[i + zSize + _zFixIndex]
                };
                _temp.GetMidPoint();
                cells.Add(_temp);
            }
        }
        #endregion

        #region public methods
        public void GenerateGrid()
        {
            gridVertices = new Vector3[xSize * zSize];
            cells.Clear();
            //generate all vertices
            int _tempVertIndex = 0;
            Vector3 _tempTransform = (updateWithObjectTransform) ? transform.position : Vector3.zero;
            for (int i = 0; i < xSize; i++)
            {
                for (int j = 0; j < zSize; j++)
                {
                    gridVertices[_tempVertIndex] = new Vector3(i, 0, j) * cellSize + _tempTransform;
                    gridVertices[_tempVertIndex] = transform.rotation *
                        gridVertices[_tempVertIndex];//apply rotation on vector
                    _tempVertIndex++;
                }
            }
            if (xSize >= 1 && zSize >= 1)
                GetCellData();

        }
        public void ClearGrid()
        {
            xSize = 0;
            zSize = 0;
            cells.Clear();
        }

        public int GetNearestCellIndex(Vector3 _pos)
        {
            //can be optimized by adding algorithms, following linear search now
            for (int i = 0; i < cells.Count; i++)
            {
                _pos.y = transform.position.y;//exclude y distance for snap
                if (Vector3.Distance(_pos, cells[i].midPos) < cellSize / 2)
                {
                    return i;
                }
            }
            return -1;
        }

        public void SetGridSize(int _x,int _z)
        {
            xSize = _x;
            zSize = _z;
            GenerateGrid();
        }
        public void SetCellSize(float _size)
        {
            cellSize = _size;
            GenerateGrid();
        }
        #endregion
    }
}