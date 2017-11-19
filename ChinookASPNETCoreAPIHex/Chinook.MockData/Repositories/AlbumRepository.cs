﻿using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Chinook.Domain.Repositories;
using Chinook.Domain.Entities;

namespace Chinook.MockData.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        public void Dispose()
        {
        }

        public async Task<List<Album>> GetAllAsync(string sortOrder = "", string searchString = "", int page = 0, int pageSize = 0, CancellationToken ct = default(CancellationToken))
        {
            IList<Album> list = new List<Album>();

            Album album = new Album
            {
                AlbumId = 1,
                ArtistId = 1,
                Title = "Hellow World",
                ArtistName = "Foo"
            };
            list.Add(album);

            return list.ToList();
        }

        public async Task<Album> GetByIdAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            Album album = new Album
            {
                AlbumId = id,
                ArtistId = 1,
                Title = "Hello World",
                ArtistName = "Foo"
            };
            return album;
        }

        public async Task<Album> AddAsync(Album newAlbum, CancellationToken ct = default(CancellationToken))
        {
            newAlbum.AlbumId = 1;
            return newAlbum;
        }

        public async Task<bool> UpdateAsync(Album album, CancellationToken ct = default(CancellationToken))
        {
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            return true;
        }

        public async Task<List<Album>> GetByArtistIdAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            IList<Album> list = new List<Album>();
            Album newisd = new Album
            {
                Title = "hello World",
                ArtistId = 1,
                AlbumId = 1,
                ArtistName = "Foo"
            };
            list.Add(newisd);
            return list.ToList();
        }
    }
}
