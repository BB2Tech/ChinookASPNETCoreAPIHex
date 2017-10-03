﻿using System.Collections.Generic;

namespace Chinook.Domain.Entities
{
    public class Album
    {
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public int ArtistId { get; set; }
        public string ArtistName { get; set; }
        public virtual ICollection<Track> Tracks { get; set; }
        public virtual Artist Artist { get; set; }
    }
}
