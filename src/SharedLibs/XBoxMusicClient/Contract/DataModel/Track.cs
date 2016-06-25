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
using System.Runtime.Serialization;

namespace Microsoft.Xbox.Music.Platform.Contract.DataModel
{
    [DataContract(Namespace = Constants.Xmlns)]
    public class Track : Content
    {
        // These items are available when this is the main element of the query or if an extra details parameter has been specified for that sub-element
        [DataMember(EmitDefaultValue = false)]
        public DateTime? ReleaseDate { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public TimeSpan? Duration { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? TrackNumber { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool? IsExplicit { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public GenreList Genres { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public GenreList Subgenres { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public RightList Rights { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Subtitle { get; set; }

        /// <summary>
        /// Possible values are:
        ///     - audio/mp3
        ///     - audio/aac
        ///     - audio/wma
        ///     - audio/mp4
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string ContentType { get; set; }

        // This sub-element is null when this Track is queried as a sub-element of an album (to avoid looping), populated with just the minimal stuff by default when this Track is the main element, and extra details can obtained with a details parameter
        [DataMember(EmitDefaultValue = false)]
        public Album Album { get; set; }

        // This sub-element populated with just the minimal stuff by default when this Track is the main element, and extra details can obtained with a details parameter
        [DataMember(EmitDefaultValue = false)]
        public List<Contributor> Artists { get; set; }
    }
}
