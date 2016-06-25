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

using System.Runtime.Serialization;

namespace Microsoft.Xbox.Music.Platform.Contract.DataModel
{
    // This enum contains all possible ordering of our backend services
    // However, we don't expose all of them to our end-user
    // In case of functionnally duplicated values (AlbumTitle and AlbumName for example), we will have both but expose only one
    [DataContract(Namespace = Constants.Xmlns)]
    public enum OrderBy : byte
    {
        None = 0,
        [EnumMember]
        AllTimePlayCount,               // Catalog
        [EnumMember]
        ReleaseDate,                    // Catalog & Collection
        [EnumMember]
        ArtistName,                     // Collection's artists, albums, tracks
        [EnumMember]
        AlbumTitle,                     // Collection's albums, tracks
        [EnumMember]
        TrackTitle,                     // Collection's tracks
        [EnumMember]
        GenreName,                      // Collection's albums, tracks
        [EnumMember]
        CollectionDate,                 // Collection : date added to the collection
        [EnumMember]
        TrackNumber,                    // Collection lookup of an album's tracks
        [EnumMember]
        MostPopular                     // Catalog
    }
}
