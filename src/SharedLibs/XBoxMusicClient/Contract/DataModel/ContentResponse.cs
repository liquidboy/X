// Copyright (c) Microsoft Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Microsoft.Xbox.Music.Platform.Contract.DataModel
{
    [DataContract(Namespace = Constants.Xmlns)]
    public class ContentResponse : BaseResponse
    {
        [DataMember(EmitDefaultValue = false)]
        public PaginatedList<Artist> Artists { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public PaginatedList<Album> Albums { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public PaginatedList<Track> Tracks { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public PaginatedList<Playlist> Playlists { get; set; } 

        [DataMember(EmitDefaultValue = false)]
        public PaginatedList<ContentItem> Results { get; set; } 

        [DataMember(EmitDefaultValue = false)]
        public GenreList Genres { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Culture { get; set; }

        private void GenericAddPieceOfContent<T>(Content content, PaginatedList<T> container, Func<PaginatedList<T>> containerInstanciator)
            where T : Content
        {
            if (container == null)
            {
                container = containerInstanciator();
            }
            if (container.Items == null)
            {
                container.Items = new List<T>();
            }
            container.Items.Add(content as T);
        }

        public void AddPieceOfContent(Content content)
        {
            if (content is Artist)
            {
                GenericAddPieceOfContent(content, Artists, () => Artists = new PaginatedList<Artist>());
            }
            else if (content is Album)
            {
                GenericAddPieceOfContent(content, Albums, () => Albums = new PaginatedList<Album>());
            }
            else if (content is Track)
            {
                GenericAddPieceOfContent(content, Tracks, () => Tracks = new PaginatedList<Track>());
            }
            else if (content is Playlist)
            {
                GenericAddPieceOfContent(content, Playlists, () => Playlists = new PaginatedList<Playlist>());
            }
            else
            {
                throw new ArgumentException("Unknown content type:" + content.GetType().ToString());
            }
        }

        public IEnumerable<Content> GetAllTopLevelContent()
        {
            return GetAllContentLists()
                .Where(c => c != null && c.ReadOnlyItems != null)
                .SelectMany(x => x.ReadOnlyItems);
        }

        public IEnumerable<IPaginatedList<Content>> GetAllContentLists()
        {
            yield return Artists;
            yield return Albums;
            yield return Tracks;
            yield return Playlists;
        }
    }
}
