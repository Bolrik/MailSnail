using FreschGames.Core.Misc;
using MailSnail.Game;
using MailSnail.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MailSnail.Board
{
    public class BoardStateGenerator
    {
        const byte CellState_Empty = 0;
        const byte CellState_Blocked = 1;
        const byte CellState_Link = 2;

        GameData GameData { get; set; }
        System.Random Random { get; set; }


        private int ID { get; set; }

        private int ScaleX { get; set; }
        private int ScaleY { get; set; }

        private float NoiseOffsetX { get; set; }
        private float NoiseOffsetY { get; set; }

        byte[,] CellStates { get; set; }
        int[,] FeatureMap { get; set; }
        Int2D FeatureSpot { get; set; }
        BoardLink[] Links { get; set; }

        public BoardStateGenerator(int id, int scaleX, int scaleY, GameData gameData)
        {
            this.GameData = gameData;

            this.ID = id;
            this.ScaleX = scaleX;
            this.ScaleY = scaleY;
        }

        private void ResetCellStates()
        {
            this.CellStates = new byte[this.ScaleX, this.ScaleY];
            this.FeatureMap = new int[this.ScaleX, this.ScaleY];
            this.FeatureSpot = new Int2D();
            this.Links = null;

            for (int y = 0; y < this.ScaleY; y++)
            {
                for (int x = 0; x < this.ScaleX; x++)
                {
                    this.CellStates[x, y] = CellState_Blocked;

                    //if (x == 0 ||
                    //    x == this.ScaleX - 1 ||
                    //    y == 0 ||
                    //    y == this.ScaleY - 1)
                    //    this.CellStates[x, y] = CellState_Blocked;
                    //else
                    //    this.CellStates[x, y] = CellState_Empty;
                }
            }
        }

        public BoardState Create()
        {
            this.Random = new System.Random(this.ID);

            this.NoiseOffsetX = this.Random.Next() / 1000f;
            this.NoiseOffsetY = this.Random.Next() / 1000f;

            this.ResetCellStates();

            this.CreateLinks(true);
            this.ConnectLinks();

            this.EnsureLinkConnections();
            this.CreateFeatureMap();

            TileState[,] tileStates = this.ConvertCellStates();

            BoardState toReturn = new BoardState(this.ID, tileStates, this.Links, this.FeatureSpot);
            
            return toReturn;
        }

        private void ApplyLink(BoardLink link)
        {
            foreach (var position in link.Positions)
            {
                this.CellStates[position.X, position.Y] = CellState_Link;
            }
        }

        private void EnsureLinkConnections()
        {
            this.MakeAccessable(
                this.Links[0].Direction.OffsetDirection(this.Links[0].Positions[1], true),
                this.Links[2].Direction.OffsetDirection(this.Links[2].Positions[1], true));
            this.MakeAccessable(
                this.Links[1].Direction.OffsetDirection(this.Links[1].Positions[1], true),
                this.Links[3].Direction.OffsetDirection(this.Links[3].Positions[1], true));
        }

        private bool MakeAccessable(Int2D pointA, Int2D pointB)
        {
            List<Int2D> poolA = this.FloodFill(pointA, CellState_Empty);

            if (poolA.Contains(pointB))
                return true;

            List<Int2D> poolB = this.FloodFill(pointB, CellState_Empty);

            // Debug.Log("Make Accessable");

            int Distance(Int2D a, Int2D b)
            {
                return (a.X - b.X).Abs() + (a.Y - b.Y).Abs();
            }

            Int2D bA = poolA[0];
            Int2D bB = poolB[0];
            int bDis = Distance(bA, bB);

            foreach (var tileA in poolA)
            {
                foreach (var tileB in poolB)
                {
                    int distance = Distance(tileA, tileB);

                    if (distance < bDis)
                    {
                        bDis = distance;
                        bA = tileA;
                        bB = tileB;
                    }
                }
            }

            var directPath = this.GenerateDirectPath(bA, bB);

            foreach (var item in directPath)
            {
                this.CellStates[item.X, item.Y] = CellState_Empty;
            }

            // Debug.Log($"Best To Connect {bA} > {bB}");

            return true;
        }

        private TileState[,] ConvertCellStates()
        {
            TileState[,] boardStateTiles = new TileState[this.ScaleX, this.ScaleY];

            for (int y = 0; y < this.ScaleY; y++)
            {
                for (int x = 0; x < this.ScaleX; x++)
                {
                    int cellState = this.CellStates[x, y];

                    TileData tileData = null;
                    TileContentData tileContentData = null;

                    if (cellState == CellState_Blocked)
                    {
                        tileData = this.GameData.TileDataPool.Grass;
                        tileContentData = this.GameData.TileContentDataPool.Tree;
                    }
                    else if (cellState == CellState_Empty)
                    {
                        float noiseValue = Mathf.PerlinNoise(this.NoiseOffsetX + x * .1f, this.NoiseOffsetY + y * .1f);
                        tileData = noiseValue > .4 ? this.GameData.TileDataPool.Grass : this.GameData.TileDataPool.Ground;
                    }
                    else if (cellState == CellState_Link)
                    {
                        tileData = this.GameData.TileDataPool.Grass;
                        tileContentData = this.GameData.TileContentDataPool.Rock;
                    }

                    //if (this.FeatureSpot.X == x && this.FeatureSpot.Y == y)
                    //{
                    //    // Debug.Log($"Feature Spot is: {x}/{y}");
                    //    tileContentData = this.GameData.TileContentDataPool.Town;
                    //}

                    boardStateTiles[x, y] = new TileState(x, y, tileData, tileContentData);
                    boardStateTiles[x, y].Accessibility = this.FeatureMap[x, y];
                }
            }

            return boardStateTiles;
        }

        private void CreateLinks(bool autoApply)
        {
            BoardLink[] toReturn = new BoardLink[4];
            Direction2D[] directions = new Direction2D[]
            {
                Direction2D.North, Direction2D.East, Direction2D.South, Direction2D.West
            };

            Range<int> xRange = new Range<int>(2, this.ScaleX - 3);
            Range<int> yRange = new Range<int>(2, this.ScaleY - 3);



            for (int i = 0; i < 4; i++)
            {
                Direction2D direction = directions[i];

                int x = 0,
                    y = 0;

                switch (direction)
                {
                    case Direction2D.North:
                        x = this.Random.Next(xRange.Min, xRange.Max);
                        y = this.ScaleY - 1;
                        break;
                    case Direction2D.East:
                        x = this.ScaleX - 1;
                        y = this.Random.Next(yRange.Min, yRange.Max);
                        break;
                    case Direction2D.South:
                        x = this.Random.Next(xRange.Min, xRange.Max);
                        y = 0;
                        break;
                    case Direction2D.West:
                        x = 0;
                        y = this.Random.Next(yRange.Min, yRange.Max);
                        break;
                }

                Int2D[] positions = new Int2D[3];
                Int2D directionOffset = direction.GetOffset();

                for (int j = -1; j <= 1; j++)
                {
                    positions[j + 1] = new Int2D(x + directionOffset.Y * j, y + directionOffset.X * j);
                }

                BoardLink link = new BoardLink(direction, positions);
                toReturn[i] = link;

                if (autoApply)
                {
                    this.ApplyLink(link);
                }
            }

            this.Links = toReturn;
        }

        private void ConnectLinks()
        {
            int length = this.Links.Length;

            for (int i = 0; i < length; i++)
            {
                var link1 = this.Links[i];
                int j = i;
                while (i == j) j = this.Random.Next(length);
                var link2 = this.Links[j];

                Int2D pointA = link1.Direction.OffsetDirection(link1.Positions[1], true);
                Int2D pointB = link2.Direction.OffsetDirection(link2.Positions[1], true);

                var path = this.GenerateChaoticPath(pointA, pointB);

                foreach (var tilePos in path)
                {
                    // this.CellStates[tilePos.x, tilePos.y] = CellState_Empty;
                    this.CellStates[tilePos.X, tilePos.Y] = CellState_Empty;
                }
            }
        }

        private void CreateFeatureMap()
        {
            Direction2D[] directions = new Direction2D[]
            {
                Direction2D.North,
                Direction2D.East,
                Direction2D.South,
                Direction2D.West
            };

            foreach (var link in this.Links)
            {
                Int2D startingPoint = link.Direction.OffsetDirection(link.Positions[1], true);

                int[,] localAccessMap = new int[this.ScaleX, this.ScaleY];
                bool[,] visited = new bool[this.ScaleX, this.ScaleY];

                List<Int2D> nextList = new List<Int2D>();
                nextList.Add(startingPoint);
                visited[startingPoint.X, startingPoint.Y] = true;

                int distance = 0;
                while (nextList.Count > 0)
                {
                    List<Int2D> currentList = nextList;
                    nextList = new List<Int2D>();

                    foreach (Int2D current in currentList)
                    {
                        if (this.CellStates[current.X, current.Y] != CellState_Empty)
                            continue;

                        localAccessMap[current.X, current.Y] = distance;

                        foreach (Direction2D direction in directions)
                        {
                            Int2D next = direction.OffsetDirection(current);

                            if (next.X < 0
                                || next.Y < 0
                                || next.X >= this.ScaleX
                                || next.Y >= this.ScaleY
                                || visited[next.X, next.Y])
                            {
                                continue;
                            }

                            visited[next.X, next.Y] = true;

                            if (this.CellStates[next.X, next.Y] != CellState_Empty)
                                continue;

                            nextList.Add(next);
                        }
                    }

                    distance++;
                }

                for (int y = 0; y < this.ScaleY; y++)
                {
                    for (int x = 0; x < this.ScaleX; x++)
                    {
                        this.FeatureMap[x, y] += localAccessMap[x, y];
                    }
                }
            }

            int min = int.MaxValue;
            int halfX = this.ScaleX / 2;
            int halfY = this.ScaleY / 2;

            for (int y = 0; y < this.ScaleY; y++)
            {
                for (int x = 0; x < this.ScaleX; x++)
                {
                    if (this.CellStates[x, y] != CellState_Empty)
                        continue;

                    this.FeatureMap[x, y] += (x - halfX).Abs().Min((y - halfY).Abs()) * 4;
                    int value = this.FeatureMap[x, y];

                    if (value < min)
                        min = value;
                }
            }

            for (int y = 0; y < this.ScaleY; y++)
            {
                for (int x = 0; x < this.ScaleX; x++)
                {
                    if (this.CellStates[x, y] != CellState_Empty)
                        continue;

                    this.FeatureMap[x, y] -= min;
                }
            }

            Range<int> validRange = new Range<int>(this.Random.Next(1), this.Random.Next(4, 10));
            List<Int2D> validPositions = new List<Int2D>();

            // Search valid positions
            for (int y = 2; y < this.ScaleY - 2; y++)
            {
                for (int x = 2; x < this.ScaleX - 2; x++)
                {
                    if (this.CellStates[x, y] != CellState_Empty)
                        continue;

                    if (this.FeatureMap[x, y] == validRange)
                    {
                        validPositions.Add(new Int2D(x, y));
                    }
                }
            }

            int featureSpotIndex = this.Random.Next(validPositions.Count);
            this.FeatureSpot = validPositions[featureSpotIndex];
        }


        List<Int2D> GenerateDirectPath(Int2D pointA, Int2D pointB)
        {
            // Bresenham's compact line algorithm
            List<Int2D> points = new List<Int2D>();

            int dx = Math.Abs(pointB.X - pointA.X);
            int dy = Math.Abs(pointB.Y - pointA.Y);
            int sx = pointA.X < pointB.X ? 1 : -1;
            int sy = pointA.Y < pointB.Y ? 1 : -1;
            int err = dx - dy;

            Int2D current = pointA;

            while (true)
            {
                points.Add(current);
                if (current == pointB) break;

                Int2D next = current;

                int e2 = 2 * err;

                if (e2 > -dy)
                {
                    err -= dy;
                    next.X += sx;
                }
                else if (e2 < dx)
                {
                    err += dx;
                    next.Y += sy;
                }

                current = next;
            }

            return points;
        }
    
        List<Int2D> GenerateChaoticPath(Int2D pointA, Int2D pointB)
        {
            // Random Walk algorithm, kind of
            List<Int2D> connectedToEnd = this.FloodFill(pointB, CellState_Empty);

            HashSet<Int2D> path = new HashSet<Int2D>();
            path.Add(pointA);

            Int2D current = pointA;
            
            int[] directions = new int[] { 0, 1, 2, 3 };

            void Shuffle()
            {
                for (int i = 0; i < 4; i++)
                {
                    int idx1 = this.Random.Next(4);
                    int idx2 = this.Random.Next(4);
                    int t = directions[idx1];
                    directions[idx1] = directions[idx2];
                    directions[idx2] = t;
                }
            }

            int maxX = this.ScaleX - 1;
            int maxY = this.ScaleY - 1;

            for (int i = 0; i < 500 && current != pointB; i++)
            {
                Shuffle();

                if (current.Equals(pointB)) break;

                if (connectedToEnd.Contains(current))
                    break;

                // check if path is complete
                if (current.X == pointB.X && current.Y == pointB.Y) break;

                bool directionFound = false;
                Int2D next = current;

                foreach (var direction in directions)
                {
                    next = current;
                    switch (direction)
                    {
                        case 0: // up
                            next.Y++;
                            break;
                        case 1: // right
                            next.X++;
                            break;
                        case 2: // down
                            next.Y--;
                            break;
                        case 3: // left
                            next.X--;
                            break;
                    }

                    // check if next cell is valid
                    if (next.X >= 1 &&
                        next.X <= maxX - 1 &&
                        next.Y >= 1 &&
                        next.Y <= maxY - 1 &&
                        !path.Contains(next))
                    {
                        path.Add(next);
                        current = next;

                        directionFound = true;
                        break;
                    }
                }

                if (!directionFound && path.Count > 1)
                {
                    path.Remove(current);
                    current = path.Last();
                    next = current;
                }
            }

            return path.ToList();
        }


        private List<Int2D> FloodFill(Int2D target)
        {
            int targetState = this.CellStates[target.X, target.Y];

            return this.FloodFill(target, targetState);
        }
        private List<Int2D> FloodFill(Int2D target, int targetState)
        {
            List<Int2D> toReturn = new List<Int2D>();
            Queue<Int2D> toCheck = new Queue<Int2D>();
            bool[,] visited = new bool[this.ScaleX, this.ScaleY];

            toCheck.Enqueue(target);

            Direction2D[] directions = new Direction2D[]
            {
                Direction2D.North,
                Direction2D.East,
                Direction2D.South,
                Direction2D.West
            };

            while (toCheck.Count > 0)
            {
                Int2D check = toCheck.Dequeue();
                toReturn.Add(check);

                foreach (var direction in directions)
                {
                    Int2D current = direction.OffsetDirection(check, false);

                    if (visited[current.X, current.Y])
                        continue;

                    visited[current.X, current.Y] = true;

                    if (current.X >= 1
                        && current.Y >= 1
                        && current.X < this.ScaleX - 1
                        && current.Y < this.ScaleY - 1
                        && this.CellStates[current.X, current.Y] == targetState)
                    {
                        toCheck.Enqueue(current);
                    }
                }
            }

            return toReturn;
        }
    }
}