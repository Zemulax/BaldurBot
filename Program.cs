using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaldurBot
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var baldur = new Bot();
			//getAwaiter allows use of asynchronous code
			//getResult ends the wait for asynchronous
			baldur.RunBaldurAsync().GetAwaiter().GetResult();

		}
	}
}
