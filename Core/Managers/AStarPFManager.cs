using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Tiled;
using System;
using System.Collections.Generic;

namespace Core.Managers
{
	class AStarPFManager
	{
		private const float BEST_POSSIBLE_F = float.PositiveInfinity;
		private const int POSIBLE_MAP_MOVES = 4;

		public bool AreStartAndGoalInitialized { get; private set; }
		public bool IsGoalFound { get; private set; }
		public bool IsGoalReached { get; private set; }

		private TiledMap TiledMap;

		public AStarPFCell StartCell { get; private set; }
		public AStarPFCell GoalCell { get; private set; }

		public List<AStarPFCell> OpenList { get; private set; }
		public List<AStarPFCell> VisitedList { get; private set; }
		public List<Vector2> ShortestPath { get; private set; }

		public AStarPFManager(TiledMap tiledMap)
		{
			TiledMap = tiledMap;

			AreStartAndGoalInitialized = false;
			IsGoalFound = false;
			IsGoalReached = false;

			StartCell = null;
			GoalCell = null;

			OpenList = new List<AStarPFCell>();
			VisitedList = new List<AStarPFCell>();
			ShortestPath = new List<Vector2>();
		}

		public void resetPath()
		{
			AreStartAndGoalInitialized = false;
			IsGoalFound = false;
			IsGoalReached = false;

			StartCell = null;
			GoalCell = null;

			OpenList.Clear();
			VisitedList.Clear();
			ShortestPath.Clear();
		}

		public void FindPath(Vector2 currentNode, Vector2 goalNode)
		{
			if (IsGoalFound)
			{
				return;
			}

			if (AreStartAndGoalInitialized)
			{
				ContinuePath();
			}
			else
			{
				OpenList.Clear();
				VisitedList.Clear();
				ShortestPath.Clear();

				AStarPFCell startCell = new AStarPFCell
				{
					XCoord = (int) currentNode.X,
					YCoord = (int) currentNode.Y
				};

				AStarPFCell goalCell = new AStarPFCell()
				{
					XCoord = (int) goalNode.X,
					YCoord = (int) goalNode.Y
				};

				SetStartAndGoal(startCell, goalCell);

				AreStartAndGoalInitialized = true;
			}
		}

		public Vector2 NextPathPosition(Vector2 entityPosition)
		{
			int index = 1;
			Vector2 shortestPathNextCell = ShortestPath[ShortestPath.Count - index];
			Vector2 nextCell = new Vector2()
			{
				X = shortestPathNextCell.X,
				Y = shortestPathNextCell.Y
			};

			Vector2 entityPositionBasedOnMapTiles = new Vector2()
			{
				X = entityPosition.X / TiledMap.TileWidth,
				Y = entityPosition.Y / TiledMap.TileWidth
			};

			if (!IsGoalReached && entityPositionBasedOnMapTiles == nextCell)
			{
				if (ShortestPath.Count > 1)
				{
					ShortestPath.RemoveAt(0);
				}
				else
				{
					OpenList.Clear();
					VisitedList.Clear();
					ShortestPath.Clear();

					IsGoalReached = true;
				}
			}

			return nextCell;
		}

		private void ContinuePath()
		{
			// LIMIT TO POSIBLE MOVES

			if (OpenList.Count <= 0)
			{
				return;
			}

			AStarPFCell currentCell = GetNextCell();

			if (currentCell.ID == GoalCell.ID)
			{
				GoalCell.ParentCell = currentCell;

				AStarPFCell getPath = null;

				for (getPath = GoalCell; getPath != null; getPath = getPath.ParentCell)
				{
					ShortestPath.Add(new Vector2(getPath.XCoord, getPath.YCoord));
				}

				IsGoalFound = true;

				return;
			}

			// Sides
			IsInOpenedList(currentCell.XCoord + 1, currentCell.YCoord, currentCell.G + 1, currentCell);
			IsInOpenedList(currentCell.XCoord - 1, currentCell.YCoord, currentCell.G + 1, currentCell);
			IsInOpenedList(currentCell.XCoord, currentCell.YCoord + 1, currentCell.G + 1, currentCell);
			IsInOpenedList(currentCell.XCoord, currentCell.YCoord - 1, currentCell.G + 1, currentCell);

			// To calculate diagonals, add/subtract appropriate x/y
			//IsInOpenedList(currentCell.XCoord + 1, currentCell.YCoord + 1, currentCell.G + 1, currentCell);
			//IsInOpenedList(currentCell.XCoord + 1, currentCell.YCoord - 1, currentCell.G + 1, currentCell);
			//IsInOpenedList(currentCell.XCoord - 1, currentCell.YCoord - 1, currentCell.G + 1, currentCell);
			//IsInOpenedList(currentCell.XCoord - 1, currentCell.YCoord + 1, currentCell.G + 1, currentCell);

			int OpenListCount = OpenList.Count;
			for (int i = 0; i < OpenListCount; i++)
			{
				if (currentCell.ID == OpenList[i].ID)
				{
					// TODO Check this
					OpenList.RemoveAt(i);
				}
			}
		}

		private AStarPFCell GetNextCell()
		{
			int bestCellIndex = -1;
			float bestF = BEST_POSSIBLE_F;

			AStarPFCell nextCell = null;

			int OpenListCount = OpenList.Count;
			for (int i = 0; i < OpenListCount; i++)
			{
				AStarPFCell openCell = OpenList[i];

				if (openCell.GetF() < bestF)
				{
					bestF = openCell.GetF();
					bestCellIndex = i;
				}
			}

			if (bestCellIndex >= 0)
			{
				nextCell = OpenList[bestCellIndex];

				VisitedList.Add(nextCell);
				OpenList.RemoveAt(bestCellIndex);
			}

			return nextCell;
		}

		private void IsInOpenedList(int xCoord, int yCoord, int newCost, AStarPFCell parentCell)
		{
			// TODO Check Collision
			// If the tile is not transitable, return

			int searchID = GenerateCellID(xCoord, yCoord);

			// TODO Check also out of bounds of tiled map
			if (xCoord < 0 || yCoord < 0)
			{
				return;
			}

			// If it is already visited, return
			foreach (AStarPFCell visitedCell in VisitedList)
			{
				if (searchID == visitedCell.ID)
				{
					return;
				}
			}

			AStarPFCell newChild = new AStarPFCell(searchID, xCoord, yCoord, parentCell)
			{
				G = newCost
			};
			newChild.SetHeuristicByManhattanDistance(GoalCell);

			foreach (AStarPFCell openCell in OpenList)
			{
				if (searchID == openCell.ID)
				{
					float newF = newChild.G + newCost + openCell.H;

					if (newF < openCell.GetF())
					{
						// Replace with the better one
						openCell.G = newChild.G + newCost;
						openCell.ParentCell = newChild;
					}
					else
					{
						// If the F is not better
						return;
					}
				}
			}

			OpenList.Add(newChild);
		}

		private void SetStartAndGoal(AStarPFCell startCell, AStarPFCell goalCell)
		{
			StartCell = new AStarPFCell(
				GenerateCellID(startCell.XCoord, startCell.YCoord),
				startCell.XCoord, startCell.YCoord, null);

			StartCell.G = 0;
			StartCell.H = StartCell.GetManhattanDistance(goalCell);

			GoalCell = new AStarPFCell(
				GenerateCellID(goalCell.XCoord, goalCell.YCoord),
				goalCell.XCoord, goalCell.YCoord, goalCell);

			OpenList.Add(StartCell);
		}

		private int GenerateCellID(int xCoord, int yCoord)
		{
			return yCoord * (TiledMap.WidthInPixels / TiledMap.TileWidth) + xCoord;
		}

		public Vector2 ParseEntityPositionToCoords(Vector2 entityPosition)
		{
			return new Vector2()
			{
				X = entityPosition.X / TiledMap.TileWidth,
				Y = entityPosition.Y / TiledMap.TileHeight
			};
		}

		public void DrawDebug(SpriteBatch spriteBatch, BitmapFont font, OrthographicCamera camera)
		{
			if (IsGoalFound)
			{
				string debugText = "OpenList: " + OpenList.Count
					+ ", VisitedList: " + VisitedList.Count
					+ ", ShortestPath: " + ShortestPath.Count;

				spriteBatch.DrawString(font, debugText, camera.ScreenToWorld(0, 0), Color.White);

				for (var i = 0; i < TiledMap.WidthInPixels / TiledMap.TileWidth; i++)
				{
					for (var j = 0; j < (TiledMap.HeightInPixels / TiledMap.TileHeight - 1); j++)
					{
						spriteBatch.DrawRectangle(
							new Rectangle(i * TiledMap.TileWidth, j * TiledMap.TileWidth,
							TiledMap.TileWidth, TiledMap.TileWidth + TiledMap.TileWidth),
							Color.White);
					}
				}

				foreach (AStarPFCell cell in VisitedList)
				{
					spriteBatch.DrawRectangle(
						new Rectangle(cell.XCoord * TiledMap.TileWidth, cell.YCoord * TiledMap.TileWidth,
						TiledMap.TileWidth, TiledMap.TileWidth + TiledMap.TileWidth),
						Color.Blue);
				}

				foreach (AStarPFCell cell in OpenList)
				{
					spriteBatch.DrawRectangle(
						new Rectangle(cell.XCoord * TiledMap.TileWidth, cell.YCoord * TiledMap.TileWidth,
						TiledMap.TileWidth, TiledMap.TileWidth + TiledMap.TileWidth),
						Color.Orange);
				}

				foreach (Vector2 path in ShortestPath)
				{
					spriteBatch.DrawRectangle(
						new Rectangle((int)path.X * TiledMap.TileWidth, (int)path.Y * TiledMap.TileWidth,
						TiledMap.TileWidth, TiledMap.TileWidth + TiledMap.TileWidth),
						Color.Green);
				}
			}

		}

	}
}
