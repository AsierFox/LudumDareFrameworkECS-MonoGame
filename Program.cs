using System;
using Core.Game;

namespace LudumDareFrameworkECS
{
	public static class Program
	{
		[STAThread]
		static void Main()
		{
			new GameCore()
				.Run();
		}
	}
}
