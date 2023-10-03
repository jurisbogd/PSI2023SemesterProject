using System;
using System.Collections.Generic;

namespace SEProject.Models
{
	public class FlashcardPack
	{
		public String name { get; set; }
		public Guid ID { get; set; }
		public List<Flashcard> flaschard { get; set; }

		public FlashcardPack(String name, Guid ID, List<Flashcard> flaschard)
		{
			this.name = name;
			this.ID = ID;
			this.flaschard = flaschard;
		}
    }
}

