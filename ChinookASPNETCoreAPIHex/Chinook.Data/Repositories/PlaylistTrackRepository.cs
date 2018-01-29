﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Chinook.Domain.Repositories;
using Chinook.Domain.Entities;
namespace Chinook.Data.Repositories
{
    public class PlaylistTrackRepository : IPlaylistTrackRepository
    {
        private readonly ChinookContext _context;
        private readonly IPlaylistRepository _playlistRepo;
        private readonly ITrackRepository _trackRepository;

        public PlaylistTrackRepository(ChinookContext context, IPlaylistRepository playlistRepo,
            ITrackRepository trackRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _playlistRepo = playlistRepo;
            _trackRepository = trackRepository;
        }

        private async Task<bool> PlaylistTrackExists(int id, CancellationToken ct = default(CancellationToken))
        {
            return await GetByPlaylistIdAsync(id, ct) != null;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<List<PlaylistTrack>> GetAllAsync(CancellationToken ct = default(CancellationToken))
        {
            IList<PlaylistTrack> list = new List<PlaylistTrack>();
            var playlistTracks = await _context.PlaylistTrack.ToListAsync(cancellationToken: ct);
            foreach (var i in playlistTracks)
            {
                var playlist = await _playlistRepo.GetByIdAsync(i.PlaylistId, ct);
                var track = await _trackRepository.GetByIdAsync(i.TrackId, ct);
                var playlistTrack = new PlaylistTrack
                {
                    PlaylistId = i.PlaylistId,
                    TrackId = i.TrackId,
                    Playlist = playlist,
                    Track = track
                };
                list.Add(playlistTrack);
            }
            return list.ToList();
        }

        public async Task<List<PlaylistTrack>> GetByPlaylistIdAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            IList<PlaylistTrack> list = new List<PlaylistTrack>();
            var current = await _context.PlaylistTrack.Where(a => a.PlaylistId == id).ToListAsync(cancellationToken: ct);
            foreach (var i in current)
            {
                var playlist = await _playlistRepo.GetByIdAsync(i.PlaylistId, ct);
                var track = await _trackRepository.GetByIdAsync(i.TrackId, ct);
                var newisd = new PlaylistTrack
                {
                    PlaylistId = i.PlaylistId,
                    TrackId = i.TrackId,
                    Playlist = playlist,
                    Track = track
                };
                list.Add(newisd);
            }
            return list.ToList();
        }

        public async Task<List<PlaylistTrack>> GetByTrackIdAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            IList<PlaylistTrack> list = new List<PlaylistTrack>();
            var current = await _context.PlaylistTrack.Where(a => a.TrackId == id).ToListAsync(cancellationToken: ct);
            foreach (var i in current)
            {
                var playlist = await _playlistRepo.GetByIdAsync(i.PlaylistId, ct);
                var track = await _trackRepository.GetByIdAsync(i.TrackId, ct);
                var newisd = new PlaylistTrack
                {
                    PlaylistId = i.PlaylistId,
                    TrackId = i.TrackId,
                    Playlist = playlist,
                    Track = track
                };
                list.Add(newisd);
            }
            return list.ToList();
        }

        public async Task<PlaylistTrack> AddAsync(PlaylistTrack newPlaylistTrack, CancellationToken ct = default(CancellationToken))
        {
            var playlist = new DataModels.PlaylistTrack
            {
                PlaylistId = newPlaylistTrack.PlaylistId,
                TrackId = newPlaylistTrack.TrackId
            };

            _context.PlaylistTrack.Add(playlist);
            await _context.SaveChangesAsync(ct);
            return newPlaylistTrack;
        }

        public async Task<bool> UpdateAsync(PlaylistTrack playlistTrack, CancellationToken ct = default(CancellationToken))
        {
            if (!await PlaylistTrackExists(playlistTrack.PlaylistId, ct))
                return false;
            var changing = await _context.PlaylistTrack.FindAsync(playlistTrack.TrackId);
            _context.PlaylistTrack.Update(changing);

            changing.PlaylistId = playlistTrack.PlaylistId;
            changing.TrackId = playlistTrack.TrackId;

            await _context.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default(CancellationToken))
        {
            if (!await PlaylistTrackExists(id, ct))
                return false;
            var toRemove = _context.PlaylistTrack.Find(id);
            _context.PlaylistTrack.Remove(toRemove);
            await _context.SaveChangesAsync(ct);
            return true;
        }
    }
}
