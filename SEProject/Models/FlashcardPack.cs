using System;
using System.Collections.Generic;

namespace SEProject.Models
{
	public class FlashcardPack
	{
		public string name { get; set; }
		public Guid ID { get; set; }
		public List<Flashcard> flashcard { get; set; }

		public FlashcardPack(string name, Guid ID, List<Flashcard> flashcard)
		{
			this.name = name;
			this.ID = ID;
			this.flashcard = flashcard;
		}
    }
}