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
    /// <summary>
    /// See http://msdn.microsoft.com/en-us/library/aa347850.aspx, paragraph "Customizing Collection Types"
    /// </summary>
    [CollectionDataContract(Namespace = Constants.Xmlns, ItemName = "Genre")]
    public class GenreList : List<string>
    {
        public GenreList()
            : base()
        {
        }

        public GenreList(IEnumerable<string> collection)
            : base(collection)
        {
        }
    }

    [CollectionDataContract(Namespace = Constants.Xmlns, ItemName = "Right")]
    public class RightList : List<string>
    {
        public RightList()
            : base()
        {
        }

        public RightList(IEnumerable<string> collection)
            : base(collection)
        {
        }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2237:Mark ISerializable types with SerializableAttribute", Justification = "SerializableAttribute does not exist in Portable .NET")]
    [CollectionDataContract(Namespace = Constants.Xmlns, ItemName = "OtherId", KeyName="Namespace", ValueName="Id")]
    public class IdDictionary : Dictionary<string, string>
    {
        public IdDictionary()
            : base()
        {
        }

        public IdDictionary(IDictionary<string, string> dictionary)
            : base(dictionary)
        {
        }
    }

    [CollectionDataContract(Namespace = Constants.Xmlns, ItemName = "TrackId")]
    public class TrackIdList : List<string>
    {
        public TrackIdList()
            : base()
        {
        }

        public TrackIdList(IEnumerable<string> collection)
            : base(collection)
        {
        }
    }
}
