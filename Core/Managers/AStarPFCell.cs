using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Managers
{
	class AStarPFCell
	{

		public int ID;
		public int XCoord;
		public int YCoord;

		// Distance between start node and current node
		public int G;
		// Heuristic value is the estimated distance to the goal
		public int H;

		public AStarPFCell ParentCell;

		public AStarPFCell()
		{
			ParentCell = null;
		}

		public AStarPFCell(int ID, int xCoord, int yCoord, AStarPFCell parentCell)
		{
			this.ID = ID;
			XCoord = xCoord;
			YCoord = yCoord;
			ParentCell = parentCell;
			G = 0;
			H = 0;
		}

		// Distance between start node to goal node using straight lines _|
		public int GetManhattanDistance(AStarPFCell goalCell)
		{
			int xx = Math.Abs(XCoord - goalCell.XCoord);
			int yy = Math.Abs(YCoord - goalCell.YCoord);

			return xx + yy;
		}

		public void SetHeuristicByManhattanDistance(AStarPFCell goalCell)
		{
			H = GetManhattanDistance(goalCell);
		}

		// Direct distance between start node to goal node in diagonal
		public int GetEuclideanDistance(AStarPFCell goalCell)
		{
			double xx = Math.Pow(Math.Abs(XCoord - goalCell.XCoord), 2);
			double yy = Math.Pow(Math.Abs(YCoord - goalCell.YCoord), 2);

			return (int) Math.Sqrt(xx + yy);
		}

		// G + H
		public int GetF()
		{
			return G + H;
		}

	}
}
